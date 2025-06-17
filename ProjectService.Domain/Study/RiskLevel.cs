using System.Runtime.Serialization;

namespace ProjectService.Domain.Study;

public enum RiskLevel
{
    [EnumMember(Value = "Low")]
    Low,
    [EnumMember(Value = "Medium")]
    Medium,
    [EnumMember(Value = "High")]
    High
}