# OctoGWT
## A Given-When-Then test framework on steroids
For GWT there are many test frameworks to pick from. One of these is Cucumber, which is good for what it's for (increasing readability of tests for "normal" human beings). However, developing tests in this system can take a long time.

If you think the following sample code excites you, please do read on.

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

                context
                    .Given(g =>
                    {
                        g.IAmOnPage("https://www.google.com");
                        g.ICanSeeAnElement(By.XPath(googleSearchTextFieldXPath));
                    })
                    .When(w =>
                    {
                        w.ITypeInAnElement(By.XPath(googleSearchTextFieldXPath), "The \"Given-When-Then\" framework is cool!");
                        w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

                        //wait for the search result list to appear.
                        w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
                    })
                    .Then(t =>
                    {
                        //now we should see multiple result items.
                        t.IShouldSeeMultipleElements(By.CssSelector(googleSearchResultListItemCssSelector));

                        //one of these items should contain the text GivenWhenThen.
                        t.IShouldSeeAnElementContainingText(By.CssSelector(googleSearchResultListItemCssSelector), "Given-When-Then");
                    });

                context
                    .Given(new GivenIAmOnGooglesFrontPage())
                    .When(new WhenISearchOnGoogleFor("I can also use \"instructions\"!"))
                    .Then(new ThenIShouldSeeMyGoogleResultContaining("instructions"));
                
            }
        }

    }
