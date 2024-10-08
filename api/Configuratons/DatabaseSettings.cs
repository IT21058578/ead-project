namespace api.Configurations
{
    /// <summary>
    /// The DatabaseSettings class represents the configuration settings for the database.
    /// </summary>
    /// 
    /// <remarks>
    /// The DatabaseSettings class is used to store the connection string and database name
    /// for the database configuration. These settings are used to establish a connection
    /// to the database and specify the target database for the application.
    /// </remarks>
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
    }
}