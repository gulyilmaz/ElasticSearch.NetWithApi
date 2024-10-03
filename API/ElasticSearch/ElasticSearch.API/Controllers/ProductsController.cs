using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreaateActionResult(await _productService.SaveAsync(request));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreaateActionResult(await _productService.GetAllAsync());
        }
      [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreaateActionResult(await _productService.GetByIdAsync(id));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto request)
        {
            return CreaateActionResult(await _productService.UpdateAsync(request));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return CreaateActionResult(await _productService.DeleteAsync(id));
        }
    }
}
