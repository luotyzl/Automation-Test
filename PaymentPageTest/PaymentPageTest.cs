using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using Selenium;

namespace PaymentPageTest
{
    [TestClass]
    public class PaymentPageTest
    {
        private readonly IWebDriver _driver = new ChromeDriver();
        private const string Weburl = "http://stagingmerchant.myrapidpay.com/trade/payment.action?token=79f077de5d09ce3244301656a8ac0d00e69d3d7d05cc5bb2";

        [TestMethod]
        public void PerfermanceTest()
        {
            _driver.Navigate().GoToUrl(Weburl);
            var cardName = _driver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            cardName.SendKeys("姚鑫");
            var card = _driver.FindElement(By.Name("idCard"));
            card.Clear();
            card.SendKeys("13010519811103125X");
            var email = _driver.FindElement(By.Name("email"));
            email.Clear();
            email.SendKeys("test@gmail.com");
            var amount = _driver.FindElement(By.Name("amount"));
            amount.Clear();
            amount.SendKeys("99");
            var reference = _driver.FindElement(By.Name("reference"));
            reference.Clear();
            reference.SendKeys("test reference");

            var submitBtn = _driver.FindElement(By.Id("goPayId"));
            submitBtn.Click();
            WebDriverWait submitWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            submitWait.Until(ExpectedConditions.ElementExists(By.Id("pay_now")));
            var payBtn = _driver.FindElement(By.Id("pay_now"));
            var ccb = _driver.FindElement(By.ClassName("bank02"));
            ccb.Click();
            payBtn.Click();
            Thread.Sleep(10000);
            var windowHandle = _driver.WindowHandles;
            foreach (var window in windowHandle)
            {
                _driver.SwitchTo().Window(window);
                if (_driver.Title != "Payment") break; 
            }
            Assert.IsTrue(_driver.Title.Contains("中国建设银行"));
        }
    }
}
