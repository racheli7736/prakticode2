
using praktikod2;

string html = """
<div id="main" class="container active">
    <h1>Hello</h1>
    <p class="text">Welcome</p>
    <section>
        <p class="text special">Nested</p>
        <input type="text" disabled />
        <br>
    </section>
    <p>Goodbye</p>
</div>
""";

HtmlElement root = new HtmlParser().Parse(html);

int passed = 0;
int total = 0;

void Test(string name, bool condition)
{
    total++;

    if (condition)
        passed++;

    Console.WriteLine(
        $"{name}: {(condition ? "PASS" : "FAIL")}");
}

int Count(string query)
{
    return root.FindElements(Selector.Parse(query)).Count;
}

// בדיקות חיפוש
Test("Tag selector", Count("p") == 3);
Test("ID selector", Count("#main") == 1);
Test("Class selector", Count(".text") == 2);
Test("Multiple classes", Count("p.text.special") == 1);
Test("Combined selector", Count("div#main.container.active") == 1);
Test("Nested selector", Count("div section .text") == 1);
Test("No matches", Count(".unknown") == 0);

// בדיקות מבנה העץ
HtmlElement div = root.Children[0];
HtmlElement section = div.Children[2];

Test("Parent relation", section.Parent == div);
Test("Ancestors", section.Ancestors().Any(e => e == div));
Test("Descendants", div.Descendants().Count() == 8);

// בדיקות תגיות ללא סגירה
Test("Input is self closing", section.Children.Count == 3);
Test("BR is self closing", section.Children[2].Name == "br");

// בדיקת מאפיינים
Test("ID parsed", div.Id == "main");
Test("Classes parsed", div.Classes.Contains("active"));

// בדיקות רגרסיה

// תגית לא מוכרת לא משבשת את ההיררכיה
HtmlElement unknownRoot = new HtmlParser().Parse(
    "<div><custom class=\"x\"><p>a</p></custom><span>b</span></div>");
HtmlElement unknownSpan = unknownRoot.Descendants().First(e => e.Name == "span");
Test("Unknown tag: span stays under div", unknownSpan.Parent?.Name == "div");
Test("Unknown tag: unknown element not in tree",
    !unknownRoot.Descendants().Any(e => e.Name == "custom"));
Test("Unknown tag: p stays under div",
    unknownRoot.Descendants().First(e => e.Name == "p").Parent?.Name == "div");

// השוואת שם תגית בסלקטור לא רגישה לאותיות
Test("Case-insensitive tag selector", Count("DIV") == 1);
Test("Case-insensitive combined selector", Count("Div#main.container") == 1);

// הפרסור נעצר ב-</html>
HtmlElement htmlRoot = new HtmlParser().Parse(
    "<html><body><p>a</p></body></html><p>after</p>");
Test("Parsing stops at </html>",
    htmlRoot.Descendants().Count(e => e.Name == "p") == 1);
HtmlElement upperHtmlRoot = new HtmlParser().Parse(
    "<HTML><BODY><P>a</P></BODY></HTML><P>after</P>");
Test("Parsing stops at </HTML> (upper-case)",
    upperHtmlRoot.Descendants().Count(e => e.Name == "p") == 1);

Console.WriteLine();
Console.WriteLine($"FINAL RESULT: {passed}/{total}");

// הדגמה מלאה: URL -> HTML -> עץ HtmlElement -> Selector -> תוצאות
string url = args.Length > 0 ? args[0] : "https://example.com";
string query = args.Length > 1 ? args[1] : "body p";

Console.WriteLine();
Console.WriteLine($"DEMO: {url}  |  selector: \"{query}\"");

try
{
    string pageHtml = await new HtmlLoader().Load(url);
    HtmlElement pageRoot = new HtmlParser().Parse(pageHtml);
    Selector selector = Selector.Parse(query);
    var results = pageRoot.FindElements(selector);

    Console.WriteLine($"Loaded {pageHtml.Length} chars, " +
        $"{pageRoot.Descendants().Count()} elements, {results.Count} result(s)");

    foreach (HtmlElement result in results)
    {
        string text = result.InnerHtml.Length > 60
            ? result.InnerHtml.Substring(0, 60) + "..."
            : result.InnerHtml;

        Console.WriteLine(
            $"  <{result.Name}> id={result.Id ?? "-"} " +
            $"classes=[{string.Join(" ", result.Classes)}] \"{text}\"");
    }
}
catch (Exception ex) when (ex is HttpRequestException || ex is ArgumentException)
{
    Console.WriteLine($"Demo failed: {ex.Message}");
}
