using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Renty.Server;
using Renty.Server.Auth.Domain;
using Renty.Server.Auth.Domain.Query;
using Renty.Server.Auth.Domain.Repository;
using Renty.Server.Auth.Infrastructer;
using Renty.Server.Chat.Controller;
using Renty.Server.Chat.Domain.Repository;
using Renty.Server.Chat.Infrastructer;
using Renty.Server.Chat.Service;
using Renty.Server.Global;
using Renty.Server.My.Domain.Query;
using Renty.Server.My.Domain.Repository;
using Renty.Server.My.Infrastructer;
using Renty.Server.My.Service;
using Renty.Server.Product.Domain.Query;
using Renty.Server.Product.Domain.Repository;
using Renty.Server.Product.Infrastructer;
using Renty.Server.Product.Service;
using Renty.Server.Transaction.Domain.Repository;
using Renty.Server.Transaction.Infrastructer;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// --- Options Pattern 설정 추가 ---
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("Settings") // "Setting" 섹션과 매핑
);

// DI 클래스 연결
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ITradeOfferRepository, TradeOfferRepository>();
builder.Services.AddScoped<IWishListQuery, WishListQuery>();
builder.Services.AddScoped<IWishListRepository, WishListRepository>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IProductQuery, ProductQuery>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MyService>();


// swagger 설정
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // JWT 인증을 위한 보안 정의 추가
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br/> 
                      Enter 'Bearer' [space] and then your token in the text input below. <br/>
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization", // 요청 헤더 이름
        In = ParameterLocation.Header, // 헤더에 위치
        Type = SecuritySchemeType.ApiKey, // 실제로는 ApiKey 타입으로 지정하지만, 동작은 Bearer 인증처럼 함
        Scheme = "Bearer" // 스킴 이름
    });

    // 전역적으로 모든 API에 인증 요구 사항 추가 (선택 사항, 또는 특정 API에만 적용 가능)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // 위에서 정의한 보안 스킴의 ID
                },
                Scheme = "oauth2", // 이 부분은 Swagger UI 표시에 영향을 줌
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>() // 필요한 scope가 있다면 여기에 추가 (일반적인 JWT Bearer는 빈 리스트)
        }
    });
});

// SignalR 설정
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Enum을 문자열로 변환하는 JsonStringEnumConverter 추가
});

// enum을 문자열로 변환하는 JsonStringEnumConverter 추가
builder.Services.AddControllers()
    .AddJsonOptions(options => // JSON 옵션 설정
    {
        // 모든 Enum 타입을 문자열로 변환하도록 JsonStringEnumConverter 추가
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // (선택적) JsonStringEnumConverter 생성자에 NamingPolicy를 지정하여
        // CamelCase 등의 네이밍 규칙을 적용할 수도 있습니다.
        // 예: options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// 1. DB 등록
builder.Services.AddDbContext<RentyDbContext>();

// 2. ASP.NET Core Identity 서비스 등록 (Users 클래스 사용)
builder.Services.AddIdentity<Users, IdentityRole>(options => {
    // UserName 값 제한 해제
    options.User.AllowedUserNameCharacters = null;
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

// 2. << 중요 >> JWT 기반 인증 설정으로 변경
builder.Services.AddAuthentication(options =>
{
    // 기본 인증 스키마를 JWT Bearer로 설정
    // API 서버에서는 인증되지 않은 요청 시 로그인 페이지로 리디렉션하는 대신 401을 반환해야 함
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // 경우에 따라 추가
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true; // HttpContext.GetTokenAsync("access_token") 등으로 토큰 접근 가능하게 함 (선택 사항)
        options.RequireHttpsMetadata = builder.Environment.IsProduction(); // 운영 환경에서는 HTTPS 강제 (권장)

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, // 발급자 검증
            ValidateAudience = true, // 대상 검증
            ValidateLifetime = true, // 만료 시간 검증 <<-- 이게 ExpireTimeSpan과 유사한 역할
            ValidateIssuerSigningKey = true, // 서명 키 검증 (필수)

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

            // ClockSkew: 토큰 만료 시간을 검증할 때 허용하는 시간 오차 (기본값 5분)
            // 짧은 만료 시간의 토큰을 사용하고, 만료 직후 요청이 실패하는 것을 방지하기 위해 약간의 유예를 둘 수 있음
            // ClockSkew = TimeSpan.Zero // 오차 없이 정확히 만료시키려면
        };
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
var imagePath = Path.Combine(settings["DataStorage"]!, settings["ImagesFolder"]!);
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

app.UseWebSockets();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
