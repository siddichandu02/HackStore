using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Views;
using StoreBuy.Views.Forms;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Forms
{
    [Preserve(AllMembers = true)]
    class OTPVerificationViewModel: BaseViewModel
    {
        #region Fields

        private string oTP;
        private static ISettings AppSettings => CrossSettings.Current;

        private string misMatchText;
        private INavigation Navigation { get; set; }

        #endregion

        #region Constructor

        public OTPVerificationViewModel(INavigation _navigation)
        {
            
             Navigation = _navigation;
             SubmitCommand = new Command( SubmitClicked);
        }

        #endregion

        #region Command
        public Command SubmitCommand { get; set; }

        #endregion

        #region Public property

        public string OTP
        {
            get
            {
                return  oTP;
            }

            set
            {
                if ( oTP == value)
                {
                    return;
                }

                 oTP = value;
                 NotifyPropertyChanged();
            }
        }

       
        

        public string MisMatchText
        {
            get
            {
                return  misMatchText;
            }

            set
            {
                if ( misMatchText == value)
                {
                    return;
                }

                 misMatchText = value;
                 NotifyPropertyChanged();
            }
        }


#endregion

        #region Methods

       
        private async void SubmitClicked(object obj)
        {
            if (string.IsNullOrEmpty(OTP))
            {
                MisMatchText = Resources.TextBoxEmpty;
                return;
            }
            if (CheckOTP())
            {
                var FormContent = MakeFormContent();
                OTPVerificationDataService oTPVerificationData = new OTPVerificationDataService();
                HttpResponseMessage response = await oTPVerificationData.ServiceVerifyOTP(FormContent);
                var status = response.StatusCode;

                if (status == HttpStatusCode.OK)
                {
                    await Navigation.PushAsync(new ResetPasswordPage());
                }
                else if (status == HttpStatusCode.Found)
                {
                    MisMatchText = Resources.OtpWrong;
                }
                else
                {
                    MisMatchText = Resources.OtpExpired;
                }
            }
            else
            {
                MisMatchText = Resources.OtpFormatMsg;
            }
            

        }

        

        private bool CheckOTP()
        {
            try
            {
                var convertedOTP = Int64.Parse(OTP);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        private FormUrlEncodedContent MakeFormContent()
        {
            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.OTP,OTP),
                        new KeyValuePair<string,string>(Resources.Email,AppSettings.GetValueOrDefault(Resources.ForgotPasswordMail,Resources.DefaultStringValue))
            });
            return formcontent;
        }
        #endregion

    }
}
