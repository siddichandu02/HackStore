using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.MenuItems;
using StoreBuy.Views.Bookmarks;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.Detail;
using StoreBuy.Views.Forms;
using StoreBuy.Views.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StoreBuy
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    { 
        public List<MasterPageItem> menuList { get; set; }
        private static ISettings AppSettings => CrossSettings.Current;
       
        public MainPage()
        {
            var UserFirstName = "Hello "+ AppSettings.GetValueOrDefault("UserFirstName", "User");
            
            InitializeComponent();
            Username.Text = UserFirstName;

            menuList = new List<MasterPageItem>();

            // Adding menu items to menuList and you can define title ,page and icon
            menuList.Add(new MasterPageItem() { Title = "Home", Icon = "home.png", TargetType = typeof(CategoryTilePage) });
            menuList.Add(new MasterPageItem() { Title = "Profile", Icon = "profileicon.png", TargetType = typeof(ProfilePage) });
            menuList.Add(new MasterPageItem() { Title = "Book Slot", Icon = "Slot.png", TargetType = typeof(StoresPage) });
            menuList.Add(new MasterPageItem() { Title = "Orders", Icon = "Order.png", TargetType = typeof(OrderHistoryPage) });
            menuList.Add(new MasterPageItem() { Title = "R10 Category", Icon = "r10category.png", TargetType = typeof(R10CategoryTilePage) });

            menuList.Add(new MasterPageItem() { Title = "Logout", Icon = "logout.png", TargetType = typeof(LogoutPage) });


            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CategoryTilePage)));

        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }

        
    }
}
