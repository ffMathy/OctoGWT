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
    sealed class WhenISearchOnGoogleFor : IWhenInstruction
    {
        private string term;

        public WhenISearchOnGoogleFor(string term)
        {
            this.term = term;
        }

        public void Run(WhenWebDriverFacade w)
        {
            //type in the term to the search field.
            w.ITypeInAnElement(By.XPath("//input[@name='q']"), term + Keys.Enter);
            w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

            //wait for the search result list to appear.
            w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
        }
    }
}
