using StaticWebEpiserverPlugin.RequiredCssOnly.Services;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RequiredCssOnlyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string folder = @"C:\code\RequiredCssOnlyApp\";

            RequiredCssOnlyService service = new RequiredCssOnlyService();

            var pageName = "page.html";
            var html = System.IO.File.ReadAllText(folder + pageName, System.Text.Encoding.UTF8);

            var linkMatches = Regex.Matches(html, "(?<link><link.*href=\"(?<url>[^\"]+)\"[^>]*>)");
            foreach (Match linkMatch in linkMatches)
            {
                var linkGroup = linkMatch.Groups["link"];
                if (!linkGroup.Success)
                {
                    continue;
                }

                var urlGroup = linkMatch.Groups["url"];
                if (!urlGroup.Success)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(urlGroup.Value) || !urlGroup.Value.EndsWith(".css"))
                {
                    continue;
                }

                var fileName = urlGroup.Value;

                var cssContent = System.IO.File.ReadAllText(folder + fileName, System.Text.Encoding.UTF8);
                var resultingCss = service.RemoveUnusedRules(cssContent, html);

                System.IO.File.WriteAllText(folder + fileName.Replace(".css", "-out.css"), resultingCss, System.Text.Encoding.UTF8);

                if (!string.IsNullOrEmpty(resultingCss))
                {
                    resultingCss = $"<style>{resultingCss}</style>";
                }

                html = html.Replace(linkGroup.Value, resultingCss);
            }

            System.IO.File.WriteAllText(folder + pageName.Replace(".html", "-out.html"), html, System.Text.Encoding.UTF8);
        }
    }
}
