using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TeleTraderAssignment.Helpers
{
    public class EmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Field cannot be empty.");
        }
    }

}
