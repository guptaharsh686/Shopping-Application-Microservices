using Azure.Messaging.ServiceBus;
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

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            userRegistrationQueue = _configuration.GetValue<string>("TopicAndQueueNames:IdentityRegistrationQueue");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailService = emailService;

            //processor to listen to topic or queue
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _userRegistrationProcessor = client.CreateProcessor(userRegistrationQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _userRegistrationProcessor.ProcessMessageAsync += OnUserRegistrationRequestReceived;
            _userRegistrationProcessor.ProcessErrorAsync += ErrorHandler;
            await _userRegistrationProcessor.StartProcessingAsync();

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
                _emailService.EmailCartAndLog(objMsg);
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
