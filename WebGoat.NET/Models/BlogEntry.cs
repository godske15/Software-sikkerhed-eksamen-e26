using System;
using System.Collections.Generic;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Models
{
    public class BlogEntry
    {
        public int Id { get; set; }
        public BlogTitle Title { get; set; }
        public DateTime PostedDate { get; set; }
        public BlogContent Contents { get; set; }
        public string Author { get; set; }

        public virtual IList<BlogResponse> Responses { get; set; }
    }
}
