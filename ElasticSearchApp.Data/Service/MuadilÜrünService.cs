using ElasticSearchApp.Data.Entity;
using ElasticSearchApp.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data.Service
{
    public class MuadilÜrünService : IMuadilÜrünService
    {

        public async Task<Urun> GetMuadilÜrün(string ürünKodu)
        {
            Urun _ürün = new Urun();
            _ürün.BagliUrunKodu = "23cc45";
            _ürün.Description = "Deneme Kaydı";
            _ürün.Id = 1;
            return _ürün;
        }
    }
}
