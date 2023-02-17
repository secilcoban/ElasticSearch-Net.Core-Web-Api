using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data.Entity
{
    public class Urun
    {
        public int Id { get; set; }
        public string BagliUrunKodu { get; set; }
        public string Description { get; set; }
    }
}
