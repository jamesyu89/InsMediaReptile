using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new PhantomJSDriver(PhantomJSDriverService.CreateDefaultService());
            var result= ((IJavaScriptExecutor)driver).ExecuteScript("var page = require(\"webpage\").create();page.open(\"http://zc.97down.info\",function(status){if(status != 'success'){console.log(window.location.href);}phantom.exit();})");
            Thread.Sleep(2000);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
