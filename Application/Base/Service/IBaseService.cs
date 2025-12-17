namespace CBVSignalR.Application.Base.Service
{
    public interface IBaseService<TEntity, TKey> where TEntity : class
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
    }
}
