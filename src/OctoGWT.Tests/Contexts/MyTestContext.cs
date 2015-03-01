using OctoGWT.Facades;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
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

                var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

                //add chrome if installed.
                var chromePath = Path.Combine(programFilesPath, "Google", "Chrome", "Application", "chrome.exe");
                var isChromeInstalled = File.Exists(chromePath);
                if (isChromeInstalled)
                {
                    constructors.Add(delegate
                    {
                        var options = new ChromeOptions();
                        options.LeaveBrowserRunning = false;
                        options.BinaryLocation = chromePath;

                        var service = ChromeDriverService.CreateDefaultService();

                        var driver = new ChromeDriver(service, options);
                        return driver;
                    });
                }

                //add firefox if installed.
                var firefoxPath = Path.Combine(programFilesPath, "Mozilla Firefox", "firefox.exe");
                var isFirefoxInstalled = File.Exists(firefoxPath);
                if (isFirefoxInstalled)
                {
                    constructors.Add(delegate
                    {
                        var portOffset = firefoxPortOffset = firefoxPortOffset + 10;

                        var binary = new FirefoxBinary(firefoxPath);

                        var profile = new FirefoxProfile();
                        profile.EnableNativeEvents = false;
                        profile.Port = portOffset + 13379;

                        var driver = new FirefoxDriver(binary, profile);
                        return driver;
                    });
                }

                //add opera if installed.
                var operaPath = Path.Combine(programFilesPath, "Opera", "launcher.exe");
                var isOperaInstalled = File.Exists(operaPath);
                if (isOperaInstalled)
                {
                    constructors.Add(delegate
                    {
                        var options = new OperaOptions();
                        options.BinaryLocation = operaPath;
                        options.LeaveBrowserRunning = false;

                        var service = OperaDriverService.CreateDefaultService();

                        var driver = new OperaDriver(service, options);
                        return driver;
                    });
                }

                return constructors;
            }
        }
    }
}
