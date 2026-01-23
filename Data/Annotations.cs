using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DxBlazorApplication7.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinPasswordLengthAttribute : ValidationAttribute
    {
        int MinLength { get; }
        public MinPasswordLengthAttribute(int minLength, string errorMsg) : base(errorMsg)
        {
            MinLength = minLength;
        }

        public override bool IsValid(object value)
        {
            return ((string)value).Length >= MinLength;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Regex.IsMatch((string)value, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                             + "@"
                                              + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        }
    }
}
