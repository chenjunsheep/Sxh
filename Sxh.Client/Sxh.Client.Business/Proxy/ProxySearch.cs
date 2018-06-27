﻿using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Sxh.Shared.Response.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sxh.Client.Business.Proxy
{
    public class ProxySearch : ProxyBase
    {
        public async Task<PortionTransferList> SearchAsync(CookieCollection tokenOffical)
        {
            var cookieJar = new CookieContainer();
            using (var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
            })
            {
                using (var client = CreateHttpClient(handler))
                {
                    var formData = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("title", string.Empty),
                        new KeyValuePair<string, string>("currentPage", "1"),
                        new KeyValuePair<string, string>("maxRowsPerPage", "15"),
                        new KeyValuePair<string, string>("projectType", string.Empty),
                        new KeyValuePair<string, string>("remainingCount", string.Empty),
                        new KeyValuePair<string, string>("repayStrategy", string.Empty),
                        new KeyValuePair<string, string>("hasTransfering", "false"),
                        new KeyValuePair<string, string>("hasAcquiring", "false"),
                        new KeyValuePair<string, string>("noDealing", "false"),
                        new KeyValuePair<string, string>("orderBy", "minTransferingRate"),
                        new KeyValuePair<string, string>("orderType", "desc"),
                    });

                    var Uri = CreateUri("/portionTransfer/list");

                    foreach (Cookie token in tokenOffical)
                    {
                        cookieJar.Add(Uri, new Cookie(token.Name, token.Value));
                    }

                    var response = await client.PostAsync(Uri, formData);
                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var target = JsonConvert.DeserializeObject<PortionTransferList>(jsonString);
                    return target;
                }
            }
        }
    }
}