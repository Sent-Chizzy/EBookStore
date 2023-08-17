using EBookStore.Models.Domain;
using EBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookservice;
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IPublisherService publisherService;

        public BookController(IBookService bookservice, IPublisherService publisherService,IAuthorService authorService, IGenreService genreService)
        {
            this.bookservice = bookservice;
            this.publisherService = publisherService;
            this.genreService = genreService;
            this.authorService = authorService;
        }
        public IActionResult Add()
        {
            var model = new Book();
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString() }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Book model)
        {
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(),Selected =a.Id == model.AuthorId }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected =a.Id == model.PublisherId }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected = a.Id == model.GenreId }).ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = bookservice.Add(model);
            if (result)
            {
                TempData["msg"] =  $"{model.Title} Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            TempData["msg"] = "Server Side Error";
            return View(model);
        }

        public IActionResult Update(int id)
        {
            var model = bookservice.FindById(id);
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(), Selected = a.Id == model.AuthorId }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected = a.Id == model.PublisherId }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected = a.Id == model.GenreId }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Book model)
        {
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(), Selected = a.Id == model.AuthorId }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected = a.Id == model.PublisherId }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected = a.Id == model.GenreId }).ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = bookservice.Update(model);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            TempData["msg"] = "Server Side Error";
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var result = bookservice.Delete(id);
            return RedirectToAction("GetAll");
        }

        public IActionResult GetAll()
        {
            var data = bookservice.GetAll();
            return View(data);
        }

    }
}
