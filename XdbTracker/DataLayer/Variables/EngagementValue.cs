using Sitecore.Analytics;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.XdbTracker.DataLayer.Variables
{
    /// <summary>
    /// Returns the name of the matching pattern for the profile specified.
    /// </summary>
    public class EngagementValue : IDataLayerVariableProvider
    {
        public string Process(Item variableConfig)
        {
            if (Tracker.Current.IsActive)
            {
                return Tracker.Current.Interaction.Value.ToString();
            }

            return string.Empty;
        }
    }
}