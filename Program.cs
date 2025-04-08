using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Server.Infrastructer;
using Renty.Server.Infrastructer.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RentyDbContext>();

// 2. ASP.NET Core Identity ���� ��� (Users Ŭ���� ���)
builder.Services.AddIdentity<Users, IdentityRole>(options => {
    // ��й�ȣ ��å �� �ʿ� �� ����
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.User.RequireUniqueEmail = true; // ���� �̸��� �䱸
    options.SignIn.RequireConfirmedAccount = false; // �̸��� Ȯ�� �ʿ� ���� (API������ ���� false)
})
    .AddEntityFrameworkStores<RentyDbContext>()
    .AddDefaultTokenProviders(); // ��й�ȣ �缳�� ��ū � �ʿ�

// 3. << �߿� >> ��Ű ��� ���� ����
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // JavaScript���� ��Ű ���� �Ұ� (���� �ʼ�)
    // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS ���� �� (� ȯ�� �ʼ�)
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // ��Ȱ�� �� ���� �ð� (��: 30��)
    options.SlidingExpiration = true; // <<--- Ȱ�� �� ���� �ð� �ڵ� ���� (�α��� ���� �ٽ�)
    options.Cookie.Name = ".Renty.AuthCookie"; // ��Ű �̸� ���� (����)

    // API ������ ���� ���𷺼� ��� ���� �ڵ� ��ȯ ����
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context => // 401 Unauthorized
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context => // 403 Forbidden
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
    };
});

// 4. CORS ���� (�÷��� �� �� �ٸ� ��ó������ ��û ���)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", // ��å �̸�
        policy => policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // <<-- ��Ű�� �ְ��ޱ� ���� �ʼ�!!!
});



var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<RentyDbContext>();
context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS �̵���� �߰� (Authentication/Authorization ����!)
app.UseCors("AllowFlutterApp");

// << �߿� >> ���� �� ���� �ο� �̵���� �߰� (���� �߿�!)
app.UseAuthentication(); // ��û�� ��Ű�� Ȯ���ϰ� ����� ����
app.UseAuthorization(); // [Authorize] Ư�� ó��

app.MapControllers();

app.Run();
