using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShamgunovLinAR_KT_44_21.Database.Helpers;
using ShamgunovLinAR_KT_44_21.Models;

namespace ShamgunovLinAR_KT_44_21.Database.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        private const string TableName = "cd_сourse";
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            //Задаем первичный ключ
            builder
                .HasKey(p => p.CourseId)
                .HasName($"pk_{TableName}_сourse_id");
            //Для целочисленного первичного ключа задаем автогенерацию (к каждой новой записи будет добавлять +1)
            builder.Property(p => p.CourseId)
                    .ValueGeneratedOnAdd();
            //Расписываем как будут называться колонки в БД, а так же их обязательность и тд
            builder.Property(p => p.CourseId)
                .HasColumnName("сourse_id")
                .HasComment("Идентификатор записи группы");
            //HasComment добавит комментарий, который будет отображаться в СУБД (добавлять по желанию)
            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnName("c_сourse_title")
                .HasColumnType(ColumnType.String).HasMaxLength(100)
                .HasComment("Название предмета");
            builder.Property(p => p.GroupId)
                .IsRequired()
                .HasColumnName("c_group_id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Идентификатор группы");
            builder.ToTable(TableName)
                .HasOne(p => p.Group)
                .WithMany(t => t.Courses)
                .HasForeignKey(p => p.GroupId)
                .HasConstraintName("fk_c_group_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable(TableName)
                .HasIndex(p => p.GroupId, $"idx_{TableName}_fk_c_group_id");
            //Добавим явную автоподгрузку связанной сущности
            builder.Navigation(p => p.Group)
                .AutoInclude();
        }
    }
}
