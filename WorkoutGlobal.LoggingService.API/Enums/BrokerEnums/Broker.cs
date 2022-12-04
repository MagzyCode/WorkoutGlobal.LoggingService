using System.ComponentModel;

namespace WorkoutGlobal.LoggingService.Api.Enums
{
    /// <summary>
    /// Represents using message brokers.
    /// </summary>
    public enum Broker
    {
        /// <summary>
        /// RabbitMQ message broker.
        /// </summary>
        [Description("Male sex.")]
        RabbitMQ = 1,
        /// <summary>
        /// Azure Service Bus message broker.
        /// </summary>
        [Description("Male sex.")]
        AzureServiceBus
    }
}
