using DatingApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConnection")));
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipelinesssssss

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
