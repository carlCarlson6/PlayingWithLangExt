using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using static WebApplication1.TodoItems.ReadAff;

namespace WebApplication1.TodoItems;

public class TodoItemsHandlers
{
    private readonly ITodoItemsRepository _repo;
    public TodoItemsHandlers(ITodoItemsRepository repo) => _repo = repo;

    public async Task<TodoItemsResponse<Seq<TodoItem>>> GetAll() => (await GetAllItemsAff(_repo).Run()).Match(
            seq => new TodoItemsResponse<Seq<TodoItem>>(seq, Errors.None.ToString()),
            error => new TodoItemsResponse<Seq<TodoItem>>(Seq<TodoItem>.Empty, error.ToException().Message));
    
    public async Task<TodoItemsResponse<TodoItem>> GetById(Guid id) => (await GetItemAff(_repo, id).Run())
        .Match(
            item => new TodoItemsResponse<TodoItem>(item, Errors.None.ToString()), 
            error => new TodoItemsResponse<TodoItem>(null!, error.ToException().Message));
    
    public async Task<ActionResult> FindById(Guid id) =>
        (await GetMaybeItemAff(_repo, id)
            .Map(option => option.Match<ActionResult>(
                item => new OkObjectResult(item),
                new NotFoundResult()))
            .Run())
        .Match(
            response => response,
            runtimeError => new ObjectResult(runtimeError.Message) { StatusCode = 500 });

    public async Task<ActionResult> AddNewItem(string description) =>
        (await WriteAff.AddNewTodoItem(_repo, description)
            .Map(validation => validation.Match<ActionResult>(
                item => new OkObjectResult(item),
                errors => new BadRequestObjectResult(string.Join('-', errors.Select(x => x.Message)))))
            .Run())
        .Match<ActionResult>(
            response => response,
            runtimeError => throw runtimeError);
}