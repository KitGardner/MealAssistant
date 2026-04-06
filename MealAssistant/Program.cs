using MealAssistant.Data;
using MealAssistant.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("SQLConnString");

builder.Services.AddControllers();
builder.Services.AddScoped<IIngredientRepo, IngredientRepo>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountIngredientRepo, AccountIngredientRepo>();
builder.Services.AddScoped<IAccountIngredientService, AccountIngredientService>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.Run();
