using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models.Detail;
using StoreBuy.Views.Transaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Detail
{
    [Preserve(AllMembers = true)]
    public class StoreSlotViewModel : BaseViewModel
    {
        #region Fields
        private static ISettings AppSettings => CrossSettings.Current;
        private Command previousDateCommand;
        private Command nextDateCommand;
        private ObservableCollection<Slot> storeSlots;
        private INavigation Navigation;
        private bool today;
        private bool selectedGrid = false;
        private string date;
        private string selectedSlot;
        private string selectedSlotDate;
        private string selectedSlotTime;
        private long days;
        private DateTime dayStartTime;
        private DateTime dayEndTime;
        private long spanMinutes;
        private List<string> Dates = new List<string>();
        private int page = 0;
        private StoreModel Store;
        private Command confirmSlotCommand;

        #endregion

        #region Constructor
        public StoreSlotViewModel(INavigation navigation,StoreModel SelectedStore)
        {
            Navigation = navigation;
            Store = SelectedStore;
            days = SelectedStore.SlotDays;
            dayStartTime = SelectedStore.SlotStartTime;
            dayEndTime = SelectedStore.SlotEndTime;
            spanMinutes = SelectedStore.SlotDuration;

            if (DateTime.Now.TimeOfDay < dayEndTime.AddMinutes(-spanMinutes).TimeOfDay)
            {
                Dates.Add(DateTime.Now.ToString("dd-MMMM-yyyy"));
                for (int i = 2; i <= days; i++)
                {
                    Dates.Add(DateTime.Now.AddDays(i - 1).ToString("dd-MMMM-yyyy"));
                }
                today = true;
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    Dates.Add(DateTime.Now.AddDays(i).ToString("dd-MMMM-yyyy"));
                }
                today = false;
            }
            GetSlots(page);
            Date = Dates[page];
        }
        #endregion

        #region Public Properties

        [DataMember(Name = "Date")]
        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                if (date == value)
                {
                    return;
                }

                date = value;
                NotifyPropertyChanged();
            }
        }
        [DataMember(Name = "StoreSlots")]
        public ObservableCollection<Slot> StoreSlots
        {
            get
            {
                return storeSlots;
            }

            set
            {
                if (storeSlots == value)
                {
                    return;
                }

                storeSlots = value;
                NotifyPropertyChanged();
            }
        }
        public string SelectedSlot
        {
            get
            {
                return selectedSlot;
            }

            set
            {
                if (selectedSlot == value)
                {
                    return;
                }

                selectedSlot = value;
                NotifyPropertyChanged();
            }
        }

        public bool SelectedGrid
        {
            get
            {
                return selectedGrid;
            }

            set
            {
                if (selectedGrid == value)
                {
                    return;
                }

                selectedGrid = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public Command NextDateCommand
        {
            get { return nextDateCommand ?? (nextDateCommand = new Command(NextPageClicked)); }

        }
        
        public Command ConfirmSlotCommand
        {
            get { return confirmSlotCommand ?? (confirmSlotCommand = new Command(ConfirmSlotClicked)); }

        }

        public Command PreviousDateCommand
        {
            get { return previousDateCommand ?? (previousDateCommand = new Command(PreviousPageClicked)); }

        }

        #endregion

        #region Methods
        public void Button_Clicked(string ButtonText)
        {
            SelectedSlot = ButtonText;
            string[] SelectedText = ButtonText.Split(' ');
            selectedSlotDate = SelectedText[0];
            selectedSlotTime = SelectedText[1]; 
            SelectedGrid = true;
        }

        public async void GetSlots(int DateIndex)
        {
            StoreSlotDataService storeSlotData = new StoreSlotDataService();
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            var FilledSlots = await storeSlotData.GetAllSlots(Dates[DateIndex],UserId,Store);
            var SlotTime = new DateTime(2020, 1, 1, dayStartTime.Hour, dayStartTime.Minute, dayStartTime.Second);
            var storeSlots = new ObservableCollection<Slot>();
            while (SlotTime.TimeOfDay < dayEndTime.TimeOfDay)
            {
                var StartTime = SlotTime.ToString(Resources.SlotTimeFormat);
                SlotTime = SlotTime.AddMinutes(spanMinutes);
                var EndTime = SlotTime.ToString(Resources.SlotTimeFormat);
                Slot now = new Slot();
                now.SlotTime = StartTime + "-" + EndTime;
                now.SlotDate = Dates[DateIndex];
                bool IsFilled = FilledSlots.Any(s => s.Contains(StartTime));
                if (IsFilled)
                    now.IsSlotFilled = true;
                else
                    now.IsSlotFilled = false;
                if (today && now.SlotDate == Dates[0])
                {
                    if (DateTime.Now.TimeOfDay > SlotTime.AddMinutes(-spanMinutes).TimeOfDay)
                    {
                        now.IsSlotFilled = true;
                    }
                }
                now.IsSlotEmpty = !now.IsSlotFilled;
                now.ButtonText = now.SlotDate + " " + now.SlotTime;
                if (now.ButtonText == SelectedSlot)
                {
                    now.ButtonCheck = true;
                }
                else
                {
                    now.ButtonCheck = false;
                }
                storeSlots.Add(now);
            }
            StoreSlots = storeSlots;

        }
        private void PreviousPageClicked(object obj)
        {
            if (page == 0)
                return;
            page -= 1;
            GetSlots(page);
            Date = Dates[page];
        }

        private void NextPageClicked(object obj)
        {
            if (page == days - 1)
                return;
            page += 1;
            GetSlots(page);
            Date = Dates[page];
        }

        private async void ConfirmSlotClicked(object obj)
        {
            await Navigation.PushAsync(new OrderViewPage(selectedSlotDate, selectedSlotTime, Store));
        }

        #endregion
    }
}
