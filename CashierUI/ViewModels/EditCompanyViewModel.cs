using CashierDB;
using CashierUI.Dto;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class EditCompanyViewModel
    {
        public CashierContext _context { get; set; }
        public MainTabViewModel Parent { get; set; }
        public EditCompanyViewModel(CashierContext context, MainTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            CompanyName = parent.CompanyName;
            Address = parent.Address;
            PriceModifiers = parent.PriceModifiers;
        }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public ObservableCollection<PriceModifiersName> PriceModifiers { get; set; } = new();
        public ObservableCollection<string> Errors { get; set; } = new();
        public void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var company = _context.Company.First();
                company.Name = CompanyName;
                company.Address = Address;
                try
                {
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
            Parent.CheckEditComp();
        }
        public void Cancel()
        {
            DialogResult = true;
            Parent.CheckEditComp();
        }
        public bool DialogResult { get; set; }
        public void LoadPriceMods()
        {
            var priceMods = _context.PriceModifiers.Select(c => new PriceModifiersName(c.PriceModifierId, c.Name, c.Percentage, c.IsAdd)).ToList();
            PriceModifiers.Clear();
            foreach (var mod in priceMods) PriceModifiers.Add(mod);
        }
        public virtual bool Validate()
        {
            var validator = new EditCompanyValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
    }
    public class EditCompanyValidator : AbstractValidator<EditCompanyViewModel>
    {
        public EditCompanyValidator()
        {
            RuleFor(c => c.CompanyName).NotEmpty();
            RuleFor(c => c.Address).NotEmpty();         
        }
    }
}
