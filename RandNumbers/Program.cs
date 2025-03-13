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

            // Get - запит на зміст основної сторінки
            // Зміст сторінки формує відповідь сервера через
            // context.Response.WriteAsync

            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html";
                var sb = new StringBuilder();

                // Форма (unput) для введення даних,
                // Формує запит типу пост за адресою "input" та передає дані "dest"
                sb.Append($@"
                <form action=""/input?dest=linear"" method=""post"">
                <input type=""number"" name=""number"" value=""{nr}"" min=""1""> 
                <button type=""submit"">Set Number</button>
                </form>");

                // генерує ряд чисел
                for (int i = 0; i < nr; i++)
                    sb.Append(i.ToString() + " ");
                sb.AppendLine();

                // Кнопка викликає запит типу Get на підсторінку "/rand"                 
                sb.Append(@"
                <form action=""/rand"" method=""get"">
                <button>random</button>
                </form>");

                await context.Response.WriteAsync(sb.ToString());
            });

            // Get - запит на зміст сторінки /rand
            // Зміст стоірнки визначає відповідь сервера через
            // context.Response.WriteAsync

            app.MapGet("/rand", async context =>
            {
                var sb = new StringBuilder();
                context.Response.ContentType = "text/html; charset=utf-8";

                // Форма для введення числа 'nr'
                // це число передається як максимальне при генерації випадкових                
                sb.Append($@"
                <form action=""/input?dest=random"" method=""post"">
                <input type=""number"" name=""number"" value=""{nr}"" min=""1"">
                <button type=""submit"">Set Number</button>
                </form>");

                // генерування рандомних чисел і їх додавання у рядок (sb) для виведення
                for (int i = 0; i < nr; i++)
                    sb.Append(rnd.Next(nr).ToString() + " ");
                sb.AppendLine();

                // Додаємо кнопку що викликає метод для завантаження сторінки "/" методом "get" 
                sb.Append(@"
                <form action=""/"" method=""get"">
                <button>linear</button>
                </form>");

                await context.Response.WriteAsync(sb.ToString());
            });

            //Post - запит з введенням даних.
            //Запит приймає параметри введені через форму (form) і введені через чергу (query)
            //І повертає на сторінки ("/") або ("/rand") які завантажуться методом Get

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
