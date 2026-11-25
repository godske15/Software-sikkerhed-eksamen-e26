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

        public void CreateBlogResponse(BlogResponse response)
        {
            var validatedContents = BlogEntry.BlogContent.Create(response.Contents);
            response.Contents = validatedContents.Value;
            _context.BlogResponses.Add(response);
            _context.SaveChanges();
        }
    }
}
