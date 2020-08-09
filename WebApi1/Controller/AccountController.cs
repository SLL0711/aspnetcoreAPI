using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Lib.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Models.CommonModel;
using WebApi1.Extension.ExtensionModel;
using WebApi1.Extension.ExtensionModel.ConfigOptions;
using WebApi1.Model;

namespace WebApi1.Controller
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AccountController : ApiBaseController
    {
        private readonly mJsonResult json;
        private ILogger _logger;
        private MyOptions options;
        private MyOptions options2;
        private const string USERNAME = "admin";
        private const string PASSWORD = "admin";

        public AccountController(mJsonResult jsonResult, ILogger<AccountController> logger,
            IOptions<MyOptions> configOptions, IOptionsSnapshot<MyOptions> snapshot)
        {
            this.json = jsonResult;
            this._logger = logger;
            this.options = configOptions == null ? null : configOptions.Value;
            this.options2 = configOptions == null ? null : snapshot.Value;
        }

        //[Route("GetToken")]
        [HttpGet]
        [AllowAnonymous]
        public void Login(string userName, string passWord)
        {
            #region 废弃代码

            //IOptions<MyOptions> a = null;
            //IOptionsMonitor<MyOptions> b = null;
            //IOptionsFactory<MyOptions> c = null;
            //IOptionsMonitorCache<MyOptions> d = null;
            //IOptionsSnapshot<MyOptions> e = null;

            //string url = options.UrlBase;
            //string connStr = options.ConnectionStrings.DefaultConnection;

            //用法不推荐，官方建议从构造函数注入服务
            //var a = Request.HttpContext.RequestServices.GetRequiredService(typeof(ITest));
            //((ITest)a).cw();

            #endregion

            if (userName != USERNAME || passWord != PASSWORD)
            {
                json.Success = false;
                json.Msg = "用户认证未通过！";
                return;
            }

            var token = generateToken(userName);

            json.Obj = new
            {
                token
            };

            _logger.LogInformation($"Token:{token}");
        }

        #region 测试代码

        /// <summary>
        /// 模拟控制器方法需要授权之后访问
        /// </summary>
        /// <returns></returns>
        //[Route("GetServerTime")]
        [HttpPost]
        public void GetServerTime()
        {
            var date = DateTime.Now;
            var dateStr = date.ToString("yyyy-MM-dd HH:mm:ss");
            json.Obj = new
            {
                serverDate = dateStr
            };
        }

        [HttpPost]
        public void PostMethodWithJson([FromBody]TestModel testModel)
        {
            json.Obj = new
            {
                className = testModel.className
            };
        }

        [HttpPost]
        public void PostMethodWithUrlcode(ChildModel childModel)
        {
            json.Obj = new
            {
                name = childModel.name,
                age = childModel.age
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public void LogInfo()
        {
            Lib.Log.LogHelper.Info("1111");
            Lib.Log.LogHelper.Debug("1111");
        }

        [HttpGet]
        [AllowAnonymous]
        public void SqlSelect()
        {
            using (SqlHelper sqlHelper = new SqlHelper("Server=10.1.1.7;database=School;User ID=sa;Password=123456;"))
            {
                string sql = "SELECT * FROM STUDENTS";
                json.Rows = sqlHelper.GetDataTable(sql);
            }
        }

        #endregion
    }
}