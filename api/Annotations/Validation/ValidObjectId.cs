
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace api.Annotations.Validation
{
    /// <summary>
    /// The ValidObjectIdAttribute class is a custom validation attribute that checks if a string value represents a valid ObjectId.
    /// </summary>
    /// 
    /// <remarks>
    /// The ValidObjectIdAttribute class is used to validate string values that are expected to represent a valid ObjectId.
    /// It inherits from the ValidationAttribute class and overrides the IsValid method to perform the validation logic.
    /// The IsValid method checks if the value is a string and if it matches the pattern of a valid ObjectId.
    /// If the value is not a string or does not match the pattern, the validation fails and an error message is set.
    /// </remarks>
    public class ValidObjectIdAttribute : ValidationAttribute
    {
        // This method is called by the IsValid method to check if the value is valid.
        public override bool IsValid(object? value)
        {
            if (value is not string objectId)
            {
                return false;
            }
            if (!IsValidObjectId(objectId))
            {
                ErrorMessage = $"Invalid ObjectId: {objectId}";
                return false;
            }
            return true;
        }

        //  This method checks if the value is a valid ObjectId.
        private static bool IsValidObjectId(string objectId)
        {
            return Regex.IsMatch(objectId, "^[0-9a-fA-F]{24}$");
        }
    }
}