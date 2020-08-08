using Degree53.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Degree53.Domain.Models
{
    public class PostModel
    {
        public static explicit operator PostModel(Post post)
        {
            if (post == null)
                return null;

            return new PostModel
            {
                Id = post.Id,
                Content = post.Content,
                PostDetail = (PostDetailModel)post.PostDetail
            };
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostDetailModel PostDetail { get; set; }
    }
}
