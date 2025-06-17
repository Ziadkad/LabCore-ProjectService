using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.StudyResult;

namespace ProjectService.Infrastructure.Data.Config;

public class StudyResultConfig : IEntityTypeConfiguration<StudyResult>
{
    public void Configure(EntityTypeBuilder<StudyResult> builder)
    {
        builder.HasKey(s => s.Id);

        builder
            .HasOne(sr=>sr.Study)
            .WithOne(s=> s.StudyResult)
            .HasForeignKey<StudyResult>(sr => sr.StudyId);
        
        builder.Property(sr=>sr.Id)
            .IsRequired();
        builder.Property(sr=>sr.StudyId)
            .IsRequired();
        builder.Property(sr=>sr.Result)
            .IsRequired();
        builder.Property(s => s.PathsFiles)
            .HasConversion(
                t => JsonSerializer.Serialize(t, JsonSerializerOptions.Default),
                t => JsonSerializer.Deserialize<List<string>>(t, JsonSerializerOptions.Default)
            )
            .Metadata.SetValueComparer(ValueComparerHelper.CreateListComparer<string>());
    }
}