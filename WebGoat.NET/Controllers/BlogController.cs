using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebGoatCore.ViewModels;

//Testpush

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
        // Step 4: DTO'en valideres før domæneprimitiven oprettes og svaret gemmes.
        // GAMMELT: public IActionResult Reply(int entryId, string contents)
        public IActionResult Reply([FromRoute] int entryId, [FromForm] BlogResponseDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contents = new BlogResponseContent(input.Contents);
            var userName = User?.Identity?.Name ?? "Anonymous";
            /* GAMMELT: Controlleren oprettede selv entiteten uden en domæneprimitiv:
            var response = new BlogResponse()
            {
                Author = userName,
                Contents = contents,
                BlogEntryId = entryId,
                ResponseDate = DateTime.Now
            };
            _blogResponseRepository.CreateBlogResponse(response);
            */
            _blogResponseRepository.CreateBlogResponse(entryId, contents, userName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(string title, string contents)
        {
            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, contents, User!.Identity!.Name!);
            return View(blogEntry);
        }

    }
}