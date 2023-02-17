using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data
{
    public interface ISearchMagmaService
    {
        Task CheckIndex(string indexName);
        Task<SearchMagma> InsertDocument(string indexName, SearchMagma searchData);
        Task DeleteIndex(string indexName);
        Task DeleteByIdDocument(string indexName, SearchMagma searchdata);
        Task<List<SearchMagma>> InsertBulkDocument(string indexName, List<SearchMagma> searchdatas);
        Task<SearchMagma> GetDocument(string indexName, string id);
        Task<List<SearchMagma>> GetDocuments(string indexName);
        Task UpdateByIdDocument(string indexName, SearchMagma searchdata);

        Task<List<SearchMagma>> GetSearchResults(string indexName, string key);

        ICollection<SearchMagma> GetSearchData(string indexName, string text);

        Task DeleteAllDocument(string indexName);
    }
}
