using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;

namespace api.Annotations.Validation
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        private readonly Type enumType;

        public ValidEnumValueAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new BadRequestException($"The given type is not an enum.");
            }

            this.enumType = enumType;
        }

        public override bool IsValid(object? value)
        {
            if (!IsValidEnumValue(value))
            {
                ErrorMessage = $"Invalid Enum Value: {value}";
                return false;
            }
            return true;
        }

        public bool IsValidEnumValue(object? value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().All(val => Enum.IsDefined(enumType, val));
            }
            else
            {
                return Enum.IsDefined(enumType, value);
            }
        }
    }
}