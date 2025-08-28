using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Farms;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Farms
{
    public class FarmImageRepository : DataGeneric<FarmImage>, IFarmImageRepository
    {
        public FarmImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task AddImages(List<FarmImage> images)
        {
            if (images == null || !images.Any())
                return Task.CompletedTask;

            // No transacciones ni SaveChanges aquí
            _dbSet.AddRange(images);
            return Task.CompletedTask;
        }

        public async Task<bool> DeleteByPublicIdAsync(string publicId)
        {
            var image = await _dbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
            if (image == null) return false;

            _dbSet.Remove(image);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteLogicalByPublicIdAsync(string publicId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
            if (entity == null) return false;

            entity.Active = false;
            entity.IsDeleted = true; 
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FarmImage>> GetByFarmIdAsync(int farmId)
        {
            return await _dbSet
                .Where(e => e.FarmId == farmId && !e.IsDeleted)
                .ToListAsync();
        }
    }
}
