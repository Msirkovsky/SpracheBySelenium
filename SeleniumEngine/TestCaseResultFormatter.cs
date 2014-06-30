using System;
using System.Text;
using OpenQA.Selenium.Remote;
using TestCasesDSL;

namespace SeleniumEngine
{
    public class TestResultFormatter
    {
        private const string ODDELOVAC = "---------";
        private const string ODDELOVAC_BIG = "------------------------";
        public string Format(TestResult result, out bool nalezenaChyba)
        {
            StringBuilder builder = new StringBuilder(500);
            builder.AppendLine(ODDELOVAC);

            nalezenaChyba = result.PocetChybnychTestu > 0;

            builder.AppendLine("Počet TestCase: " + result.PocetTestu + " - testy:, které skončili chybou: " + result.PocetChybnychTestu);

            foreach (TestCaseResult testCase in result.TestCaseResults)
            {
                builder.AppendLine(ODDELOVAC_BIG);
                builder.AppendLine("TestCase: " + testCase.Nazev);
                builder.AppendLine("WebTesty: " + testCase.PocetTestu + " - testy, které skončili chybou: " + testCase.PocetChybnychTestu);
                builder.AppendLine(ODDELOVAC);

                foreach (WebTestResult webTest in testCase.WebTestResults)
                {
                    builder.Append("   " + webTest.NazevTestu);
                    builder.Append(": ");
                    if (webTest.IsOk)
                        builder.AppendLine("ok");
                    else
                    {                        
                        builder.AppendLine(webTest.Chyba);
                        builder.AppendLine(ODDELOVAC);
                    }
                }
            }
            return builder.ToString();
        }
    }
}