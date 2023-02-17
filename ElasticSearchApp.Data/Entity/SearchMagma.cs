using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp.Data
{
    public class SearchMagma
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public string ErpKod { get; set; }
        public string Name { get; set; }
        public string ModelAdi { get; set; }
        public string ProductName { get; set; }
        public string Dok { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> IsSale { get; set; }
        public Nullable<System.Guid> LanguageId { get; set; }
        public Nullable<System.Guid> ProductGroupId { get; set; }
        public string Dimension { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string MediaList { get; set; }
        public string CategoryList { get; set; }
        public string SortOrder { get; set; }
        public string SearchMetaKey { get; set; }
        public Nullable<System.DateTimeOffset> CreateDate { get; set; }
        public Nullable<int> Counter { get; set; }
    }
}
