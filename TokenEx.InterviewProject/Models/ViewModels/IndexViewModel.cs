using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TokenEx.InterviewProject.Models.DataModels;

namespace TokenEx.InterviewProject.Models
{
    public class IndexViewModel //: IValidatableObject
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //[Required]
        //[CreditCard]
        //public string CreditCardNumber { get; set; }
        [Required]
        [Range(100,9999)]
        public int  CVV { get; set; }
        [Required]
        public int ExpMonth { get; set; }
        [Required]
        public int ExpYear { get; set; }

        public IframeConfigModel IframeData { get; set; }
        private string tokenExID { get { return IframeData.tokenExID; } }
        private int tokenScheme { get { return IframeData.tokenScheme; } }
        private string authenticationKey { get { return IframeData.authenticationKey; } }
        private string timeStamp { get { return IframeData.timeStamp; } }
        private string origin { get { return IframeData.origin; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (IsValidExpDate()) {
                yield return new ValidationResult("Invalid Expiration Date");
            }
        }
        private bool IsValidExpDate() {
            //get the month and year and make a datetime without the time...
            DateTime date = DateTime.Parse(string.Format("{0}/{1}/{2}", this.ExpMonth, DateTime.DaysInMonth(this.ExpYear, this.ExpMonth), this.ExpYear));
            DateTime now = DateTime.Now;
            //get the current date.
            DateTime validUntildate = DateTime.Parse(string.Format("{0}/{1}/{2}", now.Month, DateTime.DaysInMonth(now.Year, now.Month), now.Year));

            if (date >= validUntildate) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}