using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.TodoItems;

public static class Endpoints
{
    public static void MapTodoItemsEndpoints(this WebApplication app)
    {
        app.MapGet(
            "/items", 
            (TodoItemsHandlers handlers) => handlers.GetAll());
        app.MapGet(
            "/items/{id:guid}", 
            (TodoItemsHandlers handlers, Guid id) => handlers.GetById(id));
        app.MapGet(
            "/items/find/{id:guid}", 
            (TodoItemsHandlers handlers, Guid id) => handlers.FindById(id));
        app.MapPost(
            "items", 
            (TodoItemsHandlers handlers, [FromBody] string description) => handlers.AddNewItem(description));
    }
}