using System.Reflection;
using Projeli.UserService.Api.Extensions;
using Projeli.UserService.Application.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserServiceCors(builder.Configuration, builder.Environment);
builder.Services.AddUserServiceSwagger();
builder.Services.AddUserServiceServices();
builder.Services.AddUserServiceRepositories();
builder.Services.AddControllers().AddUserServiceJson();
builder.Services.AddUserServiceDatabase(builder.Configuration, builder.Environment);
builder.Services.AddUserServiceAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfile)));
builder.Services.AddUserServiceOpenTelemetry(builder.Logging, builder.Configuration);
builder.Services.UseUserServiceRabbitMq(builder.Configuration);

var app = builder.Build();

app.UseUserServiceMiddleware();
app.MapControllers();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseUserServiceSwagger();
}

app.UseUserServiceCors();
app.UseUserServiceAuthentication();
app.UseUserServiceDatabase();
app.UseUserServiceOpenTelemetry();

app.Run();