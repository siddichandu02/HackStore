using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StoreBuy.ViewModels.Forms
{
    public class LogoutViewModel
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public LogoutViewModel()
        {
            AppSettings.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
