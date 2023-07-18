using Grupo2_Identity_Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddIdentity();
builder.AddToken();

var app = builder.Build();
var environment = app.Environment;

app.UseExceptionHandling(environment);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
