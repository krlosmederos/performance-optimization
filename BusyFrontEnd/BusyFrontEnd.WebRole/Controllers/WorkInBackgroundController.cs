﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using System.Web.Http;
using BusyFrontEnd.ServiceBusQueueHandling;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;

namespace BusyFrontEnd.WebRole.Controllers
{
    public class WorkInBackgroundController : ApiController
    {
        private const string ServiceBusConnectionStringKey = "Microsoft.ServiceBus.ConnectionString";

        private const string ServiceBusQueueNameKey = "Microsoft.ServiceBus.QueueName";

        private static readonly QueueClient QueueClient;
        private static readonly string QueueName;
        private static readonly ServiceBusQueueHandler ServiceBusQueueHandler;

        static WorkInBackgroundController()
        {
            var serviceBusConnectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStringKey);
            QueueName = CloudConfigurationManager.GetSetting(ServiceBusQueueNameKey);
            ServiceBusQueueHandler = new ServiceBusQueueHandler(serviceBusConnectionString);
            QueueClient = ServiceBusQueueHandler.GetQueueClientAsync(QueueName).Result;
        }

        [HttpPost]
        [Route("api/workinbackground")]
        public Task<long> Post()
        {
            return ServiceBusQueueHandler.AddWorkLoadToQueueAsync(QueueClient, QueueName, 0);
        }
    }
}
