using System;

namespace todo_cqrs.Core
{
    public abstract class Entity
    {
        private Entity() 
        {

        }

        public Entity(Int64 id) 
        {
            Id = id;
        }
        
        public Int64 Id { get; }
    }
}