using Enviroself.Context;
using Enviroself.Features.Account.Entities;
using Enviroself.Features.Document.Entities;
using Enviroself.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Document.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DbApplicationContext _context;

        public DocumentService(DbApplicationContext context)
        {
            this._context = context;
        }

        public virtual async Task<MessageDto> AssignClientToEditDocument(int ownerId, int editorId, int documentId)
        {
            await _context.UserDocumentEdits.AddAsync(new UserDocumentEdit()
            {
                DocumentId = documentId,
                OwnerId = ownerId,
                EditorId = editorId,
            });
            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Success" };
        }

        public virtual async Task<MessageDto> AssignClientToViewDocument(int ownerId, int viewerId, int documentId)
        {
            /* Userul ce a creat documentul este by default si Viewer si Editor */
            await _context.UserDocumentViews.AddAsync(new UserDocumentView()
            {
                DocumentId = documentId,
                OwnerId = ownerId,
                ViewerId = viewerId,
            });
            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Success" };
        }

        public virtual async Task<MessageDto> CheckIfClientCanEdit(int clientId, int documentId)
        {
            var result = await _context.UserDocumentEdits.Where(c => (c.EditorId == clientId || c.OwnerId == clientId) && c.DocumentId == documentId).FirstOrDefaultAsync();
            if(result != null)
                return new MessageDto() { Success = true, Message = "Success" };
            else
                return new MessageDto() { Success = false, Message = "Error" };
        }

        public virtual async Task<MessageDto> CheckIfClientCanView(int clientId, int documentId)
        {
            var result = await _context.UserDocumentViews.Where(c => (c.ViewerId == clientId || c.OwnerId == clientId) && c.DocumentId == documentId).FirstOrDefaultAsync();
            if (result != null)
                return new MessageDto() { Success = true, Message = "Success" };
            else
                return new MessageDto() { Success = false, Message = "Error" };
        }

        public virtual async Task<MessageDto> DeleteDocument(int id)
        {
            var document = await _context.Documents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
                return new MessageDto() { Success = true, Message = "Document Deleted." };
            }
            return new MessageDto() { Success = false, Message = "Error." };
        }

        public virtual async Task<MessageDto> EditDocument(Entities.Document document)
        {
            if(document == null)
                return new MessageDto() { Success = false, Message = "Error." };

            var entity = await _context.Documents.Where(c => c.Id == document.Id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Title = document.Title;
                entity.Subject = document.Subject;
                entity.UpdatedOnUtc = DateTime.Now;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return new MessageDto() { Success = true, Message = "Document Updated." };
            }
            return new MessageDto() { Success = false, Message = "Error." };
        }

        public virtual async Task<IList<Entities.Document>> GetAllDocumentsForOwner(int ownerId)
        {
            var result = await _context.Documents.Where(c => c.OwnerId == ownerId).Include(c => c.Owner).OrderByDescending(c => c.CreatedOnUtc).ToListAsync();

            return result;
        }

        public virtual async Task<IList<Entities.Document>> GetAllOtherEditDocumentsForClient(int clientId)
        {
            var documentIds = await _context.UserDocumentEdits.Where(c => c.EditorId == clientId && c.OwnerId != clientId).Select(c => c.DocumentId).ToListAsync();
            List<Entities.Document> result = new List<Entities.Document>();

            foreach(var id in documentIds)
            {
                var document = await GetDocumentById(id);
                if (document != null)
                    result.Add(document);
            }

            return result;
        }

        public virtual async Task<IList<Entities.Document>> GetAllOtherViewDocumentsForClient(int clientId)
        {
            var documentIds = await _context.UserDocumentViews.Where(c => c.ViewerId == clientId && c.OwnerId != clientId ).Select(c => c.DocumentId).ToListAsync();
            List<Entities.Document> result = new List<Entities.Document>();

            foreach (var id in documentIds)
            {
                var document = await GetDocumentById(id);
                if (document != null)
                    result.Add(document);
            }

            return result;
        }

        public virtual async Task<Entities.Document> GetDocumentById(int id)
        {
            var result = await _context.Documents.Where(c => c.Id == id).Include(c => c.Owner).FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<List<User>> GetEditersForDocument(int ownerId, int documentId)
        {
            return await _context.UserDocumentEdits.Where(c => c.EditorId != ownerId && c.DocumentId == documentId).Include(c => c.Editor).Select(c => c.Editor).ToListAsync();
        }

        public virtual async Task<Invitation> GetInvitationById(int invitationId)
        {
            return await _context.Invitations.Where(c => c.Id == invitationId)
                        .Include(c => c.Document)
                        .Include(c => c.ToUser)
                        .Include(c => c.FromUser)
                        .FirstOrDefaultAsync();
        }

        public virtual async Task<IList<Invitation>> GetPendingInvitations(int clientId)
        {
            return  await _context.Invitations.Where(c => c.ToUserId == clientId && c.Status == (int)InvitationStatus.Pending).ToListAsync();
        }

        public virtual async Task<List<User>> GetViewersForDocument(int ownerId, int documentId)
        {
            return await _context.UserDocumentViews.Where(c => c.ViewerId != ownerId && c.DocumentId == documentId).Include(c => c.Viewer).Select(c => c.Viewer).ToListAsync();
        }

        public virtual async Task<MessageDto> InsertDocument(Entities.Document document)
        {
            if(document == null)
                return new MessageDto() { Success = false, Message = "Error." };

            await _context.Documents.AddAsync(document);

            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Document Inserted." };
        }

        public virtual async Task<MessageDto> InsertInvitation(Invitation invitation)
        {
            if (invitation == null)
                return new MessageDto() { Success = false, Message = "Error." };

            await _context.Invitations.AddAsync(invitation);

            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Document Inserted." };
        }

        public virtual async Task<MessageDto> RemoveClientFromEditDocument(int clientId, int documentId)
        {
            var result = await _context.UserDocumentEdits.Where(c => c.EditorId == clientId && c.DocumentId == documentId && c.OwnerId != clientId).FirstOrDefaultAsync();
            if(result != null)
                _context.UserDocumentEdits.Remove(result);
            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Succesfully removed." };
        }

        public virtual async Task<MessageDto> RemoveClientFromViewDocument(int clientId, int documentId)
        {
            var result = await _context.UserDocumentViews.Where(c => c.ViewerId == clientId && c.DocumentId == documentId && c.OwnerId != clientId).FirstOrDefaultAsync();
            if (result != null)
            {
                _context.UserDocumentViews.Remove(result);
                await RemoveClientFromEditDocument(clientId, documentId);
            }
            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Succesfully removed." };
        }

        public virtual async Task<MessageDto> UpdateInvitation(Invitation invitation)
        {
            if (invitation == null)
                return new MessageDto() { Success = false, Message = "Error." };

            var entity = await _context.Invitations.Where(c => c.Id == invitation.Id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = invitation.Status;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return new MessageDto() { Success = true, Message = "Success." };
            }
            return new MessageDto() { Success = false, Message = "Error." };
        }
    }
}
