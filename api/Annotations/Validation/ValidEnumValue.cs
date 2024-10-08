using System.Collections;
using System.ComponentModel.DataAnnotations;
using api.Exceptions;

namespace api.Annotations.Validation
{
    /// <summary>
    /// The ValidEnumValueAttribute class represents a validation attribute that checks if a value is a valid enum value.
    /// </summary>
    /// 
    /// <remarks>
    /// The ValidEnumValueAttribute class is used to validate if a value is a valid enum value.
    /// It can be applied to properties or fields to ensure that the value assigned is a valid enum value.
    /// The attribute takes the enum type as a parameter in its constructor.
    /// The IsValid method is called to check if the value is valid, and it returns true if the value is a valid enum value.
    /// The IsValidEnumValue method is used to check if the value is a valid enum value.
    /// If the value is not a valid enum value, an error message is set and false is returned.
    /// </remarks>
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