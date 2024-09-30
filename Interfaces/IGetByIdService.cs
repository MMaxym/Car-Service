namespace CarService.Interfaces;

public interface IGetByIdService<T>
{
    Task<T?> GetByIdAsync(int id);
}