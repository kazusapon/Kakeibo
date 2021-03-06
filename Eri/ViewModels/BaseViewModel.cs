﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Eri.Models;
using Eri.Services;

namespace Eri.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<IncomeList> DataStore => DependencyService.Get<IDataStore<IncomeList>>();
        
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        static DateTime now;
        public DateTime Now
        {
            get { return now; }
            set { now= value; }
        }

        static DateTime spend_now;
        public DateTime Now_Spend
        {
            get { return spend_now; }
            set { spend_now = value; }
        }

        static DateTime summary_now;
        public DateTime Now_Summary
        {
            get { return summary_now; }
            set { summary_now = value; }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
