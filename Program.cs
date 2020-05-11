using StaticWebEpiserverPlugin.RequiredCssOnly.Services;
using System.Collections.Generic;

namespace RequiredCssOnlyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string folder = @"C:\code\RequiredCssOnlyApp\";

            RequiredCssOnlyService service = new RequiredCssOnlyService();

            var html = System.IO.File.ReadAllText(folder + "page.html", System.Text.Encoding.UTF8);

            var cssFiles = new List<string>(new []{
                @"bootstrap.css",
                @"bootstrap-responsive.css",
                @"style.css",
                @"editmode.css",
                @"media.css"
            });

            foreach (string fileName in cssFiles)
            {
                var cssContent = System.IO.File.ReadAllText(folder + fileName, System.Text.Encoding.UTF8);
                var resultingCss = service.RemoveUnusedRules(cssContent, html);

                System.IO.File.WriteAllText(folder + fileName + "-out.css", resultingCss, System.Text.Encoding.UTF8);

                if (!string.IsNullOrEmpty(resultingCss))
                {
                    html = html.Replace($"<link href=\"" + fileName + "\" rel=\"stylesheet\" />", $"<style>{resultingCss}</style>");
                }
                else
                {
                    html = html.Replace($"<link href=\"" + fileName + "\" rel=\"stylesheet\" />", string.Empty);
                }
            }

            System.IO.File.WriteAllText(folder + @"page.html-out.html", html, System.Text.Encoding.UTF8);
        }
    }
}
