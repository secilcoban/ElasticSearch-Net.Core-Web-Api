using ElasticSearchApp.Data;
using ElasticSearchApp.Data.Entity;
using ElasticSearchApp.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchApp.Api.Controllers
{
    public class MuadilÜrünController : Controller
    {

        private IMuadilÜrünService _muadilUrünService;
        public MuadilÜrünController(IMuadilÜrünService muadilUrünService)
        {
            _muadilUrünService = muadilUrünService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{ürünkodu}")]
        // [HttpGet("search")]
        public async Task<ActionResult<Urun>> GetMuadilÜrün(string ürünkodu)
        {
            return await _muadilUrünService.GetMuadilÜrün(ürünkodu);
        }
    }
}
