using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JuanNotTheHuman.Spending.Services
{
    internal class FetchOptions
    {
        public object Body { get; set; }
        public string Authorization { get; set; }
        public string Method { get; set; }
    }
    internal class SqlRequest
    {
        public string type { get; set; }
        public SqlStatement stmt { get; set; }
    }
    internal class SqlStatement
    {
        public string sql { get; set; }
    }
    internal class RootRequest
    {
        public List<SqlRequest> requests { get; set; }
    }
    public class RootResponse
    {
        [JsonPropertyName("baton")]
        public object Baton { get; set; }

        [JsonPropertyName("base_url")]
        public string BaseUrl { get; set; }

        [JsonPropertyName("results")]
        public List<ResultItem> Results { get; set; }

        public class ResultItem
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("response")]
            public Response Response { get; set; }
        }

        public class Response
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("result")]
            public ExecuteResult Result { get; set; }
        }

        public class ExecuteResult
        {
            [JsonPropertyName("cols")]
            public List<string> Cols { get; set; }

            [JsonPropertyName("rows")]
            public List<List<object>> Rows { get; set; }

            [JsonPropertyName("affected_row_count")]
            public int AffectedRowCount { get; set; }

            [JsonPropertyName("last_insert_rowid")]
            public object LastInsertRowid { get; set; }

            [JsonPropertyName("replication_index")]
            public string ReplicationIndex { get; set; }
        }
    }
    internal static class ApiService
    {
        public static async Task<T> Post<T>(string url, FetchOptions options = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,UserSecretsService.GetSecret("Turso:ConnectionString"));
                Debug.WriteLine(request.RequestUri);
                request.Headers.Add("Authorization", $"Bearer {options.Authorization}");
                request.Content = new StringContent("{\"requests\":[{\"type\":\"execute\",\"stmt\":{\"sql\":\"SELECT * FROM records\"}},{\"type\":\"close\"}]}"); //Placeholder for testing
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                var rootResponse = JsonSerializer.Deserialize<RootResponse>(responseBody);
                var executeResult = rootResponse.Results.Find(r => r.Response?.Type == "execute")?.Response?.Result;
                if (executeResult != null)
                {
                    Debug.WriteLine(executeResult.Rows.Select(r => JsonSerializer.Serialize(r)));
                    return default;
                }
                else return default;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error while sending post request: {ex.Message}");
            }
        }
        public static async Task<bool> Post(string url, object body, string authorization = null)
        {
            var client = new HttpClient();
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body), "Body cannot be null");
            }
            try
            {
                string json = JsonSerializer.Serialize(body, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                    if (!string.IsNullOrEmpty(authorization))
                    {
                        request.Headers.Add("Authorization", authorization);
                    }
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to post data", ex);
            }
        }
    }
}
