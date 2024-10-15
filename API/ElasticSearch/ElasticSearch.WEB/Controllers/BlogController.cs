using ElasticSearch.WEB.Models;
using ElasticSearch.WEB.Services;
using ElasticSearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.WEB.Controllers
{
    public class BlogController : Controller
    { private  BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
          var isSuccess=  await _blogService.SaveAsync(model);
            if(!isSuccess)
            {
                TempData["result"] = "Kayıt Başarısız";
                return RedirectToAction("Save");
            }
            TempData["result"] = "Kayıt Başarılı";
            return RedirectToAction("Save");
        }
        public async Task< IActionResult> Search()
        {
           
            return View(await _blogService.SearchAsync(string.Empty));
        }
        [HttpPost]
        public async Task< IActionResult> Search(string searchText)
        {
            ViewBag.searchText = searchText;
            var blogList=await _blogService.SearchAsync(searchText);
            return View(blogList);
        }
    }
}
