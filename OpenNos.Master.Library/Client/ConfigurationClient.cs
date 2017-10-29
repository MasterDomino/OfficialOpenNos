using OpenNos.Master.Library.Data;
using OpenNos.Master.Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Master.Library.Client
{
    internal  class ConfigurationClient : IConfigurationClient
    {
        public void ConfigurationUpdated(ConfigurationObject configurationObject) => Task.Run(() => ConfigurationServiceClient.Instance.OnConfigurationUpdated(configurationObject));

    }
}
