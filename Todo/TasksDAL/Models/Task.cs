using System.Collections.Generic;

namespace TasksDAL
{
    public class Task
    {
        public Task()
        {
            TaskCategories = new HashSet<TaskCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }

        public virtual ICollection<TaskCategory> TaskCategories { get; set; }
    }
}
