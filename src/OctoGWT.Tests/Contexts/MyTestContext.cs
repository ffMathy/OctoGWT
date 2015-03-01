using OctoGWT.Facades;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;

namespace OctoGWT.Tests.Contexts
{
    sealed class MyTestContext : ContextBase
    {

        private static volatile int firefoxPortOffset;

        protected override IEnumerable<Func<IWebDriver>> WebDriverConstructors
        {
            get
            {
                var constructors = new List<Func<IWebDriver>>();

                var chromePath = Path.Combine("Google", "Chrome", "Application", "chrome.exe");
                var chromeInstalled = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), chromePath)) || File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), chromePath));
                if (chromeInstalled)
                {
                    constructors.Add(delegate
                    {
                        var options = new ChromeOptions();
                        options.LeaveBrowserRunning = false;

                        var service = ChromeDriverService.CreateDefaultService();

                        var chromeDriver = new ChromeDriver(service, options);
                        return chromeDriver;
                    });
                }

                return constructors;
            }
        }
    }
}
