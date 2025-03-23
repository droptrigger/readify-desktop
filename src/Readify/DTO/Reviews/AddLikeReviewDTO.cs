using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Reviews
{
    public class AddLikeReviewDTO
    {
        [Required]
        public int IdAuthor { get; set; }

        [Required]
        public int IdReview { get; set; }

        [Required]
        public byte ReactionType { get; set; }
    }
}
