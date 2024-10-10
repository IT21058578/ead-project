namespace api.Utilities
{
    /// <summary>
    /// The Criteria enum represents the different criteria that can be used for comparison.
    /// </summary>
    /// 
    /// <remarks>
    /// The Criteria enum is used to specify the criteria for comparison operations.
    /// It includes options for equality, inequality, greater than, greater than or equal to,
    /// less than, less than or equal to, and containment.
    /// </remarks>
    public enum Criteria
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
    }
}