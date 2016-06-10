namespace Fooidity.AzureIntegration
{
    using System;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;


    public class ConfigurationCloudStorageAccountProvider :
        ICloudStorageAccountProvider
    {
        readonly string _connectionName;
        readonly Lazy<CloudStorageAccount> _value;

        public ConfigurationCloudStorageAccountProvider(string connectionName)
        {
            _connectionName = connectionName;
            _value = new Lazy<CloudStorageAccount>(GetConfiguredStorageAccount);
        }

        public CloudStorageAccount GetStorageAccount()
        {
            return _value.Value;
        }

        CloudStorageAccount GetConfiguredStorageAccount()
        {
            string connectionString =
                "DefaultEndpointsProtocol=https;AccountName=te0mccadpatsettings;AccountKey=OOeXix2bajLl6w1V+zedoQWHe2j35obIBTVHoA649eHJBQ+RW9r4EmmN0YiVr0I7iGFn2AgsE1B2uygYI/8Xbg==;";// CloudConfigurationManager.GetSetting(_connectionName);

            return CloudStorageAccount.Parse(connectionString);
        }
    }
}