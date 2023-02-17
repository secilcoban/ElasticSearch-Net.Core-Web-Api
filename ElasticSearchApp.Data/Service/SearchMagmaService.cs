using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data
{
    public class SearchMagmaService : ISearchMagmaService
    {

        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        public SearchMagmaService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task CheckIndex(string indexName)
        {
            var anyy = await _client.Indices.ExistsAsync(indexName);
            if (anyy.Exists)
                return;

            var response = await _client.Indices.CreateAsync(indexName,
                ci => ci
                .Index(indexName)
                .SearchMagmaMapping()
                .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                ) ;

            return;
        }

        public async Task DeleteByIdDocument(string indexName, SearchMagma searchdata)
        {
            var response = await _client.CreateAsync(searchdata, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.DeleteAsync(DocumentPath<SearchMagma>.Id(searchdata.Id).Index(indexName));
            }
        }

        public async Task DeleteIndex(string indexName)
        {
            await _client.Indices.DeleteAsync(indexName);
        }

        public async Task<SearchMagma> GetDocument(string indexName, string id)
        {
            var response = await _client.GetAsync<SearchMagma>(id, q => q.Index(indexName));
            return response.Source;
        }

        public async Task<List<SearchMagma>> GetDocuments(string indexName)
        {
            // var response = await _client.SearchAsync<SearchMagma>();
            //settings.EnableApiVersioningHeader();
            var response = await _client.SearchAsync<SearchMagma>(s => s
            .From(0)
            .Take(10)
            .Index(indexName)
            .Query(q => q
            .Bool(b => b
            .Should(m => m
            .Wildcard(w => w
            .Field("SearchMetaKey")))))
          );



            #region //Fuzzy kelimeyi kendi tammalar parametrikte olabilir
            //var response2 = await _client.SearchAsync<Cities>(s => s
            //  .Index(indexName)
            //  .Query(q => q
            //  .Fuzzy(fz => fz.Field("city")
            //  .Value("anka").Fuzziness(Fuzziness.EditDistance(4)))));

            ////harflerin yer değiştirmesi
            //var response3 = await _client.SearchAsync<Cities>(s => s.Index(indexName)
            //  .Query(q => q.Fuzzy(fz => fz.Field("city")
            //    .Value("rie").Transpositions(true))));
            #endregion
            #region Matchprefix aradaki harfi kendi tamalıyor wilcard göre performans olarak daha yüksek
            //var response4 = await _client.SearchAsync<Cities>(s => s
            // .Index(indexName)
            // .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.City).Query("iz").MaxExpansions(10))));
            #endregion
            #region Multimatch çoklu büyük küçük duyarlığı olmaz
            //var response5 = await _client.SearchAsync<Cities>(s => s
            //  .Index(indexName)
            //  .Query(q => q
            //  .MultiMatch(mm => mm
            //  .Fields(f => f
            //  .Field(ff => ff.City)
            //  .Field(ff => ff.Region))
            //  .Type(TextQueryType.PhrasePrefix).Query("iz").MaxExpansions(10))));
            #endregion

            #region Term burada tamamı küçük harf olmalı
            //var response6 = await _client.SearchAsync<Cities>(s => s.Index(indexName)
            //  .Size(10000)
            //  .Query(query => query.Term(f => f.City, "rize")));

            #endregion
            #region Match büyük küçük duyarlığı olmaz
            //var response7 = await _client.SearchAsync<Cities>(s => s
            //  .Index(indexName)
            //  .Size(10000)
            //  .Query(q => q
            //  .Match(m => m.Field("city").Query("ankara"))));
            #endregion

            #region Analyzewildcard like sorgusu mantığında çalışmakta
            //var response8 = await _client.SearchAsync<Cities>(s => s
            //  .Index(indexName)
            //  .Query(q => q
            //  .QueryString(qs => qs
            //  .AnalyzeWildcard()
            //  .Query("*" + "iz" + "*")
            //  .Fields(fs => fs
            //  .Fields(f1 => f1.City
            //  )
            //  ))));
            #endregion

            if (!response.IsValid)
            {
                // Handle errors
                var debugInfo = response.DebugInformation;
                // var error = response.ServerError.Error;
            }
            return response.Documents.ToList();
        }

        public async Task<List<SearchMagma>> InsertBulkDocument(string indexName, List<SearchMagma> searchdatas)
        {

            await _client.IndexManyAsync(searchdatas, index: indexName);
            return searchdatas;
        }

        public async Task<SearchMagma> InsertDocument(string indexName, SearchMagma searchData)
        {
            var response = await _client.CreateAsync(searchData, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<SearchMagma>(searchData.Id, a => a.Index(indexName).Doc(searchData));
            }
            return searchData;
        }

        public async Task UpdateByIdDocument(string indexName, SearchMagma searchdata)
        {
            var response = await _client.CreateAsync(searchdata, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<SearchMagma>(searchdata.Id, a => a.Index(indexName).Doc(searchdata));
            }
        }


    }
}
