using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik_Kocherzhenko.Model
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
