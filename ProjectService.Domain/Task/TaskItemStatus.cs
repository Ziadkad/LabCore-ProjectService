using System.Runtime.Serialization;

namespace ProjectService.Domain.Task;

public enum TaskItemStatus
{
    [EnumMember(Value = "NotStarted")]
    NotStarted,
    [EnumMember(Value = "InProgress")]
    InProgress,
    [EnumMember(Value = "Completed")]
    Completed,
}