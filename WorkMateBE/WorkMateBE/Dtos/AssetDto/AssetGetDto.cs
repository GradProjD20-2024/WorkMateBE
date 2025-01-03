﻿namespace WorkMateBE.Dtos.AssetDto
{
    public class AssetGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantiy { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public int Status { get; set; } //Assign To Employee ID
    }
}
