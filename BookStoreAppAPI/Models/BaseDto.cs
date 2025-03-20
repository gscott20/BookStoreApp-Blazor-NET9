using System.ComponentModel.DataAnnotations;
namespace BookStoreAppAPI.Models
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
    }
}
