using System.Collections.Generic;

namespace TasksDAL
{
    public class Category
    {
        public Category()
        {
            TaskCategories = new HashSet<TaskCategory>();
            DoneTaskCategories = new HashSet<DoneTaskCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TaskCategory> TaskCategories { get; set; }
        public virtual ICollection<DoneTaskCategory> DoneTaskCategories { get; set; }
    }
}
