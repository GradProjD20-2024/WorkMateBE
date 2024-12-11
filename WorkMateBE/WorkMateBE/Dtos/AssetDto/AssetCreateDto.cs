namespace WorkMateBE.Dtos.AssetDto
{
    public class AssetCreateDto
    {
        public string Name { get; set; }
        public int Quantiy { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}