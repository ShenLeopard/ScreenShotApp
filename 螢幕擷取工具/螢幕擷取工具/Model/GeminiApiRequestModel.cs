using Newtonsoft.Json;

namespace GeminiApi.Models.Request
{
    public class ApiRequestModel
    {
        [JsonProperty("contents")]
        public Content[] Contents { get; set; }
    }

    public class Content
    {
        [JsonProperty("parts")]
        public object[] Parts { get; set; }
    }

    public class TextPart
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class InlineDataPart
    {
        [JsonProperty("inlineData")]
        public InlineData InlineData { get; set; }
    }

    public class InlineData
    {
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}