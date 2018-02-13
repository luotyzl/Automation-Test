using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using Selenium;

namespace PaymentPageTest
{
    [TestClass]
    public class PaymentPageTest
    {
        private readonly IWebDriver _chromeDriver = new ChromeDriver();
        private readonly IWebDriver _fireFoxDriver = new FirefoxDriver();
        private const string Weburl = "http://stagingmerchant.myrapidpay.com/trade/payment.action?token=79f077de5d09ce3244301656a8ac0d00e69d3d7d05cc5bb2";

        [TestMethod]
        public void PerfermanceOnChrome()
        {
            var driver = PerfermanceTest(_chromeDriver);
            Assert.IsTrue(driver.Title.Contains("中国建设银行"));
            driver.Quit();
        }

        [TestMethod]
        public void PerfermanceOnFirefox()
        {
            var driver = PerfermanceTest(_fireFoxDriver);
            Assert.IsTrue(driver.Title.Contains("中国建设银行"));
            driver.Quit();
        }
        public IWebDriver PerfermanceTest(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(Weburl);

            //check language is chinese
            var languageTag = driver.FindElement(By.ClassName("lan-container"));
            var languateSpan = languageTag.FindElement(By.TagName("span"));
            if (languateSpan.Text == "中文")
            {
                languageTag.Click();
            }
            //Input right information
            var cardName = driver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            cardName.SendKeys("姚鑫");
            var card = driver.FindElement(By.Name("idCard"));
            card.Clear();
            card.SendKeys("13010519811103125X");
            var email = driver.FindElement(By.Name("email"));
            email.Clear();
            email.SendKeys("test@gmail.com");
            var telNo = driver.FindElement(By.Name("telNo"));
            telNo.Clear();
            telNo.SendKeys("64");
            var telNum = driver.FindElement(By.Name("telNum"));
            telNum.Clear();
            telNum.SendKeys("123456");
            var amount = driver.FindElement(By.Name("amount"));
            amount.Clear();
            amount.SendKeys("99");
            var reference = driver.FindElement(By.Name("reference"));
            reference.Clear();
            reference.SendKeys("test reference");

            var submitBtn = driver.FindElement(By.Id("goPayId"));
            submitBtn.Click();

            //Wait page loaded
            WebDriverWait submitWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            submitWait.Until(ExpectedConditions.ElementExists(By.Id("pay_now")));
            var payBtn = driver.FindElement(By.Id("pay_now"));
            var ccb = driver.FindElement(By.ClassName("bank02"));
            ccb.Click();
            payBtn.Click();
            Thread.Sleep(20000);

            //Wait openning bank payment page in new  tab
            var windowHandle = driver.WindowHandles;
            foreach (var window in windowHandle)
            {
                driver.SwitchTo().Window(window);
                if (driver.Title != "Payment") break; 
            }
            //Check if open the bank page
            return driver;
        }

        [TestMethod]
        public void EmptyInputTest()
        {
            _chromeDriver.Navigate().GoToUrl(Weburl);

            //Input empty information
            var cardName = _chromeDriver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            var card = _chromeDriver.FindElement(By.Name("idCard"));
            card.Clear();
            var email = _chromeDriver.FindElement(By.Name("email"));
            email.Clear();
            var telNo = _chromeDriver.FindElement(By.Name("telNo"));
            telNo.Clear();
            var telNum = _chromeDriver.FindElement(By.Name("telNum"));
            telNum.Clear();
            var amount = _chromeDriver.FindElement(By.Name("amount"));
            amount.Clear();
            var reference = _chromeDriver.FindElement(By.Name("reference"));
            reference.Clear();

            //Click next button
            var submitBtn = _chromeDriver.FindElement(By.Id("goPayId"));
            submitBtn.Click();
            var errorMessages = _chromeDriver.FindElements(By.ClassName("formError"));
            var wrongMessageFlg = false;

            //check all validation message
            foreach (var message in errorMessages)
            {
                if (!message.Text.Contains("此处不可空白"))
                {
                    wrongMessageFlg = true;
                }
            }
            //Check if open the bank page
            Assert.IsFalse(wrongMessageFlg);
            _chromeDriver.Quit();
        }

        [TestMethod]
        public void NotSeletBankTest()
        {
            _chromeDriver.Navigate().GoToUrl(Weburl);

            //Input right information
            var cardName = _chromeDriver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            cardName.SendKeys("姚鑫");
            var card = _chromeDriver.FindElement(By.Name("idCard"));
            card.Clear();
            card.SendKeys("13010519811103125X");
            var email = _chromeDriver.FindElement(By.Name("email"));
            email.Clear();
            email.SendKeys("test@gmail.com");
            var amount = _chromeDriver.FindElement(By.Name("amount"));
            amount.Clear();
            amount.SendKeys("99");
            var reference = _chromeDriver.FindElement(By.Name("reference"));
            reference.Clear();
            reference.SendKeys("test reference");

            //Click next button
            var submitBtn = _chromeDriver.FindElement(By.Id("goPayId"));
            submitBtn.Click();

            //Wait page loaded
            WebDriverWait wait = new WebDriverWait(_chromeDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("pay_now")));
            var payBtn = _chromeDriver.FindElement(By.Id("pay_now"));
            payBtn.Click();

            var bankError = _chromeDriver.FindElement(By.Id("bank_err"));
            Assert.IsTrue(bankError.Text.Contains("请选择银行"));
            _chromeDriver.Quit();
        }
    }
}
