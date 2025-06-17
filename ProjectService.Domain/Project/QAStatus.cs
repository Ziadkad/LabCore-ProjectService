using System.Runtime.Serialization;

namespace ProjectService.Domain.Project;

public enum QAStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Approved")]
    Approved,
    [EnumMember(Value = "Rejected")]
    Rejected
}