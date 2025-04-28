using System.Text;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobControllers;
using sylas_api.JobManagers;
using sylas_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add log4net
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// Exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Required

ILog log = LogManager.GetLogger(typeof(Program));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<HashService>();
builder.Services.AddDbContext<SyContext>();

// Add managers and controllers
builder.Services.AddScoped<SyManager>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ProjectManager>();
builder.Services.AddScoped<TimeManager>();

builder.Services.AddControllers(o => {
        o.ModelBinderProviders.Insert(0, new UTCDateTimeBinderProvider());
    })
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.;
        // options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Disables the string conversions from empty to null
builder.Services.AddMvc().AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new CustomMetadataProvider()));

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

// Disable CORS
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Init default data
log.Info("Init default data");
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<SyContext>();
var admin = context.Users.FirstOrDefault(u => u.Email == "stephane.biehler.priv@gmail.com");
if (admin == null)
{
    log.Info("Init default admin");
    var hashService = scope.ServiceProvider.GetRequiredService<HashService>();    
    var password = hashService.HashPassword("test");
    var user = new User { Email = "stephane.biehler.priv@gmail.com", Password = password, Name = "Jikai" };
    user.MarkCreated(0);
    context.Users.Add(user);
    context.SaveChanges();
}

// Init global parameters
log.Info("Init global parameters");
SyProjectInit.InitGlobalParameters(context);

log.Info("Sylas API started");
app.Run();
