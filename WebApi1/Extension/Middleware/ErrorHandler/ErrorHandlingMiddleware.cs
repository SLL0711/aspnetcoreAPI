using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models.CommonModel;
using WebApi1.Extension.ExtensionModel;

namespace WebApi1.Extension.Middleware.ErrorHandler
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, mJsonResult jsonResult, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                jsonResult.Success = false;
                jsonResult.Msg = $"服务器错误：{e.Message}";
                logger.LogError($"服务器错误：{e.Message}");
                //throw;
            }
            finally
            {
                try
                {
                    jsonResult.Code = context.Response.StatusCode;
                    if (jsonResult.Code.ToString().Substring(0, 1) == "4")
                    {
                        jsonResult.Success = false;
                    }

                    context.Response.StatusCode = 200;//axios 根据statuscode判定响应是否成功
                    var jsonStr = JsonConvert.SerializeObject(jsonResult);
                    await context.Response.WriteAsync(jsonStr);
                }
                catch (Exception e)
                {
                    logger.LogError($"服务器错误：{e.Message}");
                }
            }
        }
    }
}
