using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server1
{
    class MessagesManager
    {
        List<Message> messages = new List<Message>(); 

        public void Add(Message m)
        {
            m.MessageNumber = messages.Count + 1;
            messages.Add(m);
        }

        public ICollection<Message> GetAll()
        {
            return messages;
        }
        
        /*
        <messages>
          <message id="0001" from="Kalle Anka">
           message text goes here
          </message>
          ...

        </messages>
             
             
        */
        public void SaveToXML()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement messagesElement = xmlDoc.CreateElement("","messages","");

            foreach(Message m in messages)
            {
                XmlElement message = xmlDoc.CreateElement("", "message", "");

                XmlAttribute msgId = xmlDoc.CreateAttribute("", "id", "");
                msgId.Value = m.MessageNumber.ToString();
                message.Attributes.Append(msgId);

                XmlAttribute msgFrom = xmlDoc.CreateAttribute("", "from", "");
                msgFrom.Value = m.Sender;
                message.Attributes.Append(msgFrom);

                XmlText msgtext = xmlDoc.CreateTextNode(m.Text);
                message.AppendChild(msgtext);
                
                messagesElement.AppendChild(message);
            }

            xmlDoc.AppendChild(messagesElement);
            xmlDoc.Save("messages.xml");
        }

        public void GetFromXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load("messages.xml");
                messages.Clear();
                XmlNodeList messageList = xmlDoc.SelectNodes("messages/message");

                foreach (XmlNode m in messageList)
                {
                    int msgId = int.Parse(m.Attributes["id"].Value);
                    string sender = m.Attributes["from"].Value;
                    string text = m.InnerText;

                    messages.Add(new Message(sender, text, msgId));
                }
            }
            catch
            {

            }
        }
    }
}
