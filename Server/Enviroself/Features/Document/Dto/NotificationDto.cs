using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Dto
{
    public class NotificationDto
    {
        public int InvitationId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int Status { get; set; }
        public int DocumentId { get; set; }
        public int ActionType { get; set; }
        public string Message { get; set; }

    }
}
