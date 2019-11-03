using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocuSQL.API.Models;

namespace DocuSQL.API.Repositories
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> Get(params Int32[] ids);
        Task<bool> Create(Document request);
        Task<bool> Delete(Int32 id);
    }
}
