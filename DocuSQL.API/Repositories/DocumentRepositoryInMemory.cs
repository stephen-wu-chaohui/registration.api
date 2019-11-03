using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocuSQL.API.Models;

namespace DocuSQL.API.Repositories
{
    public class DocumentRepositoryInMemory : IDocumentRepository
    {
        readonly IList<Document> Documents = new List<Document>();
        readonly IList<Field> Fields = new List<Field>();

        public async Task<bool> Create(Document request)
        {
            request.Id = Documents.Count == 0 ? 1 : Documents.Max(d => d.Id) + 1;
            request.SubmissionTime = DateTime.UtcNow;
            Documents.Add(request);

            request.Fields.ForEach(f => {
                f.Id = Fields.Count == 0 ? 1 : Fields.Max(ff => ff.Id) + 1;
                f.DocumentId = request.Id;
                f.Document = request;
                Fields.Add(f);
            });
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var doc = Documents.FirstOrDefault(d => d.Id == id);
            if (doc != null) {
                Documents.Remove(doc);
                var toDeleted = Fields.Where(x => x.DocumentId == id).ToList();
                toDeleted.ForEach(td => Fields.Remove(td));
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Document>> Get(params int[] ids)
        {
            if (ids.Length == 0) {
                return Documents;
            }
            return ids.Select(id => Documents.FirstOrDefault(d => d.Id == id)).Where(y => y != null);
        }
    }
}
