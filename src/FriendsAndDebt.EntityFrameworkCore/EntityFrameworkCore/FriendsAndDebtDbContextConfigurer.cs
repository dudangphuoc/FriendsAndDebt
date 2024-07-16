using Abp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Data.Common;

namespace FriendsAndDebt.EntityFrameworkCore
{
    public static class FriendsAndDebtDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FriendsAndDebtDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString);
            var serverVersion = Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql");
            builder.UseMySql(connectionString, serverVersion);
        }

        public static void Configure(DbContextOptionsBuilder<FriendsAndDebtDbContext> builder, DbConnection connection)
        {
            //var serverVersion = ServerVersion.AutoDetect(connection.ConnectionString);
            var serverVersion = Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql");

            builder.UseMySql(connection, serverVersion);
        }

        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v));

            ValueComparer<T> comparer = new ValueComparer<T>(
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
                v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v)));

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }

        public static PropertyBuilder<string> HasValueGenerator<T>(this PropertyBuilder<string> propertyBuilder)
        {

            return propertyBuilder;
        }

        public static void ConfigureBaseService(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            //builder.HasSequence<long>("BoardCoderSequence").StartsAt(1);
            //builder.HasSequence<long>("CardCodeSequence").StartsAt(1);
            //builder.Entity<Board>(b =>
            //{
            //    b.Property(e => e.BoardCode)
            //    .HasDefaultValueSql("CONCAT('Board', FORMAT(NEXT VALUE FOR BoardCoderSequence, '000000000'))")
            //    .IsRequired()
            //    .HasMaxLength(64);
            //});
            //builder.Entity<Board>(b =>
            //{
            //    b.Property(e => e.BoardCode)
            //    .HasDefaultValueSql("CONCAT('Card', FORMAT(NEXT VALUE FOR CardCodeSequence, '000000000'))")
            //    .IsRequired()
            //    .HasMaxLength(64);
            //});
        }


    }
}
