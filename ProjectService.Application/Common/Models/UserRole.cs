using System.Runtime.Serialization;

namespace ProjectService.Application.Common.Models;

public enum UserRole
{
    None,
    
    [EnumMember(Value = "Researcher")]
    Researcher,
    
    [EnumMember(Value = "Manager")]
    Manager,

    [EnumMember(Value = "Admin")]
    Admin
}