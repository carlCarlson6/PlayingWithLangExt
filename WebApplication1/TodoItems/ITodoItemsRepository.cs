using LanguageExt;

namespace WebApplication1.TodoItems;

public interface ITodoItemsRepository
{
    ValueTask<Seq<TodoItem>> GetAllTodoItems(Unit _);
    ValueTask<Option<TodoItem>> GetTodoItem(Guid id);
    ValueTask Store(TodoItem item);
}

public class InMemoryTodoItems : ITodoItemsRepository
{
    public ValueTask<Seq<TodoItem>> GetAllTodoItems(Unit _)
    {
        if (new Random().Next() % 2 != 0)
        {
            throw new Exception("error with db");          
        }
        
        var item = new TodoItem(Guid.NewGuid(), "something that i have to do", Status.WIP);
        return new ValueTask<Seq<TodoItem>>(Prelude.Seq(new[] { item }));
    }

    public ValueTask<Option<TodoItem>> GetTodoItem(Guid id)
    {
        var item = new TodoItem(id, "something to do", Status.WIP);
        return (new Random().Next() % 3) switch
        {
            0 => new ValueTask<Option<TodoItem>>(Prelude.Some(item)),
            1 => new ValueTask<Option<TodoItem>>(Option<TodoItem>.None),
            2 => throw new Exception("error with db"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ValueTask Store(TodoItem item)
    {
        throw new NotImplementedException();
    }
}