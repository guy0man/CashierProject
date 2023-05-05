using CashierDB;
using CashierDB.Tables;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class AddItemTypeViewModel
    {
        public CashierContext _context { get; set; }
        public AddMenuItemViewModel Parent { get; set; }
        public AddItemTypeViewModel(CashierContext context, AddMenuItemViewModel parent)
        {
            _context = context;
            Parent = parent;
        }
        public string Name { get; set; }
        public ObservableCollection<string> Errors { get; set; } = new();
        public void Add()
        {
            var isValid = Validate();
            if (isValid)
            {
                var type = new ItemType();
                type.Name = Name;
                try
                {
                    _context.Add(type);
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
            Parent.CheckAddType();
        }
        public void Cancel()
        {
            DialogResult = true;
            Parent.CheckAddType();
        }
        public bool DialogResult { get; set; }
        public virtual bool Validate()
        {
            var validator = new AddItemTypeValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
    }
    public class AddItemTypeValidator : AbstractValidator<AddItemTypeViewModel>
    {
        public AddItemTypeValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
