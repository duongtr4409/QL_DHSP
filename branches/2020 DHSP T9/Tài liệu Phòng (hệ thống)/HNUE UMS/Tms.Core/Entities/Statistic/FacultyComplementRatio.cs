namespace Ums.Core.Entities.Statistic
{
    public class FacultyComplementRatio
    {
        public string Name { get; set; }
        public int Stave { get; set; }
        public double TeachingDuty { get; set; }
        public double TeachingDone { get; set; }
        public double TeachingOver => TeachingDone > TeachingDuty ? TeachingDone - TeachingDuty : 0;
        public double TeachingPersonOver { get; set; }
        public double TeachingRatio => TeachingOver / TeachingPersonOver;
        public double FacultyDuty { get; set; }
        public double FacultyDone { get; set; }
        public double FacultyOver => FacultyDone > FacultyDuty ? FacultyDone - FacultyDuty : 0;
        public double PersonOver { get; set; }
        public double Ratio => FacultyOver / PersonOver;
    }
}