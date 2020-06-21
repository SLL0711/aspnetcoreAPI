using DataAccess.Repository.File;
using DB.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Lib.Common;

namespace Service.Services.FileServices
{
    public class FileManageService
    {
        private readonly IFileRepository repository;
        public FileManageService(IFileRepository fileRepository)
        {
            repository = fileRepository;
        }

        public async Task<Guid> AddFileForCategory(IFormFile file)
        {
            string dirPath = Environment.CurrentDirectory+ "/MyStaticFiles";
            string dirPath2 = $"Imgs/{DateTime.Now.ToString("yyyyMMdd")}";
            string fileName = ConvertHelper.GetTimestamp().ToString() + ".jpg";

            var filePath = Path.Combine(dirPath, dirPath2);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string path = Path.Combine(dirPath,dirPath2,fileName);

            var stream = file.OpenReadStream();

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new byte[(int)file.Length];
                stream.Read(bytes);
                await fs.WriteAsync(bytes, 0, (int)file.Length);
            }

            TbFile tbFile = new TbFile()
            {
                FileName = file.FileName,
                FileSize = (int)file.Length,
                FileType = file.ContentType,
                FilePath = Path.Combine(dirPath2 , fileName)
            };

            tbFile.CreatedBy = "SLL";
            tbFile.CreatedOn = DateTime.Now;
            tbFile.ModifiedBy = "SLL";
            tbFile.ModifiedOn = DateTime.Now;
            tbFile.Statecode = 0;
            tbFile.FileId = Guid.NewGuid();
            tbFile.TbName = "tb_PetCategory";

            await repository.Add(tbFile);

            return tbFile.FileId;
        }

        private void CreateFile()
        {

        }

        public async Task UploadTbid(string fileId, string tbId,string tbname)
        {
            var obj = await repository.RetrieveById(new Guid(fileId));
            obj.TbId = tbId;
            obj.TbName = tbname;

            await repository.Update(obj);
        }

        public async Task UnBindFile(string tbId, string tbname)
        {
            var list = repository.RetrieveAll().Where(t => t.TbName == tbname && t.TbId == tbId).ToList();

            foreach (var item in list)
            {
                item.TbId = "";
                item.TbName = "";
                await repository.Update(item);
            }
        }
    }
}
