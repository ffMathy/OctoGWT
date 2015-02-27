# OctoGWT
## A Given-When-Then test framework on steroids
For GWT there are many test frameworks to pick from. One of these is Cucumber, which is good for what it's for (increasing readability of tests for "normal" human beings). However, developing tests in this system can take a long time.

If you think the following sample code excites you, please do read on.

```csharp
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
                w.ITypeInAnElement(By.XPath("//input[@name='q']"), "The \"GWT\" framework is cool!");
                w.IClickOnAnElement(By.XPath("//button[@type='submit']"));

                //wait for the search result list to appear.
                w.IWaitForAnElementToAppear(By.CssSelector(".srg"));
                w.IWait(5000);
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
            .Then(new ThenIShouldSeeMyGoogleResultContaining("instructions"));
        
    }
}
```

So, what exactly is going on in the above example?
