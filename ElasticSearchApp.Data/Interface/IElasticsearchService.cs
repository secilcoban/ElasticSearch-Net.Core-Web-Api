using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data
{
    public interface IElasticsearchService
    {
        Task CheckIndex(string indexName);
        Task InsertDocument(string indexName, Cities cities);
        Task DeleteIndex(string indexName);
        Task DeleteByIdDocument(string indexName, Cities cities);
        Task InsertBulkDocument(string indexName, List<Cities> cities);
        Task<Cities> GetDocument(string indexName, string id);
        Task<List<Cities>> GetDocuments(string indexName);

    }
}
