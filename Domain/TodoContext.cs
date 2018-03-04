using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace todo_cqrs.Domain
{
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<TodoContext>(options => 
                options.UseSqlite("Data Source=todos.db"));
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<TodoContext>();
        }
    }
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }
        
        public DbSet<TodoItem> Todos { get; set; }
    }

    public class TodoItem 
    {
        private bool _isCompleted;
        public TodoItem() { }
        public TodoItem(Int64 id, string title, bool isCompleted = false) 
        {
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
            CreatedUtc = DateTime.UtcNow;            
            if (isCompleted) CompletedUtc = CreatedUtc;
        }
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted 
        { 
            get { return _isCompleted; }
            set 
            {
                _isCompleted = value;
                CompletedUtc = _isCompleted ? DateTime.UtcNow : (DateTime?)null;
            }
        }
        public DateTime CreatedUtc { get; set; }
        public DateTime? CompletedUtc { get; set; }
    }
}