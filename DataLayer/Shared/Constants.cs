using System;

namespace DataLayer
{
    public static class Constants
    {
        public const string EP_ValidateLogin = "User/ValidateLogin";
        public const string EP_Registration = "User/Registration";
        public const string EP_ValidateEmail = "User/ValidateEmail";
        public const string EP_VerifyCode = "User/VerifyCode";
        public const string EP_ResendCode = "User/ResendCode";
        public const string EP_ResetPassword = "User/ResetPassword";
        public const string EP_UserProfile = "User/GetUserProfile";
        public const string EP_UpdateProfile = "User/UpdateProfile";
        public const string EP_ChangePassword = "User/ChangePassword";
        public const string EP_RMAType = "RMAType";
        public const string EP_CreditReason = "CreditReason";
        public const string EP_GetReturnAddress = "ReturnAddress/GetReturnAddress";
        public const string EP_UpdateReturnAddress = "ReturnAddress/UpdateReturnAddress";
        public const string EP_States = "States";
        public const string EP_Countries = "Country";
        public const string EP_AddReturnAddress = "ReturnAddress/AddReturnAddress";
        public const string EP_Companies = "Company/CompanyName";
        public const string EP_Firms = "Firm/FirmName";
        public const string EP_CompanyAddresses = "Company/CompanyAddress";
        public const string EP_AddCompanyAddress = "Company/AddCompanyAddress";
        public const string EP_AddCompanyDetails = "Company/AddCompanyDetails";
        public const string EP_DispatchReason = "DispatchReason";
        public const string EP_Faults = "Faults";
        public const string EP_Parts = "Parts";
        public const string EP_PartDetails = "Parts/PartDetails";
        public const string EP_SubmitRMA = "RMA/SubmitRMARequest";
    }
}