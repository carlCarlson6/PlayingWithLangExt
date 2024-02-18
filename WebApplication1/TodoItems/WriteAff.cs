using LanguageExt;
using LanguageExt.Common;
using static System.String;
using static LanguageExt.Aff<LanguageExt.Validation<LanguageExt.Common.Error,WebApplication1.TodoItems.TodoItem>>;
using static LanguageExt.Prelude;
using static LanguageExt.Validation<LanguageExt.Common.Error,WebApplication1.TodoItems.TodoItem>;

namespace WebApplication1.TodoItems;

public static class WriteAff
{
    public static Aff<Validation<Error, TodoItem>> AddNewTodoItem(ITodoItemsRepository repo, string itemDescription) =>
        ValidateInput(itemDescription)
            .Map(validDescription => new TodoItem(Guid.NewGuid(), validDescription, Status.NEW))
            .Match(
                item => Aff(async () =>
                {
                    await repo.Store(item);
                    return Success(item);
                }),
                errors => Success(Fail(errors)))
            .Bind(result => result.Match(
                todoItem => Aff(async () =>
                {
                    await PublishEvent(todoItem);
                    return Success(todoItem);
                }), 
                errors => Success(Fail(errors))));
    
    private static Validation<Error, string> ValidateInput(string itemDescription) =>
        !IsNullOrWhiteSpace(itemDescription)
            ? Validation<Error, string>.Success(itemDescription)
            : Validation<Error, string>.Fail(new Seq<Error> { "missing description" });

    private static ValueTask<Unit> PublishEvent(TodoItem item) => ValueTaskSucc(unit);
}
