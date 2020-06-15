namespace TasksDAL
{
    public class DoneTaskCategory
    {
        public int DoneTaskId { get; set; }
        public DoneTask DoneTask { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
