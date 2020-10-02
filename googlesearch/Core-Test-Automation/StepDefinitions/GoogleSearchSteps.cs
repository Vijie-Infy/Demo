using Core_Test_Automation.Common;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace Core_Test_Automation.StepDefinitions
{
    [Binding]
    public class GoogleSearchSteps
    {
        [When(@"I Navigate to Google Home Page")]
        public void WhenINavigateToGoogleHomePage()
        {
            BaseTest.EnterUrl("https://www.google.com/");
        }

        [Then(@"I do Google search")]
        public void WhenIdoGoogleSearch()
        {
            Element searchTextbox = new Element(By.Name("q"), "Search Textbox");

            searchTextbox.EnterText("Cheese!");
            BaseTest.WaitLoading(1);
        }
    }
}
