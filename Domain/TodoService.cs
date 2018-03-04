using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace todo_cqrs.Domain
{
    public interface ITodoService
    {
        void CreateTodo(TodoItem item);
        void CompleteTodo(Int64 id);
        void DeleteTodo(Int64 id);
        IEnumerable<TodoItem> GetTodos();
    }
    public class TodoService : ITodoService
    {
        private readonly ILogger<TodoService> _logger;
        private TodoContext _ctx;
        public TodoService(TodoContext ctx, ILoggerFactory loggerFactory)
        {
            _ctx = ctx;
            _logger = loggerFactory.CreateLogger<TodoService>();
        }
        public void CreateTodo(TodoItem item)
        {
            _ctx.Todos.Add(item);
            _ctx.SaveChanges();
            _logger.LogInformation("Todo Item Create");
        }
        public void CompleteTodo(Int64 id)
        {
            var todo = _ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                throw new InvalidOperationException($"Todo Item with Id {id} does not exist.");
            todo.IsCompleted = true;
            _ctx.SaveChanges();
            _logger.LogInformation("Todo Item Completed");
        }
        public void DeleteTodo(Int64 id)
        {
            var todo = _ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                throw new InvalidOperationException($"Todo Item with Id {id} does not exist.");
            _ctx.Todos.Remove(todo);
            _ctx.SaveChanges();
            _logger.LogInformation("Todo Item Deleted");
        }
        public IEnumerable<TodoItem> GetTodos() => _ctx.Todos.ToList();
    }
}