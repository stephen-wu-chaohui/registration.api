using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DocuSQL.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DocuSQL.API.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocuSQLContext Context;

        public DocumentRepository(DocuSQLContext context)
        {
            Context = context;
        }

        public async Task<bool> Create(Document request)
        {
            await Context.Document.AddAsync(request);

            request.Fields.ForEach(async f => {
                f.DocumentId = request.Id;
                f.Document = request;
                await Context.Field.AddAsync(f);
            });

            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            try {
                var entity = (await Get(id)).FirstOrDefault();
                if (entity == null) {
                    return false;
                }
                Context.Field.RemoveRange(entity.Fields);
                Context.Document.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Document>> Get(params int[] ids)
        {
            if (ids.Length == 0) {
                return Context.Document.Include(b => b.Fields);
            } else {
                return Context.Document.Where(doc => ids.Contains(doc.Id)).Include(b => b.Fields);
            }
        }
    }
}
