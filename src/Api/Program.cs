using Api.Installers;
using Application.Common.Constants;
using Application.Common.Hubs;
using Infrastructure.Persistence;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InstallServicesInAssembly(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSignalR();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddSingleton<UserConnectionManager>();
var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VGA API V1");
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseCors(CorsConstant.PolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenValidationMiddleware>();
app.MapHub<NotificationHub>("/notification_hub");

app.MapControllers();

app.Run();
