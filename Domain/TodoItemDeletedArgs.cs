using System;

namespace todo_cqrs.Domain 
{
    public class TodoItemDeletedArgs : EventArgs
    {
        public TodoItemDeletedArgs(Int64 id)
        {
            Id = id;
        }
        public Int64 Id { get; private set; }
    }
}