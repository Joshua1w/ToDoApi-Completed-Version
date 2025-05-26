using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApi.Models;
using TodoApi.Dtos;
using TodoApi.Interfaces;

namespace TodoApi.Pages.Task
{
public class EditModel : PageModel
{
    private readonly ITaskService _taskService;

    public EditModel(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new TaskItem();
    public TaskUpdateDto? UpdateTaskItem { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        TaskItem = await _taskService.GetTaskByIdAsync(id);
        if (TaskItem == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var taskId = TaskItem.Id; // Replace with actual task ID retrieval logic
        var userId = 1; // Replace with actual user ID retrieval logic
        var updateTaskDto = new TaskUpdateDto
        {
            Title = TaskItem.Title,
            Description = TaskItem.Description,
            DueDate = TaskItem.DueDate,
            IsCompleted = TaskItem.IsCompleted
        };
        await _taskService.UpdateTaskAsync(taskId, updateTaskDto, userId);

        return RedirectToPage("./Index");
    }
}
}
