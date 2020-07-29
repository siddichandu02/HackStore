using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Views.Forms;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Profile
{
    [Preserve(AllMembers = true)]
    public class ProfileViewModel:BaseViewModel
    {
        private static ISettings AppSettings => CrossSettings.Current;
        private string name;
        private string email;
        private string phone;
        #region Constructor

        INavigation Navigation;
        public ProfileViewModel(INavigation navigation)
        {
            
            
            NextCommand = new Command(NextClicked);
            Navigation = navigation;
            Email = AppSettings.GetValueOrDefault(Resources.UserName, Resources.DefaultStringValue);
            Phone = AppSettings.GetValueOrDefault(Resources.UserPhone, Resources.DefaultStringValue);
            Name = AppSettings.GetValueOrDefault(Resources.UserFullName, Resources.DefaultStringValue);
        }


        #endregion

        #region Properties

        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone == value)
                {
                    return;
                }
                phone = value;
                NotifyPropertyChanged();

            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                NotifyPropertyChanged();

            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (email == value)
                {
                    return;
                }

                email = value;
                NotifyPropertyChanged();


            }
        }
        #endregion

        #region Command

        

        public Command NextCommand { get; set; }

        #endregion

        #region Methods


        private async void NextClicked(object obj)
        {
            Application.Current.Resources.TryGetValue("Gray-100", out var retVal);
            (obj as Grid).BackgroundColor = (Color)retVal;
            await Task.Delay(100);
            (obj as Grid).BackgroundColor = Color.Transparent;
            await Navigation.PushAsync(new ChangePasswordPage());
        }
        

        #endregion
    }
}
