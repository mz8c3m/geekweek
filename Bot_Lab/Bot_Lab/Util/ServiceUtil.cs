using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace Bot_Lab.Util
{
    public class ServiceUtil
    {
        public String SendReq(String id, String[] args)
        {
            if(id.Equals("get_user_info"))
            {
                String url = "http://peaceful-fortress-82166.herokuapp.com/api/employee?user=" + args[0];

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentLength = 0;
                request.ContentType = "text/json";

                using (var resp = (HttpWebResponse)request.GetResponse())
                {
                    var respVal = "";

                    if(resp.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("BAD RESPONSE");
                    }

                    using (var respStream = resp.GetResponseStream())
                    {
                        if(respStream != null)
                        {
                            using (var value = new StreamReader(respStream))
                            {
                                respVal = value.ReadToEnd(); 
                            }
                        }
                    }

                    return respVal;
                }
            }
            else if(id.Equals("submit_new_hire"))
            {
                String url = "";

                return url;
            }

            return "";
        }
    }
}