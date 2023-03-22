using System.Diagnostics.CodeAnalysis;

namespace Ums.App.Constant
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Security
    {
        public const string CurrentUserKey = "Current_User_Key";
        public const string CurrentRoleKey = "Current_Role_Key";
        public const string CurrentFunctionKey = "Current_Permission_Key";
        public const string CHANGE_ROLE = "CHANGE-ROLE";
        public const string LOGIN_URL = "LOGIN-URL";
        public const string CurrentMainMenu = "CURRENT-MAIN-MENU";
    }

}