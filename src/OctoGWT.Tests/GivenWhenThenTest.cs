using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OctoGWT.Tests.Contexts;
using OctoGWT.Tests.Instructions;

namespace OctoGWT.Tests
{
    [TestClass]
    public class GivenWhenThenTest
    {
        [TestMethod]
        public void TestGoogleSearch()
        {
            using (var context = new MyTestContext())
            {
                const string googleSearchTextFieldXPath = "//input[@name='q']";
                const string googleSearchResultListItemCssSelector = ".g";

                //start a GWT test by specifying it with predicates.
                context
                    .Given(g =>
                    {
                        g.IAmOnPage("https://www.google.com");
                        g.ICanSeeAnElement(By.XPath(googleSearchTextFieldXPath));
                    })
                    .When(w =>
                    {
                        w.ITypeInAnElement(By.XPath(googleSearchTextFieldXPath), "The \"GWT\" framework is cool!");
                        w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

                        //wait for the search result list to appear.
                        w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
                        w.IWait(5000);
                    })
                    .Then(t =>
                    {
                        //now we should see multiple result items.
                        t.IShouldSeeMultipleElements(By.CssSelector(googleSearchResultListItemCssSelector));

                        //one of these items should contain the text GivenWhenThen.
                        t.IShouldSeeAnElementContainingText(By.CssSelector(googleSearchResultListItemCssSelector), "GWT");
                    });

                //start yet another GWT test, this time by specifying it with custom instructions.
                context
                    .Given(new GivenIAmOnGooglesFrontPage())
                    .When(new WhenISearchOnGoogleFor("I can also use \"instructions\"!"))
                    .Then(new ThenIShouldSeeMyGoogleResultContaining("instructions"));
                
            }
        }

    }
}
