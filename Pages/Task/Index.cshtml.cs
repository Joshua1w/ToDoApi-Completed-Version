using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApi.Models;
using TodoApi.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TodoApi.Pages.Task
{
    public class IndexModel : PageModel
    {
        private readonly ITaskService _taskService;

        public IndexModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public List<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                TaskItems = (await _taskService.GetTasksAsync(userId)).ToList();
            }
        }
    }
}