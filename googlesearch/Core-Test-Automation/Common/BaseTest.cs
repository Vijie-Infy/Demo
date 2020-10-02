using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
using TextCopy;

namespace Core_Test_Automation.Common
{
    /// <summary>
    /// Contains all initialization and termination functions on the framework.
    /// </summary>
    [Binding]
    public class BaseTest
    {
        public static string scenarioName = null;

        public static IWebDriver driver = null;
        
        //Test Results Variables
        public static string folderSlash = null;
        public static string localfolderSlash = "\\";
        public static string gitFolderSlashLocal = "/";
        public static string folderLocation = null;

        public static int screenshotCounter = 1;

        /// <summary>
        /// Initialization of test session.
        /// </summary>
        [Before]
        public static void StartTest()
        {
            var remote = Environment.GetEnvironmentVariable("Remote");
            //var remote = "true";
            bool useRemote;
            if (remote == null)
            {
                useRemote = false;
            }
            else if (remote.Equals("true"))
            {
                useRemote = true;
            }
            else
            {
                useRemote = false;
            }


            // Chrome
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            options.AddArgument("no-sandbox");
            
            // Add statement to open Chrome during local testing, and run headless during GitHub Actions execution
            if (!Environment.CurrentDirectory.Contains(":"))
            {
                options.AddArguments("headless");
                folderSlash = gitFolderSlashLocal;
            }
            else
            {
                folderSlash = localfolderSlash;
            }

            // Create folder to store test results of current run.
            folderLocation = Constant.ROOT + folderSlash
                + "TestResults" + folderSlash + "Screenshots";
            // + folderSlash
            //+ DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            CreateFolder(folderLocation);

            
            if (useRemote)
            {
                string sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
                string sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");
                string sauceName = Environment.GetEnvironmentVariable("SAUCE_NAME");
                string sauceBuild = Environment.GetEnvironmentVariable("SAUCE_BUILD");
                string sauceTags = Environment.GetEnvironmentVariable("SAUCE_TAGS");

                var sauceOptions = new Dictionary<string, object>
                {
                    ["username"] = sauceUserName,
                    ["accessKey"] = sauceAccessKey,
                    ["name"] = sauceName,
                    ["build"] = sauceBuild,
                    ["tags"] = sauceTags
                };

                options.AddAdditionalCapability(CapabilityType.Version, "latest", true);
                options.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);
                options.AddAdditionalCapability("sauce:options", sauceOptions, true);

                driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"), options.ToCapabilities(), TimeSpan.FromSeconds(600));
            }
            else
            {
                driver = new ChromeDriver(options);
            }    
        }

        [BeforeScenario()]
        public static void BeforeScenario()
        {
            Console.WriteLine("Starting " + scenarioName);
        }

        /// <summary>
        /// Function to terminate sessions used during test.
        /// </summary>
        [After]
        public static void EndTest()
        {
            driver.Close();
            driver.Quit();
        }

        /// <summary>
        /// Create folder in a specified location. If folder already exists then the function will skip creation.
        /// </summary>
        /// <param name="folderLocation">Complete folder path.</param>
        public static void CreateFolder(string folderLocation)
        {
            if (!Directory.Exists(folderLocation))
                Directory.CreateDirectory(folderLocation);
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">URL of page to navigate.</param>
        public static void EnterUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Saves screenshot of current page.
        /// </summary>
        /// <param name="text">Description of screenshot</param>
        public static void Screenshot(string text)
        {
            try
            {
                Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                string filename = screenshotCounter.ToString("000") + "_" + driver.Title + "_" + text + ".png";
                image.SaveAsFile(folderLocation + folderSlash + filename, ScreenshotImageFormat.Png);

                screenshotCounter++;

                // Check if file is created
                if (File.Exists(folderLocation + folderSlash + filename))
                {
                    Console.WriteLine("Screenshot saved: " + folderLocation + folderSlash + filename);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Screenshot failed." + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Saves screenshot of current page.
        /// </summary>
        /// <param name="text">Description of screenshot</param>
        /// <param name="status">Indicate of screenshot is PASSED or FAILED</param>
        public static void Screenshot(string text, string status)
        {
            Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
            image.SaveAsFile(folderLocation + folderSlash
                + screenshotCounter.ToString("000") + "_" + driver.Title + "_" + status + "_" + text + ".png",
                ScreenshotImageFormat.Png);

            screenshotCounter++;
        }

        /// <summary>
        /// Handles windows form for uploading files.
        /// </summary>
        /// <param name="filePath">Full path of file to upload.</param>
        public static void UploadFile(string filePath)
        {
            ClipboardService.SetText(filePath);
            Actions actions = new Actions(driver);
            actions.KeyDown(Keys.Control)
                .SendKeys("v")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Return)
                .Perform();
        }

        /// <summary>
        /// Inserts wait time (in seconds) before proceeding with next line of code.
        /// </summary>
        /// <param name="seconds">Wait time in seconds.</param>
        public static void WaitLoading(int seconds)
        {
            try
            {
                Console.WriteLine("Waiting for " + seconds + " second(s) to resume.");
                Thread.Sleep(seconds * 1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}