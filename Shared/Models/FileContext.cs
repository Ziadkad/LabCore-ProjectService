using System.Runtime.Serialization;

namespace ProjectService.Application.Common.Models;

public enum FileContext
{
    [EnumMember(Value = "Project")]
    Project,
    
    [EnumMember(Value = "Study")]
    Study,
    
    [EnumMember(Value = "Task")]
    Task,
    
    [EnumMember(Value = "Result")]
    Result
}