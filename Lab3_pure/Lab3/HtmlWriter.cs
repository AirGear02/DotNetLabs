using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Interfaces;

namespace Lab3
{
    public class HtmlWriter
    {
        public static readonly string[] Scripts = 
        { 
            "/lib/jquery/dist/jquery.min.js",
            "/lib/bootstrap/dist/js/bootstrap.js"
        };

        public static readonly string[] Styles =
        {
            "/lib/bootstrap/dist/css/bootstrap.min.css",
            "/css/site.css"
        };
        public static readonly string HeaderFilePath = @"./wwwroot/header.html";

        private delegate Task PrintPageContent(HttpContext context);

        public static async Task PrintHead(HttpContext context, string title)
        {
            var res = context.Response;
            await res.WriteAsync("<head><meta charset=\"utf-8\" />" +
                "<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1.0\" />");

            foreach(string style in Styles)
            {
                await res.WriteAsync($"<link rel=\"stylesheet\" href=\"{style}\" />");
            }
            await res.WriteAsync($"<title>{title}</title>");

            await res.WriteAsync("</head>");
        }

        public static async Task PrintHeader(HttpContext context)
        {
            string[] lines = await File.ReadAllLinesAsync(HeaderFilePath);
            foreach (string line in lines)
            {
                await context.Response.WriteAsync(line);

            }
        }

        private static async Task PrintPage(HttpContext context, PrintPageContent content)
        {
            await context.Response.WriteAsync("<html>");

            await HtmlWriter.PrintHead(context, "Start");
            await context.Response.WriteAsync("<body>");
            await HtmlWriter.PrintHeader(context);
            await context.Response.WriteAsync("<div class=\"container\"><main role = \"main\" class=\"pb-3\">");
            await content(context);
            foreach(var script in Scripts)
            {
                await context.Response.WriteAsync($"<script src=\"{script}\"></script>");
            }

            await context.Response.WriteAsync("</main></div></body></html>");
        }

        private static async Task PrintMainPageContent(HttpContext httpContext)
        {
            using(var context = new PhotoStudioContext() )
            {
                var query = context.Orders
                 .Join(context.Clients, orders => orders.ClientId, clients => clients.Id,
                 (o, c) => new
                 {
                     OptionId = o.OptionId,
                     Name = c.Name,
                     Surname = c.Surname,
                     Quantity = o.Quantity,
                     DateStart = o.DateStart,
                     DateFinish = o.DateFinish,
                     Id = c.Id
                 })

                 .Join(context.Options, c => c.OptionId, o => o.Id, (c, o) => new
                 {
                     Option = o.Title,
                     Name = c.Name,
                     Surname = c.Surname,
                     Quantity = c.Quantity,
                     DateStart = c.DateStart,
                     DateFinish = c.DateFinish,
                     Price = c.Quantity * o.Price,
                     Id = c.Id
                 })
                 .ToList()
                 .GroupBy(table => new { table.Surname, table.Name, table.Id });

                await httpContext.Response.WriteAsync("<div class=\"row\"><div class=\"col-4\"><div class=\"list-group\" id=\"list-tab\" role=\"tablist\">");
                foreach(var client in query)
                {
                    await httpContext.Response.WriteAsync($"<a class=\"list-group-item list-group-item-action\"  id=\"list-{client.Key.Id}-list\"" +
                       $"data-toggle=\"list\" href=\"#list-{client.Key.Id}\" role=\"tab\""+
                       $"aria-controls=\"{client.Key.Id}\">{client.Key.Name} {client.Key.Surname}</a>");
                }
                await httpContext.Response.WriteAsync("</div></div ><div class=\"col-8\"><div class=\"tab-content\" id=\"nav-tabContent\">");
                foreach (var client in query)
                {
                    await httpContext.Response.WriteAsync($"<div class=\"tab-pane fade\" id=\"list-{client.Key.Id}\" role=\"tabpanel\"" + 
                         $"aria-labelledby=\"list -{client.Key.Id}-list\"><table class=\"table table-striped table-dark\">" +
                        "<thead><tr><th>Назва послуги</th><th>Дата замовлення </th><th> Дата виконанння </th><th>Ціна </th> </tr></thead><tbody>");
                    foreach (var option in client)
                    {
                        await httpContext.Response.WriteAsync($"<tr><td>{ option.Option} </td ><td>{option.DateStart.ToShortDateString()}" +
                            $"</td> <td> {option.DateFinish.ToShortDateString()}</td><td>{ decimal.Round(option.Price, 2)}</td> </tr> ");  
                    }
                    await httpContext.Response.WriteAsync($"<tr align=\"center\"><td colspan=\"4\">Загальна кількість замовлень: {client.Count()}</td></tr></tbody>" +
                        "</table></div>");
                }

                await httpContext.Response.WriteAsync("</div></div></div>");


            }
        }

        private static async Task PrintTable(HttpContext httpContext, List<IPrintableToHtmlRow> objects, string[] columnsTitle)
        {
            {
                await httpContext.Response.WriteAsync("<table class=\"table table-striped table-dark\"><thead><tr>");
                foreach(var title in columnsTitle)
                {
                    await httpContext.Response.WriteAsync($"<th>{title}</th>");
                }
                await httpContext.Response.WriteAsync("</tr></thead><tbody>");

                foreach(var ob in objects)
                {
                    await httpContext.Response.WriteAsync(ob.PrintToHtmlRow());
                }

                await httpContext.Response.WriteAsync("</tbody></table>");
            }
        }

        private static async Task PrintClientsPage(HttpContext httpContext)
        {
            string[] columns = { "Ім'я", "Прізвище", "По-батькові", "Адреса", "Номер телефону" };            
            using (var context = new PhotoStudioContext())
            {
                await PrintTable(httpContext, context.Clients.ToList<IPrintableToHtmlRow>(), columns);
            } 
        }

        private static async Task PrintOptionsPage(HttpContext httpContext)
        {
            string[] columns = { "Назва", "Опис", "Ціна"};
            using (var context = new PhotoStudioContext())
            {
                await PrintTable(httpContext, context.Options.ToList<IPrintableToHtmlRow>(), columns);
            }
        }

        public static async  Task MainPage(HttpContext context)
        {
            await PrintPage(context, PrintMainPageContent);
        }

        public static async Task ClientsPage(HttpContext context)
        {
            await PrintPage(context, PrintClientsPage);
        }

        public static async Task OptionsPage(HttpContext context)
        {
            await PrintPage(context, PrintOptionsPage);
        }


    }
}
