using ElasticSearchApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearchApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HomeController : ControllerBase
    {
        private IElasticsearchService _elasticsearchService;
        public HomeController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            //  await InsertFulData();
            // await _elasticsearchService.InsertDocument("cities", new Cities { City = "Bolu", CreateDate = System.DateTime.Now, Id = "779186ff-434f-4dab-bd6c-77c45a2112e2", Population = 50000, Region = "Karadeniz" });
            //await _elasticsearchService.DeleteByIdDocument("cities", new Cities { City = "Bolu", CreateDate = System.DateTime.Now, Id = "3ab59244-dad6-4422-83a5-72a5e5f9e5f2", Population = 50000, Region = "Karadeniz" });

            // await _elasticsearchService.InsertDocument("cities", new Cities { City = "Eskişehir", CreateDate = DateTime.Now, Id = Guid.NewGuid().ToString(), Population = 50000, Region = "İç Anadolu" });
            // await _elasticsearchService.DeleteIndex("cities");

            await _elasticsearchService.GetDocuments("cities");
            return Ok();
        }
        private async Task InsertFulData()
        {
            List<Cities> citiesList = new List<Cities>()
            {
                new Cities{City="Ankara",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=50000,Region="İç Anadolu"},
                 new Cities{City="İzmir",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=30500,Region="Ege"},
                  new Cities{City="Aydın",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=65000,Region="Ege"},
                   new Cities{City="Rize",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=36522,Region="Karadeniz"},
                    new Cities{City="İstanbul",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=25620,Region="Marmara"},
                     new Cities{City="Sinop",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=50669,Region="Karadeniz"},
                      new Cities{City="Kars",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=55500,Region="Doğu Anadolu"},
                       new Cities{City="Van",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=55500,Region="Doğu Anadolu"},
                        new Cities{City="Adıyaman",CreateDate=DateTime.Now,Id=Guid.NewGuid().ToString(),Population=555000,Region="Güneydoğu Anadolu"}
            };
            await _elasticsearchService.InsertBulkDocument("cities", citiesList);

        }

        //string indexName = "searchdata";
        //private readonly ISearchMagmaService _searchMagmaService;
        //public HomeController(ISearchMagmaService searchMagmaService)
        //{
        //    _searchMagmaService = searchMagmaService;
        //}

        //[HttpGet]
        //public async Task<IEnumerable<SearchMagma>> GetSearchMagma()
        //{
        //    return await _searchMagmaService.GetDocuments(indexName);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<SearchMagma>> GetSearchMagma(int id)
        //{
        //    return await _searchMagmaService.GetDocument(indexName, id.ToString());
        //}

        //[HttpPost]
        //public async Task<ActionResult<SearchMagma>> PostSearchMagma([FromBody] SearchMagma searchMagma)
        //{
        //    var newSearchData = await _searchMagmaService.InsertDocument(indexName, searchMagma);

        //    return CreatedAtAction(nameof(GetSearchMagma), new { id = newSearchData.Id }, newSearchData);
        //}

        //[HttpPut]
        //public async Task<ActionResult> PutSearchMagma(int id, [FromBody] SearchMagma searchMagma)
        //{
        //    if (id != searchMagma.Id)
        //    {
        //        return BadRequest();
        //    }
        //    await _searchMagmaService.UpdateByIdDocument(indexName, searchMagma);
        //    return NoContent();
        //}


        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var searchDataDelete = await _searchMagmaService.GetDocument(indexName, id.ToString());
        //    if (searchDataDelete == null)
        //        return NotFound();
        //    await _searchMagmaService.DeleteByIdDocument(indexName, searchDataDelete);
        //    return NoContent();
        //}


        //private async Task InsertFulData(List<SearchMagma> searchData)
        //{

        //    await _searchMagmaService.InsertBulkDocument(indexName, searchData);

        //}

        //[HttpPost]
        //public async Task<ActionResult<SearchMagma>> PostSearchMagmaFull([FromBody] List<SearchMagma> searchMagmaList)
        //{
        //    await _searchMagmaService.DeleteIndex(indexName);
        //    var newSearchData = await _searchMagmaService.InsertBulkDocument(indexName, searchMagmaList);
        //    return Ok();
        //}


    }
}
