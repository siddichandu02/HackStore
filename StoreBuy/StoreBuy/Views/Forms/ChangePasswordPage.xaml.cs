using StoreBuy.ViewModels.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
            BindingContext = new ChangePasswordViewModel(Navigation);
        }
        public void CheckFields(object sender, EventArgs args)
        {
            var PasswordValidation = PasswordValidationEntry.IsVisible;
            var ConfirmValidation = ConfirmValidationEntry.IsVisible;
            var OldPasswordValidation = OldPasswordValidationEntry.IsVisible;



            var PasswordText = NewPassword.Text;
            var ConfirmPasswordText = ConfirmNewPassword.Text;
            var OldPasswordText = OldPassword.Text;



            if (
                string.IsNullOrEmpty(PasswordText) ||
                string.IsNullOrEmpty(ConfirmPasswordText) ||
                string.IsNullOrEmpty(OldPasswordText)
                )
            {
                DependencyService.Get<IMessage>().LongAlert("Fileds cannot be Empty");
            }
            else if (
                PasswordValidation ||
                ConfirmValidation ||
                OldPasswordValidation)
            {
                DependencyService.Get<IMessage>().LongAlert("Please fill the fields in correct format");



            }



            else
            {
                (BindingContext as ChangePasswordViewModel).SubmitClicked();
            }
        }
    }
}