using FTPSearch.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureLibraries();

app.UseHttpsRedirection();

app.Run();
