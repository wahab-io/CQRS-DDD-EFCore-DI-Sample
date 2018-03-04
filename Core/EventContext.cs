using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace todo_cqrs.Core
{
    public class EventContextFactory : IDesignTimeDbContextFactory<EventContext>
    {
        public EventContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<EventContext>(options => 
                options.UseSqlite("Data Source=events.db"));
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<EventContext>();
        }
    }
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e._Data).HasColumnName("Data");
        }
        public DbSet<Event> Events { get; set; }
    }
    public class Event
    {

        [Column("Id")]
        public Int64 EventId { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }

        internal string _Data { get; set; }
        
        [NotMapped]
        public dynamic Data 
        { 
            get { return _Data == null ? null : JsonConvert.DeserializeObject<dynamic>(_Data); }
            set { _Data = JsonConvert.SerializeObject(value); }
        }
    }
}