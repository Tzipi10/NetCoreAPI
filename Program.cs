using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MyApi.Middlewares;
using MyApi.Services;

//התקנת תעודת אבטחה dotnet dev-certs https --trust

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

app.UseMyLog();
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

