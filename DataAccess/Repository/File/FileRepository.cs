using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.DbContexts;
using DB.DbModels;

namespace DataAccess.Repository.File
{
    class FileRepository : IFileRepository
    {
        private readonly PetshopContext context;
        public FileRepository(PetshopContext petshopContext)
        {
            context = petshopContext;
        }

        public async Task Add(TbFile t)
        {
            context.TbFile.Add(t);
            await context.SaveChangesAsync();
        }

        public async Task Delete(TbFile t)
        {
            context.TbFile.Remove(t);
            await context.SaveChangesAsync();
        }

        public Task DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TbFile> RetrieveAll()
        {
            var query = context.TbFile.Where(t => true);
            return query;
        }

        public async Task<TbFile> RetrieveById(Guid id)
        {
            var obj = context.TbFile.FirstOrDefault(t => t.FileId == id);
            return obj;
        }

        public async Task Update(TbFile t)
        {
            context.TbFile.Update(t);
            await context.SaveChangesAsync();
        }
    }
}
