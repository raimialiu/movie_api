namespace MovieSvc.Domain.Attributes;

public class SelectPropertyAttribute: Attribute
{
    public string PropertyDisplayName { get; set; }
        
}