using CmciSiteAPI.Controllers;
using CmciSiteAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

byte[] secretByte = new byte[64];
using (var random =  RandomNumberGenerator.Create())
{
    random.GetBytes(secretByte);
}

string secretKey = Convert.ToBase64String(secretByte); 

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CmciDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidateAudience =  false,
        ValidateLifetime = true,
        ValidIssuer = "Mon essaie"
    };

});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;

}).AddUserStore<BaseDeDonneesController>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipelinert
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
