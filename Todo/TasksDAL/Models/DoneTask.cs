using System.Collections.Generic;

namespace TasksDAL
{
    public class DoneTask
    {
        public DoneTask()
        {
            DoneTaskCategories = new HashSet<DoneTaskCategory>();
        }

        public int Id { get; set; }

        public int OldId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DoneTaskCategory> DoneTaskCategories { get; set; }
    }
}
