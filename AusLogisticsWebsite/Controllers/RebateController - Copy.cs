using Apache.NMS;
using Apache.NMS.ActiveMQ;
using AusLogisticsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace AusLogisticsWebsite.Controllers
{
    public class RebateController
    {
        public TransportOrder Order { get; set; }
        
        private IConnection _Connection;
        private ConnectionFactory _ConnectionFactory;
        private ISession _Session;
        private MqSubscriber _Subscriber;
        
        public string BrokerUrl { get; set; }

        public string ClientId { get; set; }

        public string ProducerQueue { get; set; }

        public string SubscriberQueue { get; set; }

        public string ConsumerId { get; set; }

        private object _EventHandles;

        public RebateController()
        {
            this.BrokerUrl = Properties.Settings.Default.BrokerUrl;
            this.ProducerQueue = Properties.Settings.Default.ProducerQueue;
            this.SubscriberQueue = Properties.Settings.Default.SubscriberQueue;
            this.ClientId = Guid.NewGuid().ToString();
            this.ConsumerId = Guid.NewGuid().ToString();

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
            this._Subscriber = new MqSubscriber(this._Session, this.ProducerQueue);
            this._Subscriber.Start(this.ConsumerId);

            this._Subscriber.OnMessageReceived += Subscribe_OnMessageReceived;
        }

        public void ProcessRebate(object eventHandles)
        {
            _EventHandles = eventHandles;
            var autoEventHandles = (AutoResetEvent[])this._EventHandles;

            autoEventHandles[0].WaitOne();

            OrderMessage orderMessage = new OrderMessage(this.Order);

            // Send orderMessage to gateway
            using (var publisher = new MqPublisher(this._Session, this.ProducerQueue))
            {
                publisher.SendMessage(orderMessage.ToString());
            }
        }


        private void Subscribe_OnMessageReceived(string message)
        {
            Thread.Sleep(10000);
            
            // Check if correlation Id is correct
            // If so process message then dispose of connection.
            Debug.WriteLine(message);

            dispose();

            var autoEventHandles = (AutoResetEvent[])this._EventHandles;
            autoEventHandles[1].Set();
        }

        public void dispose()
        {
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