using Rebus.Activation;
using Rebus.Config;
using Rebus.NLog.Config;

namespace Abpro.MessageBus.ErrorQueue
{
    public class RebusErrorQueueBootstrapper : IRebusErrorQueueBootstrapper
    {
        private readonly IRebusErrorQueueConfig _config;

        static readonly BuiltinHandlerActivator ContainerAdapter = new BuiltinHandlerActivator();


        public RebusErrorQueueBootstrapper(IRebusErrorQueueConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (_config.ErrorMessageHandler != null) ContainerAdapter.Register(_config.ErrorMessageHandler);

            var configurer = Configure.With(ContainerAdapter);

            if (_config.LoggingConfigurer != null) configurer.Logging(c => c.NLog());

            if (_config.MqConnectionString != null && _config.QueueName != null)
            {
                configurer.Transport(c => c.UseRabbitMq(_config.MqConnectionString, _config.QueueName));
            }

            configurer.Options(c =>
            {
                c.SetMaxParallelism(_config.MaxParallelism);
                c.SetNumberOfWorkers(_config.NumberOfWorkers);
            });

            if (_config.SerializerConfigurer == null)
            {
                configurer.Serialization(c => c.Register(d => new ErrorMessageSerializer()));
            }

            configurer.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            ContainerAdapter.Bus.Dispose();
            ContainerAdapter.Dispose();
        }
    }
}