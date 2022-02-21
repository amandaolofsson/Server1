using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server1
{
    class Program
    {
        static TcpListener tcpListener;
        static MessagesManager messages = new MessagesManager();
        
        static void Main(string[] args)
        {
            IPAddress iP = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(iP, 800);
            tcpListener.Start();
            while (true)
            {
                try
                {
                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Connected to: {0}", socket.RemoteEndPoint);
                    Session session = new Session(socket, messages);
                    session.Start();
                }
                catch
                {

                }
            }
        }
    }
}
