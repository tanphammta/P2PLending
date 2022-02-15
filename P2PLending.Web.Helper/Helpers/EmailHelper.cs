using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace P2PLending.Web.Helper.Helpers
{
    public static class EmailHelper
    {
        public static string GetEmailTemplate(string template, List<string> bodyParams)
        {
            var fastReplacer = new FastReplacer("[$", "$]", true);
            var assembly = Assembly.GetExecutingAssembly();
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            var resourceName = string.Format("P2PLending.Web.Helper.EmailTemplates.{0}.html", template);

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return "";

                using (var reader = new StreamReader(stream))
                {
                    fastReplacer.Append(reader.ReadToEnd());
                }
            }

            for (var i = 0; i < bodyParams.Count(); i++)
            {
                fastReplacer.Replace(string.Format("[${0}$]", i), bodyParams[i]);
            }

            var result = fastReplacer.ToString();
            return result;
        }
    }
}
