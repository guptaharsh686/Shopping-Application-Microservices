using Azure.Messaging.ServiceBus;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Messaging;
using Mango.Services.RewardAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Reward .Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string OrderCreatedTopic;
        private readonly string OrderCreatedRewardSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;
        private ServiceBusProcessor _RewardProcessor;
        private ServiceBusProcessor _userRegistrationProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            OrderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            OrderCreatedRewardSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardService = rewardService;

            //processor to listen to topic or queue
            _RewardProcessor = client.CreateProcessor(OrderCreatedTopic,OrderCreatedRewardSubscription);
        }

        public async Task Start()
        {
            _RewardProcessor.ProcessMessageAsync += OnNewRewardsRequestReceived;
            _RewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _RewardProcessor.StartProcessingAsync();

        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //Can send an email to user for now just console.log
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnNewRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);

            try
            {
                await _rewardService.UpdateRewards(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Stop()
        {
            await _RewardProcessor.StartProcessingAsync();
            await _RewardProcessor.DisposeAsync();
        }

    }
}
