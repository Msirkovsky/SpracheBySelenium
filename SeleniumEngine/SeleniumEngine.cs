using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.PhantomJS;
using TestCasesDSL;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;

namespace SeleniumEngine
{
    public class SeleniumWebEngine : IDisposable
    {
        public SeleniumWebEngine()
        {
            PrepareToTest();
        }
        private RemoteWebDriver _driver;
        private void PrepareToTest()
        {
            _driver = new PhantomJSDriver(@"d:\Programovani\NET\Git\WebLab\phantomjs-1.9.2-windows");
            //_driver = new FirefoxDriver();// (@"d:\Programovani\NET\Git\WebLab\phantomjs-1.9.2-windows");
            //_driver = new PhantomJSDriver(@"c:\programs\phantomjs-1.9.2-windows");
        }

        public TestResult Run(IEnumerable<TestCase> testCases)
        {
            TestResult r = new TestResult();

            foreach (TestCase testCaseToRun in testCases)
            {
                TestCaseResult testCaseResult = new TestCaseResult();
                testCaseResult.Nazev = testCaseToRun.Nazev;

                foreach (WebTest webTestTuRun in testCaseToRun.WebTests)
                {
                    WebTestResult webTestResult = new WebTestResult();
                    try
                    {
                        webTestResult.NazevTestu = webTestTuRun.Nazev;
                        RunWebTest(webTestTuRun);
                        webTestResult.IsOk = true;
                    }
                    catch (Exception exc)
                    {
                        webTestResult.Chyba = exc.ToString();
                    }
                    testCaseResult.WebTestResults.Add(webTestResult);
                }
                r.TestCaseResults.Add(testCaseResult);
            }
            return r;
        }

        public void RunWebTest(WebTest webTest)
        {


            int index = 0;
            foreach (ControlCommand c in webTest.ControlCommand.ToList())
            {
                RunCommand(c, index);
                index++;
            }

            index = 0;
            foreach (ControlCommand c in webTest.CheckCommand.ToList())
            {
                RunCommand(c, index);
                index++;
            }
        }

        private void RunCommand(ControlCommand c, int index)
        {
            ControlCommandBaseWD cmd = CommandFactory.CreateSeleniumCommand(_driver, c, index);
            cmd.Proceed();
        }

        public void End()
        {
            _driver.Close();
        }

        public void Dispose()
        {
            End();
        }
    }
}