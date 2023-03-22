namespace Ums.Core.Entities.System
{
    public class DbStat
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public long Rows { get; set; }
        public long TotalSpaceKB { get; set; }
        public decimal TotalSpaceMB { get; set; }
        public long UsedSpaceKB { get; set; }
        public decimal UsedSpaceMB { get; set; }
        public long UnusedSpaceKB { get; set; }
        public decimal UnusedSpaceMB { get; set; }
    }
}
