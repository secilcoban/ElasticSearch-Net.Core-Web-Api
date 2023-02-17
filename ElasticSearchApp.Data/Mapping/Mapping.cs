using Nest;

namespace ElasticSearchApp.Data
{
    public static class Mapping
    {
        public static CreateIndexDescriptor CitiesMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Cities>(m => m.Properties(p => p
              .Keyword(k => k.Name(n => n.Id))
              .Text(t => t.Name(n => n.City))
              .Text(t => t.Name(n => n.Region))
              .Number(t => t.Name(n => n.Population))
              .Date(t => t.Name(n => n.CreateDate)))
            );
        }


        public static CreateIndexDescriptor SearchMagmaMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<SearchMagma>(m => m.AutoMap().Properties(p => p
              .Keyword(k => k.Name(n => n.Id))
              .Text(t => t.Name(n => n.SearchMetaKey).Fields(f => f.Keyword(k => k.Name("keyword").IgnoreAbove(256)).Keyword(k => k.Name("keyword_lowercase").Normalizer("lowercase").IgnoreAbove(256))))
              .Text(t => t.Name(n => n.ErpKod))
              .Text(t => t.Name(n => n.Name))
              .Text(t => t.Name(n => n.ModelAdi))
              .Boolean(t => t.Name(n => n.IsSale))
              .Number(t => t.Name(n => n.Id))
              .Date(t => t.Name(n => n.CreateDate))
              .Text(t => t.Name(n => n.ProductId))
              .Text(t => t.Name(n => n.ProductName))
              .Text(t => t.Name(n => n.Dok))
              .Text(t => t.Name(n => n.CompanyName))
              .Number(t => t.Name(n => n.IsSale))
              .Text(t => t.Name(n => n.LanguageId))
              .Text(t => t.Name(n => n.ProductGroupId))
              .Text(t => t.Name(n => n.Dimension))
              .Text(t => t.Name(n => n.Description))
              .Text(t => t.Name(n => n.ShortDescription))
              .Text(t => t.Name(n => n.MediaList))
              .Text(t => t.Name(n => n.CategoryList))
              .Text(t => t.Name(n => n.SortOrder))
              .Text(t => t.Name(n => n.ShortDescription))
              .Text(t => t.Name(n => n.ShortDescription))
              )
            );
        }
    }
}
