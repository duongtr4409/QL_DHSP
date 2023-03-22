namespace Ums.Models.Connect
{
    public class NoticeModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public string Name { get; set; } 
        public string Content { get; set; }
        public bool Public { get; set; }
        public int[] RoleIds { get; set; } = new int[0];
    }
}