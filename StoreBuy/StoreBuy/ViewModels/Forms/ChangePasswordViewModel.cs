    using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Utilities;
using StoreBuy.ViewModels;
using StoreBuy.Views.Profile;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;


namespace StoreBuy.ViewModels.Forms
{
    class ChangePasswordViewModel : BaseViewModel
    {


        private static ISettings AppSettings => CrossSettings.Current;


        private string oldPassword;


        private string newPassword;


        private string confirmNewPassword;


        private string misMatchText;


        INavigation Navigation { get; set; }




        #region Constructor


        public ChangePasswordViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }


        #endregion



        #region Public property



        public string NewPassword
        {
            get
            {
                return newPassword;
            }


            set
            {
                if (newPassword == value)
                {
                    return;
                }


                newPassword = value;
                NotifyPropertyChanged();
            }
        }


        public string OldPassword
        {
            get
            {
                return oldPassword;
            }


            set
            {
                if (oldPassword == value)
                {
                    return;
                }


                oldPassword = value;
                NotifyPropertyChanged();
            }
        }

        public string ConfirmNewPassword
        {
            get
            {
                return confirmNewPassword;
            }


            set
            {
                if (confirmNewPassword == value)
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
                return misMatchText;
            }


            set
            {
                if (misMatchText == value)
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
            var IsMatch = CheckPasswords();
            if (IsMatch != true)
            {
                MisMatchText = Resources.PasswordMismatch;
            }

            else
            {
                MisMatchText = "";
                var FormContent = MakeFormContent();
                ChangePasswordDataService changePasswordData = new ChangePasswordDataService();
                HttpResponseMessage response = await changePasswordData.ServicePassword(FormContent);
                var status = response.StatusCode;


                if (status == HttpStatusCode.OK)
                {
                    DependencyService.Get<IMessage>().LongAlert(Resources.PasswordUpdated);


                    await Navigation.PushAsync(new ProfilePage());
                }
                else if (status == HttpStatusCode.BadRequest)
                {
                    MisMatchText = Resources.OldPasswordMsg;


                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(Resources.PasswordErrorMsg);


                    await Navigation.PushAsync(new ProfilePage());
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
                        new KeyValuePair<string,string>(Resources.UserId,AppSettings.GetValueOrDefault(Resources.UserId,Resources.DefaultStringValue)),
                        new KeyValuePair<string, string>(Resources.NewPassword,CryptoEngine.Encrypt(NewPassword,Resources.CryptoKey)),
                        new KeyValuePair<string, string>(Resources.OldPassword,CryptoEngine.Encrypt(OldPassword,Resources.CryptoKey))



            });
            return formcontent;
        }
        #endregion
    }
}





