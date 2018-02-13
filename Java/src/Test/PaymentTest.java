package Test;

import java.util.List;
import java.util.Set;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import junit.framework.TestCase;

public class PaymentTest extends TestCase{
	
	private enum browser{
		Chrome,
		Firefox
	}
	
	private WebDriver setUpDriver(browser browser) {
		
		if(browser == browser.Chrome) {
			System.setProperty("webdriver.chrome.driver", "driver/chromedriver.exe");
			WebDriver chromeDriver = new ChromeDriver();
	        //Launch the Chrome and open Website
			chromeDriver.get("http://stagingmerchant.myrapidpay.com/trade/payment.action?token=79f077de5d09ce3244301656a8ac0d00e69d3d7d05cc5bb2");
			return chromeDriver;
		} else {
			String service = "driver/geckodriver.exe";  
			System.setProperty("webdriver.gecko.driver", service);
			WebDriver fireFoxDriver = new FirefoxDriver();
	        //Launch the firefox and open Website
			fireFoxDriver.get("http://stagingmerchant.myrapidpay.com/trade/payment.action?token=79f077de5d09ce3244301656a8ac0d00e69d3d7d05cc5bb2");
			return fireFoxDriver;
		}
		
	}
	
	//test on chrome browser
	public void testPerfermanceOnChrome() throws InterruptedException {
		WebDriver driver = PerfermanceTest(setUpDriver(browser.Chrome));
		assertTrue(driver.getTitle().contains("中国建设银行"));
		System.out.println("test perfermance on chrome successfully");
		driver.quit();
	}
	
	//test on firefox browser
	public void testPerfermanceOnFirefox() throws InterruptedException {
		WebDriver driver = PerfermanceTest(setUpDriver(browser.Firefox));
		assertTrue(driver.getTitle().contains("中国建设银行"));
		System.out.println("test perfermance on firefox successfully");
		driver.quit();
	}
	
	public void testEmptyInputOnChrome() throws InterruptedException {
		WebDriver driver = setUpDriver(browser.Chrome);
		boolean testResult = EmptyInput(driver);
		assertFalse(testResult);
		System.out.println("test empty input on chrome successfully");
		driver.quit();
	}
	
	public void testEmptyInputOnFirefox() throws InterruptedException {
		WebDriver driver = setUpDriver(browser.Firefox);
		boolean testResult = EmptyInput(driver);
		assertFalse(testResult);
		System.out.println("test empty input on firefox successfully");
		driver.quit();
	}

	public void testNotSelectBankOnChrome() throws InterruptedException {
		WebDriver driver = setUpDriver(browser.Chrome);
		String testResult = NotSelectBank(driver);
        assertTrue(testResult.contains("请选择银行") || testResult.contains("Select your bank"));
        System.out.println("test not select bank on firefox successfully");
        driver.quit();
	}
	
	public void testNotSelectBankOnFirefox() throws InterruptedException {
		WebDriver driver = setUpDriver(browser.Firefox);
		String testResult = NotSelectBank(driver);
        assertTrue(testResult.contains("请选择银行") || testResult.contains("Select your bank"));
        System.out.println("test not select bank on firefox successfully");
        driver.quit();
	}
	
	//Input all right information and select ccb bank 
	public WebDriver PerfermanceTest(WebDriver driver) throws InterruptedException{		
		
		//Input right information
		WebElement cardName = driver.findElement(By.id("idCardName"));
        cardName.clear();
        cardName.sendKeys("姚鑫");
        WebElement card = driver.findElement(By.name("idCard"));
        card.clear();
        card.sendKeys("13010519811103125X");
        WebElement email = driver.findElement(By.name("email"));
        email.clear();
        email.sendKeys("test@gmail.com");
        WebElement telNumber = driver.findElement(By.name("telNum"));
        telNumber.clear();
        telNumber.sendKeys("123456");
        WebElement telNo = driver.findElement(By.name("telNo"));
        telNo.clear();
        telNo.sendKeys("64");
        WebElement amount = driver.findElement(By.name("amount"));
        amount.clear();
        amount.sendKeys("99");
        WebElement reference = driver.findElement(By.name("reference"));
        reference.clear();
        reference.sendKeys("test reference");

        //Click next button
        WebElement submitBtn = driver.findElement(By.id("goPayId"));
        submitBtn.click();     
        
        //Wait page loaded
        WebDriverWait wait = new WebDriverWait(driver, 20);
        wait.until(ExpectedConditions.presenceOfElementLocated(By.id("pay_now")));
        WebElement ccb = driver.findElement(By.className("bank02"));
        WebElement payBtn = driver.findElement(By.id("pay_now"));
        ccb.click();
        payBtn.click();
        
        //Wait openning bank payment page in new tab
        Thread.sleep(20000);
        Set<String> windowHandle = driver.getWindowHandles();
        for (String windowId : windowHandle) {
            if (driver.switchTo().window(windowId).getTitle().contains("Payment")) {
                continue;
            }
        }
        //return bank page title        
        return driver;
	}
	
	//Test empty input and click next
	public boolean EmptyInput(WebDriver driver) throws InterruptedException{

		//Input Empty information
		WebElement cardName = driver.findElement(By.id("idCardName"));
        cardName.clear();
        WebElement card = driver.findElement(By.name("idCard"));
        card.clear();
        WebElement email = driver.findElement(By.name("email"));
        email.clear();
        WebElement amount = driver.findElement(By.name("amount"));
        amount.clear();
        WebElement reference = driver.findElement(By.name("reference"));
        reference.clear();

        //Click next button
        WebElement submitBtn = driver.findElement(By.id("goPayId"));
        submitBtn.click();     
        
        //check all validation message
        List<WebElement> errorMessages = driver.findElements(By.className("formError"));
        boolean wrongMessageFlg = false;
        for(WebElement message : errorMessages) {
        	if(!message.getText().contains("此处不可空白") && !message.getText().contains("This field cannot be left blank")) {
        		wrongMessageFlg = true;
        		System.out.println(message.getText());
        	}
        }
        return wrongMessageFlg;
	}
	
	//Test if use don't select bank
	public String NotSelectBank(WebDriver driver) { 
		
		//Input right information
		WebElement cardName = driver.findElement(By.id("idCardName"));
        cardName.clear();
        cardName.sendKeys("姚鑫");
        WebElement card = driver.findElement(By.name("idCard"));
        card.clear();
        card.sendKeys("13010519811103125X");
        WebElement email = driver.findElement(By.name("email"));
        email.clear();
        email.sendKeys("test@gmail.com");
        WebElement amount = driver.findElement(By.name("amount"));
        amount.clear();
        amount.sendKeys("99");
        WebElement reference = driver.findElement(By.name("reference"));
        reference.clear();
        reference.sendKeys("test reference");

        //Click next button
        WebElement submitBtn = driver.findElement(By.id("goPayId"));
        submitBtn.click();     
        
        //Wait page loaded
        WebDriverWait wait = new WebDriverWait(driver, 20);
        wait.until(ExpectedConditions.presenceOfElementLocated(By.id("pay_now")));
        WebElement payBtn = driver.findElement(By.id("pay_now"));
        payBtn.click();
        
        WebElement bankError = driver.findElement(By.id("bank_err"));
        return bankError.getText();
	}
	
}
