namespace WebApplication1.TodoItems;

public record TodoItem(Guid Id, string Description, Status Status);

public enum Status
{
    NEW,
    WIP,
    DONE
}

public record TodoItemsResponse<T>(T Data, string Error);


