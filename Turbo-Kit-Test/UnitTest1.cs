using Turbo_Kit.PDF;
using Turbo_Kit.Text;
using Turbo_Kit.WORD;

namespace Turbo_Kit_Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var path = "C:\\Users\\betha\\RiderProjects\\Turbo-Auth\\Turbo-Kit-Test\\Resources\\04-2024届论文文档日期规定.docx";
        var res = (new WordDocumentProcessor()).Process(path);
        Console.WriteLine("word file");
        Console.WriteLine(res);
        Assert.Pass();
    }
    [Test]
    public void Test2()
    {
        var path = "C:\\Users\\betha\\RiderProjects\\Turbo-Auth\\Turbo-Kit-Test\\Resources\\04-2024届论文文档日期规定.pdf";
        var res = (new PdfDocumentProcessor()).Process(path);
        Console.WriteLine("pdf file");
        Console.WriteLine(res);
        Assert.Pass();
    }

    [Test]
    public void Test3()
    {
        var path = "C:\\Users\\betha\\Desktop\\sandbox.txt";
        Console.WriteLine((new TextDocumentProcessor()).Process(path));
    }
}