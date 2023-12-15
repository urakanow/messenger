using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace praktik_15._12._2023_messenger
{
    public class UserMessage
    {
        private string message;
        public string Message { get => message; set { message = value; } }
        private DateTime sendTime;
        public DateTime SendTime { get => sendTime; }
        private User sender;
        public User Sender { get => sender; }
        private HorizontalAlignment horizontalAlignment;
        public HorizontalAlignment HorizontalAlignment { get => horizontalAlignment; }
        public UserMessage(User sender, HorizontalAlignment horizontalAlignment)
        {
            this.sender = sender;
            sendTime = DateTime.Now;
            this.horizontalAlignment = horizontalAlignment;
        }
        public UserMessage(User sender, DateTime sendTime, HorizontalAlignment horizontalAlignment) : this(sender, horizontalAlignment)
        {
            this.sendTime = sendTime;
        }
    }
}
