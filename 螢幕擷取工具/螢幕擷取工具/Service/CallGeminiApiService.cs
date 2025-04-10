using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using GeminiApi.Models.Request;
using Req = GeminiApi.Models.Request;       // Request 模型別名
using Resp = GeminiApi.Models.Response;
using 螢幕擷取工具;       // Response 模型別名

namespace GeminiApi.Services
{
    public static class GeminiApiService
    {
        public static async Task CallGeminiApiAsync(Bitmap bitmap)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 將 Bitmap 轉換為 byte[]
            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bitmapBytes = ms.ToArray();

            var inlineData = new Req.InlineData
            {
                MimeType = "image/jpeg",
                Data = Convert.ToBase64String(bitmapBytes)
            };

            var requestBody = new Req.ApiRequestModel
            {
                Contents = new Req.Content[]
                {
                    new Req.Content
                    {
                        Parts = new object[]
                        {
                            new Req.TextPart { Text = "請辨識此圖片中的文字，並依照原本的格式排版。不需回應額外內容，只需輸出圖片上的文字即可" },
                            new Req.InlineDataPart { InlineData = inlineData }
                        }
                    }
                }
            };

            string apiJson = JsonConvert.SerializeObject(requestBody);
            string url = Program.config["GeminiApi:BaseUrl"];
            string key = Program.config["GeminiApi:Key"];


            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(
                    new Uri(url + key),
                    new StringContent(apiJson, Encoding.UTF8, "application/json"));

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<Resp.ApiResponseModel>(responseContent);
                    MessageBox.Show(
                        $"回應訊息: {apiResponse.Candidates[0].Content.Parts[0].Text}",
                        "API Response",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show(responseContent, "(按OK複製錯誤訊息)", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Clipboard.SetText(responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
