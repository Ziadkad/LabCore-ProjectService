using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProjectService.Infrastructure.Data.Config;

public class ValueComparerHelper
{
    public static ValueComparer<List<T>> CreateListComparer<T>()
    {
        return new ValueComparer<List<T>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v != null ? v.GetHashCode() : 0)),
            c => c == null ? new List<T>() : c.ToList()
        );
    }
}