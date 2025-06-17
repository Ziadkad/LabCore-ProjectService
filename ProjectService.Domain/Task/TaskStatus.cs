using System.Runtime.Serialization;

namespace ProjectService.Domain.Task;

public enum TaskStatus
{
    [EnumMember(Value = "Rejected")]
    NotStarted,
    [EnumMember(Value = "Rejected")]
    InProgress,
    [EnumMember(Value = "Rejected")]
    Completed,
}