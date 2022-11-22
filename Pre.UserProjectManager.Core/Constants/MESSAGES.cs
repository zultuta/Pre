namespace Pre.UserProjectManager.Core.Constants
{
    public sealed class MESSAGES
    {
        public MESSAGES() {}

        public const string INVALID_LOGIN = "Login failed. Invalid username or password.";
        public const string LOGIN_SUCCESS = "Login successful.";
        public const string GENERAL_FAILURE = "Problem encountered processing your request.";
        public const string INVALID_USERNAME = "Username not available. Please use a different username.";
        public const string USER_ADDED = "Account created successfully.";
        public const string PROJECT_ADDED = "New user project added successfully.";
        public const string INVALID_PROJECT_NAME = "You alreade have a project with this name. Please use a different Name.";
        public const string INVALID_PROJECT_ASSIGNMENT = "You can only assign user to a project you own.";
        public const string INVALID_PROJECT_UNASSIGNMENT = "You can only unassign user from a project you own.";
        public const string INVALID_ASSIGNEE = "You cannot assign your project to a user that does not exist.";
        public const string DUPLICATE_ASSIGNMENT = "You have alreade assigned this user to this project.";
        public const string NON_EXISTING_ASSIGNMENT = "This user was never assigned to this project.";
        public const string PROJECT_ASSIGNED = "User assigned to project successfully.";
        public const string PROJECT_UNASSIGNED = "User unassigned from project successfully.";
        public const string PRODUCT_ADDED = "New product added successfully.";
        public const string INVALID_UNIT = "The unit specified is inavlid. Valid units are (g, kg, t).";
        public const string INVALID_PRODUCT_ADDITION = "You can only add products to a project you own.";
        public const string INVALID_PROJECT_DELETE = "You can only delete a project you own.";
        public const string PROJECT_DELETED = "Project deleted successfully.";
        public const string INVALID_PROJECT_UPDATE = "You can only edit a project you own.";
        public const string PROJECT_UPDATED = "Project updated successfully.";
        public const string USER_UPDATED = "User details updated successfully.";
        public const string SUCCESS = "Request successfull.";
        public const string USER_DELETED = "User deleted successfully.";
        public const string PROJECT_NOT_FOUND = "Project not found";
        public const string INVALID_CARBON_FILTER = "MaxCarbonFootPrint cannot be less that MinCarbonFootPrint";
        public const string INVALID_DATE_ASSIGNED_FILTER = "MaxDateAssigned cannot be less that MinDateAssigned";

    }
}
