using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transpose.Translator.Tests;

[TestClass]
public sealed class NullForgivingEmitTests {
    [TestMethod]
    public void NullForgivingOperatorIsCompileTimeOnly() {
        var source = """
            #nullable enable
            using Transpose;

            public static class Example {
                public static object Build(object? cursor) =>
                    Script.Write<object>("({cursor:{0}})", cursor!);

                public static object? Identity(object? cursor) => cursor!;
            }
            """;

        var javascript = new RoslynTranslator().Translate(source).Javascript ?? "";
        StringAssert.Contains(javascript, "cursor");
        Assert.IsFalse(javascript.Contains("cursor!"),
            "C# null-forgiving operator must not be emitted as JavaScript punctuation.");
    }
}
