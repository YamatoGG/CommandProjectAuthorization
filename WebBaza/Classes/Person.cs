using System.ComponentModel.DataAnnotations.Schema;

namespace WebBaza.Classes;
[Table("Person")]
public record Person
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("login")]
    public required string Login { get; set; }

    [Column("password")]
    public required string Password { get; set; }
}
