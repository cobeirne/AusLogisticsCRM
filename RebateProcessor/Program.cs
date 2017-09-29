using AusLogisticsLibrary.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AusLogisticsLibrary.Models;
using System.Xml;
using RebateProcessor.Controllers;

namespace RebateProcessor
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    class Program
    {
        static AutoResetEvent _AutoRestEvent = new AutoResetEvent(false);

        public static MqGatewayController Gateway { get; set; }

        public static Mutex DbAccessMutex = new Mutex();

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                _AutoRestEvent.Set();
            };

            // Initialise the persistent ActiveMq gateway connection
            Gateway = new MqGatewayController("Rebate",
                Properties.Settings.Default.BrokerUrl,
                Properties.Settings.Default.ProducerQueue,
                Properties.Settings.Default.SubscriberQueue
                );

            // Set gateway callback delegate
            Gateway.OnMessageReceived += Subscribe_OnRequestReceived;

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Aus Logistics - Rebate Processor");
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
            Console.WriteLine("Press CTRL+C to exit the program.");
            _AutoRestEvent.WaitOne();
            Console.WriteLine("Progam stopped by user.");

            Gateway.Dispose();
        }

        static void Subscribe_OnRequestReceived(string message)
        {
            // Spawn new processor thread for each message received
            RebateRequestController rebateController = new RebateRequestController();
            Thread rebateThread = new Thread(new ParameterizedThreadStart(rebateController.ProcessRebate));
            rebateThread.Start(message);
        }
    }
}
