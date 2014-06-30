using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.PhantomJS;
using TestCasesDSL;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;
using SeleniumEngine;
using TestCasesDSL.HtmlConvertor;

namespace SeleniumEngine.HtmlSeleniumEngine
{
    public class Engine : IDisposable
    {
        public Engine()
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

        public TestResult Run(HtmlTestCase testCase, Action<string> callbackToWrite)
        {
            TestResult r = new TestResult();
            TestCaseResult testCaseResult = new TestCaseResult();
            r.TestCaseResults.Add(testCaseResult);
            testCaseResult.Nazev = testCase.Title;
            
            ControlCommandHtmlBase cmd = null;
            int index = 0;
            foreach (HtmlControlCommand c in testCase.Commands)
            {
                WebTestResult webTestResult = new WebTestResult();
                try
                {
                    testCaseResult.WebTestResults.Add(webTestResult);
                    cmd = RunOneTest(cmd, c, webTestResult);
                    if (callbackToWrite != null)
                        callbackToWrite(index + " ");
                    
                    index++;
                }
                catch (Exception exc)
                {
                    webTestResult.Chyba = exc.ToString();
                    break;
                }
                
            }
            return r;
        }

        private ControlCommandHtmlBase RunOneTest(ControlCommandHtmlBase cmd, HtmlControlCommand c, WebTestResult webTestResult)
        {
            webTestResult.NazevTestu = c.GeneratedTitle;
            cmd = CommandHtmlFactory.CreateSeleniumCommand(_driver, c, cmd);
            cmd.Proceed();
            webTestResult.IsOk = true;
            return cmd;
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