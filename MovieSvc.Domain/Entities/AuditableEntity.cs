using System.ComponentModel.DataAnnotations.Schema;
using MovieSvc.Domain.Attributes;

namespace MovieSvc.Domain.Entities;

public class AuditableEntity : BaseEntity
{
    protected AuditableEntity()
    {
        CreationDate = DateTime.Now;
    }

    [SelectProperty(PropertyDisplayName = "Created Date")]
    [OrderByColumn]
    [Column("creation_date")]
    public DateTime CreationDate { get; private set; }

    [SelectProperty(PropertyDisplayName = "Last Modified Date")]
    [Column("last_modified_date")]
    public DateTime? LastModifiedDate { get; private set; } 
    
    [Column("is_deprecated")]
    public bool IsDeprecated { get; set; }
    

    public void UpdateLastModifiedDate(DateTime? dateTime)
    {
        LastModifiedDate = dateTime ?? DateTime.Now;
    }
    
    public void Delete() => IsDeprecated = true;
}