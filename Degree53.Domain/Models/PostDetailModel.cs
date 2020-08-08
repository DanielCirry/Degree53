using Degree53.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Degree53.Domain.Models
{
    public class PostDetailModel
    {
        public static explicit operator PostDetailModel(PostDetail postDetail)
        {
            if (postDetail == null)
                return null;

            return new PostDetailModel
            {
                Id = postDetail.Id,
                CreationDate = postDetail.CreationDate,
                NumberOfViews = postDetail.NumbersOfViews,
                PostId = postDetail.PostId
            };
        }

        public int Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int NumberOfViews { get; set; }
        public int PostId { get; set; }
    }
}
