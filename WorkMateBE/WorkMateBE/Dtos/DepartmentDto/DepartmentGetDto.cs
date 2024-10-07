namespace WorkMateBE.Dtos.DepartmentDto
{
    public class DepartmentGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
    }
}
