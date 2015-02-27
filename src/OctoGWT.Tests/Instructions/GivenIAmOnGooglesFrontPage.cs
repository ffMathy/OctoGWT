using OctoGWT.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoGWT.Facades;
using OpenQA.Selenium;

namespace OctoGWT.Tests.Instructions
{
    sealed class GivenIAmOnGooglesFrontPage : IGivenInstruction
    {
        public void Run(GivenWebDriverFacade g)
        {
            g.IAmOnPage("https://www.google.com");
            g.ICanSeeAnElement(By.XPath("//input[@name='q']"));
        }
    }
}
