using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using QuizChallenge.Models;


var builder = WebApplication.CreateBuilder(args);

// Register the database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Đăng ký HttpClient, Repositories và BackgroundService
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OData services
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
    .Filter().Select().Expand().OrderBy().SetMaxTop(100).Count());
// Define the EDM (Entity Data Model) for OData
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Quiz>("Quiz");
    return builder.GetEdmModel();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
