
using CookieCloudClientDotnet.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace CookieCloudClientDotnet
{
    public static class CookieCloudClient
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public enum OutputRestrict
        {
            All = 0,
            CookieOnly = 1,
            LocalStorageOnly = 2
        }
        public static async Task<string?> Get(
            string url,
            string uuid, 
            string password,
            OutputRestrict restrict = OutputRestrict.All,
            string? userAgent = null)
        {
            httpClient.BaseAddress = new Uri(url);
            if (userAgent != null)
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            }
            string path = $"/get/{uuid}";
            var key = string.Concat(MD5.HashData(Encoding.UTF8.GetBytes($"{uuid}-{password}")).Select(p => p.ToString("x2")))[..16];
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                // 判断body是否是json
                if (responseBody.StartsWith("{") && responseBody.EndsWith("}"))
                {
                    var encData = JsonSerializer.Deserialize(responseBody, EncryptedDataContext.Default.EncryptedData)!.Encrypted!;
                    var cookies = Infrastructure.Security.AES.Decrypt(encData, key);
                    var recStruct = JsonSerializer.Deserialize<CookieData>(cookies, CookieDataContext.Default.CookieData)!;
                    switch (restrict)
                    {
                        case OutputRestrict.CookieOnly:
                            return recStruct.Cookie ?? "";
                        case OutputRestrict.LocalStorageOnly:
                            return recStruct.LocalStorage ?? "";
                        case OutputRestrict.All:
                            return cookies;
                        default:
                            return null;
                    }
                } else
                {
                    throw new HttpRequestException("Response body is not valid JSON.");
                }
            } else
            {
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
