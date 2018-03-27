using CrossBrowserPreviews.Attr;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace seleniumscreenshotwizard
{
    public class ExtraSteps
    {
        [ExtraStep("[[IUBENDA_CLOSEWINDOW]]")]
        public static void CloseIubendaOverlay(IWebDriver webDriver)
        {
            try
            {
                webDriver.FindElement(By.ClassName("iubenda-cs-close-btn")).Click();
            }
            catch (Exception Ex)
            {

            }
        }

        [ExtraStep("[[LOGIN_PROCEDURE]]")]
        public static void DoAppLogin(IWebDriver webDriver)
        {
            //Define how Selenium should login to your app
            try
            {
                String username = "lachlan";
                String password = "*****";

                webDriver.FindElement(By.Id("cs-login-username")).SendKeys(username);
                webDriver.FindElement(By.Id("cs-login-password")).SendKeys(password);
                webDriver.FindElement(By.Id("cs-login-submitlogin")).Click();

                Thread.Sleep(2000);
            }
            catch (Exception Ex)
            {

            }
        }
    }
}
