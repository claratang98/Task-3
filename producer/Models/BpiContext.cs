using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace producer.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; } = null!;
    }
}
