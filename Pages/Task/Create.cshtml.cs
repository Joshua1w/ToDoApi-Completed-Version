using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TodoApi.Models;
using TodoApi.Dtos;
using TodoApi.Interfaces;

namespace TodoApi.Pages;
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ITaskService _taskService;

        public CreateModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [BindProperty]
        public TaskCreateDto TaskItem { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Login");
            }

            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                username = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? "User";
            }
            ViewData["Username"] = username;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

                Console.WriteLine("Submitting...");

                if(TaskItem == null)
                {
                    Console.WriteLine("Task is null");
                    return Page();
                } else{

                Console.WriteLine("Task Name: " + TaskItem.Title);
                Console.WriteLine("Task Description: " + TaskItem.Description);
                Console.WriteLine("Task Due Date: " + TaskItem.DueDate);
                Console.WriteLine("Task Priority: " + TaskItem.Priority);
                }


            // Logging for debugging model binding
            Console.WriteLine($"[DEBUG] Title: {TaskItem.Title}, Description: {TaskItem.Description}, DueDate: {TaskItem.DueDate}, Priority: {TaskItem.Priority}");

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Login");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToPage("/Login");
            }

            await _taskService.CreateTaskAsync(TaskItem, userId);
            return RedirectToPage("./Index");
        }
    }

