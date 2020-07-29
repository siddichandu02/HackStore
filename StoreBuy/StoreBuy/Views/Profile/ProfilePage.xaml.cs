using StoreBuy.ViewModels.Profile;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Profile
{
    /// <summary>
    /// Page to show chat profile page
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatProfilePage" /> class.
        /// </summary>
        public ProfilePage()
        {

            InitializeComponent();
            BindingContext = new ProfileViewModel(Navigation);
            // this.ProfileImage.Source = App.BaseImageUrl + "ProfileImage11.png";
        }


    }
}