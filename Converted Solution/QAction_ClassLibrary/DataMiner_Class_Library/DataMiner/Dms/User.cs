// ReSharper disable UsePatternMatching
namespace DataMiner.Dms
{
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("Interop.SLDms.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.DirectoryServices.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.DirectoryServices.AccountManagement.dll")]

    public class User
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class UserProperties
        {
            public string cn { get; set; }
            public string sn { get; set; }
            public string givenName { get; set; }
            public string displayName { get; set; }
            public string name { get; set; }
            public string employeeID { get; set; }
            public string sAMAccountName { get; set; }
            public string userPrincipalName { get; set; }
            public string mail { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class UserInfo
        {
            public string uID { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string telephoneNumber { get; set; }
            public string pager { get; set; }
            public string[] groups { get; set; }
        }

        /// <summary>
        /// Gets the account name of the user from DataMiner Agent
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns>The Account Name</returns>
        public static string GetAccountName(SLProtocol protocol)
        {
            var userCookie = protocol.UserCookie;
            var user = (string)protocol.NotifyDataMiner(108 /* NT_GET_USER */, userCookie, null);
            return !string.IsNullOrEmpty(user) ? user : string.Empty;
        }

        /// <summary>
        /// Gets the user information from the DataMiner Agent
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns>UserInfo object</returns>
        public static UserInfo GetInfo(SLProtocol protocol)
        {
            var accountName = GetAccountName(protocol);
            var userInformation = (object[])protocol.NotifyDataMiner(120 /*NT_GET_USER_INFO*/, accountName, null);
            var user = (string[])userInformation.ElementAtOrDefault(0);

            if (user == null) return null;

            var userInfo = new UserInfo
            {
                uID = user[0],
                fullName = user[1],
                email = user[2],
                telephoneNumber = user[3],
                pager = user[4]
            };

            var groupsCount = user.Length - 5;

            if (groupsCount <= 0) return userInfo;

            var groups = new string[groupsCount];

            for (var i = 5; i < user.Length; i++)
            {
                groups[i - 5] = user[i];
            }

            userInfo.groups = groups;

            return userInfo;
        }

        /// <summary>
        /// Gets the user properties from Sky BSkyB Active Directory
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns>UserProperties Object or null if not found</returns>
        public static UserProperties GetDirectoryProperties(SLProtocol protocol)
        {
            var accountName = GetAccountName(protocol);

            using (var domainContext = new PrincipalContext(ContextType.Domain, "broadcast.bskyb.com", "DC=broadcast,DC=bskyb,DC=com"))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, accountName))
                {
                    if (foundUser == null) return null;
                    try
                    {
                        var directoryEntry = foundUser.GetUnderlyingObject() as DirectoryEntry;
                        if (directoryEntry != null)
                        {
                            return new UserProperties
                            {
                                cn = directoryEntry.Properties["cn"].Value.ToString(),
                                sn = directoryEntry.Properties["sn"].Value.ToString(),
                                givenName = directoryEntry.Properties["givenName"].Value.ToString(),
                                displayName = directoryEntry.Properties["displayName"].Value.ToString(),
                                employeeID = directoryEntry.Properties["employeeID"].Value.ToString(),
                                sAMAccountName = directoryEntry.Properties["sAMAccountName"].Value.ToString(),
                                userPrincipalName = directoryEntry.Properties["userPrincipalName"].Value.ToString(),
                                mail = directoryEntry.Properties["mail"].Value.ToString()
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return null;
        }
    }
}