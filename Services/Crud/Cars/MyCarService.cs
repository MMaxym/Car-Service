using CarService.Interfaces;
using CarService.Models;
using CarService.Data;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services.Crud
{
    public class MyCarService : ICrudService<Car>
    {
        private readonly DBContext _context;

        public MyCarService(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Car entity)
        {
            var car = await _context.Cars.FindAsync(entity.Id);

            if (car == null)
            {
                return false;
            }

            car.Make = entity.Make;
            car.Model = entity.Model;
            car.Year = entity.Year;
            car.Number = entity.Number;
            car.Vin = entity.Vin;
            car.Mileage = entity.Mileage;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Car entity)
        {
            var car = await _context.Cars.FindAsync(entity.Id);
            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
