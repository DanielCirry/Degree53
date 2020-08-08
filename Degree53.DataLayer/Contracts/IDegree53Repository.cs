using Degree53.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Degree53.DataLayer.Contracts
{
    public interface IDegree53Repository : IDisposable
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostAsync(int id);
        Task AddPostAsync(Post post);
        Task CompleteTransactionAsync();
    }
}
