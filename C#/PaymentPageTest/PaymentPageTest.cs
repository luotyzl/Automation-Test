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

            //Input right information
            var cardName = _driver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            cardName.SendKeys("姚鑫");
            var card = _driver.FindElement(By.Name("idCard"));
            card.Clear();
            card.SendKeys("13010519811103125X");
            var email = _driver.FindElement(By.Name("email"));
            email.Clear();
            email.SendKeys("test@gmail.com");
            var telNo = _driver.FindElement(By.Name("telNo"));
            telNo.Clear();
            telNo.SendKeys("64");
            var telNum = _driver.FindElement(By.Name("telNum"));
            telNum.Clear();
            telNum.SendKeys("123456");
            var amount = _driver.FindElement(By.Name("amount"));
            amount.Clear();
            amount.SendKeys("99");
            var reference = _driver.FindElement(By.Name("reference"));
            reference.Clear();
            reference.SendKeys("test reference");

            var submitBtn = _driver.FindElement(By.Id("goPayId"));
            submitBtn.Click();

            //Wait page loaded
            WebDriverWait submitWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            submitWait.Until(ExpectedConditions.ElementExists(By.Id("pay_now")));
            var payBtn = _driver.FindElement(By.Id("pay_now"));
            var ccb = _driver.FindElement(By.ClassName("bank02"));
            ccb.Click();
            payBtn.Click();
            Thread.Sleep(20000);

            //Wait openning bank payment page in new  tab
            var windowHandle = _driver.WindowHandles;
            foreach (var window in windowHandle)
            {
                _driver.SwitchTo().Window(window);
                if (_driver.Title != "Payment") break; 
            }
            //Check if open the bank page
            Assert.IsTrue(_driver.Title.Contains("中国建设银行"));
            _driver.Quit();
        }

        [TestMethod]
        public void EmptyInputTest()
        {
            _driver.Navigate().GoToUrl(Weburl);

            //Input empty information
            var cardName = _driver.FindElement(By.Id("idCardName"));
            cardName.Clear();
            var card = _driver.FindElement(By.Name("idCard"));
            card.Clear();
            var email = _driver.FindElement(By.Name("email"));
            email.Clear();
            var telNo = _driver.FindElement(By.Name("telNo"));
            telNo.Clear();
            var telNum = _driver.FindElement(By.Name("telNum"));
            telNum.Clear();
            var amount = _driver.FindElement(By.Name("amount"));
            amount.Clear();
            var reference = _driver.FindElement(By.Name("reference"));
            reference.Clear();

            //Click next button
            var submitBtn = _driver.FindElement(By.Id("goPayId"));
            submitBtn.Click();
            var errorMessages = _driver.FindElements(By.ClassName("formError"));
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
            _driver.Quit();
        }

        [TestMethod]
        public void NotSeletBankTest()
        {
            _driver.Navigate().GoToUrl(Weburl);

            //Input right information
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

            //Click next button
            var submitBtn = _driver.FindElement(By.Id("goPayId"));
            submitBtn.Click();

            //Wait page loaded
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("pay_now")));
            var payBtn = _driver.FindElement(By.Id("pay_now"));
            payBtn.Click();

            var bankError = _driver.FindElement(By.Id("bank_err"));
            Assert.IsTrue(bankError.Text.Contains("请选择银行"));
            _driver.Quit();
        }
    }
}
