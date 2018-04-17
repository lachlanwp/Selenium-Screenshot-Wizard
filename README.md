# Selenium Screenshot Wizard
Captures screenshots using Webdrivers for Chrome, Firefox and IEEdge with Selenium

Quick Start Guide

## 1. Define the URLs that you want to perform a screenshot on.
Write a plain text file with each line being a URL for which you want to capture a screen shot. The program will run through each line and capture a screenshot of the rendered page

## 2. Write custom "Extra Steps".
If you need to do some special operations on the web page, such as login into a portal or filling out some form, you can define your own C# code in the "ExtraSteps" class.  This class has the file name "ExtraSteps.cs" and is located in the root of the solution.  There are already two examples in this file that I've defined for my own use.  These extra steps are defined in normal CSharp methods that are like selenium 'snippets'.  See below example that I've written for logging into a web application.

```csharp
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
```

The attribute "ExtraStep" at the signature of the method can be used in your plain text file defined in (1).  Simply make a new line with the content [[LOGIN_PROCEDURE]] and this snippet will run at that point.

## 3. Running the app
It's a basic console application that takes four (4) arguments. The arguments are as follows:
* The full path to the plain text file defined in (1)
* The desired browser width
* The desired browser height
* The output directory where you want the screenshots to be saved

## For example, 
* I have defined a plain text file as in (1) with the following path "C:\Users\lachlanp\Desktop\urls.txt"
* I desire screenshots to be captured at 1280x1080 dimensions
* I want the output images to be saved in the following folder "C:\Users\lachlanp\Desktop\screenshots"
> I would run the program from the command prompt like this:
```
.\CrossBrowserPreviews.exe "C:\Users\lachlanp\Desktop\urls.txt" 1280 1080 "C:\Users\lachlanp\Desktop\screenshots"
```
