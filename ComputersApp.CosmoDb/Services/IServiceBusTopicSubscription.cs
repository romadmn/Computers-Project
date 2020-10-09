using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Services
{
    public interface IServiceBusTopicSubscription
    {
        Task HandleMessagesAsync();
        Task CloseSubscriptionClientAsync();
    }
}
