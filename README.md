# OctoGWT
## Given-When-Then test framework on steroids
*Powered by Selenium WebDriver*

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

The above example contains two GWTs that will run in parallel, basically doing almost the same thing. The first one is defined via *predicates and callbacks*, while the latter is defined using *instruction classes*.

Each of them see if they can search for something on Google. They start by going to the front page (the **Given** clause), from where they type in some text in the search field (the **When** clause), and then finally assert (check) if a result is present in the result list (the **Then** clause).

## Parallelism simplified
Running tests (especially WebDriver driven integration tests) can take a while. And if you apply Continuous Integration in your workflow, this typically means that these tests must run on a build server every time you want to get something out. 

Wouldn't it be great if you could somehow **run several GWTs at once**, on **several browsers at the same time**? With OctoGWT, all GWTs are run in several browsers (that you specify) at the same time. Furthermore, each GWT that is created within the same context is run in parallel.

That's insanely effective if you think about it. Let's say you've created a setup with three test browsers. One for Chrome, one for Firefox, and one for Internet Explorer. Running the test from the code sample above (consisting of two GWTs defined in the same context) would then run in parallel, with each GWT **also** running on all three browsers. That's 6 browser instances running **at the same time** evaluating **two GWTs at once**.

## Get rid of garbage
It's frustrating when your build server runs out of memory because after a few thousand test runs, or even more frustrating when your PC is left with a lot of open webbrowsers after testing, because the test didn't clean up properly.

OctoGWT takes care of all that. When a context is disposed (after leaving the ``using`` scope), it also closes the browser windows and all associated resources with it.

