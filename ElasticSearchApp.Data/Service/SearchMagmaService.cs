using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
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
            var settings = new ConnectionSettings(new Uri(host + ":" + port)).RequestTimeout(TimeSpan.FromHours(3));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task CheckIndex(string indexName)
        {
            var anyy = await _client.Indices.ExistsAsync(indexName);
            if (anyy.Exists)
                return;

            //var response = await _client.Indices.CreateAsync(indexName,
            //    ci => ci
            //    .Index(indexName)
            //    .SearchMagmaMapping()
            //    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
            //    );

            var response = await _client.Indices.CreateAsync(indexName,
              ci => ci
              .Index(indexName)
              // .SearchMagmaMapping()
              .Settings(s => s.Analysis(a => a.Normalizers(n => n.Custom("case_insensitive", c => c.Filters("lowercase"))))).SearchMagmaMapping()
              );
            //  .Normalizers(norm => norm.Custom("lowercase", cn => cn.Filters("lowercase"));
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
            var response = await _client.SearchAsync<SearchMagma>(s => s.Index(indexName).Query(q => q.MatchAll()));
            //  var response2 = await _client.SearchAsync<SearchMagma>(s => s
            //  .From(0)
            //  .Index(indexName)
            //  .Query(q => q
            //  .Bool(b => b
            //  .Should(m => m
            //  .Wildcard(w => w
            //  .Field("SearchMetaKey")))))
            //);



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
            var cc = response.Documents.Count();
            return response.Documents.ToList();
        }

        public async Task<List<SearchMagma>> InsertBulkDocument(string indexName, List<SearchMagma> searchdatas)
        {
            await DeleteAllDocument(indexName);
            await _client.IndexManyAsync(searchdatas, index: indexName);
            //var asyncBulkIndexResponse = await _client.BulkAsync(b => b
            //   .Index(indexName)
            //   .IndexMany(searchdatas));

            //if (!asyncBulkIndexResponse.IsValid)
            //    return null;
            //return searchdatas;
            //var waitHandle = new CountdownEvent(1);

            //var bulk = _client.BulkAll(searchdatas.ToList(), a => a
            //                .Index(indexName)
            //                .BackOffRetries(2)
            //                .BackOffTime("30s")
            //                .RefreshOnCompleted(true)
            //                .MaxDegreeOfParallelism(4)
            //                .Size(searchdatas.Count()));

            //bulk.Subscribe(new BulkAllObserver(
            //                onNext: response => response.Retries.ToString(),
            //                onError: null,
            //                onCompleted: () => waitHandle.Signal()
            //            ));

            //waitHandle.Wait();


            //            var bulkAllObservable = _client.BulkAll(searchdatas, b => b
            //                                    .Index(indexName)
            //                                    .BackOffTime("30s")
            //                                    .BackOffRetries(2)
            //                                    .RefreshOnCompleted()
            //                                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
            //                                  .Size(searchdatas.Count()))
            //)
            //                    .Wait(TimeSpan.FromMinutes(15), next =>
            //                    {
            //                        // do something e.g. write number of pages to console
            //                    });


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

        public async Task<List<SearchMagma>> GetSearchResults(string indexName, string key)
        {
            var response = await _client.SearchAsync<SearchMagma>(s => s
              .Index(indexName)
              .From(0)
              .Size(2000)
              .Query(q => q
              .QueryString(qs => qs
              .AnalyzeWildcard()
              .Query("*" + key + "*")
             .Fields(fs => fs
             .Fields(f1 => f1.SearchMetaKey
             )
              ))));


            //var response = await _client.SearchAsync<SearchMagma>(s => s
            //   .Index(indexName)
            //   .From(0)
            //   .Size(2000)
            //   .Query(q => q
            //   .Bool(b => b
            //   .Should(m => m
            //   .Wildcard(c => c
            //   .Field("SearchMetaKey").Value("*" + key + "*")
            //   )))));

            //var response2 = await _client.SearchAsync<SearchMagma>(s => s
            // .Index(indexName)
            // .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.SearchMetaKey).Query("*" + key + "*").MaxExpansions(10))));


            //var response1 = await _client.SearchAsync<SearchMagma>(s => s
            //.Index(indexName)
            //.Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.SearchMetaKey.Suffix("keyword_lowercase")).Query("*" + key + "*").MaxExpansions(10))));

            //var response3 = await _client.SearchAsync<SearchMagma>(p => p
            //.Index(indexName)
            //    .Query(q => q
            //        .Match(m => m
            //            .Field(f => f.SearchMetaKey)
            //                .Query(key)
            //                .Operator(Operator.Or)
            //                )
            //        )
            //        .Sort(s => s.Descending(f => f.CreateDate))
            //);


            return response.Documents.ToList();
        }



        public ICollection<SearchMagma> GetSearchData(string indexName, string text)
        {
            var response = _client.SearchAsync<SearchMagma>(p => p
               .Query(q => q
                   .Match(m => m
                       .Field(f => f.SearchMetaKey)
                           .Query(text)
                           .Operator(Operator.Or)
                           )
                   )
                   .Sort(s => s.Descending(f => f.CreateDate))
           );

            return (ICollection<SearchMagma>)response;
        }

        public async Task DeleteAllDocument(string indexName)
        {
            await CheckIndex(indexName);
            await _client.DeleteByQueryAsync<SearchMagma>(s => s.Index(indexName).Query(q => q.MatchAll()));

        }
    }
}
