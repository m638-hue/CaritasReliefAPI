using CaritasReliefAPI;
using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var key = config["Jwt:key"];
var audience = config["Jwt:audience"];
var issuer = config["Jwt:issuer"];
var connString = config["ConnectionStrings:SQLServer"];

builder.Services
    .AddDbContextPool<SQLContext>(op => op.UseSqlServer(connString));

builder.Services
    .AddGraphQL()
    .AddGraphQLServer()
    .RegisterDbContext<SQLContext>(DbContextKind.Resolver)
    .AddQueryType<Query>()
    .AddTypeExtension<DonanteExtension>()
    .AddSorting()
    .AddFiltering()
    .AddProjections()
    .AddAuthorization(options =>
    {
        options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    });


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

//app.UseHttpsRedirection()

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapGraphQL();



app.Run();