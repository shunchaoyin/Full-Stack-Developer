using System.Net;
using System.Text.Json;

namespace UserManagementAPI.Middleware
{
    /// <summary>
    /// 全局异常处理中间件，用于捕获管道中未处理的异常，并返回统一的JSON错误响应。
    /// </summary>
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">请求管道中的下一个中间件</param>
        /// <param name="logger">日志记录器</param>
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 中间件的调用方法
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 调用管道中的下一个中间件
                await _next(context);
            }
            catch (Exception ex)
            {
                // 记录异常信息
                _logger.LogError(ex, "An unhandled exception has occurred.");

                // 设置响应状态码
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // 创建标准化的错误响应
                var errorResponse = new
                {
                    error = "Internal server error.",
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                };

                // 将错误响应序列化为JSON并写入响应体
                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
