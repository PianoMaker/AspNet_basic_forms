namespace PagesWithForms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            string name ="";
            int age = 0;

            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<h1>Hello World!</h1>");
                //��������� ��������� ����� �����
                await context.Response.WriteAsync(@"
    <div>
        <form action=""/reg"" method=""post"">
            <label for=""fname"">First name:</label>
            <input style=margin:5px type=""text"" id=""fname"" name=""fname""><br/>
            <label for=""age"">Age:</label>
            <input type=""text"" id=""age"" name=""age""><br/><br/>
            <input type=""submit"" value=""Submit"">
        </form>
    </div>");
            });

            // action p ������� post ������� reg, ��������� name �� age ����������� � ����
            // ������������ �� ������� user
            app.MapPost("/reg", async context =>
            {
                var form = await context.Request.ReadFormAsync();
                name = form["fname"];
                age = Int32.Parse(form["age"]);
                //��������������� �� ������� user
                context.Response.Redirect("/user");
            });


            app.MapGet("/user", async context =>
            {
                context.Response.ContentType = "text/html, charset = utf-8";
                //������������� ����
                await context.Response.WriteAsync(
                    @$"<h1>Info</h1>
                    <p>text = {name}</p>
                    <p>number = {age}</p>");
            });

            app.Run();
        }
    }
}
