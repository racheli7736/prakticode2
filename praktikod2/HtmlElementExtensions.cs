using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktikod2
{
    internal static class HtmlElementExtensions
    {

        private static bool IsMatch(HtmlElement element, Selector selector)
        {
            if (selector.TagName != null &&
                !element.Name.Equals(
                    selector.TagName, StringComparison.OrdinalIgnoreCase))
                return false;

            if (selector.Id != null &&
                element.Id != selector.Id)
                return false;

            foreach (string className in selector.Classes)
            {
                if (!element.Classes.Contains(className))
                    return false;
            }

            return true;
        }

        public static HashSet<HtmlElement> FindElements(
            this HtmlElement root,
            Selector selector)
        {
            HashSet<HtmlElement> results = new();

            FindRecursive(root, selector, results);

            return results;
        }

        private static void FindRecursive(
            HtmlElement root,
            Selector selector,
            HashSet<HtmlElement> results)
        {
            // מחפשים בכל הצאצאים של האלמנט הנוכחי
            foreach (HtmlElement element in root.Descendants())
            {
                // האם האלמנט מתאים לסלקטור?
                if (!IsMatch(element, selector))
                    continue;

                // אם זה הסלקטור האחרון, מצאנו תוצאה
                if (selector.Child == null)
                {
                    results.Add(element);
                }
                else
                {
                    // ממשיכים לחפש את הסלקטור הבא
                    foreach (HtmlElement child in element.Children)
                    {
                        FindRecursive(child, selector.Child, results);
                    }
                }
            }
        }


    }
}
