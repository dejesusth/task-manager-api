using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task_manager.Data;
using task_manager.Models;
using task_manager.DTOs;

namespace task_manager.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdString!);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskCreateDTO request)
    {
        var userId = GetUserId();

        var newTask = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            UserId = userId
        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        return Ok(newTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyTasks()
    {
        var userId = GetUserId(); 
        
        var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        
        return Ok(tasks);
    }

    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> ToggleCompleteTask(int id)
    {
        var userId = GetUserId();
        
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null)
            return NotFound("Tarefa não encontrada ou não pertence a você.");

        task.IsCompleted = !task.IsCompleted; 
        
        await _context.SaveChangesAsync();

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null)
            return NotFound("Tarefa não encontrada ou não pertence a você.");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok(new { Mensagem = "Tarefa deletada com sucesso!" });
    }
}