using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Data
{
    public static class Http
    {
        public static async Task<string> GetData(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Model.ResponseModel> PostData(Model.SendModel send)
        {
            try
            {
                HttpContent postContent;
                MultipartContent multiContent = new MultipartContent("form-data");
                StringContent NameContent = new StringContent(send.sendTitle, Encoding.UTF8);
                StringContent EmailContent = new StringContent(send.sendEmail, Encoding.UTF8);
                StringContent TitleContent = new StringContent(send.sendTitle, Encoding.UTF8);
                StringContent ContentContent = new StringContent(send.sendContent, Encoding.UTF8);
                StringContent FidOrRestoContent = new StringContent(send.sendId, Encoding.UTF8);

                if (send.isMain)
                    FidOrRestoContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"fid\"");
                else
                    FidOrRestoContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"resto\"");

                NameContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"name\"");
                EmailContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"email\"");
                TitleContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"title\"");
                ContentContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"content\"");
                if (send.islandCode == IslandsCode.A || send.islandCode == IslandsCode.Beitai)
                    multiContent.Add(FidOrRestoContent);

                multiContent.Add(NameContent);
                multiContent.Add(EmailContent);
                multiContent.Add(TitleContent);
                multiContent.Add(ContentContent);
                if (!string.IsNullOrEmpty(send.sendImage))
                {
                    var fs = await File.ReadFileStreamAsync(send.sendImage);
                    string filename = Path.GetFileName(send.sendImage);
                    var imageContent = new StreamContent(fs);
                    if (send.islandCode == IslandsCode.Beitai)
                        imageContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"upfile\";filename=\"" + filename + "\"");
                    else
                        imageContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data;name=\"image\";filename=\"" + filename + "\"");
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/*");
                    multiContent.Add(imageContent);
                }
                postContent = multiContent;

                string responseBody = "";
                //Set cookie
                var baseAddress = new Uri(send.Host);
                var handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };
                if (!string.IsNullOrEmpty(send.CookieValue))
                {
                    CookieContainer cc = new CookieContainer();
                    handler.CookieContainer = cc;
                    cc.Add(baseAddress, new Cookie(send.CookieName, send.CookieValue));
                }
                var api = string.Format(send.PostApi, send.Host, send.sendId);
                HttpResponseMessage response = await client.PostAsync(api, postContent);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<string> c;
                response.Headers.TryGetValues("Set-Cookie", out c);
                var res = new Model.ResponseModel()
                {
                    body = responseBody,
                    cookies = c
                };
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
