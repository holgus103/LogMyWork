using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DisallowEnumValueAttribute : ValidationAttribute
    {
        public Type EnumType;
        public SortedSet<object> DisallowedValues;
        public override bool IsValid(object value)
        {
            return this.DisallowedValues.Contains(value);
        }

        public DisallowEnumValueAttribute(Type type, SortedSet<object> disallowedValues)
        {
            this.EnumType = type;
            this.DisallowedValues = disallowedValues;

        }

    }
}
