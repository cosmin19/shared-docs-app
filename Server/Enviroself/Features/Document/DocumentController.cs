using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enviroself.Features.Document.Dto;
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
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;
        #region Fields

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
            /* Mapeaza entity la dto */

            return new OkObjectResult(model);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetDocument([FromQuery(Name = "id")] int id)
        {
            var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

            if (currentUser == null)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var entity = await _documentService.GetDocumentById(id);

            if(entity.OwnerId != currentUser.Id)
                return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

            var model = new DocumentInfoDto()
            {
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                Subject = entity.Subject,
                Title = entity.Title,
                CreatedOnUtc = entity.CreatedOnUtc.ToShortDateString(),
                UpdatedOnUtc = entity.UpdatedOnUtc != null ? entity.UpdatedOnUtc.Value.ToShortDateString() : "",
                Owner = entity.Owner.FirstName + " " + entity.Owner.LastName
            };
            return new OkObjectResult(model);
        }

        [HttpPut]
        public async Task<IActionResult> AddDocument([FromBody]DocumentNewDto model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _identityService.GetCurrentPersonIdentityAsync();

                if (currentUser == null)
                    return BadRequest(new MessageDto() { Success = true, Message = "Invalid user" });

                var entity = new Entities.Document()
                {
                    OwnerId = currentUser.Id,
                    Subject = model.Subject,
                    Title = model.Title,
                    CreatedOnUtc = DateTime.Now
                };
                var result = await _documentService.InsertDocument(entity);
                if (result.Success)
                    return new OkObjectResult(result);
                else
                    return BadRequest(result);
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


                if (document == null || document.OwnerId != currentUser.Id)
                    return BadRequest(new MessageDto() { Success = false, Message = "Bad request!" });

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

        #endregion
    }
}
