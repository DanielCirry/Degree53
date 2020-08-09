using Degree53.DataLayer.Contracts;
using Degree53.DataLayer.DbContexts;
using Degree53.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.DataLayer.Repositories
{
    public class Degree53Repository : IDegree53Repository
    {
        private readonly Degree53DbContext _context;

        public Degree53Repository(Degree53DbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _context.Posts
                .Include(d => d.PostDetail)
                .ToListAsync();
        }

        public async Task<Post> GetPostAsync(int id)
        {
            return await _context.Posts.Where(p => p.Id == id)
                .Include(d => d.PostDetail)
                .FirstOrDefaultAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.PostDetails.AddAsync(post.PostDetail);
        }

        public async Task CompleteTransactionAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                _context.Dispose();

            _disposed = true;
        }

        #endregion

    }
}
