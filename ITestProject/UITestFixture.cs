using Allure.Commons;
using Atata;
using NUnit.Allure.Core;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace ITestProject
{

    [TestFixture]
    public class UITestFixture
    {
        public AtataConfig Config
        {
            get { return AtataConfig.Current; }
        }

        [OneTimeTearDown]
        public void AddAttachmentAfter()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                AllureLifecycle.Instance.AddAttachment($"{ AtataContext.Current.TestName}.png",
                "image/png",
                ((ITakesScreenshot)AtataContext.Current.Driver).GetScreenshot().AsByteArray);
            }
        }

        [SetUp]
        public void SetUp()
        {
            AtataContext.Configure().
                ApplyJsonConfig<AtataConfig>().
                UseChrome().
                WithArguments("start-maximized").
                UseBaseUrl("https://www.i.ua/").
                UseCulture("en-us").
                UseNUnitTestName().
                AddNUnitTestContextLogging().
                LogNUnitError().
                UseAssertionExceptionType<NUnit.Framework.AssertionException>().
                UseNUnitAggregateAssertionStrategy().
                //AddScreenshotFileSaving().
                //  WithFolderPath(() => TestContext.CurrentContext.TestDirectory+ $@"\Logs").
                //  WithFileName(screenshotInfo => $"{ AtataContext.Current.TestName}").
                //TakeScreenshotOnNUnitError().
                Build();
        }

        [TearDown]
        virtual public void TearDown()
        {
            AddAttachmentAfter();
            AtataContext.Current?.CleanUp();
        }

        protected EmailPage LoginToEmail()
        {
            return Go.To<LoginPage>()
                 .Login.Set(Config.Account.Email)
                 .Password.Set(Config.Account.Password)
                 .Domain.Set(Config.Account.Domain)
                 .SignIn.ClickAndGo();
        }
    }
}
