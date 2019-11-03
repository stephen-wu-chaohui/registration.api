using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using DocuSQL.API.Models;
using DocuSQL.API.Repositories;
using System.Threading.Tasks;

namespace DocuSQL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        public IDocumentRepository Repository { get; }

        public DocumentsController(IDocumentRepository repository)
        {
            Repository = repository;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<IEnumerable<dynamic>> Get()
        {
            return (await Repository.Get()).Select(SerializeDocument);
        }

        // GET: api/Documents/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<dynamic> Get(int id)
        {
            var docs = await Repository.Get(id);
            if (docs.Count() == 0)
                return NotFound();
            return SerializeDocument(docs.First());
        }

        // POST: api/Documents
        [HttpPost]
        public async Task Post([FromBody] dynamic payLoad)
        {
            Document doc = new Document();
            var pInfo = payLoad.GetType().GetProperty("SubmissionId");
            if (pInfo != null) {
                doc.SubmissionId = pInfo.GetValue(payLoad, null);
            }

            if (JToken.FromObject(payLoad) is JObject token) {
                var properties = token.Properties().Select(p => p.Name).ToArray();
                doc.Fields = properties.Select(prop => new Field { FieldId = prop, FieldValue = GetString(token.GetValue(prop)) }).ToList();
            }

            await Repository.Create(doc);
        }

        // DELETE api/Documents/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await Repository.Delete(id);
        }

        private dynamic SerializeDocument(Document doc)
        {
            var x = new JObject();
            doc.Fields.ForEach(f => x.Add(f.FieldId, f.FieldValue));

            if (x.ContainsKey("id")) {
                x["id"] = doc.Id;
            } else {
                x.Add("id", doc.Id);
            }
            if (x.ContainsKey("submissionTime")) {
                x["submissionTime"] = doc.SubmissionTime;
            } else {
                x.Add("submissionTime", doc.SubmissionTime);
            }
            return x;
        }

        private string GetString(JToken jToken)
        {
            if (jToken is JArray) {
                return "[...]";
            } else if (jToken is JObject) {
                return "{...}";
            } else {
                string s = jToken.ToString();
                return s;
            }
        }
    }
}
