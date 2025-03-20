using Microsoft.OpenApi.Models;
using MyApi.Middlewares;
using MyApi.Services;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי ה-Controllers
builder.Services.AddControllers();
builder.Services.AddGiftJson();
builder.Services.AddUserJson();

// הוספת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

