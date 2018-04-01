using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Dto
{
    public class DocumentNewDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
