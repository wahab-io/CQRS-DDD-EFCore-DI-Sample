using System;
using System.Collections.Generic;
using System.Linq;

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
        private TodoContext _ctx;
        public TodoService(TodoContext ctx)
        {
            _ctx = ctx;
        }
        public void CreateTodo(TodoItem item)
        {
            _ctx.Todos.Add(item);
            _ctx.SaveChanges();
        }
        public void CompleteTodo(Int64 id)
        {
            var todo = _ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                throw new InvalidOperationException($"Todo Item with Id {id} does not exist.");
            todo.IsCompleted = true;
            _ctx.SaveChanges();
        }
        public void DeleteTodo(Int64 id)
        {
            var todo = _ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                throw new InvalidOperationException($"Todo Item with Id {id} does not exist.");
            _ctx.Todos.Remove(todo);
            _ctx.SaveChanges();
        }
        public IEnumerable<TodoItem> GetTodos() => _ctx.Todos.ToList();
    }
}