using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MyApi.Middlewares;
using MyApi.Services;
using Serilog;
using System.Diagnostics;

//התקנת תעודת אבטחה dotnet dev-certs https --trust

//? לעשות שלמנהל יהיה אופציה להוסיף מתנה למשתמש כלשהו (ע"י בחירת השם/תז שלו)

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי ה-Controllers
builder.Services.AddAuthentication(optiens =>
{
    optiens.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = TokenService.GetTokenValidationParameters();
});
builder.Services.AddAuthorization(cfg => 
{
    cfg.AddPolicy("Admin",
        policy => policy.RequireClaim("type","Admin"));
    cfg.AddPolicy("User",
        policy => policy.RequireClaim("type","User","Admin"));
});
builder.Services.AddControllers();
builder.Services.AddCurrentUser();
builder.Services.AddGiftJson();
builder.Services.AddUserJson();


// הוספת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() 
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();


builder.Host.UseSerilog(); // Use Serilog for logging
var app = builder.Build();

// קביעת המידלוואר לשרת את Swagger כנקודת JSON
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI(c =>
    // {
    //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //     c.RoutePrefix = string.Empty; // קבעי את Swagger UI בשורש האפליקציה
    // });
    app.UseSwaggerUI();

}

// app.UseMyLog();
app.Use(async (context, next) =>
{
    var sw = new Stopwatch();
    sw.Start();
    
    await next(context);

    sw.Stop();
    Log.Information("{Path} {Method} took {ElapsedMilliseconds}ms", context.Request.Path, context.Request.Method, sw.ElapsedMilliseconds);
});
app.UseMyErrorMiddleware();
//app.UseHttpsRedirection();
/*js*/
app.UseDefaultFiles();
app.UseStaticFiles();
/*js (remove "launchUrl" from Properties\launchSettings.json*/
app.UseRouting();

app.UseTokenMiddleware();

app.UseAuthentication();

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

