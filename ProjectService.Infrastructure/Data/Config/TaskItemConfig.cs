using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Task;

namespace ProjectService.Infrastructure.Data.Config;

public class TaskItemConfig : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired();
        builder.Property(s => s.Label)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(s => s.Description)
            .IsRequired();
        builder.Property(s => s.StartDate)
            .IsRequired();
        builder.Property(s => s.EndDate)
            .IsRequired();
        builder.Property(s => s.Resources)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<long>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<long>());
        builder.Property(s => s.PathsFiles)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<string>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<string>());
        builder.Property(s => s.AssignedTo)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<Guid>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<Guid>());
        builder.Property(s => s.PredecessorTaskIds)
            .HasConversion(
                t=> JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<Guid>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<Guid>());
        builder.Property(s => s.StudyId)
            .IsRequired();


    }
}