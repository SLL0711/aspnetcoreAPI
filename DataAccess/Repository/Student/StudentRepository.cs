using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.DbContexts;
using DB.DbModels;

namespace DataAccess.Repository.Student
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;
        public StudentRepository(SchoolContext schoolContext)
        {
            this._context = schoolContext;
        }

        public Task Add(Students t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Students t)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Students>> RetrieveAll()
        {
            var query = _context.Students.Where(t => true);
            return query;
        }

        public Task<Students> RetrieveById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Students t)
        {
            _context.Students.Update(t);
            await _context.SaveChangesAsync();
        }
    }
}
