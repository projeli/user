using System.Reflection;
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

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseUserServiceSwagger();
}

app.UseUserServiceCors();
app.UseUserServiceAuthentication();

app.Run();