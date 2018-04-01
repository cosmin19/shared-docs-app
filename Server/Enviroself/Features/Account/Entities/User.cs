using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Account.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }


        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Role { get; set; }
        public bool IsActivated { get; set; }


        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public bool Valid { get; set; }
    }
}