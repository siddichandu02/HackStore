using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Models;
using StoreBuy.Utilities;
using StoreBuy.Views;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
namespace StoreBuy.ViewModels.Forms
{
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields
        public INavigation Navigation { get; set; }

        private static ISettings AppSettings =>  CrossSettings.Current;

        #endregion

        #region Constructor

        public LoginPageViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
             SignUpCommand = new Command( SignUpClicked);
             ForgotPasswordCommand = new Command( ForgotPasswordClicked);
             SocialMediaLoginCommand = new Command( SocialLoggedIn);
        }

        #endregion     

        #region Command


        public Command SignUpCommand { get; set; }

        public Command ForgotPasswordCommand { get; set; }

        
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        public async void LoginClicked()
        {

            var formcontent = MakeFormContent();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix+Resources.LoginURI, formcontent);
            var status = response.StatusCode;

            if (status == HttpStatusCode.Found)
            {
                var URI = Resources.APIPrefix + Resources.GetUserByUserNameURI + Email;        
                var Userresponse = await client.GetStringAsync(URI);
                var User = JSONConversion.DeserializeResponseToModel<Users>(Userresponse);

                AppSettings.AddOrUpdateValue(Resources.UserName, User.Email);
                AppSettings.AddOrUpdateValue(Resources.UserFirstName, User.FirstName);
                AppSettings.AddOrUpdateValue(Resources.UserFullName, User.FirstName+" "+User.LastName);
                AppSettings.AddOrUpdateValue(Resources.UserPhone, CryptoEngine.Decrypt(User.Phone, Resources.CryptoKey));
                AppSettings.AddOrUpdateValue(Resources.UserId, User.UserId.ToString());
                
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.WrongCredentials);
            }
        }



        private FormUrlEncodedContent MakeFormContent()
        {
            var formcontent = new FormUrlEncodedContent(new[]
              {
                        new KeyValuePair<string,string>(Resources.Email,Email),
                        new KeyValuePair<string, string>(Resources.Password,CryptoEngine.Encrypt(Password,Resources.CryptoKey))
                    });
            return formcontent;
        }

        private async void SignUpClicked(object obj)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
            await Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void SocialLoggedIn(object obj)
        {
           
        }

        #endregion
    }
}