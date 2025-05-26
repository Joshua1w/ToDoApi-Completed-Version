using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(int userId);
        Task<TaskItem> GetTaskByIdAsync(int id); // 👈 MATCH THIS
        Task<TaskItem> CreateTaskAsync(TaskCreateDto dto, int userId);
        Task<TaskItem> UpdateTaskAsync(int id, TaskUpdateDto dto, int userId); // 👈 MATCH THIS
        Task<bool> DeleteTaskAsync(int id, int userId);
    }
}
