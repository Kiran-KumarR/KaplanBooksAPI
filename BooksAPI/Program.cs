using BooksAPI.Interface;
using BooksAPI.Models;
using BooksAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<ContextDb>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

//builder.Services.AddScoped<IAuthorInterface,ContextDb >();//dependency injection added 
builder.Services.AddScoped<IBooksInfoService, BooksInfoService>();//dependency injection added 
builder.Services.AddScoped<IBooksDatabaseService,BooksDatabaseService >();//dependency injection added 



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
