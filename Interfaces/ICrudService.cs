using CarService.Models;

namespace CarService.Interfaces;

public interface ICrudService<T>
{
    Task<bool> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<bool> DeleteAsync(T entity);
    
}