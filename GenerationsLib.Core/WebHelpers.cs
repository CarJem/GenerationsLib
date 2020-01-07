using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationsLib.Core
{
    public static class WebHelpers
    {
        public static bool isURLValid(string value)
        {
            Uri uriResult;
            return Uri.TryCreate(value, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
