using System.ComponentModel.DataAnnotations;

namespace Turbo_Auth.Models.Suppliers;

public class ModelFee
{
    [Key]
    public int ModelFeeId
    {
        get;
        set;
    }

    [Required]
    public int SupplierKeyId
    {
        get;
        set;
    }
    public SupplierKey? SupplierKey { get; set; }
    [Required]
    public int ModelId
    {
        get;
        set;
    }

    public Model? Model
    {
        get;
        set;
    }
    [Required]
    public double Fee
    {
        get;
        set;
    }
    public override string ToString()
    {
        return $"ModelFeeId: {ModelFeeId}, SupplierKeyId: {SupplierKeyId}, ModelId: {ModelId}, Model: {Model}, Fee: {Fee}";
    }
}