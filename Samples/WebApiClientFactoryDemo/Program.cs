using System;

namespace WebApiClientFactoryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var testCase = new TestClientFactory();
            testCase.TestHttpClientFactoryCreateInstance();

            Console.ReadLine();

        }
    }
}
