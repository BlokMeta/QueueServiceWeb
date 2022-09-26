using System.ComponentModel.DataAnnotations;

namespace ProduceMessageServices.NetCore.Models
{
    public class SendQueueModel
    {
        [Required]
        public string Messages { get; set; }
    }
}
