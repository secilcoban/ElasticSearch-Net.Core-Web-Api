using ElasticSearchApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchMagmaController : ControllerBase
    {
        string indexName = "searchdata";
        private readonly ISearchMagmaService _searchMagmaService;
        public SearchMagmaController(ISearchMagmaService searchMagmaService)
        {
            _searchMagmaService = searchMagmaService;
        }

        // [HttpGet]
        [HttpGet("")]
        public async Task<IEnumerable<SearchMagma>> GetSearchMagma()
        {
            return await _searchMagmaService.GetDocuments(indexName);
        }

        [HttpGet("{id}")]
        // [HttpGet("search")]
        public async Task<ActionResult<SearchMagma>> GetSearchMagma(int id)
        {
            return await _searchMagmaService.GetDocument(indexName, id.ToString());
        }



        [HttpPut]
        public async Task<ActionResult> PutSearchMagma(int id, [FromBody] SearchMagma searchMagma)
        {
            if (id != searchMagma.Id)
            {
                return BadRequest();
            }
            await _searchMagmaService.UpdateByIdDocument(indexName, searchMagma);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var searchDataDelete = await _searchMagmaService.GetDocument(indexName, id.ToString());
            if (searchDataDelete == null)
                return NotFound();
            await _searchMagmaService.DeleteByIdDocument(indexName, searchDataDelete);
            return NoContent();
        }


        private async Task InsertFulData(List<SearchMagma> searchData)
        {
            await _searchMagmaService.CheckIndex(indexName);
            await _searchMagmaService.InsertBulkDocument(indexName, searchData);

        }

        [HttpPost]
        public async Task PostSearchMagmaFull([FromBody] List<SearchMagma> searchMagmaList)
        {
            await _searchMagmaService.CheckIndex(indexName);
            var newSearchData = await _searchMagmaService.InsertBulkDocument(indexName, searchMagmaList);
            // return Ok();
        }

        //[HttpPost]
        //public async Task<ActionResult<SearchMagma>> PostSearchMagma([FromBody] SearchMagma searchMagma)
        //{
        //    var newSearchData = await _searchMagmaService.InsertDocument(indexName, searchMagma);

        //    return CreatedAtAction(nameof(GetSearchMagma), new { id = newSearchData.Id }, newSearchData);
        //}
    }
}
