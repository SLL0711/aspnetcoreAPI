using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.DbContexts;
using DB.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.PetCategory
{
    public class PetCategoryRepository : IPetCategoryRepository
    {
        private readonly PetshopContext context;
        public PetCategoryRepository(PetshopContext petshopContext)
        {
            context = petshopContext;
        }

        public async Task Add(TbPetCategory t)
        {
            context.Add(t);
            await context.SaveChangesAsync();
        }

        public async Task Delete(TbPetCategory t)
        {
            await DeleteById(t.CategoryId);
        }

        public async Task DeleteById(Guid id)
        {
            var obj = context.TbPetCategory.FirstOrDefault(a => a.CategoryId == id);

            if (obj == null)
            {
                return;
            }

            context.TbPetCategory.Remove(obj);

            await context.SaveChangesAsync();
        }

        public IQueryable<TbPetCategory> RetrieveAll()
        {
            var query = context.TbPetCategory.Where(a => true);
            return query;
        }

        public async Task<TbPetCategory> RetrieveById(Guid id)
        {
            var obj = await context.TbPetCategory.FirstOrDefaultAsync(a => a.CategoryId == id);
            return obj;
        }

        public async Task Update(TbPetCategory t)
        {
            context.TbPetCategory.Update(t);
            await context.SaveChangesAsync();
        }
    }
}
