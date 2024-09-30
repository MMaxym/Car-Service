using CarService.Interfaces;
using CarService.Models;
using CarService.Data;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services.Crud
{
    public class MyMasterService : ICrudService<Master>
    {
        private readonly DBContext _context;

        public MyMasterService(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Master master)
        {
            _context.Masters.Add(master);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Master entity)
        {
            var master = await _context.Masters.FindAsync(entity.Id);

            if (master == null)
            {
                return false;
            }

            master.Name = entity.Name;
            master.Surname = entity.Surname;
            master.Specialization = entity.Specialization;
            master.UserName = entity.UserName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Master>> GetAllAsync()
        {
            return await _context.Masters.ToListAsync();
        }

        public async Task<bool> DeleteAsync(Master entity)
        {
            var master = await _context.Masters.FindAsync(entity.Id);
            if (master == null)
            {
                return false;
            }

            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
