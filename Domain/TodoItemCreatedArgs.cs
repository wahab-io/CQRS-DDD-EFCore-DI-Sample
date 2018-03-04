using System;

namespace todo_cqrs.Domain 
{
    public class TodoItemCreatedArgs : EventArgs 
    {
        public TodoItemCreatedArgs(Int64 id, string title, bool isCompleted = false) 
        {
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
        }
        public Int64 Id { get; private set; }
        public string Title { get; private set; }
        public bool IsCompleted { get; private set; }
    }
}