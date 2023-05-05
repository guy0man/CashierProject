﻿using CashierDB.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CashierUI.Dto
{
    public class OpenTabDetails : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
        public int TabId { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ShortOrderItem> Orders { get; set; } = new();
        public string Total { get; set; }
        public string _paymentStatus;
        public string PaymentStatus
        {
            get => _paymentStatus;
            set
            {
                _paymentStatus = value;
                OnPropertyChanged();
            }
        }
        public OpenTabDetails(Tab tab)
        {
            TabId = tab.TabId;
            Name = tab.CustomerName;
            Total = $"₱{tab.Total.ToString()}";
            if (tab.IsPaid) _paymentStatus = "Paid";
            else _paymentStatus = "Pending";
            foreach (var order in tab.OrderLists)
            {
                var newOrder = new ShortOrderItem(order);
                Orders.Add(newOrder);
            }
        }
        
    }
    public class TabDetails
    {
        public int TabId { get; set; }
        public string Name { get; set; }
        public string Tip { get; set; }
        public string Total { get; set; }
        public string Date { get; set; }
        public TabDetails(Tab tab)
        {
            TabId = tab.TabId;
            Name = tab.CustomerName;
            Tip = $"₱{tab.Tip}";
            Total = $"₱{tab.Total}";
            Date = $"{tab.Date:g}";
        }
    }
}
