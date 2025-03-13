using System.Text;

namespace RandNumbers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            var rnd = new Random();
            int nr = 100; // Default value

            // Get - ����� �� ���� ������� �������
            // ���� ������� ����� ������� ������� �����
            // context.Response.WriteAsync

            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html";
                var sb = new StringBuilder();

                // ����� (unput) ��� �������� �����,
                // ����� ����� ���� ���� �� ������� "input" �� ������ ��� "dest"
                sb.Append($@"
                <form action=""/input?dest=linear"" method=""post"">
                <input type=""number"" name=""number"" value=""{nr}"" min=""1""> 
                <button type=""submit"">Set Number</button>
                </form>");

                // ������ ��� �����
                for (int i = 0; i < nr; i++)
                    sb.Append(i.ToString() + " ");
                sb.AppendLine();

                // ������ ������� ����� ���� Get �� ��������� "/rand"                 
                sb.Append(@"
                <form action=""/rand"" method=""get"">
                <button>random</button>
                </form>");

                await context.Response.WriteAsync(sb.ToString());
            });

            // Get - ����� �� ���� ������� /rand
            // ���� ������� ������� ������� ������� �����
            // context.Response.WriteAsync

            app.MapGet("/rand", async context =>
            {
                var sb = new StringBuilder();
                context.Response.ContentType = "text/html; charset=utf-8";

                // ����� ��� �������� ����� 'nr'
                // �� ����� ���������� �� ����������� ��� ��������� ����������                
                sb.Append($@"
                <form action=""/input?dest=random"" method=""post"">
                <input type=""number"" name=""number"" value=""{nr}"" min=""1"">
                <button type=""submit"">Set Number</button>
                </form>");

                // ����������� ��������� ����� � �� ��������� � ����� (sb) ��� ���������
                for (int i = 0; i < nr; i++)
                    sb.Append(rnd.Next(nr).ToString() + " ");
                sb.AppendLine();

                // ������ ������ �� ������� ����� ��� ������������ ������� "/" ������� "get" 
                sb.Append(@"
                <form action=""/"" method=""get"">
                <button>linear</button>
                </form>");

                await context.Response.WriteAsync(sb.ToString());
            });

            //Post - ����� � ��������� �����.
            //����� ������ ��������� ������ ����� ����� (form) � ������ ����� ����� (query)
            //� ������� �� ������� ("/") ��� ("/rand") �� ������������� ������� Get

            app.MapPost("/input", async context =>
            {
                var form = context.Request.Form;
                var dest = context.Request.Query["dest"];  // Get "dest" from query string

                if (int.TryParse(form["number"], out int inputNr) && inputNr > 0)
                {
                    nr = inputNr;  // Update the number based on user input
                }

                if (dest == "linear")
                    context.Response.Redirect("/");
                else if (dest == "random")
                    context.Response.Redirect("/rand");
                else
                    throw new Exception("Invalid destination");
            });

            app.Run();
        }
    }
}
