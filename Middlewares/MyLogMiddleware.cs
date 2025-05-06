using System.Diagnostics;

namespace MyApi.Middlewares;

public class MyLogMiddleware
{
    private RequestDelegate next;

    public MyLogMiddleware(RequestDelegate next){
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //await context.Response.WriteAsync($"start my log middleware\n");
        var sw = new Stopwatch();
        sw.Start();
        await next(context);
        Console.WriteLine($"{context.Request.Path}.{context.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" Success: {context.Items["success"]}");
        //await context.Response.WriteAsync("end my Log Middleware\n");
    }
}

public static class MyLogMiddlewareHelper
{
    public static void UseMyLog(this IApplicationBuilder application)
    {
        application.UseMiddleware<MyLogMiddleware>();

    }
    public static void UseTokenMiddleware(this IApplicationBuilder application)
    {
        application.UseMiddleware<TokenMiddleware>();
        
    }
}