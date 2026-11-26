using System;
using WebGoatCore.Models;

namespace WebGoatCore.Data
{
    public class BlogResponseRepository
    {
        private readonly NorthwindContext _context;

        public BlogResponseRepository(NorthwindContext context)
        {
            _context = context;
        }

        // Step 5: Repositoryet kræver valideret tekst og sætter selv datoen.
        // GAMMELT: public void CreateBlogResponse(BlogResponse response)
        public void CreateBlogResponse(int entryId, BlogResponseContent contents, string author)
        {
            ArgumentNullException.ThrowIfNull(contents);
            ArgumentNullException.ThrowIfNull(author);

            var response = new BlogResponse
            {
                BlogEntryId = entryId,
                Author = author,
                ResponseDate = DateTime.Now,
                Contents = contents.Value
            };
            _context.BlogResponses.Add(response);
            _context.SaveChanges();
        }
    }
}
