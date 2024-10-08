namespace api.Configuratons
{
    /// <summary>
    /// The MailSettings class represents the configuration settings for email.
    /// </summary>
    /// 
    /// <remarks>
    /// The MailSettings class is used to store the host, port, username, and password
    /// for the email server. These settings are used to send emails from the application.
    /// </remarks>
    public class MailSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 0;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}