using System.ComponentModel.DataAnnotations;

namespace Blocks.Messaging;

public class RabbitMqOptions
{
    [Required]
    public string Host { get; set; } = "localhost";
    [Required]
    public string UserName { get; set; } = "guest";
    [Required]
    public string Password { get; set; } = "guest";
    [Required]
    public string VirtualHost { get; set; } = "/";
}
