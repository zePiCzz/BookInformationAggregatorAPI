using BookInformationAggregatorAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Optional: Removes default logging providers if not needed
builder.Logging.AddConsole();    // Logs to the console
builder.Logging.AddDebug();      // Logs to Visual Studio Debug Output

// Add services to the container
builder.Services.AddControllers();

// Register the BookService as a singleton
builder.Services.AddSingleton<BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed error pages in development mode
    app.UseHsts(); // Optional: Enforce HSTS in development
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
