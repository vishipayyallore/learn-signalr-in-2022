namespace NumbersNotification.Core.Common
{

    public static class Constants
    {

        public static class CosmosDB
        {
            public static string AccountEndpoint { get; } = "CosmosDbConnectionStrings:AccountEndpoint";

            public static string AccountKey { get; } = "CosmosDbConnectionStrings:AccountKey";

            public static string ApplicationName { get; } = "NumberGeneratorWorkerService";
        }

        public static class NotificationDataStore
        {
            public static string DatabaseId { get; } = "NumbersNotificationDbDetails:DatabaseId";

            public static string ContainerId { get; } = "NumbersNotificationDbDetails:ContainerId";

            public static string PartitionKey { get; } = "NumbersNotificationDbDetails:PartitionKey";
        }

    }

}
