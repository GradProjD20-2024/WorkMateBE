namespace WorkMateBE.Dtos.AssetDto
{
    public class AssetCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Status { get; set; }
        public int? EmployeeId { get; set; } //Assign To Employee ID
    }
}
