using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Subscribe
{
    public class SubscribeDTO
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int SubscriberId { get; set; }
    }
}
