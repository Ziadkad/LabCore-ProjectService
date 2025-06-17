using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Project;

namespace ProjectService.Infrastructure.Data.Config;

public class ProjectConfig: IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder
            .HasMany(p => p.Studies)
            .WithOne(s=> s.Project)
            .HasForeignKey(s => s.ProjectId);
        
        builder.Property(p => p.Id)
            .IsRequired();
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(p => p.Description)
            .IsRequired();
        builder.Property(p => p.StartDate)
            .IsRequired();
        builder.Property(p => p.EndDate)
            .IsRequired();
        builder.Property(p => p.IsPublic)
            .IsRequired();
        builder.Property(p => p.Status)
            .IsRequired();
        builder.Property(p => p.ProgressPercentage)
            .IsRequired();
        builder.Property(p => p.Tags)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<string>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<string>());
        builder.Property(p => p.PathsFiles)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<string>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<string>());
        builder.Property(p => p.Researchers)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<Guid>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<Guid>());
        builder.Property(p=>p.ManagerId)
            .IsRequired();
    }
}