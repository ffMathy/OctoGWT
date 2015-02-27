using OctoGWT.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OctoGWT.Facades
{
    public sealed class ThenWebDriverFacade
    {
        private ParallelWebDriverFacade driver;

        internal ThenWebDriverFacade(ParallelWebDriverFacade driver)
        {
            this.driver = driver;
        }

        public void IShouldSeeAnElement(By by)
        {
            driver.GetElements(by, (elements) =>
            {
                if (!elements.Any())
                {
                    throw new ThenException("Expected a single element, but received no elements with the selector [" + by + "].");
                }

                if (elements.Count() != 1)
                {
                    throw new ThenException("Expected a single element, but received several elements with the selector [" + by + "].");
                }
            });
        }

        public void IShouldNotSeeAnElement(By by)
        {
            driver.GetElements(by, (elements) =>
            {
                if (elements.Any())
                {
                    var amount = "several elements";
                    if(elements.Count() == 1)
                    {
                        amount = "a single element";
                    }

                    throw new ThenException("Expected no elements, but received " + amount + " with the selector [" + by + "].");
                }
            });
        }

        public void IShouldSeeMultipleElements(By by)
        {
            driver.GetElements(by, (elements) =>
            {
                if (!elements.Any())
                {
                    throw new ThenException("Expected multiple elements, but received no elements with the selector [" + by + "].");
                }

                if (elements.Count() == 1)
                {
                    throw new ThenException("Expected multiple elements, but received a single element with the selector [" + by + "].");
                }
            });
        }

        public void IShouldSeeAnElementContainingText(By by, string text)
        {
            driver.GetElements(by, (elements) =>
            {
                if (!elements.Any())
                {
                    throw new ThenException("Expected a single element, but received no elements with the selector [" + by + "].");
                }

                var firstElement = elements.First();
                if(!firstElement.Text.Contains(text))
                {
                    throw new ThenException("Expected a single element, but received no elements with the text [" + text + "].");
                }
            });
        }

        public void IShouldSeeMultipleElementsContainingText(By by, string text)
        {
            driver.GetElements(by, (elements) =>
            {
                if (!elements.Any())
                {
                    throw new ThenException("Expected multiple elements, but received no elements with the selector [" + by + "].");
                }

                if (elements.Count() == 1)
                {
                    throw new ThenException("Expected multiple elements, but received a single element with the selector [" + by + "].");
                }

                var matchCount = elements.Count(e => e.Text.Contains(text));
                if (matchCount == 0)
                {
                    throw new ThenException("Expected multiple elements, but received no elements with the text [" + text + "].");
                } else if(matchCount == 1)
                {
                    throw new ThenException("Expected multiple elements, but received a single element with the text [" + text + "].");
                }
            });
        }

        public void IShouldNotSeeAnElementContainingText(By by, string text)
        {
            driver.GetElements(by, (elements) =>
            {
                var matchCount = elements.Count(e => e.Text.Contains(text));
                if (matchCount != 0)
                {
                    var amount = "several elements";
                    if (matchCount == 1)
                    {
                        amount = "a single element";
                    }

                    throw new ThenException("Expected no elements, but received " + amount + " with the text [" + text + "].");
                }
            });
        }

    }
}
