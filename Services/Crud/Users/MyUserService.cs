using CarService.Interfaces;
using CarService.Models;
using CarService.Data;
using Microsoft.EntityFrameworkCore;


namespace CarService.Services.Crud.Users
{
    public class MyUserService : ICrudService<User>, IGetUserByLoginService<User>
    {
        private readonly DBContext _context;

        public MyUserService(DBContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            var user = await _context.Users.FindAsync(entity.Id);

            if (user == null)
            {
                return false; 
            }

            user.Name = entity.Name;
            user.Surname = entity.Surname;
            user.UserName = entity.UserName;
            user.Email = entity.Email;

            
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeleteAsync(User entity)
        {
            try
            {
                var user = await _context.Users.FindAsync(entity.Id);

                if (user == null)
                {
                    return false;
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == login);
        }

    }
}