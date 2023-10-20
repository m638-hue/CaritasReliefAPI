using CaritasReliefAPI.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reto;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var key = config["Jwt:Key"];
var audience = config["Jwt:Audience"];
var issuer = config["Jwt:Issuer"];
var connString = config["ConnectionStrings:SQLServer"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContextPool<SQLContext>(op => op.UseSqlServer(connString));

builder.Services.AddSingleton(new JwtService(key, issuer, audience));

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
