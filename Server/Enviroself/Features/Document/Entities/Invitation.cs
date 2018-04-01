using Enviroself.Features.Account.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Entities
{
    [Table("Invitation")]
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int Status { get; set; }

        [ForeignKey(nameof(FromUserId))]
        public virtual User FromUser { get; set; }

        [ForeignKey(nameof(ToUserId))]
        public virtual User ToUser { get; set; }
    }
}
