using CaritasReliefAPI;
using CaritasReliefAPI.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var key = config["Jwt:key"];
var audience = config["Jwt:audience"];
var issuer = config["Jwt:issuer"];
var connString = config["ConnectionStrings:SQL"];

builder.Services
    .AddDbContextPool<SQLContext>(op => op.UseSqlServer(connString));

builder.Services
    .AddGraphQL()
    .AddGraphQLServer()
    .RegisterDbContext<SQLContext>(DbContextKind.Resolver)
    .AddQueryType<Query>()
    .AddSorting()
    .AddFiltering()
    .AddProjections()
    .AddAuthorization();


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidAudience = audience,
            ValidIssuer = issuer
        };
    });

var app = builder.Build();

//app.UseHttpsRedirection();

//app.Urls.Add("http://10.0.2.15:5054");
app.Urls.Add("http://localhost:5054");

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapGraphQL();



app.Run();