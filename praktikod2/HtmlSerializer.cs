using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class HtmlSerializer
    {
        private readonly HtmlLoader loader = new HtmlLoader();
        private readonly HtmlParser parser = new HtmlParser();

        public async Task<string> Load(string url)
        {
            return await loader.Load(url);
        }

        public HtmlElement BuildTree(string html)
        {
            return parser.Parse(html);
        }
    }
}
