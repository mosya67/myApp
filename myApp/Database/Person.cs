using System.ComponentModel.DataAnnotations;

namespace myApp;
public class Person
{
    public int personid { get; set; }
    public string? name { get; set; }

    [Required]
    public DateOnly dateofbirth { get; set; }

    [Required]
    public string gender { get; set; }
}
