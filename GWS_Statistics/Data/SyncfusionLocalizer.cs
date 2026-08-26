using Syncfusion.Blazor;

namespace GWS_Statistics.Data
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        public string GetText(string key)
        {
            return ResourceManager!.GetString(key)!;
        }

        public System.Resources.ResourceManager? ResourceManager
        {
            get
            {
                // Replace the ApplicationNamespace with your application name.
                return GWS_Statistics.Resources.SfResources.ResourceManager;

            }
        }
    }
}
