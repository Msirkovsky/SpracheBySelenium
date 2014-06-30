// -----------------------------------------------------------------------
// <copyright file="Commands.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TestCasesDSL;
using System.Collections.ObjectModel;

namespace SeleniumEngine.HtmlSeleniumEngine
{
    public abstract class ControlCommandHtmlBase
    {
        protected IWebDriver _wd;
        public string WebObject { get; set; }

        public ControlCommandHtmlBase Previous { get; set; }

        public bool IsExistElement(string identitifier, IWebDriver vlastniWD = null)
        {
            IWebDriver wd = vlastniWD ?? _wd;
            ReadOnlyCollection<IWebElement> elements;
            string identifierToSearch = GetRealIndentitifier(identitifier);
            var searchPredicate = GetSearchPredicate(identitifier);
            elements = wd.FindElements(searchPredicate(identifierToSearch));
            return elements.Count > 0;
        }


        // umí dle IDčka tak i css class
        public IWebElement FindElement(string identitifier, IWebDriver vlastniWD = null)
        {
            IWebDriver wd = vlastniWD ?? _wd;
            string identifierToSearch = GetRealIndentitifier(identitifier);
            IWebElement element;

            var searchPredicate = GetSearchPredicate(identitifier);
            //element = wd.FindElement(searchPredicate(identifierToSearch));
            element = wd.FindElement(searchPredicate(identifierToSearch));

            return element;
        }

        private static string GetRealIndentitifier(string identitifier)
        {
            if (identitifier.StartsWith("//"))
                return identitifier;
            string[] pole = identitifier.Split('=');
            string identifierToSearch = pole[1];
            return identifierToSearch;
        }

        private static Func<string, By> GetSearchPredicate(string identitifier)
        {
            Func<string, By> searchFunkce = null;
            if (identitifier.StartsWith("id="))
            {
                searchFunkce = By.Id;
            }

            if (identitifier.StartsWith("//"))
            {
                searchFunkce = By.XPath;
            }

            if (identitifier.StartsWith("link="))
            {
                searchFunkce = By.LinkText;
            }
            if (identitifier.StartsWith("class="))
            {
                searchFunkce = By.ClassName;
            }
            if (identitifier.StartsWith("css="))
            {
                searchFunkce = By.CssSelector;
            }

            return searchFunkce;
        }

        public ControlCommandHtmlBase(RemoteWebDriver wd)
        {
            _wd = wd;
        }
        public abstract void Proceed();

        internal virtual bool NeedWait { get { return false; } }
    }


    public class NavigationCommandHtml : ControlCommandHtmlBase
    {
        private string _url;
        public NavigationCommandHtml(RemoteWebDriver wd, string url)
            : base(wd)
        {
            _url = url;
        }

        public override void Proceed()
        {
            _wd.Navigate().GoToUrl(_url);
        }

        internal override bool NeedWait
        { 
            get
            {
            return true;
            }
        }
    }

    public class SendKeysControlCommandHtml : ControlCommandHtmlBase
    {
        public string SendKeys { get; private set; }
        public SendKeysControlCommandHtml(RemoteWebDriver wd, string sendKeys)
            : base(wd)
        {
            SendKeys = sendKeys;
        }

        public override void Proceed()
        {
            if (Previous.NeedWait) // musí dojít k čekání protože předchozí command volá navigate
            {
                //počkám na element do které budu posílat data
                WebDriverWait wait = new WebDriverWait(_wd, TimeSpan.FromSeconds(10));
                wait.Until((d) => IsExistElement(WebObject));
            }
            IWebElement element = FindElement(WebObject);
            element.SendKeys(SendKeys);
        }
    }
    public class ClickControlCommandHtml : ControlCommandHtmlBase
    {
        public ClickControlCommandHtml(RemoteWebDriver wd)
            : base(wd)
        {
        }

        public override void Proceed()
        {
            IWebElement element = FindElement(WebObject);
            element.Click();
        }
    }

    public class ClickAndWaitControlCommandHtml : ClickControlCommandHtml
    {
        public ClickAndWaitControlCommandHtml(RemoteWebDriver wd)
            : base(wd)
        {
        }

        internal override bool NeedWait
        {
            get
            {
                return true;
            }
        }
    }


    public abstract class CheckControlCommandBaseHtml : ControlCommandHtmlBase
    {
        public string ContentToCheck { get; protected set; }
        public CheckControlCommandBaseHtml(RemoteWebDriver wd)
            : base(wd)
        { }

    }

    public class CheckWaitableControlCommandHtml : CheckControlCommandBaseHtml
    {
        public CheckWaitableControlCommandHtml(RemoteWebDriver wd, CheckControlCommand command)
            : base(wd)
        {
            ContentToCheck = command.Argument;
            WebObject = command.DomObject;
        }

        public override void Proceed()
        {
            WebDriverWait wait = new WebDriverWait(_wd, TimeSpan.FromSeconds(10));

            if (ContentToCheck == "Exists")
            {
                wait.Until((d) => IsExistElement(WebObject));
            }
            else if (WebObject == "PageTitle")
                wait.Until((d) => d.Title == ContentToCheck);
            else
            {

                wait.Until((d) =>
                {
                    bool isExistElement = IsExistElement(WebObject, d);
                    if (isExistElement)
                    {
                        IWebElement element = FindElement(WebObject, d);

                        string value;
                        if (element.TagName == "input")
                            value = element.GetAttribute("value");
                        else
                            value = element.Text;

                        return value == ContentToCheck;
                    }
                    return false;
                });
            }
        }
    }


    public class CheckSimpleControlCommandHtml : CheckControlCommandBaseHtml
    {
        public CheckSimpleControlCommandHtml(RemoteWebDriver wd, CheckControlCommand command)
            : base(wd)
        {
            ContentToCheck = command.Argument;
            WebObject = command.DomObject;
        }

        public override void Proceed()
        {
            if (WebObject == "PageTitle")
            {
                CheckContent(_wd.Title);
            }
            else
            {
                IWebElement element = FindElement(WebObject);
                CheckContent(element.Text);
            }
        }

        private void CheckContent(string ToCompare)
        {
            if (ToCompare == ContentToCheck)
            {
                throw new SeleniumEngine.CheckContentException();
            }
        }
    }
}
