using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]   
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _repository.TermQuery(customerFirstName));
        }
        [HttpPost]
        public async Task<IActionResult> TermsQuery(List< string> customerFirstNameList)
        {
            return Ok(await _repository.TermsQuery(customerFirstNameList));
        }
        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _repository.PrefixQuery(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _repository.RangeQuery(fromPrice,toPrice));
        }
        [HttpGet]
        public async Task<IActionResult> MatchQuery()
        {
            return Ok(await _repository.MatchAllQuery());
        }
        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page=1,int pageSize=3)
        {
            return Ok(await _repository.PaginationQuery(page,pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> WildCardQuery(string customerFullName)
        {
            return Ok(await _repository.WildCardQuery(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerFirstName)
        {
            return Ok(await _repository.FuzzyQuery(customerFirstName));
        }
        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string category)
        {
            return Ok(await _repository.MatchQueryFullText(category));
        }
        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerFullName)
        {
            return Ok(await _repository.MatchBoolPrefixQueryFullText(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult> MatchPhraseQueryFullText(string customerFullName)
        {
            return Ok(await _repository.MatchPhraseQueryFullText(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxFulTotalPrice, string categoryName, string menufacturer)
        {
            return Ok(await _repository.CompoundQueryExampleOne(cityName,taxFulTotalPrice,categoryName,menufacturer));
        }
        [HttpGet]
        public async Task<IActionResult>
        CompoundQueryExampleTwo(string customerFullName)
        {
            return Ok(await _repository.CompoundQueryExampleTwo(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult>MultiMatchQueryFullText(string name)
        {
            return Ok(await _repository.MultiMatchQueryFullText(name));
        }
    }
}
