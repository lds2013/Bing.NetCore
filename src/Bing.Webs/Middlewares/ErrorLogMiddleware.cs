﻿using System;
using System.Threading.Tasks;
using Bing.Logs;
using Bing.Logs.Extensions;
using Microsoft.AspNetCore.Http;

namespace Bing.Webs.Middlewares
{
    /// <summary>
    /// 错误日志中间件
    /// </summary>
    public class ErrorLogMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly ILog _log;

        /// <summary>
        /// 初始化一个<see cref="ErrorLogMiddleware"/>类型的实例
        /// </summary>
        /// <param name="next">方法</param>
        public ErrorLogMiddleware(RequestDelegate next,ILog log)
        {
            _next = next;
            _log = log;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                WriteLog(context,ex);
                throw;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="ex">异常</param>
        private void WriteLog(HttpContext context, Exception ex)
        {
            if (context == null)
            {
                return;
            }
            //var log = Log.GetLog().Caption("全局异常捕获").Content($"状态码：{context.Response.StatusCode}");
            var log = _log.Caption("全局异常捕获").Content($"状态码：{context.Response.StatusCode}");
            ex.Log(log);
        }
    }
}
