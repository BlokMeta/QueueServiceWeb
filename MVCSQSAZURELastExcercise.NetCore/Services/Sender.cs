using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using ProduceMessageServices.NetCore.Models;
using SenderQueueMessageServices.NetCore.Configuration;
using System.Text;
using System.Text.Json;
using Message = Microsoft.Azure.ServiceBus.Message;

namespace SenderQueueMessageServices.NetCore.Services
{
    public class Senders : ISenders
    {
        public async Task<bool> SendAsync(SendQueueModel message, bool IsAws = true, bool IsAzure = true)
        {

            return (IsAws == false || await AwsSend(message)) && (IsAzure == false || await AzureSend(message));
        }

        public async Task<bool> AwsSend(SendQueueModel message)
        {
            var sqsClient = new AmazonSQSClient(QueueSettings.awsAccessKey, QueueSettings.awsSecret, RegionEndpoint.EUWest2);
            var request = new SendMessageRequest
            {
                QueueUrl = QueueSettings.awsQueuUrl,
                MessageBody = message.Messages.ToString()
            };
            await sqsClient.SendMessageAsync(request);
            return true;
        }

        public async Task<bool> AzureSend(SendQueueModel message)
        {

            ServiceBusClient client = new ServiceBusClient(QueueSettings.azureConnectionString);
            ServiceBusSender sender = client.CreateSender(QueueSettings.azureQueueName);
            try
            {
                ServiceBusMessage messageAzure = new ServiceBusMessage(message.Messages);
                await sender.SendMessageAsync(messageAzure);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
            return true;


        }
    }
}
