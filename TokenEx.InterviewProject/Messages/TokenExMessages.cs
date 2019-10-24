using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenEx.InterviewProject.Api.Messages {
    public interface TokenizeRequest {
        string TokenExId { get; set; }
        string APIKey { get; set; }
    }

    public interface TokenizeResponse {
        string Error { get; set; }
        string ReferenceNumber { get; set; }
        bool Success { get; set; }
    }
}
