using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ASPODES.Common
{
    [AttributeUsage(AttributeTargets.Property |AttributeTargets.Field, AllowMultiple = false)]
    public class PositiveNumber : ValidationAttribute
    {
        /// <summary>
        /// 验证value的值是否为正数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValid(object value)
        {
            Double init = Double.Epsilon;
            if( Double.TryParse( value.ToString(), out init ))
            {
                return init >= Double.Epsilon;
            }
            return false;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Double init = Double.Epsilon;
            if( Double.TryParse( value.ToString(), out init ))
            {
                if( init >= Double.Epsilon ) return ValidationResult.Success;
            }
            List<string> members = new List<string>();
            members.Add(validationContext.MemberName);
            ValidationResult result = new ValidationResult( validationContext.MemberName + "值必须是正数", members);
            return result;
        }
    }
}
