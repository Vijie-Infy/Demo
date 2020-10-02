using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;                                                                                                                                                                                                                         

namespace Core_Test_Automation.Common
{
    /// <summary>
    /// Constains all functions that interacts with the elements of a page.
    /// </summary>
    public class Element
    {
        protected static IWebDriver driver = BaseTest.driver;
        protected By by; 
        protected string elementName;
        protected int wait;

        public Element(By by, string name, int wait = Constant.LONGWAIT)
        {
            this.by = by;
            this.elementName = name;
            this.wait = wait;
        }

        private IWebElement element;
        private IList<IWebElement> elements;

        /// <returns></returns>
        /// <summary>
        /// Function to find element with user defined wait time in seconds. This will be used only by other functions in BasePage class.
        /// </summary>
        /// <param name="by">Element parameter</param>
        /// <param name="wait">Wait time in seconds</param>
        /// <returns></returns>
        public IWebElement FindElement(By by, int wait = Constant.LONGWAIT)
        {
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(driver, TimeSpan.FromSeconds(wait))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception e)
            {
                throw new Exception("Element not found: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return element;
        }

        /// <summary>
        /// Function to find element with more than one instance in a page. This will be used only by other functions in BasePage class.
        /// </summary>
        /// <param name="by">Element parameter</param>
        /// <param name="wait">Wait time in seconds</param>
        /// <returns></returns>
        public IList<IWebElement> FindElements(By by, int wait = Constant.LONGWAIT)
        {
            IList<IWebElement> elements = null;
            try
            {
                elements = new WebDriverWait(driver, TimeSpan.FromSeconds(wait))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            }
            catch (Exception e)
            {
                throw new Exception("Element not found: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return elements;
        }

        /// <summary>
        /// Click an element in a page.
        /// </summary>
        public void ClickElement()
        {
            element = FindElement(by, wait);
            try
            {
                element.Click();
                Console.WriteLine("Element clicked: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Element not clicked: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Click an instance of element in a page.
        /// </summary>
        /// <param name="index">Index of element in a page.</param>
        public void ClickElement(int index)
        {
            elements = FindElements(by, wait);
            try
            {
                elements[index].Click();
                Console.WriteLine("Element clicked: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Element not clicked: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Drags slider to a specified amount.
        /// </summary>
        /// <param name="amount">Amount to slide.</param>
        /// <param name="maxValue">Maximum value of slider.</param>
        /// <param name="minValue">Minimum value of slider.</param>
        public void DragSlider(decimal amount, decimal maxValue, decimal minValue)
        {
            static int GetPixelsToMove(IWebElement Slider, decimal Amount, decimal SliderMax, decimal SliderMin)
            {
                int pixels = 0;
                decimal tempPixels = Slider.Size.Width;
                tempPixels = tempPixels / (SliderMax - SliderMin);
                tempPixels = tempPixels * (Amount - SliderMin);
                pixels = Convert.ToInt32(tempPixels);
                return pixels;
            }

            element = FindElement(by, wait);
            Actions SliderAction = new Actions(driver);
            
            try
            {
                int PixelsToMove = GetPixelsToMove(element, amount, maxValue, minValue);
                SliderAction.ClickAndHold(element)
                    .MoveByOffset((-(int)element.Size.Width / 2), 0)
                    .MoveByOffset(PixelsToMove, 0).Release().Perform();
                Console.WriteLine("Slider successfully updated: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Slider unsuccessful: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Drags slider to a specified amount.
        /// </summary>
        /// <param name="amount">Amount to slide.</param>
        /// <param name="maxValue">Maximum value of slider.</param>
        /// <param name="minValue">Minimum value of slider.</param>
        /// <param name="index">Index of element in a page.</param>
        public void DragSlider(decimal amount, decimal maxValue, decimal minValue, int index)
        {
            static int GetPixelsToMove(IWebElement Slider, decimal Amount, decimal SliderMax, decimal SliderMin)
            {
                int pixels = 0;
                decimal tempPixels = Slider.Size.Width;
                tempPixels = tempPixels / (SliderMax - SliderMin);
                tempPixels = tempPixels * (Amount - SliderMin);
                pixels = Convert.ToInt32(tempPixels);
                return pixels;
            }

            elements = FindElements(by, wait);
            Actions SliderAction = new Actions(driver);

            try
            {
                int PixelsToMove = GetPixelsToMove(elements[index], amount, maxValue, minValue);
                SliderAction.ClickAndHold(elements[index])
                    .MoveByOffset((-(int)elements[index].Size.Width / 2), 0)
                    .MoveByOffset(PixelsToMove, 0).Release().Perform();
                Console.WriteLine("Slider successfully updated: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Slider unsuccessful: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Enter text in an element in a page.
        /// </summary>
        /// <param name="text">Text to enter.</param>
        /// <param name="maskText">Mask text if true.</param>
        public void EnterText(string text, bool maskText = false)
        {
            element = FindElement(by, wait);
            try
            {
                element.Clear();
                element.SendKeys(text);

                if (maskText)
                {
                    text = "••••••••••";
                }

                Console.WriteLine("Entered text \"" + text + "\" in element: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Text not entered in element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Enter text in an instance of element in a page.
        /// </summary>
        /// <param name="text">Text to enter.</param>
        /// <param name="index">Index of element in a page.</param>
        /// <param name="maskText">Mask text if true.</param>
        public void EnterText(string text, int index, bool maskText = false)
        {
            elements = FindElements(by, wait);
            try
            {
                elements[index].Clear();
                elements[index].SendKeys(text);
                if (maskText)
                {
                    text = "••••••••••";
                }

                Console.WriteLine("Entered text \"" + text + "\" in element: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Text not entered in element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Get text of an element in a page.
        /// </summary>
        /// <returns>Text of an element in string format.</returns>
        public string GetText()
        {
            string text = null;
            element = FindElement(by, wait);
            try
            {
                text = element.Text;
                Console.WriteLine("Text displayed in element: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Text not found in element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Get text of an instance of element in a page.
        /// </summary>
        /// <param name="index">Index of element in a page.</param>
        /// <returns>Text of an element in string format.</returns>
        public string GetText(int index)
        {
            string text = null;
            elements = FindElements(by, wait);
            try
            {
                text = elements[index].Text;
                Console.WriteLine("Text displayed in element: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Text not found in element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Get value of an attribute of an element in a page.
        /// </summary>
        /// <param name="attributeName">Attribute of the element.</param>
        /// <returns>Attribute value of an element in string format.</returns>
        public string GetAttribute(string attributeName)
        {
            string text = null;
            element = FindElement(by, wait);
            try
            {
                text = element.GetAttribute(attributeName);
                Console.WriteLine("Attribute value in element found: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find attribute value: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Get value of an attribute of an instance of element in a page.
        /// </summary>
        /// <param name="attributeName">Attribute of the element.</param>
        /// <param name="index">Index of element in a page.</param>
        /// <returns>Attribute value of an element in string format.</returns>
        public string GetAttribute(string attributeName, int index)
        {
            string text = null;
            elements = FindElements(by, wait);
            try
            {
                text = elements[index].GetAttribute(attributeName);
                Console.WriteLine("Attribute value in element found: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find attribute value: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Get value of a css propoerty of an element in a page.
        /// </summary>
        /// <param name="propertyName">Property name of css.</param>
        /// <returns>css property value of an element in string format.</returns>
        public string GetCSSValue(string propertyName)
        {
            string text = null;
            element = FindElement(by, wait);
            try
            {
                text = element.GetCssValue(propertyName);
                Console.WriteLine("CSS value in element found: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find CSS value: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Get value of a css propoerty of an element in a page.
        /// </summary>
        /// <param name="propertyName">Property name of css.</param>
        /// <param name="index">Index of element in a page.</param>
        /// <returns>css property value of an element in string format.</returns>
        public string GetCSSValue(string propertyName, int index)
        {
            string text = null;
            element = FindElement(by, wait);
            try
            {
                text = element.GetCssValue(propertyName);
                Console.WriteLine("CSS value in element found: " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find CSS value: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return text;
        }

        /// <summary>
        /// Check if element is clickable. Currently does not support element list.
        /// </summary>
        /// <returns></returns>
        public bool IsClickable()
        {
            bool isClickable = false;
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(driver, TimeSpan.FromSeconds(wait))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
                isClickable = true;
            }
            catch
            {
                Console.WriteLine("Element not clickable: " + elementName + " (" + by + ")");
            }
            return isClickable;
        }

        /// <summary>
        /// Check if an element is displayed.
        /// </summary>
        /// <returns>True if element is displayed and False if not.</returns>
        public bool IsDisplayed()
        {
            bool isDisplayed;
            element = FindElement(by, wait);
            try
            {
                isDisplayed = element.Displayed;
            }
            catch (Exception e)
            {
                throw new Exception("Element not displayed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return isDisplayed;
        }

        /// <summary>
        /// Check if an instance of element is displayed.
        /// </summary>
        /// <param name="index">Index of element in a page.</param>
        /// <returns>True if element is displayed and False if not.</returns>
        public bool IsDisplayed(int index)
        {
            bool isDisplayed;
            elements = FindElements(by, wait);
            try
            {
                isDisplayed = elements[index].Displayed;
            }
            catch (Exception e)
            {
                throw new Exception("Element not displayed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return isDisplayed;
        }

        /// <summary>
        /// Returns the number of elements found.
        /// </summary>
        /// <returns>Number of elements having the same locator.</returns>
        public int ListSize()
        {
            int count = 0;
            elements = FindElements(by, wait);
            try
            {
                count = elements.Count;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to return element count: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
            return count;
        }

        /// <summary>
        /// Perform mouseover on an element.
        /// </summary>
        public void MouseOver()
        {
            element = FindElement(by, wait);
            try
            {
                Actions action = new Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (Exception e)
            {
                throw new Exception("Mouseover failed on element element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Perform mouseover on an instance of an element.
        /// </summary>
        /// <param name="index">Index of element in a page.</param>
        public void MouseOver(int index)
        {
            elements = FindElements(by, wait);
            try
            {
                Actions action = new Actions(driver);
                action.MoveToElement(elements[index]).Perform();
            }
            catch (Exception e)
            {
                throw new Exception("Mouseover failed on element element: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Simulates pressing of enter when an element is selected.
        /// </summary>
        public void PressEnter()
        {
            element = FindElement(by, wait);
            try
            {
                element.Submit();
                Console.WriteLine("Pressed enter.");
            }
            catch (Exception e)
            {
                throw new Exception("Enter failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Simulates pressing of enter when an element is selected.
        /// </summary>
        /// <param name="index">Index of element in a page.</param>
        public void PressEnter(int index)
        {
            elements = FindElements(by, wait);
            try
            {
                elements[index].Submit();
                Console.WriteLine("Pressed enter.");
            }
            catch (Exception e)
            {
                throw new Exception("Enter failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Select value from dropdown based from the text in dropdown list.
        /// </summary>
        /// <param name="text">Text to select in dropdown list.</param>
        public void SelectDropdownByText(string text)
        {
            element = FindElement(by, wait);
            try
            {
                SelectElement sel = new SelectElement(element);
                sel.SelectByText(text);
                Console.WriteLine("Dropdown item selected: " + text + " from element: " + by);
            }
            catch (Exception e)
            {
                throw new Exception("Dropdown selection failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Select value from an instance of dropdown based from the text in dropdown list.
        /// </summary>
        /// <param name="text">Text to select in dropdown list.</param>
        /// <param name="index">Index of element in a page.</param>
        public void SelectDropdownByText(string text, int index)
        {
            elements = FindElements(by, wait);
            try
            {
                SelectElement sel = new SelectElement(elements[index]);
                sel.SelectByText(text);
                Console.WriteLine("Dropdown item selected: " + text + " from element: "
                    + elementName + " (" + by + ")");
            }
            catch (Exception e)
            {
                throw new Exception("Dropdown selection failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Select value from dropdown based from its placement in the dropdown list.
        /// </summary>
        /// <param name="dropdownIndex">Placement in list.</param>
        public void SelectDropdownByIndex(int dropdownIndex)
        {
            element = FindElement(by, wait);
            try
            {
                SelectElement sel = new SelectElement(element);
                sel.SelectByIndex(dropdownIndex);
                Console.WriteLine("Dropdown item selected: " + (dropdownIndex + 1) + " from element: "
                    + elementName + " (" + by + ")");
            }
            catch (Exception e)
            {
                throw new Exception("Dropdown selection failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Select value from an instance of dropdown based from its placement in the dropdown list.
        /// </summary>
        /// <param name="dropdownIndex">Placement in list.</param>
        /// <param name="index">Index of element in a page.</param>
        public void SelectDropdownByIndex(int dropdownIndex, int index)
        {
            elements = FindElements(by, wait);
            try
            {
                SelectElement sel = new SelectElement(elements[index]);
                sel.SelectByIndex(dropdownIndex);
                Console.WriteLine("Dropdown item selected: " + (dropdownIndex + 1) + " from element: "
                    + elementName + " (" + by + ")");
            }
            catch (Exception e)
            {
                throw new Exception("Dropdown selection failed: " + elementName + " (" + by + ")"
                    + System.Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Scrolls page to view element on screen.
        /// </summary>
        public void ScrollToElement()
        {
            element = FindElement(by, wait);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        /// <summary>
        /// Scrolls page to view an instance of element on screen.
        /// </summary>
        /// <param name="index"></param>
        public void ScrollToElement(int index)
        {
            elements = FindElements(by, wait);
            Actions actions = new Actions(driver);
            actions.MoveToElement(elements[index]);
            actions.Perform();
        }

        /// <summary>
        /// Wait element to disappear after a specified amount of time.
        /// </summary>
        /// <param name="waitTime">Wait time per attempt to recheck.</param>
        /// <param name="attempts">Total number of attempts to check if element has disappeared.</param>
        public void WaitToDisappear(int waitTime, int attempts)
        {
            IWebElement element = null;
            bool isDisplayed = false;
            
            for (int i = 0; i < attempts; i++)
            {
                BaseTest.WaitLoading(waitTime - 1);
                element = null;
                isDisplayed = false;
                try
                {
                    element = new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                    isDisplayed = element.Displayed;
                    if (!isDisplayed)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (!(element is null) || isDisplayed)
            {
                throw new Exception("Element still displayed: " + elementName + " (" + by + ") after "
                    + (waitTime * attempts) + "seconds.");
            }
        }

        /// <summary>
        /// Wait an instance of an element to disappear after a specified amount of time.
        /// </summary>
        /// <param name="waitTime">Wait time per attempt to recheck.</param>
        /// <param name="attempts">Total number of attempts to check if element has disappeared.</param>
        /// <param name="index">Index of element in a page.</param>
        public void WaitToDisappear(int waitTime, int attempts, int index)
        {
            IList<IWebElement> elements = null;
            bool isDisplayed = false;

            for (int i = 0; i < attempts; i++)
            {
                BaseTest.WaitLoading(waitTime - 1);
                element = null;
                isDisplayed = false;
                try
                {
                    elements = (IList<IWebElement>)new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                    isDisplayed = elements[index].Displayed;
                    if (!isDisplayed)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (!(element is null) || isDisplayed)
            {
                throw new Exception("Element still displayed: " + elementName + " (" + by + ") after "
                    + (waitTime * attempts) + " seconds.");
            }
        }

        /// <summary>
        /// Wait an element to display after a specified amount of time.
        /// </summary>
        /// <param name="waitTime">Wait time per attempt to recheck.</param>
        /// <param name="attempts">Total number of attempts to check if element has disappeared.</param>
        public void WaitUntilDisplayed(int waitTime, int attempts)
        {
            IWebElement element = null;
            bool isDisplayed = false;

            for (int i = 0; i < attempts; i++)
            {
                BaseTest.WaitLoading(waitTime - 1);
                element = null;
                isDisplayed = false;
                try
                {
                    element = new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                    isDisplayed = element.Displayed;
                    if (isDisplayed)
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Waiting for element to appear: " + i + " of " + attempts + " attempts.");
                }
            }
            if (element is null)
            {
                throw new Exception("Element not displayed: " + elementName + " (" + by + ") after "
                    + (waitTime * attempts) + " seconds.");
            }
        }

        /// <summary>
        /// Wait an instance of an element to display after a specified amount of time.
        /// </summary>
        /// <param name="waitTime">Wait time per attempt to recheck.</param>
        /// <param name="attempts">Total number of attempts to check if element has disappeared.</param>
        /// <param name="index">Index of element in a page.</param>
        public void WaitUntilDisplayed(int waitTime, int attempts, int index)
        {
            IList<IWebElement> elements = null;
            bool isDisplayed = false;

            for (int i = 0; i < attempts; i++)
            {
                BaseTest.WaitLoading(waitTime - 1);
                element = null;
                isDisplayed = false;
                try
                {
                    elements = (IList<IWebElement>)new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                    isDisplayed = elements[index].Displayed;
                    if (isDisplayed)
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Waiting for element to appear: " + i + " of " + attempts + " attempts.");
                }
            }
            if (element is null)
            {
                throw new Exception("Element not displayed: " + elementName + " (" + by + ") after "
                    + (waitTime * attempts) + " seconds.");
            }
        }
    }
}
