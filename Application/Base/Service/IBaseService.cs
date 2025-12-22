using CBVSignalR.Application.Models;

namespace CBVSignalR.Application.Base.Service
{
    public interface IBaseService<TEntity, TKey, TRequest> where TEntity : class where TRequest : PagingFilterRequest
    {
        /// <summary> Tạo mới entity </summary>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary> Cập nhật entity </summary>
        Task<TEntity?> UpdateAsync(TKey id, TEntity entity);

        /// <summary> Xóa entity theo id </summary>
        Task<bool> DeleteAsync(TKey id);

        /// <summary> Lấy entity theo id </summary>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary> Lấy tất cả entity </summary>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>
        /// Lấy danh sách có phân trang + filter nếu có
        /// </summary>
        Task<PagedResult<TEntity>> GetAsync(TRequest request);
    }
}
