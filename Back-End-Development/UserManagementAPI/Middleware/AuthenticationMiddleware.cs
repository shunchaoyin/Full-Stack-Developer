using System.Net;

namespace UserManagementAPI.Middleware
{
    /// <summary>
    /// 简单的基于令牌的认证中间件。
    /// 检查请求头中是否存在有效的API令牌。
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">请求管道中的下一个中间件</param>
        /// <param name="configuration">应用程序配置</param>
        /// <param name="logger">日志记录器</param>
        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 中间件的调用方法
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        public async Task InvokeAsync(HttpContext context)
        {
            // 跳过 Swagger 端点和健康检查
            if (context.Request.Path.StartsWithSegments("/swagger") || 
                context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            // 从配置中获取预期的API令牌
            var expectedToken = _configuration["Authentication:ApiKey"];

            // 检查请求头中是否存在 "X-API-KEY"，如果没有则检查查询参数
            string receivedToken = null;
            
            if (context.Request.Headers.TryGetValue("X-API-KEY", out var headerToken))
            {
                receivedToken = headerToken;
            }
            else if (context.Request.Query.TryGetValue("apikey", out var queryToken))
            {
                receivedToken = queryToken;
                _logger.LogInformation("API Key provided via query parameter (for development/testing only)");
            }

            if (string.IsNullOrEmpty(receivedToken))
            {
                _logger.LogWarning("API Key is missing from request to {Path}", context.Request.Path);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"API Key is missing. Provide it via X-API-KEY header or ?apikey=your_key query parameter.\"}");
                return;
            }

            // 验证API令牌是否匹配
            if (receivedToken != expectedToken)
            {
                _logger.LogWarning("Invalid API Key provided for request to {Path}", context.Request.Path);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Invalid API Key.\"}");
                return;
            }

            // 如果令牌有效，则调用下一个中间件
            await _next(context);
        }
    }
}
