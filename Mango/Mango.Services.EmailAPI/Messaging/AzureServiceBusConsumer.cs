using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.Dtos;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string userRegistrationQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _userRegistrationProcessor;
        private ServiceBusProcessor _emailOrderPlacedProcessor;


        private readonly string orderCreated_topic;
        private readonly string orderCreated_Email_Subscription;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            userRegistrationQueue = _configuration.GetValue<string>("TopicAndQueueNames:IdentityRegistrationQueue");

            orderCreated_topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Emails_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailService = emailService;

            //processor to listen to topic or queue
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _userRegistrationProcessor = client.CreateProcessor(userRegistrationQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_topic, orderCreated_Email_Subscription);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _userRegistrationProcessor.ProcessMessageAsync += OnUserRegistrationRequestReceived;
            _userRegistrationProcessor.ProcessErrorAsync += ErrorHandler;
            await _userRegistrationProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where message is received
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardMessage objMsg = JsonConvert.DeserializeObject<RewardMessage>(body);

            try
            {
                //try to log email
                await _emailService.LogOrderPlaced(objMsg);
                //tell azure service bus that message processing is completed
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserRegistrationRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var email = JsonConvert.DeserializeObject<string>(body);

            try
            {
                await _emailService.LogAndEmail($"User with email {email} registered!", "GodAdmin@HarshGupta.com");
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StartProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _userRegistrationProcessor.StartProcessingAsync();
            await _userRegistrationProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StartProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where message is received
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMsg = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                //try to log email
                await _emailService.EmailCartAndLog(objMsg);
                //tell azure service bus that message processing is completed
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //Can send an email to user for now just console.log
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
