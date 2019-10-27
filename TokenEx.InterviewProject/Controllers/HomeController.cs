using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TokenEx.InterviewProject.Models;
using TokenEx.InterviewProject.Api.Messages;
using System.Net.Http;
using System.Configuration;
using System.Collections.Specialized;
using TokenEx.InterviewProject.Models.DataModels;

namespace TokenEx.InterviewProject.Controllers
{
    public class HomeController : Controller {
        Configuration config;
        public HomeController() {
            config = new Configuration();
        }
        // GET: Home
        public ActionResult Index() {
            IndexViewModel tmpViewModel = GenerateIframe();
            return View(tmpViewModel);
        }

        // POST: Home
        [HttpPost]
        public ActionResult Index(IndexViewModel model) {
            //if the user supplied valid data, perform a credit card authorization
            if (ModelState.IsValid) {
                try {
                    //implement call to payment services API and return auth code to user
                    string authCode = "ABC123";

                    ViewBag.AuthCode = authCode;
                    return View(model);
                } catch (Exception ex) {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }
            else {
                return View(model);
            }
        }

        private string GenerateHMAC(string payload, string HMACKey) {
            string result = string.Empty;

            System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256();
            hmac.Key = System.Text.Encoding.UTF8.GetBytes(HMACKey);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
            result = Convert.ToBase64String(hash);
            return result;
        }

        public IndexViewModel GenerateIframe() {
            string originUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            var tmpDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            //DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", null)
            TokenizeRequestImpl tmpRequest = new TokenizeRequestImpl {
                TokenExId = config.TokenExId,
                APIKey = config.ApiKey,
                HMACKey = config.IframeClientCode,
                OriginUrl = originUrl,
                CustomerRefnumber = "interviewTest",
                TokenScheme = 1,
                CurrentDateTime = tmpDate,
                CSS = config.IframeCss
            };

            TokenizeResponseImpl tmpResult = new TokenizeResponseImpl();
            using (HttpClient client = new HttpClient()) {
                var response = client.PostAsJsonAsync<TokenizeRequestImpl>(config.TokenExUrl, tmpRequest).Result;
                tmpResult = response.Content.ReadAsAsync<TokenizeResponseImpl>().Result;
            }

            if (tmpResult.Success) {
                tmpRequest.OriginUrl = tmpResult.HTPURL;
                ViewBag.SessionUrl = tmpResult.HTPURL;
            }
            else {
                tmpRequest.OriginUrl = tmpRequest.OriginUrl;
                ViewBag.SessionUrl = "";
            }

            var concatTokenInfo = tmpRequest.TokenExId + "|" +
                tmpRequest.OriginUrl + "|" +
                tmpRequest.CurrentDateTime + "|" +
                tmpRequest.TokenScheme;

            var iframeHMAC = GenerateHMAC(concatTokenInfo, config.IframeClientCode);

            IframeConfigModel frameConfig = new IframeConfigModel {
                tokenExID = tmpRequest.TokenExId,
                tokenScheme = tmpRequest.TokenScheme,
                authenticationKey = iframeHMAC,
                timeStamp = tmpRequest.CurrentDateTime.ToString(),
                origin = tmpRequest.OriginUrl
            };

            return new IndexViewModel { IframeData = frameConfig };
        }
    }

    public class TokenizeRequestImpl : TokenizeRequest {
        public string TokenExId { get; set; }
        public string APIKey { get; set; }
        public string HMACKey { get; set; }
        public string OriginUrl { get; set; }
        public string CustomerRefnumber { get; set; }
        public int TokenScheme { get; set; }
        public string CurrentDateTime { get; set; }
        public string CSS { get; set; }
    }

    public class TokenizeResponseImpl : TokenizeResponse {
        public string HTPURL { get; set; }
        public string SessionID { get; set; }
        public string Error { get; set; }
        public string ReferenceNumber { get; set; }
        public bool Success { get; set; }
    }

    public class PaymentRequest : TokenizeRequest {
        public string TokenExId { get; set; }
        public string APIKey { get; set; }
        public string TransactionType { get; set; }

    }

    public class PaymentResponse : TokenizeResponse {
        public string Authorization { get; set; }
        public bool TransactionResult { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string ReferenceNumber { get; set; }
        public bool Success { get; set; }

    }


}