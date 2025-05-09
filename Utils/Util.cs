using System.Text.Json;
using Azure.Core.Serialization;

namespace OpenData.Functions.Utils
{
    public static class Util
    {
        public static readonly JsonSerializerOptions _serializaterOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static ObjectSerializer GetObjectSerializer(){
            return new JsonObjectSerializer(new JsonSerializerOptions{
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            });
        }
    }
}