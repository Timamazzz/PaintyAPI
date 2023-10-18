namespace DataAccess.Interfaces;

internal interface ICrudRepository<TEntityModel> where TEntityModel : class
{
    Task<TEntityModel?> CreateAsync(TEntityModel entity);
    Task<List<TEntityModel>?> GetAllAsync();
    Task<TEntityModel?> GetByIdAsync(int? id);
    Task<TEntityModel?> UpdateAsync(TEntityModel entity);
    Task DeleteByIdAsync(int id);
}
