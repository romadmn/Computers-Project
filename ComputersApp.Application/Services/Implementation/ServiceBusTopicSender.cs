using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class ServiceBusTopicSender : IServiceBusTopicSender
    {
        private readonly TopicClient _topicClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private const string TOPIC_PATH = "computerstopic";

        public ServiceBusTopicSender(IConfiguration configuration, IMapper mapper, ILogger<ServiceBusTopicSender> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _topicClient = new TopicClient(
                _configuration["AzureServiceBus:ConnectionString"],
                TOPIC_PATH
            );
        }

        public async Task SendMessage(ComputerDto computerDto)
        {
            var computer = _mapper.Map<Computer>(computerDto);
            string data = JsonConvert.SerializeObject(computer);
            Message message = new Message(Encoding.UTF8.GetBytes(data));
            message.UserProperties.Add("Item", typeof(ComputerDto).Name);

            try
            {
                await _topicClient.SendAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
