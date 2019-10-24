using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenEx.InterviewProject.Models.DataModels {
    public class IframeConfigModel {
        public string tokenExID { get; set; }
        public int tokenScheme { get; set; }
        public string authenticationKey { get; set; }
        public string timeStamp { get; set; }
        public string origin { get; set; }
    }
}