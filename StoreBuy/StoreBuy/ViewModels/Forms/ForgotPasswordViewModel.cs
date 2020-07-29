using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Views;
using StoreBuy.Views.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Forms
{
    

    [Preserve(AllMembers = true)]
    public class ForgotPasswordViewModel : LoginViewModel
    {
        #region Fields

        private string misMatchText;
        private static ISettings AppSettings =>  CrossSettings.Current;
        public INavigation Navigation { get; set; }


        #endregion

        #region Constructor
        public ForgotPasswordViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
             SignUpCommand = new Command( SignUpClicked);
             SendCommand = new Command( SendClicked);
        }

        #endregion

        #region Command
        public Command SendCommand { get; set; }

        public Command SignUpCommand { get; set; }

        #endregion

        #region Public Properties

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

        private async void SendClicked(object obj)
        {
            if(string.IsNullOrEmpty(Email))
            {
                MisMatchText = Resources.TextBoxEmpty;
                return;
            }
            var FormContent = MakeFormContent();
            ForgotPasswordDataService forgotPasswordData = new ForgotPasswordDataService();
            HttpResponseMessage response = await forgotPasswordData.ServiceForgotPassword(FormContent);
            var status = response.StatusCode;

            if (status == HttpStatusCode.Found)
            {
                AppSettings.AddOrUpdateValue(Resources.ForgotPasswordMail,Email);
                await Navigation.PushAsync(new OTPVerificationPage());
            }
            else if(status == HttpStatusCode.BadRequest)
            {
                MisMatchText = Resources.NoEmailExist;
            }
            else
            {
                MisMatchText = Resources.MailErrorMsg;

            }
        }


        private FormUrlEncodedContent MakeFormContent()
        {
            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.Email,Email)
            });
            return formcontent;
        }
        private async void SignUpClicked(object obj)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        #endregion
    }
}