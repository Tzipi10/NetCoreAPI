using System.Net;
using System.Net.Mail;
namespace MyApi.Middlewares;
public class MyErrorMiddleware
{
    private RequestDelegate next;

    public MyErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Items["success"] = false;
        bool success = false;
        try
        {
            await next(context);
            context.Items["success"] = true;
        }
        catch (ApplicationException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            try
            {
                //c.Response.StatusCode = 500;
                // קוד לשליחת המייל
                //לא באמת עובד...
                MailMessage mail = new MailMessage("malki566588@gmail.com", "malki566588@gmail.com", $"תקלה בשרת {e.Message}", "פנה לתמיכה התכנית");

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("malki566588.com", "");
                    smtp.EnableSsl = true;

                    Console.WriteLine("before send mail");
                    smtp.Send(mail);
                    Console.WriteLine("after send mail");
                    Console.WriteLine("המייל נשלח בהצלחה");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("שגיאה בשליחת המייל: " + ex.Message);
            }
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static WebApplication UseMyErrorMiddleware(
        this WebApplication app)
    {
        app.UseMiddleware<MyErrorMiddleware>();
        return app;
    }
}



