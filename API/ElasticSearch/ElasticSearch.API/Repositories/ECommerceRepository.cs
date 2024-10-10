using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.ECommerce;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        private const string indexName = "kibana_sample_data_ecommerce";
        public async Task<IImmutableList<ECommerce>> TermQuery(string customerFirstName)
        {
            //1.way tip güvensiz
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
            //2.way tip güvenli
            //var result= await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(q=>q.Term(t=>t.CustomerFirstName.Suffix("keyword"),customerFirstName)));
            //3 way
            var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return result.Documents.ToImmutableList();
        }


        public async Task<IImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(x =>
            {
                terms.Add(x);
            });
            //1.way
            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Terms = new TermsQueryField(terms.AsReadOnly())
            //};
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));
            //2 way
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(100)
            .Query(q => q
            .Terms(t => t
                .Field(f => f.CustomerFirstName
                .Suffix("keyword"))
                .Terms(new TermsQueryField(terms.AsReadOnly())))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return result.Documents.ToImmutableList();
        }

        public async Task<IImmutableList<ECommerce>> PrefixQuery(string customerFullName)
        {

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q
                .Prefix(p => p
                    .Field(f => f.CustomerFullName
                        .Suffix("keyword"))
                            .Value(customerFullName))));
            return result.Documents.ToImmutableList();
        }
        public async Task<IImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q
                .Range(r => r
                    .NumberRange(nr => nr
                        .Field(f => f.TaxfulTotalPrice).Gte(fromPrice).Lte(toPrice)))
                ));
            return result.Documents.ToImmutableList();
        }
        public async Task<IImmutableList<ECommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(100)
            .Query(q => q.MatchAll()));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();

        }
        public async Task<IImmutableList<ECommerce>> PaginationQuery(int page, int pageSize)
        {
            var pageFrom = (page - 1) * pageSize;
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(pageSize).From(pageFrom)
            .Query(q => q.MatchAll()));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();

        }
        public async Task<IImmutableList<ECommerce>> WildCardQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }
        public async Task<IImmutableList<ECommerce>> FuzzyQuery(string customerFirstName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.Fuzzy(fz => fz.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName).Fuzziness(new Fuzziness(1)))).Sort(sort => sort.Field(f => f.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<IImmutableList<ECommerce>> MatchQueryFullText(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
               .Size(1000).Query(q => q.Match(m => m.Field(f => f.Category).Query(categoryName).Operator(Operator.And))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }
        public async Task<IImmutableList<ECommerce>> MultiMatchQueryFullText(string name)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
               .Size(1000).Query(q => q
               .MultiMatch(mm => mm
                    .Fields(new Field("customer_first_name")
                       .And(new Field("customer_last_name"))
                       .And(new Field("customer_full_name")))
                    .Query(name))));
                       
                       
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }
        public async Task<IImmutableList<ECommerce>> MatchBoolPrefixQueryFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
              .Query(q => q.MatchBoolPrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }
        public async Task<IImmutableList<ECommerce>> MatchPhraseQueryFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
              .Query(q => q.MatchPhrase(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }
        public async Task<IImmutableList<ECommerce>> CompoundQueryExampleOne(string cityName,double taxFulTotalPrice,string categoryName,string menufacturer )
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
              .Query(q => q
                  .Bool(b => b
                    .Must(m => m
                           .Term(t => t.Field("geoip.city_name")
                            .Value(cityName)))
                    .MustNot(mn => mn
                            .Range(r => r
                                .NumberRange(nr => nr
                                    .Field(f => f.TaxfulTotalPrice)
                                        .Lte(taxFulTotalPrice))))
                    .Should(s => s
                            .Term(t => t
                                .Field(f => f.Category.Suffix("keyword"))
                                    .Value(categoryName)))
                    .Filter(f => f
                            .Term(t => t
                                .Field("manufacturer.keyword")
                                    .Value(menufacturer))))
                    ));
                  
                  
                  ;
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }
        public async Task<IImmutableList<ECommerce>> CompoundQueryExampleTwo(string customerFullName)
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //  .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
              .Query(q => q
                  .Bool(b => b
                    .Should(m => m.
                        Match(m => m
                            .Field(f => f.CustomerFullName)
                             .Query(customerFullName))
                        .Prefix(p => p
                            .Field(f => f.CustomerFullName.Suffix("keyword"))
                                .Value(customerFullName))))));


            
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }


    }
}
