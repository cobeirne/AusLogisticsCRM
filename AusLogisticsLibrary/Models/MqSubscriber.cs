using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsLibrary.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class MqSubscriber : IDisposable
    {
        private readonly ISession _Session;
        private readonly ITopic _Topic;
        private readonly string _TopicName;
        private bool _Disposed = false;

        public delegate void MessageReceivedDelegate(string message);
        public event MessageReceivedDelegate OnMessageReceived;

        public IMessageConsumer Consumer { get; private set; }
                    
        public string ConsumerId { get; private set; }

        public MqSubscriber(ISession session, string topicName)
        {
            this._Session = session;
            this._TopicName = topicName;
            _Topic = new ActiveMQTopic(this._TopicName);
        }

        public void Start(string consumerId)
        {
            ConsumerId = consumerId;
            Consumer = this._Session.CreateDurableConsumer(this._Topic, consumerId, null, false);
            Consumer.Listener += (message =>
            {
                var textMessage = message as ITextMessage;
                if (textMessage == null) throw new InvalidCastException();
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(textMessage.Text);
                }
            });
        }

        public void Dispose()
        {
            if (_Disposed) return;
            if (Consumer != null)
            {
                Consumer.Close();
                Consumer.Dispose();
            }
            _Disposed = true;
        }
    }
}