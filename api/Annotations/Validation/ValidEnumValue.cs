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

        //  This method is called by the IsValid method to check if the value is valid.
        public override bool IsValid(object? value)
        {
            if (!IsValidEnumValue(value))
            {
                ErrorMessage = $"Invalid Enum Value: {value}";
                return false;
            }
            return true;
        }

        //  This method checks if the value is a valid enum value.
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