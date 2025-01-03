using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace AutoLoginNetwork
{
    internal class Handler
    {
        public static void AutoLoginNetwork(string usernameIn, string passwordIn)
        {
            

            Console.WriteLine("[EVENT] Login is running...");
            var trackingNetwork = false;
            do
            {
                string host = "www.google.com";
                if (!IsNetworkAvailable(host))
                {
                    Console.WriteLine("Network is down.");
                    return;
                }

                var chromeOptions = new ChromeOptions();
                chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
                IWebDriver driver = new ChromeDriver(chromeOptions);
                try
                {
                    driver.Navigate().GoToUrlAsync("http://detectportal.firefox.com/canonical.html");
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
                    IReadOnlyCollection<IWebElement> getFormLogin = driver.FindElements(By.Id("login_form"));
                    if (getFormLogin.Count > 0)
                    {
                        IReadOnlyCollection<IWebElement> getUserName = driver.FindElements(By.Id("user"));
                        IReadOnlyCollection<IWebElement> getPassword = driver.FindElements(By.Id("passwd"));
                        IReadOnlyCollection<IWebElement> getSubmitButton = driver.FindElements(By.Id("submit"));
                        if(getUserName.Count > 0 && getPassword.Count > 0 && getSubmitButton.Count > 0)
                        {
                            var userName = getUserName.First();
                            var password = getPassword.First();
                            var submitButton = getSubmitButton.First();
                            userName.SendKeys(usernameIn);
                            password.SendKeys(passwordIn);
                            WebDriverWait waitSendKeys = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                            submitButton.Click();
                            WebDriverWait waitSubmit = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                            IReadOnlyCollection<IWebElement> getDetectPortal = driver.FindElements(By.Id("aside"));
                            if (getDetectPortal.Count > 0)
                            {
                                // Perform actions on the element if necessary
                                Console.WriteLine("Network is connected.");
                                trackingNetwork = true;
                            }
                        }
                    }
                    else
                    {
                        IReadOnlyCollection<IWebElement> getDetectPortal = driver.FindElements(By.Id("aside"));
                        if(getDetectPortal.Count > 0)
                        {
                            // Perform actions on the element if necessary
                            Console.WriteLine("Network is connected.");
                            trackingNetwork = true;
                        }
                    }

                }
                catch (WebDriverTimeoutException ex)
                {
                    Console.WriteLine($"Timed out waiting for element: {ex.Message}");
                }
                finally
                {
                    driver.Quit();
                }
            } while (!trackingNetwork);

        }
        static bool IsNetworkAvailable(string host)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(host);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
