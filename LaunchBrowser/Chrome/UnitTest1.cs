using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;


namespace LaunchBrowser
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestChrome()
        {
            IWebDriver driver = null;
            try
            {
                driver = new ChromeDriver(@"C:\Users\GopiThiruvengadam\source\repos\LaunchBrowser\LaunchBrowser");
                driver.Url = "https://www.foxnews.com/";
                driver.Manage().Window.Maximize();
                driver.Navigate();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.ToString());
            }
        }
    }
}
