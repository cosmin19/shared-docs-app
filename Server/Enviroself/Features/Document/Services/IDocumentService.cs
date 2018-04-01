using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
