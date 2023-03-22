using System.Collections.Generic;

namespace Ums.Models.Sync
{
    public class ResearchingSyncModel
    {
        public string Data { get; set; }
        public int YearId { get; set; }
    }
    public class ResearchingSync
    {
        public List<ResearchingSyncTask> Data { get; set; } = new List<ResearchingSyncTask>();
    }

    public class ResearchingSyncTask
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int StaffId { get; set; }
        public int ConversionId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Members { get; set; }
        public int StartYear { get; set; }
        public string[] Attach { get; set; }
        public string Desc { get; set; }
        public int WorkTime { get; set; }
        public string Conversion { get; set; }
        public string Staff { get; set; }
        public bool IsOk => ConversionId > 0 && !string.IsNullOrEmpty(Name) && Quantity > 0 && Members > 0;
        public string Message
        {
            get
            {
                var m = "";
                if (YearId == 0)
                {
                    m += "[Chưa có ID năm học]<br/>";
                }
                if (ConversionId == 0)
                {
                    m += "[Chưa có ID quy đổi nhiệm vụ]<br/>";
                }
                if (string.IsNullOrEmpty(Name))
                {
                    m += "[Chưa có tên nhiệm vụ]<br/>";
                }
                if (Quantity == 0)
                {
                    m += "[Chưa có số lượng]<br/>";
                }
                if (Members == 0)
                {
                    m += "[Chưa có số lượng thành viên]<br/>";
                }
                return m;
            }
        }
    }
}
