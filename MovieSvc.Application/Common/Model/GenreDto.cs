namespace MovieSvc.Application.Common.Model;

public class GenreDto
{
    public string GenreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDeprecated { get; set; }
    
    public DateTime CreationTime { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}