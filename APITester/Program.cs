using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient();
            Console.WriteLine("Welcome to the api tester. Press 1 to test with auth header, 2 without.");
            string response = Console.ReadLine();
            if (response == "1")
            {
                client.Headers.Add("x-auth-header", "secretmessage");
            }
            try
            {
                string json = client.DownloadString("http://localhost:49173/api/values");

                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Got unauthorized exception!");
            }
            Console.ReadKey(true);
        }
    }
}
