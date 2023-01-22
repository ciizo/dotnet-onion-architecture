using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Banking.Domain.Service.AccountLogic
{
    public class IBAN_Service : IIBAN_Service
    {
        /// <summary>
        /// TODO clone logic from js on website http://randomiban.com/?country=Netherlands
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateIBAN()
        {
            string iban;
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            // ref https://sites.google.com/chromium.org/driver/downloads
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(@"D:\chromedriver_win32_109", "chromedriver.exe");
            using (var driver = new ChromeDriver(service, options))
            {
                driver.Navigate().GoToUrl("http://randomiban.com/?country=Netherlands");
                iban = driver.FindElement(By.Id("demo")).Text;
            }

            return iban;
        }
    }
}