using System;

namespace todo_cqrs.Domain
{
    public class TodoItemCompletedArgs : EventArgs
    {
        public TodoItemCompletedArgs(Int64 id, bool isCompleted)
        {
            Id = id;
            IsCompleted = isCompleted;
        }
        public Int64 Id { get; private set; }
        public bool IsCompleted { get; private set; }
    }
}