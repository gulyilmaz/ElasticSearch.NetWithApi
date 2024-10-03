
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using Nest;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string indexName = "products";
        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }
        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;
            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName));
            //fast fail
            if (!response.IsValid) return null;
            newProduct.ID = response.Id;
            return newProduct;
        }
        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q=>q.MatchAll()));
            foreach (var hit in result.Hits)
            {
                hit.Source.ID = hit.Id;
            }
            return result.Documents.ToImmutableList();
        }
        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));
            if (!response.IsValid)
            {
                return null;
            }
            response.Source.ID = response.Id;
            return response.Source;
        }
        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>( updateProduct.id,x=>x.Index(indexName).Doc(updateProduct));
            return response.IsValid;
        }
        /// <summary>
        /// //Hata yönetimi için bu method ele alınmıştır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));
            return response;
        }
    }
}
