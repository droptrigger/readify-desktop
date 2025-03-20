using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readify.DTO.Library
{
    public class AddBookmarkDTO
    {
        [Required]
        public int Page {  get; set; }

        [Required] 
        public int IdLibrary { get; set; }

        [Required]
        public string Comment { get; set; } = null!;
    }
}
