using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace SQSProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int numberofmessages = 0;
            string messageString;
            while (numberofmessages != -1)
            {
                Console.Write("What is your Message: ");
                messageString = Console.ReadLine();

                Console.Write("How many messages should be sent? : ");
                if (Int32.TryParse(Console.ReadLine(), out numberofmessages) && numberofmessages > 0 && numberofmessages < 1000)
                {
                    for (int i = 0; i < numberofmessages; i++)
                    {
                        var message = messageString + DateTime.Now.ToString();
                        SQSMessageProducer sqsMessageProducer = new SQSMessageProducer();
                        await sqsMessageProducer.Send(message);

                        Console.WriteLine(i + 1 + ". Message Sent: " + message);
                    }
                }

                else
                    Console.WriteLine("Don't Push Me Please!. Write A Sensible Value");
            }

            Console.ReadLine();
        }
    }
}
