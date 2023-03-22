namespace Ums.Core.Entities.Shared
{
    public class StringPair
    {
        public StringPair(string key, string value, string desc = "")
        {
            Value = value;
            Key = key;
            Desc = desc;
        }
        public string Value { get; set; }
        public string Key { get; set; }
        public string Desc { get; set; }
    }
}
