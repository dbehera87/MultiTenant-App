using License.Application.Commands;
using License.Application.Queries;
using License.Persistence.Config;
using License.Persistence.Repositories;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Authentication for Jwt token using Azure Ad and enable token acquisition to call downstream apis
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.Configure<SqlConnectionConfig>(builder.Configuration.GetSection("SqlConnectionConfig"));

//Dependency Injection for Repositories
builder.Services.AddTransient<ILicenseRepository, LicenseRepository>();

//Dependency Injection for License Commands and Queries
builder.Services.AddTransient<IssueLicense>();
builder.Services.AddTransient<RenewLicense>();
builder.Services.AddTransient<GetLicenseById>();
builder.Services.AddTransient<GetLicenses>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
