using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readify.DTO.Reviews
{
    public class UpdateReviewDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(2000, ErrorMessage = "Максимальное количество символов: 2000")]
        public string Comment { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public byte Rating { get; set; }
    }
}
