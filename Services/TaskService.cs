using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;




namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(int userId)
        {
            Console.WriteLine("Tasks retrieved by servicce from: " + userId);
            var t = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
                Console.WriteLine("Tasks retrieved by servicce from: " + t.Count());
            return t;
        }
       public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id) 
                ?? throw new Exception("Task not found");
        }

        public async Task<TaskItem> CreateTaskAsync(TaskCreateDto dto, int userId)
        {
            Console.WriteLine("Task created by servicce from: " + userId);
            Console.WriteLine("Task Name: " + dto.Title);
            Console.WriteLine("Task Description: " + dto.Description);
            Console.WriteLine("Task Due Date: " + dto.DueDate);
            Console.WriteLine("Task Priority: " + dto.Priority);
            var task = new TaskItem
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority
            };

            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, TaskUpdateDto dto, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                throw new Exception("Task not found");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.DueDate = dto.DueDate;
            task.Priority = dto.Priority;

            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                throw new Exception("Task not found");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
