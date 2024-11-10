using DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDTO> CreateCommentAsync(CreateCommentDTO dto);

    public Task<CommentDTO> GetSingleCommentByIdAsync(string id);
    
    public Task<CommentDTO> UpdateCommentAsync(UpdateCommentDTO dto);

    public Task DeleteCommentAsync(string id);

    public Task LikeCommentAsync(string id);

    public Task DislikeCommentAsync(string id);

    public Task<List<CommentDTO>> GetCommentsByPostIdAsync(string id);

    public Task<List<SimpleCommentDTO>> GetCommentsByUserIdAsync(int id);
}