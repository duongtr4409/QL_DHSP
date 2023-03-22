using System;

namespace Ums.Models.Task
{
    public class ResearchingModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int ConversionResearchingId { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; } = 1;
        public int Members { get; set; } = 1;
        public double WorkTime { get; set; } = 1;
        public string Desc { get; set; }
        public string Attach { get; set; }
        public int StartYear { get; set; } = DateTime.Now.Year - 4;
        public string[] Attaches { get; set; } = new string[0];
    }
}