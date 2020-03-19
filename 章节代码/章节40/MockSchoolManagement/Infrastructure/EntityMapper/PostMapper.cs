using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockSchoolManagement.Models.BlogManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Infrastructure.EntityMapper
{
    public class PostMapper : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Blog与Post之间为 1 - 多关联关系
            builder.HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Post");
            //设置属性Title最大长度70，列名在数据库显示为BlogTitle

            builder.Property(a => a.Title).HasMaxLength(50).HasColumnName("Title");
            //设置属性PostId，列名在数据库显示为Id

            builder.Property(t => t.PostId).HasColumnName("Id");
        }
    }
}