using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Data.Configuration.Entities;

public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Code)
            .HasColumnName("code")
            .HasMaxLength(36)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime")
            .HasColumnName("updated_at");
    }
    
    protected static long DateTimeToUnixSeconds(DateTime dateTime)
    {
        DateTime baseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        
        return (long) dateTime.Subtract(baseDateTime).TotalSeconds;
    }

    protected static DateTime UnixSecondsToDateTime(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        
        return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
    }
}