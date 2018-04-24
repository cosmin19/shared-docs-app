using Enviroself.Features.Account.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Entities
{
    [Table("User_Document_View")]
    public class UserDocumentView
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ViewerId { get; set; }
        public int DocumentId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; }

        [ForeignKey(nameof(ViewerId))]
        public virtual User Viewer { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual Document Document { get; set; }
    }
}
