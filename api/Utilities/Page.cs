namespace api.Utilities
{
    /// <summary>
    /// The Page class represents a paginated collection of data.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of data contained in the page.</typeparam>
    /// 
    /// <remarks>
    /// The Page class is used to store a subset of data along with metadata for pagination.
    /// It contains properties for the metadata of the page and the collection of data.
    /// </remarks>
    public class Page<T>
    {
        public PageMetadata Meta { get; set; } = new PageMetadata();
        public IEnumerable<T> Data { get; set; } = [];

    }
}