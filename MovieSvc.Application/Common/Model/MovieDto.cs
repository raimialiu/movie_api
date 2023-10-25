using MovieSvc.Domain.Entities;

namespace MovieSvc.Application.Common.Model;

public class MovieDto
{
    public string Id { get; private set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ReleasedDate { get; set; }
    public decimal TicketPrice { get; set; }
    public int Rating { get; set; }
    public string Photo { get; set; }
    public string Country { get; set; }
    
    public string GenreId { get; set; }
    public Genre Genre { get; set; }
    
    public DateTime CreationTime { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}