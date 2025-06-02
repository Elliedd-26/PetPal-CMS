using Microsoft.EntityFrameworkCore;
using PetPalCMS.Data;

var builder = WebApplication.CreateBuilder(args);

// 注册数据库服务
builder.Services.AddDbContext<PetPalContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PetPalConnection")));

// 注册 API 控制器
builder.Services.AddControllers();

// 注册 Swagger（用于接口测试）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 开发环境中启用 Swagger 页面
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();

// 映射控制器路由
app.MapControllers();

app.Run();
