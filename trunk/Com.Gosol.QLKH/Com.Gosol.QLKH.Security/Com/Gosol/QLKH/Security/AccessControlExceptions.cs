namespace Com.Gosol.QLKH.Security
{
    using System;

    internal class AccessControlExceptions : DatabaseProxyException
    {
        public AccessControlExceptions(string errorMessage) : base(errorMessage)
        {
        }
    }
}
