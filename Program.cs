using seleniumscreenshotwizard.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using CrossBrowserPreviews.Attr;

namespace seleniumscreenshotwizard
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 4)
            {
                Console.WriteLine("Error: must specify four arguments: (1) source file, (2) window width, (3) window height, (4) output directory");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var sourceFilePath = args[0];
            var windowWidth = args[1];
            var windowHeight = args[2];
            var outputDirectory = args[3];

            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            int widthParsed;
            int heightParsed;

            if (!Int32.TryParse(windowWidth, out widthParsed))
            {
                Console.WriteLine("Error: supplied window width cannot be parsed into an int");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (!Int32.TryParse(windowHeight, out heightParsed))
            {
                Console.WriteLine("Error: supplied window height cannot be parsed into an int");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (!File.Exists(sourceFilePath))
            {
                Console.WriteLine("Error: file path in argument (1) doesn't exist");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var fileContents = File.ReadAllLines(sourceFilePath);

            if (fileContents.Length <= 0)
            {
                Console.WriteLine("Error: source file is empty");
                Console.ReadLine();
                Environment.Exit(0);
            }
                
            int browserCount = Enum.GetNames(typeof(BrowserTypes)).Length;
            int totalScreenshotsTest = fileContents.Length * browserCount;
            int totalCountDone = 0;

            for (int i = 0; i < browserCount; i++)
            {
                BrowserTypes currType = (BrowserTypes)i;
                IWebDriver webDriver = null;
                String typePrefix = "Unknown_";

                switch ((int)currType)
                {
                    case (int)BrowserTypes.IeEdge:
                        webDriver = new EdgeDriver();
                        typePrefix = "IeEdge_";
                        break;
                    case (int)BrowserTypes.Chrome:
                        webDriver = new ChromeDriver();
                        typePrefix = "Chrome_";
                        break;
                    case (int)BrowserTypes.Firefox:
                        webDriver = new FirefoxDriver();
                        typePrefix = "Firefox_";
                        break;
                }
                                    
                for (int j = 0; j < fileContents.Length; j++)
                {
                    if (webDriver != null)
                    {
                        if (fileContents[j].Substring(0, 2) == "[[")
                        {
                            //Tagged extra step
                            DoExtraTaggedStep(fileContents[j], webDriver);
                        }
                        else
                        {
                            webDriver.Navigate().GoToUrl(fileContents[j]);
                            Thread.Sleep(2000);
                            webDriver.Manage().Window.Size = new System.Drawing.Size(widthParsed, heightParsed);
                            Screenshot screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                            screenshot.SaveAsFile(Path.Combine(outputDirectory, String.Format("{3}_{0}x{1}_{2}.JPG", widthParsed, heightParsed, j, typePrefix)), ScreenshotImageFormat.Jpeg);

                            totalCountDone++;
                            Console.Clear();
                            Console.WriteLine
                            (
                                "{0}% Complete. {1} Screenshots completed. {2} Screenshots remaining.",
                                Math.Round((((decimal)totalCountDone / (decimal)totalScreenshotsTest) * (decimal)100.00), 2),
                                totalCountDone,
                                totalScreenshotsTest - totalCountDone
                            );
                        }
                    }
                }

                webDriver.Close();
                webDriver.Quit();
                Thread.Sleep(2000);
            }
            Console.Clear();
            Console.WriteLine("Job done.");

            Console.ReadLine();
        }

        private static void DoExtraTaggedStep(string tag, IWebDriver webDriver)
        {
            MethodInfo[] props = typeof(ExtraSteps).GetMethods();
            foreach (MethodInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ExtraStepAttribute authAttr = attr as ExtraStepAttribute;
                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        string thisPropTag = authAttr.StepTag;

                        if (thisPropTag == tag)
                        {
                            object[] parameters = new object[1];
                            parameters[0] = webDriver;
                            prop.Invoke(null, parameters);
                        }
                    }
                }
            }
        }
    }
}
