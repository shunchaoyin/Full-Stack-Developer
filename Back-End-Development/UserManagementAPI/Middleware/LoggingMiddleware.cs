using System.Diagnostics;

namespace UserManagementAPI.Middleware
{
    /// <summary>
    /// 日志中间件，用于记录传入的HTTP请求和传出的响应信息。
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">请求管道中的下一个中间件</param>
        /// <param name="logger">日志记录器</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
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
            var stopwatch = Stopwatch.StartNew();
            
            // 记录请求信息
            _logger.LogInformation(
                "Incoming Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            // 调用管道中的下一个中间件
            await _next(context);

            stopwatch.Stop();
            
            // 记录响应信息
            _logger.LogInformation(
                "Outgoing Response: {Method} {Path} -> {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
