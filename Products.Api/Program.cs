using FluentValidation.AspNetCore;
using Products.Api.Middlewares;
using Products.BusinessLogic;
using Products.BusinessLogic.Mapper;
using Products.DataAccess;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add infrascture services
builder.Services.AddInfrastructure();
builder.Services.AddCore();
builder.Services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly);
builder.Services.AddAutoMapper(cfg => { }, typeof(UpdateRequestMappingProfile).Assembly);
builder.Services.AddAutoMapper(cfg => { }, typeof(AddRequestMappingProfile).Assembly);


// Add controllers
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Fluent Validations
builder.Services.AddFluentValidationAutoValidation();

// Add API explorer services
builder.Services.AddEndpointsApiExplorer();

// Add swagger generation services to create swagger specification
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Build the web application
var app = builder.Build();

// Add exception handling middleware
app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();
app.Run();

