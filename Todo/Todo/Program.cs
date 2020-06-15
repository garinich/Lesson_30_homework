using System;
using System.Collections.Generic;
using System.Linq;
using TasksDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Todo
{
    // CREATE TRIGGER Task_Is_Done
    // ON tasks
    // AFTER INSERT, UPDATE
    // AS
    // INSERT INTO donetasks (OldId, name)
    // SELECT Id, name
    // FROM inserted
    // WHERE inserted.isdone = true


    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = new TasksContext(GetDbOptions()))
            {
                CreateInitialData(ctx);
                ShowTasksWithCategories(ctx);
                MoveTaskToDoneById(ctx, 3);
                ShowDoneTasksWithCategories(ctx);
            }
        }

        public static DbContextOptions<TasksContext> GetDbOptions()
        {
            // DB configuration

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return new DbContextOptionsBuilder<TasksContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }

        public static void CreateInitialData(TasksContext ctx)
        {
            Task task1 = new Task {Name = "task1"};
            Task task2 = new Task {Name = "task2"};
            Task task3 = new Task {Name = "task3"};
            ctx.Tasks.AddRange(new HashSet<Task> {task1, task2, task3});

            Category c1 = new Category {Name = "Category 1"};
            Category c2 = new Category {Name = "Category 2"};
            Category c3 = new Category {Name = "Category 3"};
            ctx.Categories.AddRange(new HashSet<Category> {c1, c2, c3});

            ctx.SaveChanges();

            task1.TaskCategories.Add(new TaskCategory {CategoryId = c1.Id, TaskId = task1.Id});
            task2.TaskCategories.Add(new TaskCategory {CategoryId = c1.Id, TaskId = task2.Id});
            task2.TaskCategories.Add(new TaskCategory {CategoryId = c2.Id, TaskId = task2.Id});
            task3.TaskCategories.Add(new TaskCategory {CategoryId = c2.Id, TaskId = task3.Id});
            task3.TaskCategories.Add(new TaskCategory {CategoryId = c3.Id, TaskId = task3.Id});
            ctx.SaveChanges();
        }

        public static void ShowTasksWithCategories(TasksContext ctx)
        {
            var tasks = ctx.Tasks.Include(c => c.TaskCategories).ThenInclude(sc => sc.Category).ToList();

            foreach (var task in tasks)
            {
                Console.WriteLine($"\n Task: {task.Name}");
                var categories = task.TaskCategories.Select(sc => sc.Category).ToList();
                foreach (var category in categories)
                    Console.WriteLine($"--- Category: {category.Name}");
            }
        }

        public static void ShowDoneTasksWithCategories(TasksContext ctx)
        {
            var doneTasks = ctx.DoneTasks.Include(c => c.DoneTaskCategories).ThenInclude(sc => sc.DoneTask).ToList();

            foreach (var task in doneTasks)
            {
                Console.WriteLine($"\n Done Task: {task.Name}");
                var categories = task.DoneTaskCategories.Select(sc => sc.Category).ToList();
                foreach (var category in categories)
                    Console.WriteLine($"--- Category: {category.Name}");
            }
        }

        public static void MoveTaskToDoneById(TasksContext ctx, int id)
        {
            Task taskGoDone = ctx.Tasks.Include(c => c.TaskCategories).FirstOrDefault(t => t.Id == id);
            taskGoDone.IsDone = true;
            DoneTask doneTask = new DoneTask {Name = taskGoDone.Name, OldId = taskGoDone.Id};

            foreach (var taskCategory in taskGoDone.TaskCategories)
            {
                doneTask.DoneTaskCategories.Add(new DoneTaskCategory {CategoryId = taskCategory.CategoryId, DoneTaskId = doneTask.Id});
            }

            ctx.DoneTasks.Add(doneTask);

            ctx.Tasks.Update(taskGoDone);
            ctx.SaveChanges();

            Console.WriteLine($"\n Change status task with Id = {id} to done!");
        }
    }
}
