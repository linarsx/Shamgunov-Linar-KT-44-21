﻿using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ShamgunovLinAR_KT_44_21.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        [JsonIgnore]
        public List<Student>? Students { get; set; }
        [JsonIgnore]
        public List<Course>? Courses { get; set; }
        public bool IsValidGroupName()
        {
            return Regex.IsMatch(GroupName, @"^[A-Za-z]{2}-\d{2}-\d{2}$");
        }
    }
}
