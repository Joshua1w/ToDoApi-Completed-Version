using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Dtos;
using TodoApi.Interfaces;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetTasksAsync(GetUserId());
            Console.WriteLine("Tasks retrieved: " + tasks.Count());
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto, GetUserId());
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            var task = await _taskService.UpdateTaskAsync(id, dto, GetUserId());
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id, GetUserId());
            return Ok(new { success = result });
        }
    }
}
