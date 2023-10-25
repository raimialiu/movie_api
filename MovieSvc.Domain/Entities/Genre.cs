using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSvc.Domain.Entities;

[Table("genre")]
public class Genre: AuditableEntity
{
    public Genre(){}

    public Genre(string name): this(name, "")
    {
        Name = name;
    }

    private Genre(string name, string description = "")
    {
        Name = name;
        Description = description;
    }

    [Key] [MaxLength(32)] public string Id { get; private set; } = Guid.NewGuid().ToString().Replace("-", "");
    
    [MaxLength(80)]
    [Column("name")]
    public string Name { get; set; }
        
    [DataType(DataType.Text)]
    [Column("description")]
    public string Description { get; set; }
    
    
    public static Genre Create(string name, string description="")
    {
        return new Genre(name, description);
    }
    
}