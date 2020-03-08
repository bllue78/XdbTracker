using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System;

namespace Sitecore.SharedSource.XdbTracker.DataLayer
{
    public class DataLayerRepository
    {
        private const string RootConfigId = "{81B824C0-2B79-4BC3-9123-BF706ED4A71F}";

        protected Item GetConfig()
        {
            Item config = null;
            
            if (ID.TryParse(Context.Site.Properties.Get("datalayer-config"), out var id))
            {
                config = Context.Database.GetItem(id);
            } 

            if (config == null)
            {
                config = Context.Database.GetItem(ID.Parse(RootConfigId));
            }

            return config;
        }

        public Dictionary<string, string> GetVariables()
        {
            var config = GetConfig();

            Assert.IsNotNull(config, $"Data Layer: No config item found. Make sure an item with ID '{RootConfigId}' exists.");

            var result = new Dictionary<string, string>();

            MultilistField variablesField = config.Fields["Variables"];

            Assert.IsNotNull(variablesField, $"Data Layer: No 'Variables' field configured on Item '{RootConfigId}'.");

            foreach (Item variableConfig in variablesField.GetItems())
            {
                if (string.IsNullOrEmpty(variableConfig["Variable Type"]))
                {
                    Log.SingleWarn($"Data Layer: No Variable Type defined in Data Layer Variable with ID '{variableConfig.ID}'",this);
                    continue;
                }

                string value = string.Empty;

                try
                {
                    IDataLayerVariableProvider dataProvider = (IDataLayerVariableProvider)InstantiateClass(variableConfig["Variable Type"]);
                    value = dataProvider.Process(variableConfig);
                } 
                catch(Exception ex)
                {
                    Log.SingleError($"Data Layer: Unable to load type '{variableConfig["Variable Type"]}' defined in '{variableConfig.ID}': {ex}", this);
                }

                string variableName = !string.IsNullOrEmpty(variableConfig["Variable Name"]) ? variableConfig["Variable Name"] : variableConfig.Name;

                if (result.ContainsKey(variableName))
                {
                    Log.SingleWarn($"Data Layer: Variable name '{variableName}' defined more than once. Using ID instead.", this);
                    variableName = variableConfig.ID.ToShortID().ToString();
                }

                result.Add(variableName, value);
            }

            return result;
        }

        private static object InstantiateClass(string classDefinition)
        {
            var dataProviderClassInfos = classDefinition.Split(',');
            var typeName = dataProviderClassInfos[0].Trim();
            var assemblyName = dataProviderClassInfos[1].Trim();
            var handle = Activator.CreateInstance(assemblyName, typeName);
            return handle.Unwrap();
        }
    }    
}