using System.ComponentModel.DataAnnotations;

namespace BookStoreAppAPI.Models.Book
{
    public class BookUpdateDto : BaseDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string Summary { get; set; }


        [Required]
        [Range(1800, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }
        public string Image { get; set; }

    }
}
