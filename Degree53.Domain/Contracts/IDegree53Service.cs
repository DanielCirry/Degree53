using Degree53.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Degree53.Domain.Contracts
{
    public interface IDegree53Service
    {
        Task<List<PostModel>> GetPostsAsync();
        Task<PostModel> GetPostAsync(int id);
        Task AddPostAsync(PostModel postModel);
        Task CompleteTransactionAsync();
    }
}
