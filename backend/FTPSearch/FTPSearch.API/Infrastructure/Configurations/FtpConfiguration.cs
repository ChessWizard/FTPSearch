namespace FTPSearch.API.Infrastructure.Configurations;

public class FtpConfiguration
{
    public string Host { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public int Port { get; set; }

    public string BasePath { get; set; }
}