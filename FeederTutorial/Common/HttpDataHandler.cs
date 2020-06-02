using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Net;
using Org.Apache.Http.Protocol;

namespace FeederTutorial.Common
{
    public class HttpDataHandler
    {
        static string stream = null;
        public string GetHttpData(string urlString)
        {
            try
            {
                URL url = new URL(urlString);
                using (var urlConnection = (HttpURLConnection)url.OpenConnection())
                {
                    if (urlConnection.ResponseCode == HttpStatus.Ok)
                    {
                        BufferedReader reader = new BufferedReader(new InputStreamReader(urlConnection.InputStream));
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        string line;
                        while ((line = reader.ReadLine()) != null)
                            sb.Append(line);
                        stream = sb.ToString();
                        urlConnection.Disconnect();
                    }
                }
            }
            catch
            {
                throw new System.Exception("oh oh");
            }
            return stream;
        }
    }
}