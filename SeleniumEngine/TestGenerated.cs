using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    public class TestGenerated
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://cedr-fm.mfcr.cz/cedrnf-test/";
            verificationErrors = new StringBuilder();
        }

        public void TheAaaaTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/cedrnf-test/Views/Public/Prihlaseni.aspx");
            driver.FindElement(By.Id("loginTextbox")).Clear();
            driver.FindElement(By.Id("loginTextbox")).SendKeys("nf@nf.cz");
            driver.FindElement(By.Id("passwordTextBox")).Clear();
            driver.FindElement(By.Id("passwordTextBox")).SendKeys("aaaaaa");
            driver.FindElement(By.Id("loginLinkButton")).Click();
            driver.FindElement(By.CssSelector("span.smIcon")).Click();
            driver.FindElement(By.Id("MainContent_Projekty_cell0_2_803ff027-1565-49d2-88ba-0595f45fb80bCreateRowRedirectTarget_0")).Click();
            driver.FindElement(By.Id("MainContent_mainTabPage__ZadostPDPMGS_NazevEng")).Clear();
            driver.FindElement(By.Id("MainContent_mainTabPage__ZadostPDPMGS_NazevEng")).SendKeys("Txx");
            driver.FindElement(By.Id("_4Save")).Click();
            driver.FindElement(By.CssSelector("#MainContent_mainTabPage__hlavniVysledekProgramuSC > input.acsBtn.acscOpenBtn")).Click();
            driver.FindElement(By.LinkText("Zlepšení přístupu a kvality zdravotní péče, včetně reprodukční a preventivní dětské zdravotní péče")).Click();
            driver.FindElement(By.Id("_4Save")).Click();
        }
    }
}
