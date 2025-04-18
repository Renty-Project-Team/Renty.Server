using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Renty.Server.Domain.Auth;
using Renty.Server.Domain.Product;
using Renty.Server.Global;
using Renty.Server.Infrastructer;
using Renty.Server.Infrastructer.Auth;
using Renty.Server.Infrastructer.Model;
using Renty.Server.Infrastructer.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Options Pattern 설정 추가 ---
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("Settings") // "Setting" 섹션과 매핑
);

// DI 클래스 연결
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 1. DB 등록
builder.Services.AddDbContext<RentyDbContext>();

// 2. ASP.NET Core Identity 서비스 등록 (Users 클래스 사용)
builder.Services.AddIdentity<Users, IdentityRole>(options => {
    // 비밀번호 정책 등 필요 시 설정
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.User.RequireUniqueEmail = true; // 고유 이메일 요구
    options.SignIn.RequireConfirmedAccount = false; // 이메일 확인 필요 여부 (API에서는 보통 false)
})
    .AddEntityFrameworkStores<RentyDbContext>()
    .AddDefaultTokenProviders(); // 비밀번호 재설정 토큰 등에 필요

// 3. << 중요 >> 쿠키 기반 인증 설정
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // JavaScript에서 쿠키 접근 불가 (보안 필수)
    // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS 강제 시 (운영 환경 필수)
    options.ExpireTimeSpan = TimeSpan.FromDays(14); // 비활성 시 만료 시간 (예: 30분)
    options.SlidingExpiration = true; // <<--- 활동 시 만료 시간 자동 연장 (로그인 유지 핵심)
    options.Cookie.Name = ".Renty.AuthCookie"; // 쿠키 이름 지정 (선택)

    // API 동작을 위해 리디렉션 대신 상태 코드 반환 설정
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

// 4. CORS 설정 (플러터 앱 등 다른 출처에서의 요청 허용)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", // 정책 이름
        policy => policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // <<-- 쿠키를 주고받기 위해 필수!!!
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

// 정적 파일(이미지) 미들웨어 추가
var settings = builder.Configuration.GetSection("Settings");
var imagePath = Path.Combine(settings["DataStorage"], settings["ImagesFolder"]);
if (!string.IsNullOrEmpty(imagePath))
{
    if (!Directory.Exists(imagePath))
    {
        Directory.CreateDirectory(imagePath);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagePath),
        RequestPath = settings["ImagesUrl"] // 이 URL 경로로 접근 가능
    });
}

// CORS 미들웨어 추가 (Authentication/Authorization 전에!)
app.UseCors("AllowFlutterApp");

// << 중요 >> 인증 및 권한 부여 미들웨어 추가 (순서 중요!)
app.UseAuthentication(); // 요청의 쿠키를 확인하고 사용자 인증
app.UseAuthorization(); // [Authorize] 특성 처리

app.MapControllers();

app.Run();
