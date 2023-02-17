using ElasticSearchApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data.Interface
{
    public interface IMuadilÜrünService
    {
        Task<Urun> GetMuadilÜrün(string ürünKodu);      
    }
}
