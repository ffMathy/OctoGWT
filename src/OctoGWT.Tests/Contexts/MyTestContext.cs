using OctoGWT.Facades;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;

namespace OctoGWT.Tests.Contexts
{
    sealed class MyTestContext : ContextBase
    {

        protected override IEnumerable<Func<IWebDriver>> WebDriverConstructors
        {
            get
            {
                var constructors = new List<Func<IWebDriver>>();

                constructors.Add(delegate
                {
                    var options = new ChromeOptions();
                    options.LeaveBrowserRunning = false;

                    var service = ChromeDriverService.CreateDefaultService();

                    var chromeDriver = new ChromeDriver(service, options);
                    return chromeDriver;
                });

                constructors.Add(delegate
                {
                    var profile = new FirefoxProfile();

                    var firefoxDriver = new FirefoxDriver(profile);
                    return firefoxDriver;
                });

                return constructors;
            }
        }
    }
}
