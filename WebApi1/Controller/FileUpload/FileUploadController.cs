using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.CommonModel;
using Service.Services.FileServices;
using WebApi1.Extension.ExtensionModel;

namespace WebApi1.Controller.FileUpload
{
    public class FileUploadController : ControllerBase
    {
        private readonly mJsonResult json;
        private readonly FileManageService fileManageService;

        public FileUploadController(mJsonResult mJsonResult, FileManageService fileManage)
        {
            fileManageService = fileManage;
            json = mJsonResult;
        }

        [HttpPost]
        public async Task UploadCategoryPic()
        {
            var files = Request.Form.Files;

            if (files.Count < 1)
            {
                json.Success = false;
                json.Msg = "文件不能为空";
                return;
            }

            var file = files[0];

            var id = await fileManageService.AddFileForCategory(file);

            json.Obj = new { fileId = id };
        }
    }
}