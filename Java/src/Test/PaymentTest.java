package Test;

import java.util.List;
import java.util.Set;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import junit.framework.TestCase;

public class PaymentTest extends TestCase{
	
	private WebDriver setUpDriver() {
		System.setProperty("webdriver.chrome.driver", "driver/chromedriver.exe");
		WebDriver driver = new ChromeDriver();
        //Launch the Chrome and open Website
		driver.get("http://stagingmerchant.myrapidpay.com/trade/payment.action?token=79f077de5d09ce3244301656a8ac0d00e69d3d7d05cc5bb2");
		return driver;
	}
	public void testPerfermanceOnChrome() throws InterruptedException {
		;
		WebDriver driver = perfermanceTest(setUpDriver());
		assertTrue(driver.getTitle().contains("中国建设银行"));
		System.out.println("test perfermance successfully");
		driver.quit();
	}
	
	//Input all right information and select ccb bank 
	public WebDriver perfermanceTest(WebDriver driver) throws InterruptedException{		
 
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
	public void testEmptyInput() throws InterruptedException{
		WebDriver driver = setUpDriver();
 
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
        	if(!message.getText().contains("此处不可空白")) {
        		wrongMessageFlg = true;
        		System.out.println(message.getText());
        	}
        }
        assertFalse(wrongMessageFlg);
        System.out.println("test empty input successfully");
        driver.quit();
	}
	
	//Test if use don't select bank
	public void testNotSelectBank() {
		
		WebDriver driver = setUpDriver();
 
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
        assertTrue(bankError.getText().contains("请选择银行"));
        System.out.println("test not select bank successfully");
        driver.quit();
	}
}
