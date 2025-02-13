using _2ID_C06_Verification.models;
using _2ID_C06_Verification.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace _2ID_C06_Verification
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            string url = "https://uat-apim.2id.vn/gateway/2id-check/v.1";
            string merchantKey = "161a56f0-b057-4b51-bf03-5f50c4e85122";
            string secretKey = "80100a554c360333ca428ef0d020c01a";
            string apiKey = "eyJ4NXQjUzI1NiI6Ik5XUXdPVFJrTWpBNU9XRmpObVUyTnpCbE5UTTNaRFV3T0RVellqWXdabUpsWlROa1pEQTRPRFU0WlRVd1pHSXdObVV5TW1abVpUTmhaRGt5TmpRMlpBPT0iLCJraWQiOiJnYXRld2F5X2NlcnRpZmljYXRlX2FsaWFzIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ==.eyJzdWIiOiJhZG1pbkBjYXJib24uc3VwZXIiLCJhcHBsaWNhdGlvbiI6eyJpZCI6MywidXVpZCI6IjM2ODczODNhLTNmZTAtNGY5MC1hZGNmLTk3MzgzYTY3OTkxOCJ9LCJpc3MiOiJodHRwczpcL1wvdWF0LWFwaW0uMmlkLnZuOjQ0M1wvb2F1dGgyXC90b2tlbiIsImtleXR5cGUiOiJTQU5EQk9YIiwicGVybWl0dGVkUmVmZXJlciI6IiIsInRva2VuX3R5cGUiOiJhcGlLZXkiLCJwZXJtaXR0ZWRJUCI6IiIsImlhdCI6MTczOTQxMzUyNCwianRpIjoiMWM3N2QzZDEtNDgzNC00OTdkLTk2ZTgtNjVjMDhlMWNkNjMxIn0=.ac4_D1B_KxiO8WDkMqPi3kEqSIJ7ZJn4PojBCfrme-sTPiO2G5nN31KvDFBEGpvaCjG3h0fv7z40XSujMBkXIaDGXaSKEpkH_UjosRlwO8aNiDx_3yagqyxCfU4ZQNkFS4hm7KRdeWrIRl25y2j9cBGALu6hx6ykcQOiHmHsX3YIUo1L923rHDTO0aOjJ9QJamvePAS00EPww-OOJ4nkt0dgFERS5dnz6Kl2Z3B2QJ_rve0m_vli8mBXepHXz4d_nAwXmjlB6c4VQmUyEzp82JwwjM7luzO49uZrRjfno2Ux_wXCr1cobZw4gqQ5B2rzgp9Y8PPu7VfOZVOeMuczNA==";

            // build request data
            string trasnsactionId = Guid.NewGuid().ToString();
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string method = VerificationMethod.C06.ToString(); // VerificationMethod.INTEGRITY.ToString()

            // read json from file
            string pathFile = "verifyRequest.json";
            string requestJson = GetRequestContent(pathFile);
            if (string.IsNullOrEmpty(requestJson))
            {
                Console.WriteLine("Get request content failed");
                Console.ReadLine();
            }

            VerificationRequest verificationRequest = JsonSerializer.Deserialize<VerificationRequest>(requestJson);

            verificationRequest.method = method;

            string hashData = BuildHashRequest(merchantKey, secretKey, trasnsactionId, timestamp, verificationRequest.dg1DataB64);
            

            // call verify api
            BaseReponse<VerificationResponse> response = await VerifyAsync(url, apiKey, merchantKey, trasnsactionId, timestamp, hashData, verificationRequest);
            if (response == null && !response.status)
            {
                Console.WriteLine("Verify card failed");
            }
            VerificationResponse verificationResponse = response.data;
            Console.WriteLine("Verify C06 CCCD result: " + verificationResponse.result);
            Console.WriteLine("Verify C06 CCCD signature: " + verificationResponse.signature);
            Console.WriteLine("Verify C06 CCCD message: " + verificationResponse.message);
            Console.WriteLine("Verify C06 CCCD timestamp: " + verificationResponse.time);
            Console.WriteLine("Verify C06 CCCD responseId: " + verificationResponse.responseId);
            Console.WriteLine("Verify C06 CCCD exitCode: " + verificationResponse.exitCode);
            Console.ReadLine();

        }

        static async Task<BaseReponse<VerificationResponse>> VerifyAsync(
            string url,
            string apiKey,
            string merchantKey,
            string transactionId,
            long timestamp,
            string hash,
            VerificationRequest verificationRequest)
        {
            try
            {
                // Thiết lập các tham số truy vấn
                var queryParams = new Dictionary<string, string>
                    {
                        {"merchantKey", merchantKey},
                        {"transactionId", transactionId},
                        {"timestamp", timestamp.ToString()},
                        {"hash", hash}
                    };
                string query = HttpUtils.ConvertDictionaryToQueryString(queryParams);
                string apiUrl = url + "/c06-verify/integration/verify-card";
                if (!string.IsNullOrEmpty(query))
                    apiUrl = apiUrl + "?" + query;

                HttpClient client = new HttpClient();
                string requestJson = JsonSerializer.Serialize(verificationRequest);

                // Tạo request
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(
                        requestJson,
                        System.Text.Encoding.UTF8,
                        "application/json"
                    )
                };
                request.Headers.Add( "ApiKey", apiKey);

                // Gửi request
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Xử lý phản hồi
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BaseReponse<VerificationResponse>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during verification: {ex.Message}", ex);
            }
        }


        static string BuildHashRequest(string merchantKey, string secretKey, string transactionId, long timestamp, string message)
        {
            try
            {
                string verifyHashPlainText = HashUtils.BuildHashValue(merchantKey, transactionId, timestamp, message);
                string hashData = HashUtils.HashSHA512(secretKey, verifyHashPlainText);
                return HashUtils.FormatHex(hashData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static string GetRequestContent(string fileName)
        {
            try
            {
                string absoluteFilePath = Environment.CurrentDirectory + "//" + fileName;
                string content = File.ReadAllText(absoluteFilePath);
                return content;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
