using System;
using AutoLoginNetwork;


namespace YourNamespace
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var job = new CronJob("0 1 * * *");
            string? username;
            string? password;
            do
            {
                Console.WriteLine("--Enter profile--");
                Console.Write("username: ");
                username = Console.ReadLine();
                Console.Write("password: ");
                password = Console.ReadLine();
            } while (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password));

            await job.StartAsync(username, password);
        }
    }
}
