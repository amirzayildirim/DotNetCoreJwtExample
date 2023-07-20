using System.ComponentModel.DataAnnotations;

namespace DotNetCoreJwtExample.Models;

public class GetTokenModel
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    [MinLength(10,ErrorMessage ="The Password's Min Length is 10")]
    public required string Password { get; set; }

}
