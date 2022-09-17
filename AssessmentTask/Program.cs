using MainModuleContext.Models;
using MainModuleDataServices.Repository;
using MainModuleInterFace.IDataServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
     // services.AddDefaultIdentity<IdentityUser>()
     .AddEntityFrameworkStores<MainModuleContext.Context.MainModuleContext>()
     .AddDefaultTokenProviders();

//builder.Services.AddDbContext<MainModuleContext.Context.MainModuleContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<MainModuleContext.Context.MainModuleContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration["ConnectionString"],
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5);
        })
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

//builder.Services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(
//    serviceProvider => serviceProvider.GetRequiredService<MainModuleContext.Context.MainModuleContext>());


builder.Services.AddScoped<IApplicationUserManager, MainModuleDataServices.Repository.AccountRepository>();
builder.Services.AddTransient<IEmployeeRepository, MainModuleDataServices.Repository.EmployeeRepository>();



builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("EnableCORS");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();