using BloggingPlatform.Data.Context;
using BloggingPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext _context;

        public PostRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post entity)
        {
            var now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            await _context.Posts.AddAsync(entity);
        }

        public async Task<Post?> GetAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task UpdateAsync(Post entity)
        {
            await Task.Run(() =>
            {
                entity.UpdatedAt = DateTime.UtcNow;
                _context.Update<Post>(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            var post = await this.GetAsync(id);

            if (post != null)
            {
                _context.Remove<Post>(post);
            }
        }

        public async Task DeleteAsync(Post entity)
        {
            await Task.Run(() =>
            {
                _context.Remove<Post>(entity);
            });
        }

        public async Task<bool> ExitsAsync(int id)
        {
            return await _context.Posts.AnyAsync(x => x.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> SearchPostsAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await _context.Posts.Where(x =>
                x.Title.ToLower().Contains(searchTerm) ||
                x.Content.ToLower().Contains(searchTerm) ||
                x.Category.ToLower().Contains(searchTerm) ||
                x.Tags.Any(t => t.Name.ToLower().Contains(searchTerm))).ToListAsync();
        }

    }
}
