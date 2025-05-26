using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApi.Models;
using TodoApi.Interfaces;

namespace TodoApi.Pages.Task
{
    public class DeleteModel : PageModel
    {
        private readonly ITaskService _taskService;

        public DeleteModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = new TaskItem();

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

            var userId = 1; // Replace with actual user ID retrieval logic

            await _taskService.DeleteTaskAsync(TaskItem.Id, userId);

            return RedirectToPage("./Index");
        }
    }
}