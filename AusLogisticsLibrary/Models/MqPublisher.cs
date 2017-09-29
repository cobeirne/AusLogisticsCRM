using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace AusLogisticsLibrary.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class MqPublisher : IDisposable
    {
        private readonly ISession _Session;
        private readonly ITopic _Topic;
        private bool _Disposed;

        public IMessageProducer Producer { get; private set; }

        public string TopicName { get; private set; }

        public MqPublisher(ISession session, string topicName)
        {
            this._Session = session;
            this.TopicName = topicName;
            _Topic = new ActiveMQTopic(this.TopicName);
            Producer = session.CreateProducer(_Topic);
        }
        
        public void SendMessage(string message)
        {
            if (_Disposed) throw new ObjectDisposedException(GetType().Name);
            var textMessage = Producer.CreateTextMessage(message);
            Producer.Send(textMessage);
        }

        public void Dispose()
        {
            if (_Disposed) return;
            Producer.Close();
            Producer.Dispose();
            _Disposed = true;
        }
    }
}