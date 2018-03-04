﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using todo_cqrs.Core;
using todo_cqrs.Domain;

namespace todo_cqrs
{
    enum DatabaseType
    {
        InMemory,
        Sqlite,
        SqlServer
    }
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static EventStore _store;
        private static ITodoService _todoService;
        private static DatabaseType _dbType = DatabaseType.InMemory;
        static void Main(string[] args)
        {
            Console.WriteLine("Todo Application");
            Console.WriteLine("================");
            // Initialize IoC container
            var services = new ServiceCollection();

            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                Enum.TryParse<DatabaseType>(args[0], out _dbType);
                
            switch (_dbType)
            {
                case DatabaseType.InMemory:
                    Console.WriteLine("Database Type = InMemory\n");
                    services.AddDbContext<EventContext>(options => 
                        options.UseInMemoryDatabase("events"));
                    services.AddDbContext<TodoContext>(options => 
                        options.UseInMemoryDatabase("todos"));
                    break;
                case DatabaseType.Sqlite:
                    Console.WriteLine("Database Type = Sqlite\n");
                    services.AddDbContext<EventContext>(options => 
                        options.UseSqlite("Data Source=events.db"));
                    services.AddDbContext<TodoContext>(options => 
                        options.UseSqlite("Data Source=todos.db"));
                    break;
                default:
                    throw new NotImplementedException("Unsupported database type");
            }

            services.AddSingleton<ITodoService, TodoService>();
            _serviceProvider = services.BuildServiceProvider();


            var eventContext = _serviceProvider.GetService<EventContext>();
            _store = new EventStore(eventContext, TodoEventHandler);

            var todoContext = _serviceProvider.GetService<TodoContext>();
            _todoService = new TodoService(todoContext);

            /* Command */
            CreateTodoItem(1, "Pickup Milk");
            CreateTodoItem(2, "Pickup Bread");
            PrintTodos();
            MarkTodoItemCompleted(1, true);
            PrintTodos();
            DeleteTodoItem(1);
            PrintTodos();

            // PrintEvents();

            
        }
        private static void TodoEventHandler(object sender, EventArgs e)
        {
            if (e.GetType() == typeof(TodoItemCreatedArgs)) 
            {                
                var args = e as TodoItemCreatedArgs;
                if (args != null) {
                    _todoService.CreateTodo(new TodoItem(args.Id, args.Title, args.IsCompleted));
                    Console.WriteLine("Todo Item Created");
                }                
            }
            else if (e.GetType() == typeof(TodoItemCompletedArgs)) 
            {
                var args = e as TodoItemCompletedArgs;
                _todoService.CompleteTodo(args.Id);
                Console.WriteLine("Todo Item Completed");
            }
            else if (e.GetType() ==typeof(TodoItemDeletedArgs)) 
            {
                var args = e as TodoItemDeletedArgs;
                _todoService.DeleteTodo(args.Id);
                Console.WriteLine("Todo Item Deleted");
            }
            else 
            {
                Console.WriteLine("Todo EventHandler called");
            }
        }
        private static void CreateTodoItem(Int64 id, string title, bool isCompleted = false)
        {
            _store.AddEvent(new Event() { 
                Name = "ITEM-CREATED", 
                Data = new 
                { 
                    Id = id, 
                    Title = title, 
                    IsCompleted = isCompleted 
                }
            });
        }

        private static void MarkTodoItemCompleted(Int64 id, bool isCompleted)
        {
            _store.AddEvent(new Event() { 
                Name = "ITEM-COMPLETED", 
                Data = new 
                { 
                    Id = id, 
                    IsCompleted = isCompleted 
                }
            });
        }

        private static void DeleteTodoItem(Int64 id)
        {
            _store.AddEvent(new Event() { 
                Name = "ITEM-DELETED", 
                Data = new 
                { 
                    Id = id 
                }
            });
        }

        // IEnumerable because you can't change the list
        private static IEnumerable<TodoItem> GetTodoItems() => _todoService.GetTodos();

        private static void PrintTodos()
        {
            Console.WriteLine("\n==== TODO LIST ====");
            var todos = GetTodoItems();
            foreach (var todo in todos)
            {
                Console.WriteLine($"{todo.Id}\t{todo.Title}\t{todo.IsCompleted}");
            }
        }

        private static void PrintEvents()
        {
            Console.WriteLine("\n==== EVENTS LIST ====");
            var events = _store.GetAllEvents();
            foreach (var e in events)
            {
                Console.WriteLine($"{e.Name}\t{e.Data}");
            }
        }
    }
}