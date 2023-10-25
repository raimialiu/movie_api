using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSvc.Domain.Entities;

[Table("movies")]
public class Movie: AuditableEntity
{
        public Movie(){}

        public Movie(string name, int rating, string photo,
                        string country, DateTime releasedDate,
                        decimal ticketPrice, string description="", Genre genre =null)
        {
            Name = name;
            Rating = rating;
            Photo = photo;
            Description = description;
            TicketPrice = ticketPrice;
            Country = country;
            ReleasedDate = releasedDate;
            Genre = genre;
        }

    
        [Key]
        [MaxLength(32)]
        public string Id { get; private set; } =  Guid.NewGuid().ToString().Replace("-", "");

        [MaxLength(80)]
        [Column("name")]
        public string Name { get; set; }
        
        [DataType(DataType.Text)]
        [Column("description")]
        public string Description { get; set; }
        
        [Column("released_date")]
        public DateTime ReleasedDate { get; set; }
        
        [Column("ticket_price")]
        public decimal TicketPrice { get; set; }
        
        [Column("rating")]
        public int Rating { get; set; }
        
        [Column("photo")]
        public string Photo { get; set; }
        
        [Column("country")]
        [MaxLength(60)]
        public string Country { get; set; }
        
        
        [Column("genre_id")]
        public string GenreId { get; set; }
        
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }

        public static Movie Create(string name, int rating, string photo,
            string country, DateTime releasedDate,
            decimal ticketPrice, string description="", Genre genre=null)
        {
            return new Movie(name, rating, photo, country, releasedDate, ticketPrice, description, genre);
        }
        
}


