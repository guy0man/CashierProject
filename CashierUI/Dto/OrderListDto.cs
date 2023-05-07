using CashierDB.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.Dto
{
    public class PartialOrderItem :INotifyPropertyChanged
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
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public float Price { get; set; }
        public float Total { get; set; }
        public bool IsServed { get; set; }      
        public int ServedQuantity { get; set; }
        public Visibility ButtonVis { get; set; }
        public bool _isOld;
        public bool IsOld
        {
            get => _isOld;
            set
            {
                _isOld = value;
                if (_isOld)
                {
                    ButtonVis = Visibility.Hidden;
                }
                else
                {
                    ButtonVis = Visibility.Visible;
                }
            }
        }
        public float RealTotal { get; set; }
        public string _realTotalText;
        public string realTotalText
        {
            get => _realTotalText;
            set
            {
                _realTotalText = value;
                OnPropertyChanged();
            }
        }
        public bool _isCanceled;
        public bool IsCanceled
        {
            get => _isCanceled;
            set
            {
                _isCanceled = value;
                if (IsServed)
                {
                    MessageBox.Show("You cannot cancel a served item", "Error");
                    _isCanceled = false;
                    return;
                }
                if (_isCanceled)
                {
                    RealTotal = 0;
                    realTotalText = $"₱0";
                    OnPropertyChanged(nameof(RealTotal));
                }
                else
                {
                    RealTotal = Total;
                    realTotalText = $"₱{Total}";
                    OnPropertyChanged(nameof (RealTotal));
                }
            }
        }
        public PartialOrderItem(MenuItemName menuItem)
        {
            MenuItemId = menuItem.MenuItemId;
            Name = menuItem.Name;
            _quantity = 1;
            Price = float.Parse(menuItem.Price.Remove(0, 1));
            Total = Price;
            RealTotal = Price;
            realTotalText = $"₱{Price}";
            IsCanceled = false;
            IsServed = false;
            ServedQuantity = 0;
            IsOld = false;
        }
    }
    public class ShortOrderItem
    {
        public int OrderItemId { get; set; }
        public int MenuItemId { get; set; }
        public bool IsCanceled { get; set; }
        public bool _isServed;
        public bool IsServed
        {
            get => _isServed;
            set
            {
                _isServed = value;
                if (_isServed == true) ServedQuantity = Quantity;
                else ServedQuantity = 0;
            }
        }
        public int ServedQuantity { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Cost { get; set; }
        public ShortOrderItem(OrderList order)
        {
            OrderItemId = order.OrderListId;
            MenuItemId = order.MenuItemId;
            Name = order.MenuItemLink.Name;
            IsCanceled = order.IsCanceled;
            IsServed = order.IsServed;
            ServedQuantity = order.ServedQuantity;
            Quantity = order.Quantity;
            if (order.IsCanceled) Cost = $"Cancelled";
            else Cost = $"₱{order.RealTotal.ToString()}";       
        }
    }
    public class OrderItemName
    {
        public int OrderListId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Total { get; set; }
        public OrderItemName(OrderList order)
        {
            OrderListId = order.OrderListId;
            Name = order.MenuItemLink.Name;
            Quantity= order.Quantity;
            Total = $"₱{order.RealTotal}";
        }
    }
}
