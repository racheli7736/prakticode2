
using praktikod2;

HtmlSerializer serializer = new HtmlSerializer();

// טעינת HTML מאתר
string html = await serializer.Load("https://example.com");

// בניית עץ HTML
HtmlElement root = serializer.BuildTree(html);

// חיפוש אלמנטים בעץ
Selector selector = Selector.Parse("body p");
var results = root.FindElements(selector);

foreach (var element in results)
{
    Console.WriteLine(element.InnerHtml);
}
