using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSConsumer
{
    public class SQSMessageConsumer
    {
        private AmazonSQSClient _sqsClient;
        private bool _isPolling;
        private string _queueUrl;
        private string _awsregion;
        private int _maxNumberOfMessages;
        private int _messageWaitTimeSeconds;
        private string _accessKey;
        private string _secret;
        private CancellationTokenSource _source;
        private CancellationToken _token;

        public SQSMessageConsumer()
        {
            _accessKey = "AKIA32DNJ6MUX7GUCW52";
            _secret = "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";
            _queueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue";
            _awsregion = "eu-west-2";
            _messageWaitTimeSeconds = 5;
            _maxNumberOfMessages = 1;


            BasicAWSCredentials basicCredentials = new BasicAWSCredentials(_accessKey, _secret);
            RegionEndpoint region = RegionEndpoint.GetBySystemName(_awsregion);

            _sqsClient = new AmazonSQSClient(basicCredentials, region);
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);

        }

        public async Task Listen()
        {
            _isPolling = true;

            int i = 0;
            try
            {
                _source = new CancellationTokenSource();
                _token = _source.Token;

                while (_isPolling)
                {
                    i++;
                    Console.Write(i + ": ");
                    await FetchFromQueue();

                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Application Terminated: " + ex.Message);
            }
            finally
            {
                _source.Dispose();
            }
        }

        private async Task FetchFromQueue()
        {

            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = _queueUrl;
            receiveMessageRequest.MaxNumberOfMessages = _maxNumberOfMessages;
            receiveMessageRequest.WaitTimeSeconds = _messageWaitTimeSeconds;
            ReceiveMessageResponse receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, _token);

            if (receiveMessageResponse.Messages.Count != 0)
            {
                for (int i = 0; i < receiveMessageResponse.Messages.Count; i++)
                {
                    string messageBody = receiveMessageResponse.Messages[i].Body;

                    Console.WriteLine("Message Received: " + messageBody);

                    await DeleteMessageAsync(receiveMessageResponse.Messages[i].ReceiptHandle);
                }
            }
            else
            {
                Console.WriteLine("No Messages to process");
            }
        }

        private async Task DeleteMessageAsync(string recieptHandle)
        {

            DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
            deleteMessageRequest.QueueUrl = _queueUrl;
            deleteMessageRequest.ReceiptHandle = recieptHandle;

            DeleteMessageResponse response = await _sqsClient.DeleteMessageAsync(deleteMessageRequest);

        }

        protected void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            _source.Cancel();
            _isPolling = false;
        }

    }
}