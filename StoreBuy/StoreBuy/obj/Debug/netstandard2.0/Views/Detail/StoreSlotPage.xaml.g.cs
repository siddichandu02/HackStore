//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("StoreBuy.Views.Detail.StoreSlotPage.xaml", "Views/Detail/StoreSlotPage.xaml", typeof(global::StoreBuy.Views.Detail.StoreSlotPage))]

namespace StoreBuy.Views.Detail {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\Detail\\StoreSlotPage.xaml")]
    public partial class StoreSlotPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.XForms.Buttons.SfButton PreviousDateButton;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.XForms.Buttons.SfButton NextDateButton;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ListView storeItemTable;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.XForms.Buttons.SfButton confirmSlotButton;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(StoreSlotPage));
            PreviousDateButton = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.XForms.Buttons.SfButton>(this, "PreviousDateButton");
            NextDateButton = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.XForms.Buttons.SfButton>(this, "NextDateButton");
            storeItemTable = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ListView>(this, "storeItemTable");
            confirmSlotButton = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.XForms.Buttons.SfButton>(this, "confirmSlotButton");
        }
    }
}