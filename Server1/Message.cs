using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server1
{
    class Message
    {
        string sender;
        int messageNumber;
        string text;

        public Message(string sender, string text, int messageNumber = 0)
        {
            this.sender = sender;
            this.text = text;
            this.messageNumber = messageNumber;
        }

        public string Sender
        {
            get
            {
                return sender;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public int MessageNumber
        {
            get
            {
                return messageNumber;
            }
            set
            {
                messageNumber = value;
            }
        }

        public override string ToString()
        {
            return String.Format("#{0:D4}|{1}|{2}\n", messageNumber, sender, text);
        }
    }
}
