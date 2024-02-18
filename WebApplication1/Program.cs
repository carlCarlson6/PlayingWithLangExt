using WebApplication1.TodoItems;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<ITodoItemsRepository, InMemoryTodoItems>()
    .AddSingleton<TodoItemsHandlers>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapTodoItemsEndpoints();

app.Run();