using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add logging 
builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Default Ping endpoint for Pong
app.MapGet("/", (ILogger logger) =>
{
    logger.Information("Pong's ping!");
    return "Pong live!";
});

// Send endpoint that posts to Pong and sends some data back 
app.MapPost("/send", async  (ILogger logger, HttpRequest request) =>
{
    var dataToWrite = string.Empty;

    // Get the request body. Using a plain text file, could also use a JSON and serialize/de-serlialize but this requires a type 
    // to be defined, and hence we stick to this for demo purposes
    using (var memStream = new MemoryStream())
    {
        await request.Body.CopyToAsync(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        dataToWrite = await new StreamReader(memStream).ReadToEndAsync();
    }

    // Form a text to write back, log
    dataToWrite = "Pong response - " + dataToWrite    ;
    logger.Information(dataToWrite);

    // Write to the shared data file
    var appPath = builder.Environment.ContentRootPath;
    var dataPath = Path.Combine(appPath, "data/data.txt");
    System.IO.File.AppendAllLines(dataPath, new string[] { dataToWrite + "  ::  " + DateTime.Now.ToString() });

    return dataToWrite;
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Using Request Logging as well
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();
app.Run();
