using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bot_Lab.Util;

namespace Bot_Lab.Dialogs
{
    public class UserIDDialog
    {
        public enum RequestFor
        {
            Self,
            Coworker,
            Multiple
        }
        [Prompt(new string[] { "Enter Employee name(s) [comma delimited list if multiple]" })]
        public string Name { get; set; }

        
        
        public string Email { get; set; }
        //[Pattern(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        public string PhoneNumber { get; set; }
        public string GMID { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }

        [Prompt("Is the user information below correct?")]
        //[Prompt("What date would you like to dine with us? example: today, tomorrow, or any date like 04-06-2017 {||}", AllowDefault = BoolDefault.True)]
        //[Describe("Reservation date, example: today, tomorrow, or any date like 04-06-2017")]
        public string CorrectUserAnswer { get; set; }

        public RequestFor? RequestForOptions;
        [Numeric(1, 5)]
        [Optional]
        [Describe("for how you enjoyed your experience with Dinner Bot today (optional)")]
        public double? Rating;

        public static IForm<UserIDDialog> BuildUserIDForm()
        {
            Util.ServiceUtil su = new Util.ServiceUtil();
            String[] args = { "WZ5YSY" };
            string userInfo = su.SendReq("get_user_info", args);
           

            return new FormBuilder<UserIDDialog>()
                .Field(nameof(userInfo))
            .Field(nameof(Name))
            .Field(nameof(Email), validate: ValidateContactInformation)
            .Field(nameof(PhoneNumber))
            .Field(nameof(Department))
            .Field(nameof(GMID))
            .Field(nameof(Location))
            .Build();
        }
        private static Task<ValidateResult> ValidateContactInformation(UserIDDialog state, object response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (GetEmailAddress((string)response, out contactInfo))
            {
                result.IsValid = true;
                result.Value = contactInfo;
            }
            else
            {
                result.IsValid = false;
                result.Feedback = "You did not enter valid email address.";
            }
            return Task.FromResult(result);
        }
        private static bool GetEmailAddress(string response, out string contactInfo)
        {
            contactInfo = string.Empty;
            var match = Regex.Match(response, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (match.Success)
            {
                contactInfo = match.Value;
                return true;
            }
            return false;
        }
        private static PromptAttribute PerLinePromptAttribute(string pattern)
        {
            return new PromptAttribute(pattern)
            {
                ChoiceStyle = ChoiceStyleOptions.PerLine
            };
        }
    }
}