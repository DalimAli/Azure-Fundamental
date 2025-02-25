using System.ComponentModel.DataAnnotations;

namespace Azure_Blob_Storage.Models;

public class Container
{
    [Required]
    public string Name { get; set; }
}