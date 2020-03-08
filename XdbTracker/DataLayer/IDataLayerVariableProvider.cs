using Sitecore.Data.Items;

namespace Sitecore.SharedSource.XdbTracker.DataLayer
{
    public interface IDataLayerVariableProvider
    {
        string Process(Item variableConfig);
    }
}