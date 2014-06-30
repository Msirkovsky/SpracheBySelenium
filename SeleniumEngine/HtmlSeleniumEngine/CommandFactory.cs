using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCasesDSL;
using OpenQA.Selenium.Remote;

namespace SeleniumEngine.HtmlSeleniumEngine
{
    class CommandHtmlFactory
    {
        public static ControlCommandHtmlBase CreateSeleniumCommand(RemoteWebDriver driver, HtmlControlCommand controlCommand, ControlCommandHtmlBase previousCommand)
        {

            //if (controlCommand.Action == "todo")
            //{
            //    if (indexOfCommand == 0)
            //    {
            //        return new CheckWaitableControlCommandWD(driver, (CheckControlCommand)controlCommand);
            //    }
            //    else
            //    {
            //}
            ControlCommandHtmlBase retVal;

            if (controlCommand.Action == "clickAndWait")
            {
                retVal = new ClickAndWaitControlCommandHtml(driver) { WebObject = controlCommand.WebObject };
            }
            else if (controlCommand.Action == "click")
            {
                retVal = new ClickControlCommandHtml(driver) { WebObject = controlCommand.WebObject };
            }

            else if (controlCommand.Action == "open")
            {
                retVal = new NavigationCommandHtml(driver, controlCommand.WebObject);
            }
            else
            {
                retVal = new SendKeysControlCommandHtml(driver, controlCommand.Argument) { WebObject = controlCommand.WebObject };
            }

            retVal.Previous = previousCommand;

            return retVal;
        }
    }
}
