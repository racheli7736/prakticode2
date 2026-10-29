using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktikod2
{
    internal class HtmlLoader
    {
        private static readonly HttpClient client = new();

        public async Task<string> Load(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
