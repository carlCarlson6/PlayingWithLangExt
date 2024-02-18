using LanguageExt;
using static LanguageExt.Prelude;

namespace WebApplication1.TodoItems;

public static class ReadAff
{
    public static Aff<Seq<TodoItem>> GetAllItemsAff(ITodoItemsRepository repo) =>
        Aff(async () => await repo.GetAllTodoItems(unit));

    public static Aff<TodoItem> GetItemAff(ITodoItemsRepository repo, Guid itemId) => 
        Aff(async () => await repo.GetTodoItem(itemId))
            // converts the none result of the option into a runtime error
            .Map(option => option.Match(
                some => some, 
                () => throw new Exception($"{itemId} item not found - sorry ):")));

    // we will need to double match - to check if there is a runtime error and for the option
    public static Aff<Option<TodoItem>> GetMaybeItemAff(ITodoItemsRepository repo, Guid itemId) => 
        Aff(async () => await repo.GetTodoItem(itemId));
}