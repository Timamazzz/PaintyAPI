namespace Domain.Interfaces;

internal interface ICrudService<TDomainModel> where TDomainModel : class
{
    Task<TDomainModel?> CreateAsync(TDomainModel dto);
    Task<List<TDomainModel>?> GetAllAsync();
    Task<TDomainModel?> GetByIdAsync(int id);
    Task UpdateAsync(TDomainModel dto);
    Task DeleteAsync(int id);
}
