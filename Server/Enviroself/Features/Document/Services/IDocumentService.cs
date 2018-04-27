using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enviroself.Features.Account.Entities;
using Enviroself.Features.Document.Entities;
using Enviroself.Models;

namespace Enviroself.Features.Document.Services
{
    public interface IDocumentService
    {
        Task<IList<Entities.Document>> GetAllDocumentsForOwner(int ownerId);
        Task<Entities.Document> GetDocumentById(int id);
        Task<MessageDto> InsertDocument(Entities.Document document);
        Task<MessageDto> EditDocument(Entities.Document document);
        Task<MessageDto> DeleteDocument(int id);
        Task<Invitation> GetInvitationById(int invitationId);
        Task<MessageDto> CheckIfClientCanView(int clientId, int documentId);
        Task<MessageDto> CheckIfClientCanEdit(int clientId, int documentId);
        Task<MessageDto> InsertInvitation(Invitation invitation);
        Task<MessageDto> UpdateInvitation(Invitation invitation);
        Task<MessageDto> AssignClientToEditDocument(int ownerId, int editorId, int documentId);
        Task<MessageDto> AssignClientToViewDocument(int ownerId, int viewerId, int documentId);
        Task<MessageDto> RemoveClientFromViewDocument(int clientId, int documentId);
        Task<MessageDto> RemoveClientFromEditDocument(int clientId, int documentId);
        Task<IList<Entities.Document>> GetAllOtherViewDocumentsForClient(int clientId);
        Task<IList<Entities.Document>> GetAllOtherEditDocumentsForClient(int clientId);
        Task<IList<Invitation>> GetPendingInvitations(int clientId);
        Task<List<User>> GetViewersForDocument(int ownerId, int documentId);
        Task<List<User>> GetEditersForDocument(int ownerId, int documentId);
        Task<List<User>> GetAllUsers();
    }
}
