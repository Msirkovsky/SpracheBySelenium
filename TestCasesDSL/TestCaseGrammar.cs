using System.Collections.Generic;
using Sprache;

namespace TestCasesDSL
{
    public static class TestCaseGrammar
    {

        public static Parser<string> Identifier = Parse.NotWhiteSpace.AtLeastOnce().Text().Token();
        
        public static Parser<string> WholeAnyWord = Parse.UkoncovaciChar.AtLeastOnce().Text().Token();

        public static readonly Parser<string> QuotedText =
    (from open in Parse.Char('"')
     from content in Parse.CharExcept('"').Many().Text()
     from close in Parse.Char('"')
     select content).Token();

        public static readonly Parser<ControlCommand> ControlCommands =
        from id in WholeAnyWord
        from prompt in QuotedText
        select new ControlCommand(id, prompt);

        public static readonly Parser<ControlCommand> CheckControlCommands =
           from id in WholeAnyWord
           from prompt in QuotedText
           select new CheckControlCommand(id, prompt);


        public static Parser<IEnumerable<char>> Sucess = Parse.String("Success").Token();

        public static readonly Parser<WebTest> WebTests =
            from IdTestCase in Parse.String("Test").Token()
            from n in QuotedText.Optional()
            //from url in WholeAnyWord
            from controlCommand in ControlCommands.Many()
            from sucess in Parse.String("Success").Token()
            from controlToCheck in CheckControlCommands.Many()
            //from end in Parse.String("End").Token()
            from endTestCase in Parse.String("End").Token()
            select new WebTest() { Nazev = n.GetOrDefault(), ControlCommand = controlCommand, CheckCommand = controlToCheck };


        public static readonly Parser<TestCase> TestCase =
    from anyWords in Parse.WhiteSpace.Many()
    from keyWordTestCase in Parse.String("TestCase")
    from nazev in QuotedText
    from testcases in WebTests.Many()
    from endKeyWordTestCase in Parse.String("EndTestCase")
    from anyWords5 in Parse.WhiteSpace.Many()
    select new TestCase() { WebTests = testcases, Nazev = nazev };

        public static readonly Parser<IEnumerable<TestCase>> AllTestCase = TestCase.Many().Select(i => i).End();

    }
}
