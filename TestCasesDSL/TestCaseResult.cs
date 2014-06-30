using System.Collections.Generic;
using System.Linq;

namespace TestCasesDSL
{
    public class TestCaseResult
    {
        public string Nazev { get; set; }
        public List<WebTestResult> WebTestResults { get; set; }

        public TestCaseResult()
        {
            WebTestResults = new List<WebTestResult>();
        }

        public int PocetTestu
        {
            get
            {
                return WebTestResults.Count();
            }


        }
        public int PocetChybnychTestu
        {
            get
            {
                return WebTestResults.Count(i => i.IsOk == false);
            }
        }

        public bool IsOk
        {
            get
            {
                return WebTestResults.All(i => i.IsOk);
            }
        }
    }

    public class WebTestResult
    {
        public bool IsOk { get; set; }
        public string NazevTestu { get; set; }
        public string Chyba { get; set; }
    }
}
