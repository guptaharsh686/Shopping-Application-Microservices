using Mango.Services.EmailAPI.Messaging;

namespace Mango.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtension
    {
        private static IAzureServiceBusConsumer serviceBusConsumer;

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            //Add singletom instance of bus consumer as we want to start and stop service on project start and stop
            serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            //lifetime object to notify when service started and stopped
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            serviceBusConsumer.Start();
        }

        private static void OnStart()
        {
            serviceBusConsumer.Stop();
        }


    }
}
