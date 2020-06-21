using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.CommonModel;
using Service.Services.FileServices;
using Service.Services.PetCategoryServices;
using WebApi1.Extension.ExtensionModel;

namespace WebApi1.Controller.PetCategory
{
    public class PetCategoryController : ControllerBase
    {
        private mJsonResult json;
        private readonly PetCategoryManageService categoryManageService;
        private readonly FileManageService fileManageService;
        public PetCategoryController(mJsonResult jsonResult, PetCategoryManageService petCategoryManageService,
            FileManageService fileManage)
        {
            this.json = jsonResult;
            this.categoryManageService = petCategoryManageService;
            this.fileManageService = fileManage;
        }

        /// <summary>
        /// 创建宠物分类
        /// </summary>
        /// <param name="tbPetCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddCategory(TbPetCategory tbPetCategory, string fileId)
        {
            #region 数据有效性判断

            if (string.IsNullOrWhiteSpace(tbPetCategory.TypeName))
            {
                json.Success = false;
                json.Msg = "类别名称不能为空";
                return;
            }

            if (tbPetCategory.TypeName.Length > 50)
            {
                json.Success = false;
                json.Msg = "类别名称长度不能为空";
                return;
            }

            #endregion

            tbPetCategory.CategoryId = Guid.NewGuid();

            if (!string.IsNullOrWhiteSpace(fileId))
            {
                //更新文件关联的实体ID
                await fileManageService.UploadTbid(fileId, tbPetCategory.CategoryId.ToString(), "tb_PetCategory");
            }

            //创建分类
            await categoryManageService.AddCategory(tbPetCategory);
            json.Success = true;
        }

        /// <summary>
        ///更新宠物分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdateCategory(TbPetCategory tbPetCategory, string fileId)
        {
            #region 数据有效性判断

            if (string.IsNullOrWhiteSpace(tbPetCategory.TypeName))
            {
                json.Success = false;
                json.Msg = "类别名称不能为空";
                return;
            }

            if (tbPetCategory.TypeName.Length > 50)
            {
                json.Success = false;
                json.Msg = "类别名称长度不能为空";
                return;
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(fileId))
            {
                //当前关联的file取消绑定
                await fileManageService.UnBindFile(tbPetCategory.CategoryId.ToString(), "tb_PetCategory");
                //更新文件关联的实体ID
                await fileManageService.UploadTbid(fileId, tbPetCategory.CategoryId.ToString(), "tb_PetCategory");
            }

            var obj = await categoryManageService.GetCategoryObj(tbPetCategory.CategoryId);
            obj.TypeName = tbPetCategory.TypeName;

            //创建分类
            await categoryManageService.UpdateCategory(obj);
            json.Success = true;
        }

        /// <summary>
        /// 获取宠物分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void GetCategoryList(int pageIndex, int pageSize, string searchKey)
        {
            json = categoryManageService.GetCategoryList(pageIndex, pageSize, searchKey);
        }

        /// <summary>
        /// 批量删除宠物分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteCategorys([FromBody]List<Guid> categoryIds)
        {
            await categoryManageService.DeleteCategorys(categoryIds);
            json.Success = true;
        }

        /// <summary>
        /// 删除宠物分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteCategory(Guid categoryId)
        {
            await categoryManageService.DeleteCategory(categoryId);
            json.Success = true;
        }
    }
}