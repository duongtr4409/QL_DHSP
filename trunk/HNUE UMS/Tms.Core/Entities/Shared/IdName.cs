namespace Ums.Core.Entities.Shared
{
    public class IdName
    {
        public IdName()
        {

        }
        public IdName(int id, string name, string note = "")
        {
            Id = id;
            Note = note;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}