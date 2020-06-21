using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.Student;
using Microsoft.EntityFrameworkCore;

namespace Service.Services.StudentServices
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;
        }

        public string Status { get; set; }

        public async Task SetEmailForStudents(int id)
        {
            var studentObj = await _studentRepository.RetrieveAll().FirstOrDefaultAsync(t=>t.Id == id);

            if (studentObj != null)
            {
                studentObj.EmailAddress = $"837316150@qq.com_{id}";

                await _studentRepository.Update(studentObj);
            }
        }
    }
}
