using Sitecore.Data.Items;
using System.Security.Cryptography;
using System.Text;

namespace Sitecore.SharedSource.XdbTracker.DataLayer.Variables
{
    public class ContactId : IDataLayerVariableProvider
    {
        public string Process(Item variableConfig)
        {
            if (Sitecore.Analytics.Tracker.Current.IsActive)
            {
                return ComputeSha256Hash(Analytics.Tracker.Current.Contact.ContactId.ToString());
            }

            return string.Empty;
        }

        static string ComputeSha256Hash(string input)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}