namespace CarService.Interfaces;

public interface IGetUserByLoginService<T>
{
    Task<T> GetUserByLoginAsync(string login);
}