using System.ComponentModel.DataAnnotations;

namespace Turbo_Auth.Models.Suppliers;

public class Model
{
    [Key]
    public int ModelId
    {
        get;
        set;
    }

    public bool Enable
    {
        get;
        set;
    }
    [Required]
    [MaxLength(30)]
    public string? Name
    {
        get;
        set;
    }
    public override string ToString()
    {
        return $"ModelId: {ModelId}, Name: {Name}";
    }
}