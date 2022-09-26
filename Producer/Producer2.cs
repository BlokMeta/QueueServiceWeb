using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSProducer
{
    public class SQSMessageProducer
    {

        public SQSMessageProducer()
        {

        }

        public async Task Send(String message)
        {
            string accessKey = "AKIA32DNJ6MUX7GUCW52";
            string secret = "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";
            string queueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue";
            bool useFifo = false;
            string messageGroupId = "";
            string awsregion = "eu-west-2";

            BasicAWSCredentials creds = new BasicAWSCredentials(accessKey, secret);

            RegionEndpoint region = RegionEndpoint.GetBySystemName(awsregion);

            SendMessageRequest sendMessageRequest = new SendMessageRequest(queueUrl, message);

            if (useFifo)
            {
                sendMessageRequest.MessageGroupId = messageGroupId;
            }

            var sqsClient = new AmazonSQSClient(creds, region);

            await sqsClient.SendMessageAsync(sendMessageRequest);

        }

    }
}