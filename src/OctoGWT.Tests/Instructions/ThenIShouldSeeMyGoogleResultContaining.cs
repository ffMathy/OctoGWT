using OctoGWT.Interfaces;
using OctoGWT.Facades;
using OpenQA.Selenium;

namespace OctoGWT.Tests.Instructions
{
    sealed class ThenIShouldSeeMyGoogleResultContaining : IThenInstruction
    {
        private string term;

        public ThenIShouldSeeMyGoogleResultContaining(string term)
        {
            this.term = term;
        }

        public void Run(ThenWebDriverFacade t)
        {
            const string googleSearchResultListItemCssSelector = ".g";

            //now we should see multiple result items.
            t.IShouldSeeMultipleElements(By.CssSelector(googleSearchResultListItemCssSelector));

            //one of these items should contain the term.
            t.IShouldSeeAnElementContainingText(By.CssSelector(googleSearchResultListItemCssSelector), term);
        }
    }
}
