using CarService.Interfaces;
using CarService.Models;
using CarService.Data;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services.Crud
{
    public class MyRepairRecordService : ICrudService<RepairRecord>
    {
        private readonly DBContext _context;

        public MyRepairRecordService(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(RepairRecord repairRecord)
        {
            _context.RepairRecords.Add(repairRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(RepairRecord entity)
        {
            var repairRecord = await _context.RepairRecords.FindAsync(entity.Id);
            if (repairRecord == null)
            {
                return false;
            }

            repairRecord.Status = entity.Status;
            repairRecord.Cost = entity.Cost;
            repairRecord.RepairDescription = entity.RepairDescription;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RepairRecord>> GetAllAsync()
        {
            return await _context.RepairRecords
                .Include(r => r.Car)
                .Include(r => r.Master)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(RepairRecord entity)
        {
            var repairRecord = await _context.RepairRecords.FindAsync(entity.Id);
            if (repairRecord == null)
            {
                return false;
            }

            _context.RepairRecords.Remove(repairRecord);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
