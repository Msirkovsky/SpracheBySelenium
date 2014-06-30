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

namespace SeleniumEngine
{
    public abstract class ControlCommandBaseWD
    {
        protected IWebDriver _wd;
        public string ObjectID { get; set; }

        public bool IsExistElement(string identitifier, IWebDriver vlastniWD = null)
        {
            IWebDriver wd = vlastniWD ?? _wd;
            ReadOnlyCollection<IWebElement> elements;
            if (identitifier.Contains("class:"))
            {
                string className = identitifier.Replace("class:", "");
                elements = _wd.FindElements(By.ClassName(className));
            }
            else
            {
                elements = _wd.FindElements(By.Id(identitifier));
            }

            return elements.Count > 0;
        }


        // umí dle IDčka tak i css class
        public IWebElement FindElement(string identitifier, IWebDriver vlastniWD = null)
        {
            IWebDriver wd = vlastniWD ?? _wd;

            IWebElement element;
            if (identitifier.Contains("class:"))
            {
                string className = identitifier.Replace("class:", "");
                element = wd.FindElement(By.ClassName(className));
            }
            else
            {
                element = wd.FindElement(By.Id(identitifier));                
            }

            return element;
        }

        public ControlCommandBaseWD(RemoteWebDriver wd)
        {
            _wd = wd;
        }
        public abstract void Proceed();
    }


    public class NavigationCommandWD : ControlCommandBaseWD
    {
        private string _url;
        public NavigationCommandWD(RemoteWebDriver wd, string url)
            : base(wd)
        {
            _url = url;
        }

        public override void Proceed()
        {
            _wd.Navigate().GoToUrl(_url);
        }
    }
    
    public class SendKeysControlCommandWD : ControlCommandBaseWD
    {
        public string SendKeys { get; private set; }
        public SendKeysControlCommandWD(RemoteWebDriver wd, string sendKeys) : base(wd)
        {
            SendKeys = sendKeys;
        }

        public override void Proceed()
        {
            IWebElement element = FindElement(ObjectID);
            element.SendKeys(SendKeys);
        }
    }
    public class ClickControlCommandWD : ControlCommandBaseWD
    {
        public ClickControlCommandWD(RemoteWebDriver wd)
            : base(wd)
        {
        }

        public override void Proceed()
        {
            IWebElement element = FindElement(ObjectID);
            element.Click();
        }
    }

    public abstract class CheckControlCommandBaseWD : ControlCommandBaseWD
    {
        public string ContentToCheck { get; protected set; }
        public CheckControlCommandBaseWD(RemoteWebDriver wd)
            : base(wd)
        { }

    }

    public class CheckWaitableControlCommandWD : CheckControlCommandBaseWD
    {
        public CheckWaitableControlCommandWD(RemoteWebDriver wd, CheckControlCommand command)
            : base(wd)
        {
            ContentToCheck = command.Argument;
            ObjectID = command.DomObject;
        }

        public override void Proceed()
        {
            WebDriverWait wait = new WebDriverWait(_wd, TimeSpan.FromSeconds(10));

            if (ContentToCheck == "Exists")
            {
                wait.Until((d) => IsExistElement(ObjectID));
            }
            else if (ObjectID == "PageTitle")
                wait.Until((d) => d.Title == ContentToCheck);
            else
            {

                wait.Until((d) =>
                {
                    bool isExistElement = IsExistElement(ObjectID, d);
                    if (isExistElement)
                    {
                        IWebElement element = FindElement(ObjectID, d);

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
    
    public class CheckSimpleControlCommandWD : CheckControlCommandBaseWD
    {
        public CheckSimpleControlCommandWD(RemoteWebDriver wd, CheckControlCommand command)
            : base(wd)
        {
            ContentToCheck = command.Argument;
            ObjectID = command.DomObject;
        }

        public override void Proceed()
        {
            if (ObjectID == "PageTitle")
            {
                CheckContent(_wd.Title);
            }
            else
            {
                IWebElement element = FindElement(ObjectID);
                CheckContent(element.Text);
            }
        }

        private void CheckContent(string ToCompare)
        {
            if (ToCompare == ContentToCheck)
            {
                throw new CheckContentException();
            }
        }
    }
}
