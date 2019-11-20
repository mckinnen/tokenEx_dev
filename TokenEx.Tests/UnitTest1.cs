using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace TokenEx.Tests {
    [TestClass]
    public class MySeleniumTests {
        private TestContext testContextInstance;
        private IWebDriver driver;
        private string appURL;

        public MySeleniumTests() {
        }

        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBingSearchTest() {
            driver.Navigate().GoToUrl(appURL + "/");
            driver.FindElement(By.Id("sb_form_q")).SendKeys("Azure Pipelines");
            driver.FindElement(By.Id("sb_form_go")).Click();
            driver.FindElement(By.XPath("//ol[@id='b_results']/li/h2/a")).Click();
            Assert.IsTrue(driver.Title.Contains("Azure Pipelines"), "Verified title of the page");
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public void SetupTest() {
            appURL = "http://www.bing.com/";

            string browser = "Firefox";
            switch (browser) {
                default:
                case "Chrome":
                var ChromeOptions = new ChromeOptions();

                ChromeOptions.AddArgument(@"--user-data-dir=C:\Users\uiTestProfile");
                driver = new ChromeDriver(ChromeOptions);
                break;

                case "Firefox":
                var FfOptions = new FirefoxOptions();
                FfOptions.AddArgument(@"--user-data-dir=C:\Users\uiTestProfile");

                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                service.FirefoxBinaryPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";

                driver = new FirefoxDriver(service);
                break;

                case "IE":
                driver = new InternetExplorerDriver();
                break;
            }

        }

        [TestCleanup()]
        public void MyTestCleanup() {
            driver.Quit();
        }
    }
}
