using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
using CashierUI.Parts;
using CashierUI.Parts.AddSystems;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashierUI.ViewModels
{
    public class AddMenuItemViewModel : INotifyPropertyChanged
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
        public CashierContext _context { get; set; }
        public MenuTabViewModel Parent { get; set; }
        public AddMenuItemViewModel(CashierContext context, MenuTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            LoadTypes();
        }
        public string Name { get; set; }
        public ItemTypeName _selectedType;
        public ItemTypeName SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }
        public string Price { get; set; }
        public string Stock { get; set; }
        public ObservableCollection<ItemTypeName> Types { get; set; } = new();
        public ObservableCollection<string> Errors { get; set; } = new();
        public void LoadTypes()
        {
            var types = _context.ItemTypes.Select(c => new ItemTypeName(c.ItemTypeId, c.Name)).ToList();
            Types.Clear();
            foreach (var type in types) Types.Add(type);
        }
        public virtual void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var newItem = new CashierDB.Tables.MenuItem();
                newItem.Name = Name;
                newItem.ItemTypeId = SelectedType.ItemTypeId;
                newItem.Price = float.Parse(Price);
                newItem.Stock = int.Parse(Stock);
                try
                {
                    _context.Add(newItem);
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    DialogResult = false;
                }
            }
            Parent.AddMenuCheck();
        }
        public virtual void Cancel()
        {
            DialogResult = true;
            Parent.AddMenuCheck();
        }
        public bool DialogResult { get; set; }
        public virtual bool Validate()
        {
            var validator = new AddMenuItemValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
        public AddItemTypeViewModel itemTypeVM { get; set; }
        public UserControl userControl { get; set; }
        public void AddType()
        {
            itemTypeVM = new AddItemTypeViewModel(_context, this);
            userControl = new AddItemType();
            userControl.DataContext = itemTypeVM;
            OnPropertyChanged(nameof(userControl));       
        }
        public void CheckAddType()
        {
            if (itemTypeVM.DialogResult)
            {
                userControl = null;
                LoadTypes();
                Parent.Parent.LoadMenuTabs();
            }
            OnPropertyChanged(nameof(userControl));
        }
        public void RemoveType()
        {
            string sureMessage = $"Are you sure to permanently remove selected item type named \"{SelectedType.Name}\"?";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes)
            {
                var type = _context.ItemTypes.First(c => c.ItemTypeId == SelectedType.ItemTypeId);
                if (type.MenuItems != null) type.MenuItems.Clear();
                try
                {
                    _context.ItemTypes.Remove(type);
                    _context.SaveChanges();
                    LoadTypes();
                    Parent.Parent.LoadMenuTabs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }         
        }
    }
    public class AddMenuItemValidator : AbstractValidator<AddMenuItemViewModel>
    {
        public AddMenuItemValidator()
        {
            RuleFor(c=>c.Name).NotEmpty();        
            RuleFor(c=>c.Price).NotEmpty();
            RuleFor(c=>c.SelectedType).NotEmpty();
            RuleFor(c => c.Price).Must(c => float.TryParse(c, out var val)).WithMessage("Invalid Price");
            RuleFor(c=>c.Stock).NotEmpty();
            RuleFor(c => c.Stock).Must(c => int.TryParse(c, out var val)).WithMessage("Invalid Stock Value");
        }
    }
}
