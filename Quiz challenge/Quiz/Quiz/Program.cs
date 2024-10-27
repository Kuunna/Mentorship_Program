using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using QuizChallenge.Models;
using QuizChallenge.Services;
using QuizChallenge.Repositories;


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


// Add repositories to the DI container
builder.Services.AddScoped<AnswerRepository>(provider => new AnswerRepository(connectionString));
builder.Services.AddScoped<QuestionRepository>(provider => new QuestionRepository(connectionString));
builder.Services.AddScoped<QuizRepository>(provider => new QuizRepository(connectionString));
builder.Services.AddScoped<QuizTagRepository>(provider => new QuizTagRepository(connectionString));
builder.Services.AddScoped<RoleRepository>(provider => new RoleRepository(connectionString));
builder.Services.AddScoped<TagRepository>(provider => new TagRepository(connectionString));
builder.Services.AddScoped<TopicRepository>(provider => new TopicRepository(connectionString));
builder.Services.AddScoped<TypeRepository>(provider => new TypeRepository(connectionString));
builder.Services.AddScoped<UserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddScoped<UserQuizRepository>(provider => new UserQuizRepository(connectionString));
builder.Services.AddScoped<UserRoleRepository>(provider => new UserRoleRepository(connectionString));



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
