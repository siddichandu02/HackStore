using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Views;
using StoreBuy.Views.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Forms
{
   
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields
        private static ISettings AppSettings => CrossSettings.Current;


        private string _firstname;



        private string _lastname;




        private string _userpassword;



        private string _confirmPassword;


        private Users _user;

        INavigation Navigation;

        #endregion

        #region Constructor

       
        public SignUpPageViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
             LoginCommand = new Command( LoginClicked);
         
        }

        #endregion

        #region Property




    public string FirstName
        {
            get
            {
                return  _firstname;
            }


            set
            {
                if ( _firstname == value)
                {
                    return;
                }


                 _firstname = value;
                 NotifyPropertyChanged();
            }
        }


        public string LastName
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (value != null)
                {
                    _lastname = value;
                     NotifyPropertyChanged();


                }
            }
        }


        public string UserPassword
        {
            get
            {
                return _userpassword;
            }
            set
            {
                if (value != null)
                {
                    _userpassword = value;
                     NotifyPropertyChanged();


                }
            }
        }


       
        public string ConfirmPassword
        {
            get
            {
                return  _confirmPassword;
            }


            set
            {
                if ( _confirmPassword == value)
                {
                    return;
                }


                 _confirmPassword = value;
                 NotifyPropertyChanged();
            }
        }

       
        #endregion

        #region Command


        public Command LoginCommand { get; set; }

       
        public Command SignUpCommand { get; set; }     
        

        #endregion

        #region Methods

        private async void LoginClicked(object obj)
        {
            await Navigation.PushAsync(new LoginPage());
        }

       
        public async void SignUpClicked()
        {
            if (_user != null)
            {
                _user.FirstName = FirstName;
                _user.LastName = LastName;
                _user.Email = Email;
                _user.UserPassword = UserPassword;
                _user.Phone = Phone;
            }
            else
            {
                _user = new Users();
                _user.FirstName = FirstName;
                _user.LastName = LastName;
                _user.Email = Email;
                _user.UserPassword = UserPassword;
                _user.Phone = Phone;
            }

            if (!CheckPasswords())
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.PasswordMismatch);
                return;
            }
            var formContent = MakeFormContent();
            SignUpDataService signUpData = new SignUpDataService();
            HttpResponseMessage response = await signUpData.ServiceSignUp(formContent);
            var status = response.StatusCode;
            if (status == HttpStatusCode.Found)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.EmailExist);

            }
            else if(status==HttpStatusCode.NotFound)
            {

                AppSettings.AddOrUpdateValue(Resources.UserName,Email);
                AppSettings.AddOrUpdateValue(Resources.UserPassword, UserPassword);
                AppSettings.AddOrUpdateValue(Resources.UserPhone, Phone);
                AppSettings.AddOrUpdateValue(Resources.FirstName, FirstName);
                AppSettings.AddOrUpdateValue(Resources.LastName, LastName);
                await Navigation.PushAsync(new EmailVerificationPage());
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.RegisterUnsuccess);
            }
        }

        private bool CheckPasswords()
        {
            if (ConfirmPassword == UserPassword)
                return true;
            return false;
        }
        private FormUrlEncodedContent MakeFormContent()
        {
            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.Email,Email)
            });
            return formcontent;
        }

        #endregion
    }
}