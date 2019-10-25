using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TokenEx.InterviewProject {
    public interface TokenExConfig {
        string TokenExId { get; }
        string ApiKey { get; }
        string TransactionType { get; }
        string IframeClientCode { get; }
        string TokenExUrl { get; }
        string LittleCoUrl { get; }
        string IframeCss { get; }
    }
    public class Configuration : TokenExConfig {
        public string TokenExId { get { return ConfigurationManager.AppSettings["TokenExId"]; } }
        public string ApiKey { get { return ConfigurationManager.AppSettings["ApiKey"]; } }
        public string TransactionType { get { return "1"; } }
        public string IframeClientCode { get { return ConfigurationManager.AppSettings["IframeClientCode"]; } }
        public string TokenExUrl { get { return ConfigurationManager.AppSettings["TokenExUrl"]; } }
        public string LittleCoUrl { get { return ConfigurationManager.AppSettings["LittleCoUrl"]; } }
        public string IframeCss { get { return ConfigurationManager.AppSettings["iframeCss"]; } }
    }
}