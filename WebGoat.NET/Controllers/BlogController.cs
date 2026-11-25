using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, NorthwindContext context)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
        }

        public IActionResult Index()
        {
            return View(_blogEntryRepository.GetTopBlogEntries());
        }

        [HttpGet("{entryId}")]
        public IActionResult Reply(int entryId)
        {
            return View(_blogEntryRepository.GetBlogEntry(entryId));
        }

        [HttpPost("{entryId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Reply(int entryId, string contents)
        {
            if (!TryValidateContent(contents))
                return View(_blogEntryRepository.GetBlogEntry(entryId));

            var userName = User?.Identity?.Name ?? "Anonymous";
            var response = new BlogResponse()
            {
                Author = userName,
                Contents = contents,
                BlogEntryId = entryId,
                ResponseDate = DateTime.Now
            };
            _blogResponseRepository.CreateBlogResponse(response);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string contents)
        {
            if (!TryValidateBlogEntry(title, contents))
                return View();

            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, contents, User!.Identity!.Name!);
            return View(blogEntry);
        }

        private bool TryValidateContent(string contents)
        {
            try
            {
                _ = BlogEntry.BlogContent.Create(contents);
                return true;
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError("contents", exception.Message);
                return false;
            }
        }

        private bool TryValidateBlogEntry(string title, string contents)
        {
            var valid = true;
            try
            {
                _ = BlogEntry.BlogTitle.Create(title);
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError("title", exception.Message);
                valid = false;
            }

            return TryValidateContent(contents) && valid;
        }

    }
}
