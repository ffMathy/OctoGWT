using OctoGWT.Interfaces;
using OctoGWT.Facades;
using OpenQA.Selenium;

namespace OctoGWT.Tests.Instructions
{
    sealed class ThenIShouldSeeAGoogleResultContaining : IThenInstruction
    {
        private string term;

        public ThenIShouldSeeAGoogleResultContaining(string term)
        {
            this.term = term;
        }

        public void Run(ThenWebDriverFacade t)
        {
            //one of these items should contain the term.
            t.IShouldSeeMultipleElementsContainingText(By.CssSelector(".g"), term);
        }
    }
}
