using System;
using System.Collections;
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
    public class TursoPipelineResponse
    {
        [JsonPropertyName("results")]
        public List<TursoResult> Results { get; set; }
    }

    public class TursoResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("response")]
        public TursoResponse Response { get; set; }

        [JsonPropertyName("error")]
        public TursoError Error { get; set; }

        [JsonPropertyName("base_url")]
        public string BaseUrl { get; set; }
    }

    public class TursoError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class TursoResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("result")]
        public TursoExecutionResponse Result { get; set; }
    }

    public class TursoExecutionResponse
    {
        [JsonPropertyName("cols")]
        public List<TursoColumn> Cols { get; set; }

        [JsonPropertyName("rows")]
        public List<List<TursoCell>> Rows { get; set; }

        [JsonPropertyName("affected_row_count")]
        public int AffectedRowCount { get; set; }

        [JsonPropertyName("last_insert_rowid")]
        public object LastInsertRowId { get; set; }

        [JsonPropertyName("replication_index")]
        public object ReplicationIndex { get; set; }
    }

    public class TursoColumn
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("decltype")]
        public string DeclType { get; set; }
    }

    public class TursoCell
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }


    internal static class ApiService
    {
        public static async Task<T> Post<T>(string url, FetchOptions options = null)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, UserSecretsService.GetSecret("Turso:ConnectionString"));
                request.Headers.Add("Authorization", $"Bearer {options.Authorization}");

                string json = JsonSerializer.Serialize(options.Body, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                var res = JsonSerializer.Deserialize<TursoPipelineResponse>(responseBody);

                foreach (var result in res.Results)
                {
                    if (result.Error != null)
                    {
                        throw new Exception($"Error in Turso response: {JsonSerializer.Serialize(result.Error)}");
                    }

                    if (result.Response != null && result.Response.Result != null)
                    {
                        return MapTo<T>(result.Response.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception($"Error while sending post request: {ex.Message}");
            }

            return default(T);
        }
        private static T MapTo<T>(TursoExecutionResponse response)
        {
            if (response == null || response.Rows == null || response.Cols == null)
            {
                if (typeof(T) == typeof(Dictionary<string, object>[]))
                    return (T)(object)Array.Empty<Dictionary<string, object>>();

                if (typeof(T) == typeof(List<Dictionary<string, object>>))
                    return (T)(object)new List<Dictionary<string, object>>();

                return default(T);
            }
            Type elementType = typeof(T);
            bool isArray = false;
            bool isList = false;

            if (typeof(T).IsArray)
            {
                elementType = typeof(T).GetElementType();
                isArray = true;
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = typeof(T).GetGenericArguments()[0];
                isList = true;
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var row in response.Rows)
            {
                var obj = Activator.CreateInstance(elementType);

                for (int i = 0; i < response.Cols.Count; i++)
                {
                    var colName = response.Cols[i].Name;
                    var cell = row[i];
                    var prop = elementType.GetProperty(colName);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(obj, ConvertCell(cell, prop.PropertyType));
                    }
                }

                list.Add(obj);
            }

            if (isArray)
            {
                var array = Array.CreateInstance(elementType, list.Count);
                list.CopyTo(array, 0);
                return (T)(object)array;
            }

            if (isList)
                return (T)list;
            return list.Count > 0 ? (T)list[0] : default(T);
        }

        private static object ConvertCell(TursoCell cell, Type targetType)
        {
            if (cell == null || cell.Value == null)
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

            var str = cell.Value.ToString();

            if (targetType.IsEnum)
            {
                if (int.TryParse(str, out var intVal))
                    return Enum.ToObject(targetType, intVal);
                try
                {
                    return Enum.Parse(targetType, str, ignoreCase: true);
                }
                catch
                {
                    return Activator.CreateInstance(targetType);
                }
            }

            if (targetType == typeof(int))
            {
                if (int.TryParse(str, out var v)) return v;
                return 0;
            }
            if (targetType == typeof(long))
            {
                if (long.TryParse(str, out var v)) return v;
                return 0L;
            }
            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(str, out var v)) return v;
                return 0m;
            }
            if (targetType == typeof(double))
            {
                if (double.TryParse(str, out var v)) return v;
                return 0d;
            }
            if (targetType == typeof(DateTime))
            {
                if (DateTime.TryParse(str, out var dt)) return dt;
                return default(DateTime);
            }

            return str;
        }


        public static async Task<bool> Post(string url, object body, string authorization)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, UserSecretsService.GetSecret("Turso:ConnectionString"));
                request.Headers.Add("Authorization", $"Bearer {authorization}");
                string json = JsonSerializer.Serialize(body, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                Debug.WriteLine(json);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<TursoPipelineResponse>(responseBody);
                foreach (var result in res.Results)
                {
                    if (result.Error != null)
                    {
                        throw new Exception($"Error in Turso response: {JsonSerializer.Serialize(result.Error)}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception($"Error while sending post request: {ex.Message}");
            }
        }
    }
}
