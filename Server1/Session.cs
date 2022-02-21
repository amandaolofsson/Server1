using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server1
{
    class Session
    {
        protected Socket socket;
        protected string username;
        protected MessagesManager messages;

        public Session(Socket socket, MessagesManager messages)
        {
            this.socket = socket;
            this.messages = messages;
        }

        public void Start()
        {
            Send(@"#########################
##                     ##
##  WELCOME TO SERVER  ##
##                     ##
#########################

**all systems operational**

What is your name? ");
            username = Receive();
            try
            {
                while (true)
                {
                    Send(Meny());
                    string answer = Receive();

                    switch (answer.ToLower())
                    {
                        case "w":
                            Send("Enter your message: ", ServerMessageEnum.ResponseEncrypted);
                            string messageText = Receive();
                            messages.Add(new Message(username, messageText));
                            break;
                        case "v":
                            foreach (Message m in messages.GetAll())
                            {
                                Send(m.ToString(), ServerMessageEnum.Message);
                            }
                            break;
                        case "l":
                            messages.GetFromXML();
                            Send("Messages loaded succesfully.", ServerMessageEnum.Text);
                            break;
                        case "s":
                            messages.SaveToXML();
                            Send("Messages saved succesfully.", ServerMessageEnum.Text);
                            break;
                        case "d":
                            Send("$$DISCONNECT");
                            throw new DisconnectException();
                    }
                }
            }
            catch(DisconnectException)
            {
                socket.Close();
            }
        }

        public string Meny()
        {
            return String.Format(@"
Hello {0}, what would you like to do?
[W] Write new message.
[V] View all messages.
[L] Load all messages from file.
[S] Save all messages to file.
[D] Disconnect.", username);
        }

        public void Send(string message, ServerMessageEnum type = ServerMessageEnum.Response)
        {
            string responseType = "R";
            switch (type)
            {
                case ServerMessageEnum.Response:
                    responseType = "R";
                    break;
                case ServerMessageEnum.Text:
                    responseType = "T";
                    break;
                case ServerMessageEnum.ResponseEncrypted:
                    responseType = "E";
                    break;
                case ServerMessageEnum.Message:
                    responseType = "M";
                    break;
            }
            message = responseType + message + "¤";

            Byte[] bSend = System.Text.Encoding.UTF8.GetBytes(message);
            socket.Send(bSend);
        }

        public string Receive()
        {
            byte[] bRead = new byte[256];
            int bReadSize = socket.Receive(bRead);

            return System.Text.Encoding.UTF8.GetString(bRead, 0, bReadSize);
        }
    }
}
