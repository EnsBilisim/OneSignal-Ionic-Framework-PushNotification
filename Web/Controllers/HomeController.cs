using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Web.Controllers
{
    /*
     * Company: ENS BİLİŞİM
     * Created Date:30.08.2016
     * Web Site: www.ensbilisim.com
     * Info Mail: info@ensbilisim.com
     * Questions And Suggestions Mail: social@ensbilisi.com
    */
    public class HomeController : Controller
    {
        private string apiUrl = "https://onesignal.com/api/v1/notifications";
        private string appId = "";
        private string restAPIKey = "";

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public JsonResult Push(string Title,string Content,string Url, string Segment, string Type)
        {
            var request = WebRequest.Create(apiUrl) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic "+ restAPIKey);
         
            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id =appId,
                contents = new { en =Content},
                headings = new { en =Title},
                included_segments = new string[] { Segment },
                data= new { Type = Type } , 
                url = !String.IsNullOrEmpty(Url) ? Url : "" 
            };

            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;
            #region POST
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                responseContent = ex.Message;
            }
            #endregion
            return Json(responseContent, JsonRequestBehavior.AllowGet);
        }

    }
}
