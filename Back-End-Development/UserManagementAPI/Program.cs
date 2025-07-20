using Microsoft.OpenApi.Models;
using UserManagementAPI.Services;
using UserManagementAPI.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 配置Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "TechHive User Management API", 
        Version = "v1",
        Description = "一个用于管理用户的RESTful API，为TechHive Solutions的HR和IT部门提供服务",
        Contact = new OpenApiContact
        {
            Name = "TechHive Solutions",
            Email = "support@techhive.com"
        }
    });

    // 包含XML注释
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 注册服务
builder.Services.AddScoped<IUserService, UserService>();

// 配置CORS（如果需要前端访问）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 配置中间件管道（按照指定顺序）
// 1. 错误处理中间件（最先）
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// 2. 认证中间件
app.UseMiddleware<AuthenticationMiddleware>();

// 3. 日志中间件（最后）
app.UseMiddleware<LoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechHive User Management API v1");
        c.RoutePrefix = string.Empty; // 让Swagger UI在根路径可访问
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// 启动时显示信息
Console.WriteLine("🚀 TechHive User Management API 正在启动...");
Console.WriteLine("📖 API文档: https://localhost:5011/swagger (开发环境)");
Console.WriteLine("🌐 基础URL: https://localhost:5011/api");

app.Run();
