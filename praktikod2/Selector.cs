using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class Selector
    {
        public string? TagName { get; set; }

        public string? Id { get; set; }

        public List<string> Classes { get; set; } = new();

        public Selector? Parent { get; set; }

        public Selector? Child { get; set; }


        public static Selector Parse(string query)
        {
            string[] parts = query.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Selector cannot be empty");

            Selector root = new Selector();

            Selector current = root;

            for (int i = 0; i < parts.Length; i++)
            {

                var tokens = Regex.Matches(
                    parts[i],
                    @"([#.])?([^#.]+)"
                );

                foreach (Match token in tokens)
                {
                    string prefix = token.Groups[1].Value;
                    string value = token.Groups[2].Value;

                    if (prefix == "#")
                    {
                        current.Id = value;
                    }
                    else if (prefix == ".")
                    {
                        current.Classes.Add(value);
                    }
                    else
                    {
                        if (HtmlHelper.Instance.HtmlTags.Contains(
                            value, StringComparer.OrdinalIgnoreCase))
                        {
                            current.TagName = value;
                        }
                        else
                        {
                            throw new ArgumentException(
                                $"Invalid HTML tag: {value}");
                        }
                    }
                }


                if (i < parts.Length - 1)
                {
                    Selector child = new Selector();

                    current.Child = child;
                    child.Parent = current;

                    current = child;
                }
            }

            return root;
        }

    }
}
