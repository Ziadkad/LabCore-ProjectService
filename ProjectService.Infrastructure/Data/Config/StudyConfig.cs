using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Study;

namespace ProjectService.Infrastructure.Data.Config;

public class StudyConfig : IEntityTypeConfiguration<Study>
{
    public void Configure(EntityTypeBuilder<Study> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder
            .HasMany(s=>s.TaskItems)
            .WithOne(s=> s.Study)
            .HasForeignKey(s => s.StudyId);

        builder.Property(s => s.Id)
            .IsRequired();
        builder.Property(s => s.Title)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(s => s.Objective)
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
        builder.Property(s => s.ProjectId)
            .IsRequired();
    }
}