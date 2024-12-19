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
            string url = "https://uat-api-c06verify.2id.vn";
            string apiKey = "ffb5c0c8-496b-42bd-a73e-02399f01b45d";
            string secretKey = "9aa94d6173cce569911116e3e5de6756";

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

            string hashData = BuildHashRequest(apiKey, secretKey, trasnsactionId, timestamp, verificationRequest.dg1DataB64);

            // call verify api
            BaseReponse<VerificationResponse> response = await VerifyAsync(url, apiKey, trasnsactionId, timestamp, hashData, verificationRequest);
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
                        {"apiKey", apiKey},
                        {"transactionId", transactionId},
                        {"timestamp", timestamp.ToString()},
                        {"hash", hash}
                    };
                string query = HttpUtils.ConvertDictionaryToQueryString(queryParams);
                string apiUrl = url + "/api/v1/c06-verify/integration/verify-card";
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


        static string BuildHashRequest(string apiKey, string secretKey, string transactionId, long timestamp, string message)
        {
            try
            {
                string verifyHashPlainText = HashUtils.BuildHashValue(apiKey, transactionId, timestamp, message);
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
