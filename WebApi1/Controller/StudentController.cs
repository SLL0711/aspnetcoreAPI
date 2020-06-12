using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Service.Services.StudentServices;
using WebApi1.Extension.ExtensionModel;

namespace WebApi1.Controller
{

    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly mJsonResult json;

        public StudentController(StudentService studentService, mJsonResult jsonResult)
        {
            this._studentService = studentService;
            this.json = jsonResult;

            studentService.Status = "1";
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task SetEmailForStudents(int id)
        {
            await _studentService.SetEmailForStudents(id);
            json.Success = true;
            json.Msg = "更新成功";
        }

        public void aaa()
        {

        }
    }
}