using training.healthcareportal.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FormAuthenticationNew
{
    public class WebRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override void CreateRole(string roleName) { throw new NotImplementedException(); }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }

        public override string[] GetAllRoles() { throw new NotImplementedException(); }

        public override string[] GetRolesForUser(string username) 
        { 
            using (var context = new HealthCareDBContext2()) 
            { 
                var result = (from user in context.Users join role in context.Roles on user.User_ID equals role.RoleID where user.Username == username select role.RoleName).ToArray(); 
                return result; } }

        public override string[] GetUsersInRole(string roleName) { throw new NotImplementedException(); }

        public override bool IsUserInRole(string username, string roleName) { throw new NotImplementedException(); }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override bool RoleExists(string roleName) { throw new NotImplementedException(); }
    }
}

