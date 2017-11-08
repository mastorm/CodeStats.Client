using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStats.Client
{
    public class ConfigurationRetriever
    {
        private readonly EnvDTE.DTE DTE = null;

        public ConfigurationRetriever()
        {
            DTE = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
        }

        private string getConfigValueByKey(string key)
        {
            var props = DTE.get_Properties(@"CodeStats", "General");
            var pathProperty = props.Item(key);
            return pathProperty.Value as string;
        }

        public string GetMachineKey() => getConfigValueByKey("MachineKey");
        public string GetPulseApiUrl() => getConfigValueByKey("PulseApiUrl");
    }
}
