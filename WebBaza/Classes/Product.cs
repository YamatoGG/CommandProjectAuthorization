using System.ComponentModel.DataAnnotations.Schema;

namespace WebBaza.Classes;
[Table("Product")]
public record Product
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("price")]
    public required decimal Price { get; set; }
}