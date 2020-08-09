using Degree53.DataLayer.Contracts;
using Degree53.DataLayer.Entities;
using Degree53.Domain.Contracts;
using Degree53.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.Domain.Services
{
    public class Degree53service : IDegree53Service
    {
        private readonly IDegree53Repository _repository;

        public Degree53service(IDegree53Repository repository)
        {
            _repository = repository;
        }

        public async Task<List<PostModel>> GetPostsAsync()
        {
            var posts = await _repository.GetPostsAsync();
            if (posts != null)
                return posts.Select(p => (PostModel)p).ToList();

            return null;
        }

        public async Task<PostModel> GetPostAsync(int id)
        {
            var post = await _repository.GetPostAsync(id);
            if (post != null)
                return (PostModel)post;

            return null;
        }

        public async Task AddPostAsync(PostModel postModel)
        {
            if (postModel != null)
            {
                var postDetail = new PostDetail
                {
                    CreationDate = postModel.PostDetail.CreationDate,
                    PostId = postModel.PostDetail.PostId,
                    NumbersOfViews = postModel.PostDetail.NumberOfViews
                };

                var post = new Post
                {
                    Title = postModel.Title,
                    Content = postModel.Content,
                    PostDetail = postDetail
                };

                await _repository.AddPostAsync(post);
            }
        }

        public async Task CompleteTransactionAsync()
        {
            await _repository.CompleteTransactionAsync();
        }
    }
}
