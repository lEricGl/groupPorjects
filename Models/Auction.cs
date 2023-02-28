using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace assignment_one.Models
{
    public class Auction
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]

        public decimal StartingPrice { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public int Condition { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string? UserId { get; set; }

        public Auction() { }

        public Auction(int id, string name, string description, string imageUrl, decimal startingPrice, DateTime startDate, int category, int condition, string userId)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            StartingPrice = startingPrice;
            StartDate = startDate;
            Category = category;
            Condition = condition;
            UserId = userId;
        }

    }
}
