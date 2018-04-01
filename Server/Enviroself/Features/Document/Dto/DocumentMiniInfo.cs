using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Dto
{
    public class DocumentMiniInfo
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string CreatedOnUtc { get; set; }

    }
}
