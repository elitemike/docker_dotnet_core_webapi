using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(
          options => options.AddPolicy("AllowCors",
          builder =>
          {
              builder
              .WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
          }));

builder.Services.AddMvc((options) => { options.EnableEndpointRouting = false; });
builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("AllowCors");
app.UseStaticFiles();

app.UseRouting();
app.UseMvc();

app.MapHub<SandboxHub>("/hub");

app.Run();

