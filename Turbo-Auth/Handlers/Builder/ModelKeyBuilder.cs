using Org.BouncyCastle.Utilities.Collections;
using Turbo_Auth.Handlers.Differentiator;
using Turbo_Auth.Handlers.Group;
using Turbo_Auth.Handlers.Model2Key;
using Turbo_Auth.Models.Ai.Chat;
using Turbo_Auth.Models.Suppliers;

namespace Turbo_Auth.Handlers.Builder;

public class ModelKeyBuilder: IModelKeyBuilder
{
    public ModelKeyBuilder()
    {
        
    }
    public async Task<QuickModel> Build(List<SupplierKey> supplierKeys)
    {
        var modelGroup = new ModelGroup(true);// default modelGroup
        /*
         * 首先构建 特异的modelGroup
         * modelGroup的每一个组都是可替换的模型，比如包含两个组
         * 组1： gpt-3.5, gemini-pro, gpt-4
         * 组2： dall-2, stable-diffusion
         * 默认都会填充更多的模型，实际的key可能并不支持这些模型，
         * 所以首先查找是否有key支持这些模型。
         */
        var factGroup = new ModelGroup(false);// 事实模型组，是默认模型组的子集
        foreach (var group in modelGroup.Group)
        {
            var fg = new List<ChatDisplayModel>();
            foreach (var item in group)
            {
                var exist = supplierKeys.Any(s =>
                    s.ModelFees != null && s.ModelFees.Any(m => m.Model!.Name==item.ModelValue));
                if (exist)
                {
                    fg.Add(new ChatDisplayModel(item.ModelName!,item.ModelValue!));
                }
            }
            factGroup.AddGroup(fg);
        }
        /*
         * 然后就可以构建模型的映射表了，这个表的基本结构是一个字典
         * 针对任何一个模型，会有一个花费评估数组与其对应
         * 后续处理时，根据根据这个数组调度合适的密钥进行使用
         * *
         */
        var models = new HashSet<string>();
        foreach (var key in
                 supplierKeys
                )
        {
            if (key.ModelFees != null)
            {
                foreach (var model in key.ModelFees!.Select(m => m.Model!.Name))
                {
                    models.Add(model!);
                }
            }
        }

        var quick = new Dictionary<string, List<WeightKey>>();
        foreach (var model in models)
        {
            // 如果不能保证每一个key都包含modelFees，这里可能会报错
            var weightKeys = supplierKeys.SelectMany(key => key.ModelFees!)
                .Where(fee => fee.Model!.Name == model)
                .Select(fee => new WeightKey
                {
                    Weight = 1 / fee.Fee,
                    SupplierKey = fee.SupplierKey
                })
                .ToList();
            quick.Add(model,weightKeys);
        }

        var novitaKeys = supplierKeys.Where(s => s.RequestIdentifier == (int)HandlerType.Novita)
            .ToList();
        
        return new()
        {
            ModelGroup = factGroup,
            Quick = quick,
            NovitaKeys = novitaKeys
        };
    }

}