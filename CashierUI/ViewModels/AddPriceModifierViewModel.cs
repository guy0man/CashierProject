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
    public class AddPriceModifierViewModel
    {
        public CashierContext _context { get;set; }
        public EditCompanyViewModel Parent { get; set; }
        public AddPriceModifierViewModel(CashierContext context, EditCompanyViewModel parent)
        {
            _context = context;
            Parent = parent;
        }
        public string Name { get; set; }
        public string Percentage { get; set; }
        public bool IsAdd { get; set; }
        public bool AutoApply { get; set; }
        public ObservableCollection<string> Errors { get; set; } = new();
        public void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var pm = new PriceModifier();
                pm.Name = Name;
                pm.Percentage = double.Parse(Percentage) / 100;
                pm.AutoApply = AutoApply;
                pm.IsAdd = IsAdd;
                try
                {
                    _context.Add(pm);
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    DialogResult = false;
                }
            }
            Parent.CheckAddPM();      
        }
        public void Cancel()
        {
            DialogResult = true;
            Parent.CheckAddPM();
        }
        public bool DialogResult { get; set; }
        public virtual bool Validate()
        {
            var validator = new AddPriceModifierValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
    }
    public class AddPriceModifierValidator : AbstractValidator<AddPriceModifierViewModel>
    {
        public AddPriceModifierValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Percentage).NotEmpty();
            RuleFor(c => c.Percentage).Must(c => int.TryParse(c, out var val)).WithMessage("Invalid Percentage");
        }
    }
}
