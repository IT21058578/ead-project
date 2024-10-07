using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Annotations.Validation
{
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