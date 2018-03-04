using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using todo_cqrs.Domain;

namespace todo_cqrs.Core
{
    public class EventStore {
        private EventContext _ctx;
        private event EventHandler OnEventAdded;
        public EventStore(EventContext ctx, EventHandler handler) {
            _ctx = ctx;
            OnEventAdded += handler;
        }

        public void AddEvent(Event @event) 
        {
            var data = @event.Data;
            Int64 id = data.Id;
            switch(@event.Name) 
            {
                case "ITEM-CREATED":
                    string title = data.Title;
                    bool isCompleted1 = data.IsCompleted;
                    OnEventAdded.Invoke(this, 
                        new TodoItemCreatedArgs(id, title, isCompleted1));
                    break;
                case "ITEM-COMPLETED":
                    bool isCompleted2 = data.IsCompleted;
                    OnEventAdded.Invoke(this, 
                        new TodoItemCompletedArgs(id, isCompleted2));
                    break;
                case "ITEM-DELETED":
                    OnEventAdded.Invoke(this, 
                        new TodoItemDeletedArgs(id));
                    break;
                default:
                    OnEventAdded.Invoke(this, new EventArgs());
                    break;
            }
            _ctx.Events.Add(@event);
            _ctx.SaveChanges();
        }

        public IList<Event> GetAllEvents() => _ctx.Events.ToList();      
    }
}

