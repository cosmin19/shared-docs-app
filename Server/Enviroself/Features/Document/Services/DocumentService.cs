using Enviroself.Context;
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

        public virtual async Task<Entities.Document> GetDocumentById(int id)
        {
            var result = await _context.Documents.Where(c => c.Id == id).Include(c => c.Owner).FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<MessageDto> InsertDocument(Entities.Document document)
        {
            if(document == null)
                return new MessageDto() { Success = false, Message = "Error." };

            await _context.Documents.AddAsync(document);

            /* Userul ce a creat documentul este by default si Viewer si Editor */
            await _context.UserDocumentViews.AddAsync(new UserDocumentView()
            {
                DocumentId = document.Id,
                OwnerId = document.OwnerId,
                ViewerId = document.OwnerId,
            });

            await _context.UserDocumentEdits.AddAsync(new UserDocumentEdit()
            {
                DocumentId = document.Id,
                OwnerId = document.OwnerId,
                EditorId = document.OwnerId,
            });

            await _context.SaveChangesAsync();
            return new MessageDto() { Success = true, Message = "Document Inserted." };
        }
    }
}
