using Enviroself.Features.Document.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Dto
{
    public class OtherClientDocumentIdsDto
    {
        public int DocumentId { get; set; }
        public int ClientId { get; set; }
        public InvitationActionType ActionType { get; set; }
    }
}
