using log4net.Config;
using log4net;
using Microsoft.AspNetCore.ResponseCompression;
using QCSMobile2024.Shared.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using QCSMobile2024.Shared.Utilities;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

var builder = WebApplication.CreateBuilder(args);
// Configure log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));
builder.Services.AddSingleton<ILog>(LogManager.GetLogger(typeof(Program)));

var Log = LogManager.GetLogger(typeof(Program));
Log.Info($"I'm alive. {DateTime.Now}");

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlServer("Server=192.168.29.120;Database=ABA_Stag;User Id=sa;Password=Munn1n!;TrustServerCertificate=True;", sqlServerOptions => sqlServerOptions.CommandTimeout(90));
});

// For AutoMapper
builder.Services.AddSingleton<IMapper>(new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}).CreateMapper());

// For Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// For Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
