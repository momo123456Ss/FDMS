using CloudinaryDotNet;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.AccountRepository;
using FDMS.Repository.AccountSessionRepository;
using FDMS.Repository.DocumentTypeRepository;
using FDMS.Repository.FDHistoryRepository;
using FDMS.Repository.FlightDocumentRepository;
using FDMS.Repository.FlightRepository;
using FDMS.Repository.GeneralRepository;
using FDMS.Repository.GroupPermission;
using FDMS.Repository.RoleRepository;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.CloudinaryService;
using FDMS.Service.JWTService;
using FDMS.Service.MailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
#region
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Using Bearer scheme {\"bearer {token}\"}",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,


        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddCors(p => p.AddPolicy("CrossCors", build =>
{
    build.WithOrigins(builder.Configuration["Cross-Origin:Domain"]).AllowAnyMethod().AllowAnyHeader();
}));
//Mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
//Cloudinary
builder.Services.AddSingleton(provider =>
{
    var account = new CloudinaryDotNet.Account(
        builder.Configuration["Cloudinary:Cloud-name"],
        builder.Configuration["Cloudinary:API-key"],
        builder.Configuration["Cloudinary:API-secret"]
    );
    return new Cloudinary(account);
});
//Mapper
builder.Services.AddAutoMapper(typeof(Program));
//Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireOwner", policy => policy.RequireRole("Owner".ToLower()));
    options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Quản trị viên".ToLower(), "System Admin".ToLower(), "Owner".ToLower()));
    options.AddPolicy("RequirePilot", policy => policy.RequireRole("Phi công".ToLower(), "Pilot".ToLower(), "Quản trị viên".ToLower(), "System Admin".ToLower(), "Owner".ToLower()));
    options.AddPolicy("RequireFlightAttendant", policy => policy.RequireRole("Tiếp viên hàng không".ToLower(), "Flight attendant".ToLower(), "Quản trị viên".ToLower(), "System Admin".ToLower(), "Owner".ToLower()));
    options.AddPolicy("RequireGOStaff", policy => policy.RequireRole("Nhân viên GO".ToLower(), "GO staff".ToLower(), "Quản trị viên".ToLower(), "System Admin".ToLower(), "Owner".ToLower()));
});
#endregion
//Service - Repository
#region
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IGeneralRepository, GeneralRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IGroupPermissionRepository, GroupPermissionRepository>();
builder.Services.AddScoped<ISystemNoficationRepository, SystemNoficationRepository>();
builder.Services.AddScoped<IAccountSessionRepository, AccountSessionRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IFlightDocumentRepository, FlightDocumentRepository>();
builder.Services.AddScoped<IFDRepository, FDRepository>();

#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FDMSContext>(option
    => option.UseSqlServer(builder.Configuration.GetConnectionString("FDMS")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
