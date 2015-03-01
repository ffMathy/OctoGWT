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

                //start a GWT test by specifying it with predicates.
                context
                    .Given(g =>
                    {
                        g.IAmOnPage("https://www.google.com");
                        g.ICanSeeAnElement(By.XPath("//input[@name='q']"));
                    })
                    .When(w =>
                    {
                        w.ITypeInAnElement(By.XPath("//input[@name='q']"), "The \"GWT\" framework is cool!" + Keys.Enter);
                        w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

                        //wait for the search result list to appear.
                        w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
                    })
                    .Then(t =>
                    {
                        //now we should see multiple result items.
                        t.IShouldSeeMultipleElementsContainingText(By.CssSelector(".g"), "GWT");
                    });

                //start yet another GWT test, this time by specifying it with custom instructions.
                context
                    .Given(new GivenIAmOnGooglesFrontPage())
                    .When(new WhenISearchOnGoogleFor("I can also use \"instructions\"!"))
                    .Then(new ThenIShouldSeeAGoogleResultContaining("instructions"));
                
            }
        }

    }
}
