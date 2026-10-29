using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class HtmlElement
    {
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public List<string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new();
        public string InnerHtml { get; set; } = "";

        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                    queue.Enqueue(child);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

    }
}
