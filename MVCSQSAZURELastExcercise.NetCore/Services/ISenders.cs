using ProduceMessageServices.NetCore.Models;

namespace SenderQueueMessageServices.NetCore.Services
{
    public interface ISenders
    {
        Task<bool> SendAsync(SendQueueModel message, bool IsAws = true, bool IsAzure = true);
    }
}