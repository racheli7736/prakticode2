using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class HtmlHelper
    {
        private static readonly Lazy<HtmlHelper> instance =
       new(() => new HtmlHelper());

        public static HtmlHelper Instance => instance.Value;

        public string[] HtmlTags { get; }
        public string[] HtmlVoidTags { get; }

        private HtmlHelper()
        {
            string path1 = Path.Combine(
                AppContext.BaseDirectory, "HtmlTags.json");

            string path2 = Path.Combine(
                AppContext.BaseDirectory, "HtmlVoidTags.json");

            HtmlTags = JsonSerializer.Deserialize<string[]>(
                File.ReadAllText(path1)) ?? Array.Empty<string>();

            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(
                File.ReadAllText(path2)) ?? Array.Empty<string>();
        }
    }
}
