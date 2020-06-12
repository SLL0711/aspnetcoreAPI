using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public void GetToken(string userName, string email, string passWord)
        {
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

            if (userName != "sll" || passWord != "123456" || string.IsNullOrWhiteSpace(email))
            {
                json.Success = false;
                json.Msg = "用户认证未通过！";
                return;
            }

            //throw new ArgumentNullException("userName");

            var token = generateToken(userName, email);

            json.Obj = new
            {
                token
            };

            _logger.LogInformation($"Token:{token}");
        }

        /// <summary>
        /// 模拟控制器方法需要授权之后访问
        /// </summary>
        /// <returns></returns>
        //[Route("GetServerTime")]
        [Authorize]
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

        [Authorize]
        [HttpPost]
        public void PostMethodWithJson([FromBody]TestModel testModel)
        {
            json.Obj = new
            {
                className = testModel.className
            };
        }

        [Authorize]
        [HttpPost]
        public void PostMethodWithUrlcode(ChildModel childModel)
        {
            json.Obj = new
            {
                name = childModel.name,
                age = childModel.age
            };
        }
    }
}