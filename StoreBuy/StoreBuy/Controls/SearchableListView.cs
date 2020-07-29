using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.Controls
{

    [Preserve(AllMembers = true)]
    public class SearchableListView : SfListView
    {
        #region Field

        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create(nameof(SearchText), typeof(string), typeof(SearchableListView), null, BindingMode.Default, null, OnSearchTextChanged);

        private string searchText;

        #endregion


        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { this.SetValue(SearchTextProperty, value); }
        }



        #region Method


        private static void OnSearchTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable as SearchableListView;
            if (newValue != null && listView.DataSource != null)
            {
                listView.searchText = (string)newValue;
                listView.DataSource.Filter = listView.FilterContacts;
                listView.DataSource.RefreshFilter();
            }

            listView.RefreshView();
        }

        public virtual bool FilterContacts(object obj)
        {
            if (this.SearchText == null)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}