using System.ComponentModel.DataAnnotations;

namespace Turbo_Auth.Models.Suppliers;

public class SupplierKey
{
    [Key]
    public int SupplierKeyId
    {
        get;
        set;
    }
    [Required]
    [MaxLength(200)]
    public string? BaseUrl
    {
        get;
        set;
    }
    [Required]
    public int RequestIdentifier
    {
        get;
        set;
    } = 0;
    [Required]
    [MaxLength(200)]
    public string? ApiKey
    {
        get;
        set;
    }

    public ICollection<ModelFee>? ModelFees
    {
        get;
        set;
    }
    public override string ToString()
    {
        var str =  $"SupplierKeyId: {SupplierKeyId}, BaseUrl: {BaseUrl}, RequestIdentifier: {RequestIdentifier}, ApiKey: {ApiKey}\n";
        if (ModelFees != null)
        {
            foreach (var modelFee in ModelFees)
            {
                str += $"    ${modelFee}\n";
            }
        }

        return str;
    }

}