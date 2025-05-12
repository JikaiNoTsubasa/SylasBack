using System.Text;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
builder.Services.AddScoped<PreferenceManager>();
builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<GlobalParameterManager>();
builder.Services.AddScoped<IssueManager>();
builder.Services.AddScoped<QuestManager>();

// Add policy management
builder.Services.AddSingleton<IAuthorizationPolicyProvider, GrantPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, GrantHandler>();

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
var hashService = scope.ServiceProvider.GetRequiredService<HashService>();
var context = scope.ServiceProvider.GetRequiredService<SyContext>();

// Init grants
log.Info("Init grants");
var allPolicies = PolicyScanner.GetAllPoliciesFromControllers(typeof(Program).Assembly);
if (allPolicies != null && allPolicies.Count > 0){
    SyProjectInit.InitGrants(context, allPolicies);
}else{
    log.Warn("No policies found");
}

// Init admin role
log.Info("Init admin role");
SyProjectInit.InitAdminRole(context);

// Init admin
log.Info("Init admin");
SyProjectInit.CreateAdminIfNotExist(context, hashService);

// Init user preferences
log.Info("Init user preferences");
SyProjectInit.InitUserPreferences(context);

// Init global parameters
log.Info("Init global parameters");
SyProjectInit.InitGlobalParameters(context);

// Init default customers
log.Info("Init default customers");
SyProjectInit.InitDefaultCustomers(context);

log.Info("Sylas API started");
app.Run();
