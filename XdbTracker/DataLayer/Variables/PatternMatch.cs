using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Sitecore.SharedSource.XdbTracker.DataLayer.Variables
{
    /// <summary>
    /// Returns the name of the matching pattern for the profile specified.
    /// </summary>
    public class PatternMatch : IDataLayerVariableProvider
    {
        public string Process(Item variableConfig)
        {
            if (Tracker.Current.IsActive)
            {
                var parameters = WebUtil.ParseQueryString(variableConfig["Variable Parameters"]);
                if (string.IsNullOrEmpty(parameters["profile"]))
                {
                    Log.SingleWarn($"Data Layer: Please provide a 'profile' parameter in config item '{variableConfig}'", this);
                }

                var interactionProfile = Tracker.Current.Interaction.Profiles[parameters["profile"]];
                if (interactionProfile != null)
                {
                    if (interactionProfile.PatternId.HasValue)
                    {
                        Item matchingPattern = Sitecore.Context.Database.GetItem(new ID(interactionProfile.PatternId.Value));
                        if (matchingPattern != null)
                        {
                            return matchingPattern.Name;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}