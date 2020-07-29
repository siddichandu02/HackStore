using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Utilities;
using StoreBuy.Views;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Forms
{
    
    [Preserve(AllMembers = true)]
    public class ResetPasswordViewModel : BaseViewModel
    {
        #region Fields
        private static ISettings AppSettings => CrossSettings.Current;


        private string newPassword;

        private string confirmNewPassword;

        private string misMatchText;

        INavigation Navigation { get; set; }
        

        #endregion

        #region Constructor

        public ResetPasswordViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
             SubmitCommand = new Command( SubmitClicked);
             SignUpCommand = new Command( SignUpClicked);
        }

        #endregion

        #region Command

        
        public Command SubmitCommand { get; set; }

      
        public Command SignUpCommand { get; set; }

        #endregion

        #region Public property

       
        public string NewPassword
        {
            get
            {
                return  newPassword;
            }

            set
            {
                if ( newPassword == value)
                {
                    return;
                }

                 newPassword = value;
                 NotifyPropertyChanged();
            }
        }

       
        public string ConfirmNewPassword
        {
            get
            {
                return  confirmNewPassword;
            }

            set
            {
                if ( confirmNewPassword == value)
                {
                    return;
                }

                 confirmNewPassword = value;
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

        
        public async void SubmitClicked()
        {
            var IsMatch=CheckPasswords();
            if(IsMatch!=true)
            {
                MisMatchText = Resources.PasswordMismatch;
            }
            else
            {
                MisMatchText = "";
                var FormContent = MakeFormContent();
                ResetPasswordDataService resetPasswordData = new ResetPasswordDataService();
                HttpResponseMessage response = await resetPasswordData.ServiceResetPassword(FormContent);
                var status = response.StatusCode;

                if (status == HttpStatusCode.OK)
                {
                    DependencyService.Get<IMessage>().LongAlert(Resources.PasswordUpdatedLogin);

                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(Resources.ErrorMsg);
                }
            }
            
        }

        private bool CheckPasswords()
        {
            if (NewPassword == ConfirmNewPassword)
                return true;
            return false;
        }


        private FormUrlEncodedContent MakeFormContent()
        {
            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.Email,AppSettings.GetValueOrDefault(Resources.ForgotPasswordMail,Resources.DefaultStringValue)),
                        new KeyValuePair<string, string>(Resources.NewPassword,CryptoEngine.Encrypt(NewPassword,Resources.CryptoKey))
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