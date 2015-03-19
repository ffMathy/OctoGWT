using OctoGWT.Facades;
using OctoGWT.Interfaces;
using OpenQA.Selenium;

namespace OctoGWT.Tests.Instructions
{
    sealed class WhenISearchOnGoogleFor : IWhenInstruction
    {
        private readonly string term;

        public WhenISearchOnGoogleFor(string term)
        {
            this.term = term;
        }

        public void Run(GivenWhenWebDriverFacade w)
        {
            //type in the term to the search field.
            w.ITypeInAnElement(By.XPath("//input[@name='q']"), term + Keys.Enter);
            w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

            //wait for the search result list to appear.
            w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
        }
    }
}
