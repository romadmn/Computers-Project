using ComputersApp.CosmoDb.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Services
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly IConfiguration _configuration;
        private readonly ICosmosDbService<Computer, string> _cosmoDbService;
        private readonly SubscriptionClient _subscriptionClient;
        private const string TOPIC_PATH = "computerstopic";
        private const string SUBSCRIPTION_NAME = "computerSubscription";
        private readonly ILogger _logger;

        public ServiceBusTopicSubscription(ICosmosDbService<Computer, string> cosmoDbService,
        IConfiguration configuration,
            ILogger<ServiceBusTopicSubscription> logger)
        {
            _cosmoDbService = cosmoDbService;
            _configuration = configuration;
            _logger = logger;

            _subscriptionClient = new SubscriptionClient(
                _configuration["AzureServiceBus:ConnectionString"],
                TOPIC_PATH,
                SUBSCRIPTION_NAME);
        }

        private async Task RemoveDefaultFilters()
        {
            try
            {
                var rules = await _subscriptionClient.GetRulesAsync();
                foreach (var rule in rules)
                {
                    if (rule.Name == RuleDescription.DefaultRuleName)
                    {
                        await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private async Task AddFilters()
        {
            try
            {
                var rules = await _subscriptionClient.GetRulesAsync();
                if (!rules.Any(r => r.Name == "Filett"))
                {
                    var filter = new SqlFilter("item = 'ComputerDto'");
                    await _subscriptionClient.AddRuleAsync("Filett", filter);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        public async Task HandleMessagesAsync()
        {
            await RemoveDefaultFilters();
            await AddFilters();

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var newComputer = JsonConvert.DeserializeObject<Computer>(Encoding.UTF8.GetString(message.Body));
            await _cosmoDbService.CreateAsync(newComputer);
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public async Task CloseSubscriptionClientAsync()
        {
            await _subscriptionClient.CloseAsync();
        }
    }
}
