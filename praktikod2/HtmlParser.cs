using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class HtmlParser
    {
        public HtmlElement Parse(string html)
        {
           
            var parts = Regex.Split(html, @"(<[^>]+>)")
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToList();

            HtmlElement root = new HtmlElement
            {
                Name = "root"
            };

            HtmlElement current = root;

            foreach (string part in parts)
            {
                if (part.StartsWith("</"))
                {
                    string closingName = Regex.Match(
                        part, @"^</\s*([^\s>]+)").Groups[1].Value.ToLowerInvariant();

                    // The opening tag of an unknown tag is skipped, so its closing tag must not go up a level
                    if (!HtmlHelper.Instance.HtmlTags.Contains(closingName))
                        continue;

                    // </html> marks the end of the document
                    if (closingName == "html")
                        break;

                    if (current.Parent != null)
                    {
                        current = current.Parent;
                    }
                }
                else if (part.StartsWith("<"))
                {
                    string content = part.Trim('<', '>', ' ');

                    string tagName = Regex.Match(
                        content, @"^[^\s/]+").Value.ToLowerInvariant();
                    string attributesText = content.Substring(tagName.Length).Trim();
                    var attributes = Regex.Matches(
    attributesText,
    @"([^\s=/>]+)(?:\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+))?"
);
                    if (!HtmlHelper.Instance.HtmlTags.Contains(tagName))
                        continue;

                    HtmlElement element = new HtmlElement
                    {
                        Name = tagName,
                        Parent = current
                    };

                    foreach (Match attribute in attributes)
                    {
                        string name = attribute.Groups[1].Value;

                        string value = attribute.Groups[2].Value
                            .Trim('"', '\'');

                        element.Attributes.Add(attribute.Value);

                        if (name.Equals("id",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            element.Id = value;
                        }

                        if (name.Equals("class",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            element.Classes.AddRange(
                                value.Split(' ',
                                    StringSplitOptions.RemoveEmptyEntries));
                        }
                    }

                    current.Children.Add(element);
                    bool isSelfClosing =
                        part.EndsWith("/>") ||
                        HtmlHelper.Instance.HtmlVoidTags.Contains(tagName);

                    if (!isSelfClosing)
                    {
                        current = element;
                    }
                }
                else
                {
                    current.InnerHtml += part;
                }
            }

            return root;
        }
    }
}
