using Apache.NMS;
using Apache.NMS.ActiveMQ;
using AusLogisticsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace AusLogisticsLibrary.Controllers 
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class MqGatewayController : IDisposable
    {       
        private IConnection _Connection;
        private ConnectionFactory _ConnectionFactory;
        private ISession _Session;
        private MqSubscriber _Subscriber;

        public delegate void MessageReceivedDelegate(string message);
        public event MessageReceivedDelegate OnMessageReceived;
        
        public string BrokerUrl { get; set; }

        public string ClientId { get; set; }

        public string ProducerQueue { get; set; }

        public string SubscriberQueue { get; set; }

        public string SubscriberId { get; set; }

        public MqGatewayController(string appName, string brokerUrl, string produceQueue, string subscriberQueue)
        {
            this.BrokerUrl = brokerUrl;
            this.ProducerQueue = produceQueue;
            this.SubscriberQueue = subscriberQueue;
            this.ClientId = appName + "Client_" + Guid.NewGuid().ToString();
            this.SubscriberId = appName + "Subscriber_" + Guid.NewGuid().ToString();

            _CreateBrokerConnection();
            _StartSubscriber();
        }
        
        private void _CreateBrokerConnection()
        {
            _ConnectionFactory = new ConnectionFactory(this.BrokerUrl, this.ClientId);
            _Connection = _ConnectionFactory.CreateConnection();
            _Connection.Start();

            _Session = _Connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
        }

        private void _StartSubscriber()
        {
            // Initialise a Subscriber endpoint to listen for members on the ActiveMQ topic channel
            this._Subscriber = new MqSubscriber(this._Session, this.SubscriberQueue);
            this._Subscriber.Start(this.SubscriberId);

            this._Subscriber.OnMessageReceived += Subscribe_OnMessageReceived;
        }

        public void SendMessage(string messageText)
        {
            // Send the message to the ActiveMQ topic channel via the Producer endpoint
            using (var publisher = new MqPublisher(this._Session, this.ProducerQueue))
            {
                publisher.SendMessage(messageText);
            }
        }

        private void Subscribe_OnMessageReceived(string message)
        {
            OnMessageReceived(message);
        }

        public void Dispose()
        {
            // Disconnect from ActiveMQ whenever the object is disposed
            try
            {
                this._Subscriber.Dispose();
                this._Session.Close();
                this._Session.Dispose();
                this._Connection.Stop();
                this._Connection.Close();
                this._Connection.Dispose();
            }
            catch (Exception)
            { }
        }
    }
}