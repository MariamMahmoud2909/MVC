using MVC_3DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_3DAL.Data.Configurations
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.ID);
            builder.Property(D => D.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            builder.Property(D => D.Code).IsRequired().HasColumnType("varchar").HasMaxLength(50);
        }
    }
}
