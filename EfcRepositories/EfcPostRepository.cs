// using Entities;
// using Microsoft.EntityFrameworkCore;
// using RepositoryContracts;
//
// namespace EfcRepositories;
//
// public class EfcPostRepository : IPostRepository
// {
//     private readonly AppContext _context;
//
//     public EfcPostRepository(AppContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<Post> AddAsync(Post post)
//     {
//         _context.Posts.Add(post);
//         await _context.SaveChangesAsync();
//         return post;
//     }
//
//     public async Task<Post> GetSingleAsync(string id)
//     {
//         return await _context.Posts.Include(p => p.User)
//             .Include(p => p.Comments)
//             .FirstOrDefaultAsync(p => p.PostId == id);
//     }
//
//     public IQueryable<Post> GetMany()
//     {
//         return _context.Posts.Include(p => p.User);
//     }
//
//     public async Task UpdateAsync(Post post)
//     {
//         _context.Posts.Update(post);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task DeleteAsync(string id)
//     {
//         var post = await _context.Posts.FindAsync(id);
//         if (post != null)
//         {
//             _context.Posts.Remove(post);
//             await _context.SaveChangesAsync();
//         }
//     }
//
//     public async Task LikePostAsync(string id)
//     {
//         var post = await _context.Posts.FindAsync(id);
//         if (post != null)
//         {
//             post.Likes++;
//             await _context.SaveChangesAsync();
//         }
//     }
//
//     public async Task RemoveLikePostAsync(string id)
//     {
//         var post = await _context.Posts.FindAsync(id);
//         if (post != null && post.Likes > 0)
//         {
//             post.Likes--;
//             await _context.SaveChangesAsync();
//         }
//     }
// }