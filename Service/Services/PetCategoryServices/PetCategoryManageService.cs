using DataAccess.Repository.PetCategory;
using DB.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.File;
using Microsoft.EntityFrameworkCore;
using Service.Models.CommonModel;

namespace Service.Services.PetCategoryServices
{
    public class PetCategoryManageService
    {
        private readonly IPetCategoryRepository repository;
        private readonly IFileRepository _fileRepository;
        private readonly mJsonResult json;
        public PetCategoryManageService(IPetCategoryRepository petCategoryRepository, IFileRepository fileRepository, mJsonResult jsonResult)
        {
            json = jsonResult;
            repository = petCategoryRepository;
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// 创建类别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task AddCategory(TbPetCategory category)
        {
            category.Createdon = DateTime.Now;
            category.Modifiedon = DateTime.Now;
            category.Createdby = "SLL";
            category.Modifiedby = "SLL";
            category.Statecode = 0;

            await repository.Add(category);
        }

        /// <summary>
        /// 更新类别
        /// </summary>
        /// <returns></returns>
        public async Task UpdateCategory(TbPetCategory category)
        {
            category.Modifiedon = DateTime.Now;
            category.Modifiedby = "SLL";

            await repository.Update(category);
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public mJsonResult GetCategoryList(int pageIndex, int pageSize, string searchKey)
        {
            var query = repository.RetrieveAll().Where(a => a.Statecode == 0);

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(t => t.TypeName.Contains(searchKey));
            }

            var query2 = _fileRepository.RetrieveAll();
            var queryRes = query.Select(a => new
            {
                a.CategoryId,
                a.Statecode,
                a.TypeName,
                a.Createdby,
                a.Createdon,
                a.Modifiedby,
                a.Modifiedon,
                imgObj = query2.FirstOrDefault(t => t.Statecode == 0 && t.TbId == a.CategoryId.ToString() && t.TbName == "tb_PetCategory")
            });

            var list = queryRes.OrderByDescending(a => a.Modifiedon)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize);

            json.Rows = list;
            json.Obj = new
            {
                Total = query.Count()
            };
            json.Success = true;

            return json;
        }

        public async Task<TbPetCategory> GetCategoryObj(Guid id)
        {
            var query = await repository.RetrieveAll()
                .FirstOrDefaultAsync(a => a.Statecode == 0 && a.CategoryId == id);

            return query;
        }

        public async Task DeleteCategorys(List<Guid> categoryIds)
        {
            foreach (var id in categoryIds)
            {
                await DeleteCategory(id);
            }
        }

        public async Task DeleteCategory(Guid id)
        {
            await repository.DeleteById(id);
        }
    }
}
