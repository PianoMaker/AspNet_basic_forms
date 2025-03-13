using Microsoft.Extensions.Primitives;
using System.Text;

// ����-�������� "�² ���в���"

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        string str = "Main";

        // ������� ������� �������
        // ��� ������ ������� ����������� Result.Content 

        app.MapGet("/", () =>
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<h1>Hello World!</h1><h4>{str}</h4>");
            sb.Append("<a href=\"/index\">click me</p>");
            Console.WriteLine( sb.ToString() );
            return Results.Content(sb.ToString(), "text/html");
        });


        // ������� ��������� �������, ��� ����������� ���� � ��������� ����� ����� "/index"
        // ��� ������ ������� ����������� context.Response.WriteAsync
        app.MapGet("/index", async context =>        {
            
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("hello from index");
            await context.Response.WriteAsync("<a href=\"/\">return</p>");
        });

        app.Run();
    }
}
