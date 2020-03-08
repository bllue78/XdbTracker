using Newtonsoft.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.SharedSource.XdbTracker.DataLayer
{
    public static class HtmlHelperExtension
    {
        public static HtmlString DataLayer(this HtmlHelper helper)
        {
            var sb = new StringBuilder();

            var variables = new DataLayerRepository().GetVariables();

            if (variables.Count == 0)
            {
                return new HtmlString(string.Empty);
            }

            sb.Append("<script>");
            sb.Append("window.dataLayer = window.dataLayer || [];");
            sb.Append("window.dataLayer.push(");
            sb.Append(JsonConvert.SerializeObject(variables));
            sb.Append(");");
            sb.Append("</script>");

            return new HtmlString(sb.ToString());                 
        }
    }
}