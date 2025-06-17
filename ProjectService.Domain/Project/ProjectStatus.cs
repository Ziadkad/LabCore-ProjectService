using System.Runtime.Serialization;

namespace ProjectService.Domain.Project;

public enum ProjectStatus
{
    [EnumMember(Value = "Draft")]
    Draft,
    [EnumMember(Value = "Active")]
    Active,
    [EnumMember(Value = "Completed")]
    Completed
}