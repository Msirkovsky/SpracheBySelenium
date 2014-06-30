using System;
using OpenQA.Selenium.Remote;
using TestCasesDSL;

namespace SeleniumEngine
{
    public class CommandFactory
    {
        public static ControlCommandBaseWD CreateSeleniumCommand(RemoteWebDriver driver, ControlCommand controlCommand, int indexOfCommand)
        {
            if (controlCommand is CheckControlCommand)
            {
                if (indexOfCommand == 0)
                {
                    return new CheckWaitableControlCommandWD(driver, (CheckControlCommand)controlCommand);
                }
                else
                {
                    //check commands
                    return new CheckSimpleControlCommandWD(driver, (CheckControlCommand)controlCommand);
                }
            }

            if (controlCommand.Argument == "Click")
            {
                return new ClickControlCommandWD(driver) { ObjectID = controlCommand.DomObject };
            }

            if (controlCommand.DomObject == "Url")
            {
                return new NavigationCommandWD(driver, controlCommand.Argument);
            }

            return new SendKeysControlCommandWD(driver, controlCommand.Argument) { ObjectID = controlCommand.DomObject };
        }
    }
}