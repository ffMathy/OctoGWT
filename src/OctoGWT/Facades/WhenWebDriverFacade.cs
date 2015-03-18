using OctoGWT.Exceptions;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

using OctoGWT.Interfaces;

namespace OctoGWT.Facades
{
    public sealed class WhenWebDriverFacade
    {
        private readonly ParallelWebDriverFacade driver;

        internal WhenWebDriverFacade(ParallelWebDriverFacade driver)
        {
            this.driver = driver;
        }

        public void Include(IWhenInstruction instruction)
        {
            instruction.Run(this);
        }

        public void IClickOnAnElement(By by)
        {
            driver.WaitForElements(by, (elements) =>
            {
                if (elements.Count() > 1)
                {
                    throw new WhenException("Tried finding an element to click on, but found multiple elements with the selector [" + by + "].");
                }

                var firstElement = elements.First();
                firstElement.Click();
            });
        }

        public void ITypeInAnElement(By by, string text, bool simulateNaturalTyping = false)
        {
            var random = new Random();

            driver.WaitForElements(by, (elements) =>
            {
                if(elements.Count() > 1)
                {
                    throw new WhenException("Tried finding an element to type in, but found multiple elements with the selector [" + by + "].");
                }
                
                var firstElement = elements.First();
                if (simulateNaturalTyping)
                {
                    var writtenText = string.Empty;
                    var currentOffset = 0;
                    while (writtenText != text)
                    {
                        var sequenceLength = random.Next(1, 3);
                        var randomDelay = random.Next(150, 250);

                        var sequence = text.Substring(currentOffset, Math.Min(sequenceLength, text.Length - currentOffset));
                        writtenText += sequence;
                        currentOffset += sequence.Length;

                        firstElement.SendKeys(sequence);

                        Thread.Sleep(randomDelay);
                    }
                } else
                {
                    firstElement.SendKeys(text);
                }
            });
        }

        /// <summary>
        /// Will hover the mouse over an element. Note that the mouse will stop hovering if you navigate away from the page or perform similar breaking tasks.
        /// </summary>
        /// <param name="by"></param>
        public bool IHoverMyMouseOverAnElement(By by)
        {
            throw new NotImplementedException();
        }

        public void IWait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public void IWaitForAnElementToAppear(By by)
        {
            driver.WaitForElements(by, (elements) =>
            {
                if (elements.Count() > 1)
                {
                    throw new WhenException("Tried to wait for an element to appear, but found multiple elements with the selector [" + by + "].");
                }
            });
        }
        
    }
}
