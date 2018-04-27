using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enviroself.Features.Account.Entities;
using Enviroself.Features.Document.Dto;
using Enviroself.Features.Document.Entities;
using Enviroself.Features.Document.Services;
using Enviroself.Models;
using Enviroself.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enviroself.Features.Document
{
    [Route("api/Document")]
    [Authorize]
    public class DocumentController : Controller
    {
        #region Fields
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;
        #endregion

        #region Ctor
        public DocumentController(IDocumentService documentService, IIdentityService identityService)
        {
            this._documentService = documentService;
            this._identityService = identityService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var result = await _documentService.GetAllDocumentsForOwner(currentUser.Id);
            var model = new List<DocumentMiniInfo>();
            foreach (var item in result)
            {
                model.Add(new DocumentMiniInfo() {
                    Id = item.Id,
                    Owner = item.Owner.FirstName + " " + item.Owner.LastName,
                    Subject = item.Subject,
                    Title = item.Title,
                    CreatedOnUtc = item.CreatedOnUtc.ToShortDateString()
                });
            }

            return new OkObjectResult(model);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetDocument([FromQuery(Name = "id")] int id)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var entity = await _documentService.GetDocumentById(id);

            MessageDto check = await _documentService.CheckIfClientCanView(currentUser.Id, entity.Id);

            if (!check.Success)
                return BadRequest(new MessageDto() { Success = true, Message = "You can view this document." });

            var model = new DocumentInfoDto()
            {
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                Subject = entity.Subject,
                Title = entity.Title,
                CreatedOnUtc = entity.CreatedOnUtc.ToShortDateString(),
                UpdatedOnUtc = entity.UpdatedOnUtc != null ? entity.UpdatedOnUtc.Value.ToShortDateString() : "",
                Owner = entity.Owner.FirstName + " " + entity.Owner.LastName,
                IsOwnerDocument = entity.OwnerId == currentUser.Id ? true : false
            };
            return new OkObjectResult(model);
        }

        [HttpGet("getviewersfordocument")]
        public async Task<IActionResult> GetViewersForDocument([FromQuery(Name = "documentId")] int documentId)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(documentId);
            if(document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid request" });

            List<User> users = await _documentService.GetViewersForDocument(currentUser.Id, documentId);

            var result = new List<ClientDto>();
            foreach(var user in users)
            {
                result.Add(new ClientDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Username = user.UserName
                });
            }

            return new OkObjectResult(result);
        }

        [HttpGet("geteditersfordocument")]
        public async Task<IActionResult> GetEditersForDocument([FromQuery(Name = "documentId")] int documentId)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(documentId);
            if (document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid request" });

            List<User> users = await _documentService.GetEditersForDocument(currentUser.Id, documentId);

            var result = new List<ClientDto>();
            foreach (var user in users)
            {
                result.Add(new ClientDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Username = user.UserName
                });
            }

            return new OkObjectResult(result);
        }


        [HttpPut]
        public async Task<IActionResult> AddDocument([FromBody]DocumentNewDto model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

                if (currentUser == null)
                    return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

                var document = new Entities.Document()
                {
                    OwnerId = currentUser.Id,
                    Subject = model.Subject,
                    Title = model.Title,
                    CreatedOnUtc = DateTime.Now
                };
                var result = await _documentService.InsertDocument(document);
                if (result.Success)
                {
                    await _documentService.AssignClientToEditDocument(currentUser.Id, currentUser.Id, document.Id);
                    await _documentService.AssignClientToViewDocument(currentUser.Id, currentUser.Id, document.Id);
                    return new OkObjectResult(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest(new MessageDto() { Success = false, Message = "Invalid document" });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditDocument([FromBody] DocumentEditDto model)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });
            if (ModelState.IsValid)
            {
                var document = await _documentService.GetDocumentById(model.Id);

                if (document == null)
                    return BadRequest(new MessageDto() { Success = false, Message = "Bad request!" });

                MessageDto check = await _documentService.CheckIfClientCanEdit(currentUser.Id, document.Id);

                if (!check.Success)
                    return BadRequest(new MessageDto() { Success = false, Message = "You can't edit this document." });

                document.Subject = model.Subject;
                document.Title = model.Title;

                var result = await _documentService.EditDocument(document);
                if (result.Success)
                    return new OkObjectResult(result);
                return BadRequest(result);
            }
            return BadRequest(new MessageDto() { Success = false, Message = "Bad request!" });

        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteDocument([FromQuery(Name = "id")] int id)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(id);


            if (document == null ||  document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = false, Message = "Bad request!" });

            var result = await _documentService.DeleteDocument(id);
            if (result.Success)
                return new OkObjectResult(result);
            return BadRequest(result);
        }

        [HttpGet("listdocumentsview")]
        public async Task<IActionResult> ListDocumentsView()
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            IList<Entities.Document> result = await _documentService.GetAllOtherViewDocumentsForClient(currentUser.Id);
            var model = new List<DocumentMiniInfo>();
            foreach (var item in result)
            {
                model.Add(new DocumentMiniInfo()
                {
                    Id = item.Id,
                    Owner = item.Owner.FirstName + " " + item.Owner.LastName,
                    Subject = item.Subject,
                    Title = item.Title,
                    CreatedOnUtc = item.CreatedOnUtc.ToShortDateString()
                });
            }

            return new OkObjectResult(model);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications()
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            IList<Invitation> result = await _documentService.GetPendingInvitations(currentUser.Id);
            var model = new List<NotificationDto>();
            foreach (var item in result)
            {
                model.Add(new NotificationDto()
                {
                    InvitationId = item.Id,
                    FromUserId = item.FromUserId,
                    ToUserId = item.ToUserId,
                    Message = item.Message,
                    ActionType = item.ActionType,
                    DocumentId = item.DocumentId,
                    Status = item.Status
                });
            }

            return new OkObjectResult(model);
        }

        [HttpGet("listdocumentsedit")]
        public async Task<IActionResult> ListDocumentsEdit()
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            IList<Entities.Document> result = await _documentService.GetAllOtherEditDocumentsForClient(currentUser.Id);
            var model = new List<DocumentMiniInfo>();
            foreach (var item in result)
            {
                model.Add(new DocumentMiniInfo()
                {
                    Id = item.Id,
                    Owner = item.Owner.FirstName + " " + item.Owner.LastName,
                    Subject = item.Subject,
                    Title = item.Title,
                    CreatedOnUtc = item.CreatedOnUtc.ToShortDateString()
                });
            }

            return new OkObjectResult(model);
        }

        [HttpPost("inviteclient")]
        public async Task<IActionResult> InviteClient([FromBody] OtherClientDocumentIdsDto model)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(model.DocumentId);
            if (document == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid document" });

            if (document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid permission" });

            if (model.ClientId == currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "You can't invite yourself" });

            string message = "You have been invited to " + (model.ActionType == InvitationActionType.View ? "VIEW" : "EDIT") + " document: " + document.Title;

            var invitation = new Invitation()
            {
                DocumentId = document.Id,
                ActionType = (int)model.ActionType,
                Status = (int)InvitationStatus.Pending,
                FromUserId = currentUser.Id,
                ToUserId = model.ClientId,
                Message = message
            };
            MessageDto result = await _documentService.InsertInvitation(invitation);
            if (result.Success)
                return new OkObjectResult(result);
            return BadRequest(result);
        }

        [HttpPost("acceptinvitation")]
        public async Task<IActionResult> AcceptInvitation([FromBody] InvitationDto model)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid user." });

            var invitation = await _documentService.GetInvitationById(model.InvitationId);
            if (invitation == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid invitation." });

            if(invitation.ToUserId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = false, Message = "Invitation is not for current user." });

            invitation.Status = (int)InvitationStatus.Accepted;

            MessageDto result = await _documentService.UpdateInvitation(invitation);
            if (result.Success)
            {
                MessageDto result2 = new MessageDto();

                if (invitation.ActionType == (int)InvitationActionType.Edit)
                {
                    result2 = await _documentService.AssignClientToViewDocument(invitation.Document.OwnerId, currentUser.Id, invitation.DocumentId);
                    result2 = await _documentService.AssignClientToEditDocument(invitation.Document.OwnerId, currentUser.Id, invitation.DocumentId);
                }
                else if (invitation.ActionType == (int)InvitationActionType.View)
                    result2 = await _documentService.AssignClientToViewDocument(invitation.Document.OwnerId, currentUser.Id, invitation.DocumentId);

                if (result2.Success)
                    return new OkObjectResult(result2);
                return BadRequest(result2);
            }
            return BadRequest(result);
        }

        [HttpPost("kickviewdocument")]
        public async Task<IActionResult> KickViewDocument([FromBody] OtherClientDocumentIdsDto model)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(model.DocumentId);
            if (document == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid document" });

            if (document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid permission" });

            if (model.ClientId == currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "You can't kick yourself" });

            MessageDto result = await _documentService.RemoveClientFromViewDocument(model.ClientId, model.DocumentId);
            if (result.Success)
                return new OkObjectResult(result);
            return BadRequest(result);
        }

        [HttpPost("kickeditdocument")]
        public async Task<IActionResult> KickEditDocument([FromBody] OtherClientDocumentIdsDto model)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid user" });

            var document = await _documentService.GetDocumentById(model.DocumentId);
            if (document == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid document" });

            if(document.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid permission" });

            if(model.ClientId == currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "You can't kick yourself" });

            MessageDto result = await _documentService.RemoveClientFromEditDocument(model.ClientId, model.DocumentId);
            if (result.Success)
                return new OkObjectResult(result);
            return BadRequest(result);
        }


        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid user" });

            var users = await _documentService.GetAllUsers();

            var result = users.Where(c => c.Id != currentUser.Id).Select(c => new ClientRecordDto()
            {
                Id = c.Id,
                Email = c.Email,
                Username = c.UserName
            }).ToList();

            return new OkObjectResult(result);
        }
        #endregion
    }
}
