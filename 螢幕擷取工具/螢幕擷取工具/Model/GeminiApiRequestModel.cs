namespace GeminiApi.Models.Request
{
    public class ApiRequestModel
    {
        public Content[] Contents { get; set; }
    }

    public class Content
    {
        public object[] Parts { get; set; }
    }

    public class TextPart
    {
        public string Text { get; set; }
    }

    public class InlineDataPart
    {
        public InlineData InlineData { get; set; }
    }

    public class InlineData
    {
        public string MimeType { get; set; }
        public string Data { get; set; }
    }
}
