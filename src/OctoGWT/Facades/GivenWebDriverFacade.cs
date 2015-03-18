using OctoGWT.Exceptions;
using OpenQA.Selenium;
using System;
using System.Linq;

using OctoGWT.Interfaces;

namespace OctoGWT.Facades
{
    public sealed class GivenWebDriverFacade
    {
        private readonly ParallelWebDriverFacade driver;

        internal GivenWebDriverFacade(ParallelWebDriverFacade driver)
        {
            this.driver = driver;
        }

        public void Include(IGivenInstruction instruction)
        {
            instruction.Run(this);
        }

        public void IAmOnPage(string url)
        {
            driver.NavigateToPage(url);
        }

        public void ICanSeeAnElement(By by)
        {
            driver.WaitForElements(by, (elements) =>
            {
                if(elements.Count() != 1)
                {
                    throw new GivenException("Expected a single element, but received several elements with the selector [" + by + "].");
                }
            });
        }

        public void ICanSeeMultipleElements(By by)
        {
            driver.WaitForElements(by, (elements) =>
            {
                if (elements.Count() == 1)
                {
                    throw new GivenException("Expected multiple elements, but received a single element with the selector [" + by + "].");
                }
            });
        }
    }
}
