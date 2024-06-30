﻿using WalksNZAPI.Models.Domain;

namespace WalksNZAPI.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }
        public RegionDTO Region { get; set; }
        public WalkDifficultyDTO walkDifficulty { get; set; } 
    }
}
