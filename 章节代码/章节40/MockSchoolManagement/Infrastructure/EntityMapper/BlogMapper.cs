using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockSchoolManagement.Models.BlogManagement;
using MockSchoolManagement.Models.Blogs;

namespace MockSchoolManagement.Infrastructure.EntityMapper
{
    public class BlogMapper : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blog");
            // 主键
            builder.HasKey(t => t.Id);
            builder.Property(a => a.Title).HasMaxLength(70).HasColumnName("BlogTitle");

            // builder.HasOne(a => a.blogImage).WithOne(a => a.Blog).HasForeignKey<BlogImage>(b => b.BlogImageId);
        }
    }
}