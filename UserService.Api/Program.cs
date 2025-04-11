using System.Reflection;
using Projeli.WikiService.Api.Extensions;
using UserService.Application.Profiles;
using UserService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserServiceCors(builder.Configuration, builder.Environment);
builder.Services.AddUserServiceSwagger();
builder.Services.AddUserServiceServices();
builder.Services.AddUserServiceRepositories();
builder.Services.AddControllers().AddUserServiceJson();
builder.Services.AddUserServiceAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfile)));
builder.Services.AddUserServiceOpenTelemetry(builder.Logging, builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseUserServiceSwagger();
}

app.UseUserServiceCors();
app.UseUserServiceAuthentication();
app.UseUserServiceOpenTelemetry();

app.Run();