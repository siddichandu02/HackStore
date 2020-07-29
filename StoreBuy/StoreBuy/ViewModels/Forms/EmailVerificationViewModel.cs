using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Utilities;
using StoreBuy.Views;
using StoreBuy.Views.Catalog;
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
    class EmailVerificationViewModel : BaseViewModel
    {
        #region Fields

        private string oTP;        
        public string misMatchText;
        public INavigation Navigation { get; set; }
        private static ISettings AppSettings => CrossSettings.Current;

        #endregion

        #region Constructor

        public EmailVerificationViewModel(INavigation _navigation)
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
            if (!CheckOTPs())
            {
                MisMatchText = Resources.OtpFormatMsg;
            }
            else
            {
                MisMatchText = "";
                var FormContent = MakeFormContent();
                EmailVerificationDataService emailVerificationData = new EmailVerificationDataService();
                HttpResponseMessage response = await emailVerificationData.ServiceEmail(FormContent);
                var status = response.StatusCode;

                if (status == HttpStatusCode.OK)
                {
                    Users NewUser = CreateUser();                   

                    string JsonUser = JSONConversion.SerializeObjectToString(NewUser);
                    StringContent Content = new StringContent(JsonUser, Encoding.UTF8, "application/json");
                    HttpResponseMessage Response = await emailVerificationData.ServiceUser(Content);
                    var statusRegister = Response.StatusCode;
                    if (statusRegister == HttpStatusCode.OK)
                    {
                        
                        var User = await emailVerificationData.ServiceGetUser(NewUser.Email);

                        AppSettings.AddOrUpdateValue(Resources.UserName, User.Email);
                        AppSettings.AddOrUpdateValue(Resources.UserFirstName, User.FirstName);
                        AppSettings.AddOrUpdateValue(Resources.UserFullName, User.FirstName + " " + User.LastName);
                        AppSettings.AddOrUpdateValue(Resources.UserId, User.UserId.ToString());
                        Application.Current.MainPage = new MainPage();
                        DependencyService.Get<IMessage>().LongAlert(Resources.RegisterSuccess);


                    }
                    else
                        await Navigation.PushAsync(new SignUpPage());
                }
                else if(status==HttpStatusCode.Found)
                {
                    MisMatchText = Resources.OtpWrong;
                }
                else
                {
                    MisMatchText = Resources.OtpExpired;
                }
            }

        }
        private Users CreateUser()
        {
            Users User = new Users();
            User.FirstName = AppSettings.GetValueOrDefault(Resources.FirstName, Resources.DefaultStringValue);
            User.LastName = AppSettings.GetValueOrDefault(Resources.LastName, Resources.DefaultStringValue);
            User.Phone = CryptoEngine.Encrypt(AppSettings.GetValueOrDefault(Resources.UserPhone, Resources.DefaultStringValue), Resources.CryptoKey);
            User.UserPassword = CryptoEngine.Encrypt(AppSettings.GetValueOrDefault(Resources.UserPassword, Resources.DefaultStringValue), Resources.CryptoKey);
            User.Email = AppSettings.GetValueOrDefault(Resources.UserName, Resources.DefaultStringValue);
            return User;
        }
        private bool CheckOTPs()
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
                        new KeyValuePair<string,string>(Resources.Email,AppSettings.GetValueOrDefault(Resources.UserName,Resources.DefaultStringValue))
            });
            return formcontent;
        }
        #endregion

    }
}
