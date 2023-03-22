namespace Ums.Models.Security
{
    public class FacebookTokenModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public FacebookUserPicture picture { get; set; }
    }
    public class FacebookUserPicture
    {
        public FBPicture data { get; set; }
    }
    public class FBPicture
    {
        public string url { get; set; }
    }
}
