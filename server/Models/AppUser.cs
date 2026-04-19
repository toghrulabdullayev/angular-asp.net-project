using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
  public class AppUser : IdentityUser
  {
    [PersonalData]
    [Column(TypeName = "nvarchar(150)")]
    public string? FullName { get; set; }
  }
}