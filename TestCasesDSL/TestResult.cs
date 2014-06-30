using System.Collections.Generic;
using System.Linq;

namespace TestCasesDSL
{
    public class TestResult
    {
        public List<TestCaseResult> TestCaseResults { get; set; }


        public TestResult()
        {
            TestCaseResults = new List<TestCaseResult>();
        }

        public int PocetTestu
        {
            get
            {
                return TestCaseResults.Count();
            }


        }
        public int PocetChybnychTestu
        {
            get
            {
                return TestCaseResults.Count(i => i.IsOk == false);
            }
        }

    }
}
