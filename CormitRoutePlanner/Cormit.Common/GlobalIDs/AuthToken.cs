using System;

// To generate a |-separated list of name|guid lines, use this regex string on this file:
// (?m-s:public.*Guid\s+(?<name>\w+)[^"]+"(?<guid>[^"]+))
// Use TextTools, paste regex into Param field, paste file contents into left box and
// go Edit>Extract (Ctrl-Shift-E).

public static class AuthToken
{
	public static readonly Guid GetMessage
		= new Guid("4DDA7120-D1FE-4B2B-80F8-351B3C60FFA4");

	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\Configuration.cs
	public static readonly Guid GetConfigValue
		= new Guid("3B8BFD51-91BC-4b9d-A2C2-460A311ACE75");

	public static readonly Guid GetConfigurationByUID
		= new Guid("305D9809-DEF3-401D-B6B9-7D25E2E6826E");

	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\Configuration.cs
	public static readonly Guid SaveConfigurationByUID
		= new Guid("8D96C798-DE11-49b3-A358-2189AAD89F32");

	// Imarda360SolutionManageir\Imarda360Solution\Control\SearchManagement\Configuration.cs
	public static readonly Guid UpdateConfigurationByUID
		= new Guid("A9DDFE3E-6FAB-4b28-A660-7EE44729A314");

	public static readonly Guid SetTempPreferredZone
		= new Guid("1B92E55F-0D96-41d2-B9AE-B653890E3471");


	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\EntitySearch.cs

	public static readonly Guid GetTrackableDriverListBySearch
		= new Guid("6258ee25-437e-46a2-a0b6-6a5dc14cb2d1");

	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\EntitySearch.cs

	public static readonly Guid GetUnitPositionByFleetID
		= new Guid("a0d65b55-27ca-46f4-bd63-10dabd8db964");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid GetCompany
		= new Guid("2b136763-9a93-4f45-840f-c0c8a0ab77ac");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid GetCompanyUpdateCount
		= new Guid("3283f30c-dc65-468d-99d0-731c17a4e8c3");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid GetCompanyListByTimeStamp
		= new Guid("7259fd11-6eff-4c53-9bef-4f2573a88555");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid GetCompanyList
		= new Guid("e42f23cb-4687-42dd-9f10-a3ffe01c3f11");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid SaveCompany
		= new Guid("f3edab7b-ccd1-4675-bb52-032004b75e04");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid SaveCompanyList
		= new Guid("18dea2ec-3bc6-476c-b362-4f65e25ae899");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Company\Company.cs

	public static readonly Guid DeleteCompany
		= new Guid("543303bf-c315-40d1-9cfe-6ba680e64cb0");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid GetCompanyLocation
		= new Guid("96634382-d54b-4563-a7b4-5b6ac28679c3");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid GetCompanyLocationUpdateCount
		= new Guid("ce429e69-a1da-419c-82b3-c29e15dc7d4a");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid GetCompanyLocationListByTimeStamp
		= new Guid("e964d820-1cd3-4d31-bb97-e4b1cd363e01");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid GetCompanyLocationList
		= new Guid("fe6896f6-47e8-4b43-8112-d0c87e36a56c");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid SaveCompanyLocation
		= new Guid("b41c7eba-b678-451d-ad5c-814df113997e");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid SaveCompanyLocationList
		= new Guid("6177aa4f-ac1f-46c5-b328-7f6accadbb15");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyLocation\CompanyLocation.cs

	public static readonly Guid DeleteCompanyLocation
		= new Guid("05c56743-da1c-47c0-a17f-b6824dbdd959");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid GetCompanyModule
		= new Guid("2b93c87c-7ebf-41f9-af13-6ce730b7a4e3");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid GetCompanyModuleUpdateCount
		= new Guid("fd5b6c5b-0dc4-4101-b20a-fae46c25a5c7");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid GetCompanyModuleListByTimeStamp
		= new Guid("b90f1a0c-3dff-45fc-9e2b-e6feb87a3788");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid GetCompanyModuleList
		= new Guid("850a5897-03a4-4dd9-a826-e413b150541e");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid SaveCompanyModule
		= new Guid("adef82d8-b422-4624-a6d8-5aaf8d2b8790");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid SaveCompanyModuleList
		= new Guid("772d3b62-84bf-45ff-8e49-b4527d7f3483");

	// Imarda360CRM\Imarda360CRMBusiness\Control\CompanyModule\CompanyModule.cs

	public static readonly Guid DeleteCompanyModule
		= new Guid("fe8745ea-55d3-4640-80eb-5edf06fab707");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid GetEmailGroup
		= new Guid("6357c4eb-e38c-4f93-817f-93550eb0e0c6");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid GetEmailGroupUpdateCount
		= new Guid("8d16cc11-2812-4403-805a-3d8df9d032bf");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid GetEmailGroupListByTimeStamp
		= new Guid("64f24ce1-03f3-400e-abb7-bac208ff8d04");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid GetEmailGroupList
		= new Guid("a31ba5a9-9f89-4e71-a786-d7479839c042");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid SaveEmailGroup
		= new Guid("eb656aa9-79cf-44bf-8741-cff56d6b30a9");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid SaveEmailGroupList
		= new Guid("c52de9bd-2c27-4272-b376-7c1033202207");

	// Imarda360CRM\Imarda360CRMBusiness\Control\EmailGroup\EmailGroup.cs

	public static readonly Guid DeleteEmailGroup
		= new Guid("60451072-f6a4-444d-b578-30addcad3bab");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid GetPerson
		= new Guid("82db27e5-c21e-47b1-8b16-74a07057d977");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid GetPersonUpdateCount
		= new Guid("373eb74c-e1dd-4a4b-b90a-107612eb42d9");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid GetPersonListByTimeStamp
		= new Guid("995f4331-2e7b-47fb-9a5b-c3fae0abf92b");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid GetPersonList
		= new Guid("88209b3a-60c6-4b24-8fc0-9d9bcb4c0ccb");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid SavePerson
		= new Guid("75b26378-707b-47bb-96c2-2c9d4e7a628c");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid SavePersonList
		= new Guid("67f9e885-3321-46bb-a94c-6490a6790f93");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Person\Person.cs

	public static readonly Guid DeletePerson
		= new Guid("9c515ca6-ef70-4e11-941a-988c4ff275d2");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid GetRole
		= new Guid("a0c71a86-806f-4951-8ac0-0ee4ccae477a");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid GetRoleUpdateCount
		= new Guid("6002d42a-51e7-4c1f-9547-3d2540ea0647");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid GetRoleListByTimeStamp
		= new Guid("79e51ae1-8f6d-42a4-8fcb-965d9fed6d6c");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid GetRoleList
		= new Guid("26ff899d-6bd7-4003-83ce-3f429f65d81d");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid SaveRole
		= new Guid("7626189c-ccd7-4047-a21c-1aec64cf1049");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid SaveRoleList
		= new Guid("ef2153e4-56a9-4559-91a4-2c122c83bbd0");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Role\Role.cs

	public static readonly Guid DeleteRole
		= new Guid("3f1f6dd7-4283-40b7-a013-d7fc41b19d81");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid GetRoleType
		= new Guid("d045a7a7-113a-473d-a4b6-2083433ffa28");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid GetRoleTypeUpdateCount
		= new Guid("58825af3-9586-45ce-a3e8-abce9b4ffc45");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid GetRoleTypeListByTimeStamp
		= new Guid("ad9f0165-39cb-487e-b018-19ff3bde92df");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid GetRoleTypeList
		= new Guid("f44bb9e1-d80e-408b-a7d1-a6c2d832b4f7");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid SaveRoleType
		= new Guid("8d449bf7-d2db-4d36-8a46-444e8d12304f");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid SaveRoleTypeList
		= new Guid("32abdba9-f6c2-4e55-94a1-7aea4a896717");

	// Imarda360CRM\Imarda360CRMBusiness\Control\RoleType\RoleType.cs

	public static readonly Guid DeleteRoleType
		= new Guid("a7ae16ad-512e-40ff-9215-00e950040a93");

	#region New Added NotificationPlan and NotificationItem in CRM

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid GetNotificationPlan
		= new Guid("609EE5DD-FE2E-4878-B47F-F3826262C6E9");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid GetNotificationPlanUpdateCount
		= new Guid("8470f82c-e8a3-4967-a3aa-0e7f22219785");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid GetNotificationPlanListByTimeStamp
		= new Guid("78f9cbdb-0437-4f83-b301-3373caf672d0");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid GetNotificationPlanList
		= new Guid("887d70dc-766e-4c98-be13-27ec4d95aeb8");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid SaveNotificationPlan
		= new Guid("f48ccb4e-dc93-4f95-9881-f8d5e6b269fb");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid SaveNotificationPlanList
		= new Guid("d8d0d2aa-de4d-4f09-afa4-f2ce47aa6b15");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationPlan\NotificationPlan.cs

	public static readonly Guid DeleteNotificationPlan
		= new Guid("72b49929-495a-4118-9d45-a2333020a11b");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid GetNotificationItem
		= new Guid("9f059c60-e99f-463d-8488-9d9b116736b1");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid GetNotificationItemUpdateCount
		= new Guid("ee629a57-747e-495f-a108-e47a01351681");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid GetNotificationItemListByTimeStamp
		= new Guid("540fa8ba-a2ec-41a2-96f7-a16ae0e28cb6");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid GetNotificationItemList
		= new Guid("6600024c-5992-401a-aaa0-db07f3cfdb73");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid SaveNotificationItem
		= new Guid("e424940d-13ab-45cb-ba3e-9864237490a8");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid SaveNotificationItemList
		= new Guid("085142ae-1a2f-455e-842b-a72c4f61694d");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationItem\NotificationItem.cs

	public static readonly Guid DeleteNotificationItem
		= new Guid("7ecad084-61f3-4223-96ea-a7168afc4ceb");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid GetNotificationHistory
		= new Guid("B87C37DB-B80A-4b27-83C9-41C5DEE35848");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid GetNotificationHistoryUpdateCount
		= new Guid("AEB04A32-FB86-4e00-93C6-A39253FE3106");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid GetNotificationHistoryListByTimeStamp
		= new Guid("B9BC0EA0-2FBF-4480-A2C1-C4BFB5A092A6");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid GetNotificationHistoryList
		= new Guid("5D977C3C-240B-43e1-8BA9-4CFDDEDDD30F");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid SaveNotificationHistory
		= new Guid("679AA686-287A-44bb-BB35-4B9A5AAD87EE");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid SaveNotificationHistoryList
		= new Guid("A5AA9AF1-E404-43d3-9C5D-3525C68C1B21");

	// Imarda360CRM\Imarda360CRMBusiness\Control\NotificationHistory\NotificationHistory.cs

	public static readonly Guid DeleteNotificationHistory
		= new Guid("BB21C3EE-32C6-4bef-9FE6-FA92080AD5E3");

	public static readonly Guid InstallNotificationItemsToNotificationPlan
		= new Guid("E47DC303-FF2D-4f11-BAA5-F5E6EC419F55");

	public static readonly Guid SendNewPassword
		= new Guid("1D3850FC-EF4C-420d-81D4-9043FDD50CE6");

	#endregion

	#region Contact
	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid GetContact
		= new Guid("219c78d5-d7c7-4749-9561-cdb66944ab49");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid GetContactUpdateCount
		= new Guid("a0353b72-6c0d-411a-8a0d-725e1412b88a");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid GetContactListByTimeStamp
		= new Guid("6ff201b8-2006-4318-8668-6f1c4ddacace");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid GetContactList
		= new Guid("ce9dedea-cf35-4157-9c78-5348ef67e7d6");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid SaveContact
		= new Guid("db0a1a9c-6646-45c2-99d3-4fc1e5541a8e");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid SaveContactList
		= new Guid("1454b631-1268-4c10-b55d-9ca2c495f000");

	// Imarda360CRM\Imarda360CRMBusiness\Control\Contact\Contact.cs

	public static readonly Guid DeleteContact
		= new Guid("be870aa0-b633-4adb-a30a-889fd3be4448");

	#endregion
	// Imarda360Security\Imarda360SecurityBusiness\Control\Imarda360Security.cs

	public static readonly Guid Login
		= new Guid("3d107b5e-384e-438b-8cf5-529a5e3b193b");

	// Imarda360Security\Imarda360SecurityBusiness\Control\Imarda360Security.cs

	public static readonly Guid GetSessionObject
		= new Guid("dfde3c34-0707-49f8-8bbd-9a0ddbfb2ea3");

	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\EntitySearch.cs

	public static readonly Guid GetTrackableListBySearch
		= new Guid("8114ae18-aea4-4f28-ae0e-2920d2ccc2d2");

	// Imarda360SolutionManager\Imarda360Solution\Control\SearchManagement\EntitySearch.cs

	public static readonly Guid GetTrackableFleetListBySearch
		= new Guid("1049a24b-c637-4539-8414-af4169892a8d");

	#region Tracking

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Imarda360Tracking.cs

	public static readonly Guid GetTrackableLastPosition
		= new Guid("99c635d1-019f-459a-8169-640cca3bfe50");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid GetAddress
		= new Guid("52029806-5c49-49aa-b3ed-8df02d799971");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid GetAddressUpdateCount
		= new Guid("1bad6874-a2b5-4585-a7ba-6f8d1b983b02");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid GetAddressListByTimeStamp
		= new Guid("116fae8f-9784-4a78-bceb-c4879bd16c37");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid GetAddressList
		= new Guid("71b49446-aaf7-4f44-bb7e-97ef736c14bf");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid SaveAddress
		= new Guid("01010354-c6fd-47dd-b027-d252f65f4785");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid SaveAddressList
		= new Guid("8485a43b-6845-438e-ad6e-90e6301d4f1f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Address\Address.cs

	public static readonly Guid DeleteAddress
		= new Guid("7bca62a2-de8e-4893-8527-dd6dae1849f1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid GetChecklistLog
		= new Guid("88cf7c58-5288-44f3-8775-23f9662ff0b8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid GetChecklistLogUpdateCount
		= new Guid("24cf4502-eb56-476b-a6d5-9f448699e369");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid GetChecklistLogListByTimeStamp
		= new Guid("46797de6-125b-424a-b326-96a598e18c42");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid GetChecklistLogList
		= new Guid("bccbdf76-327c-4152-b262-0d998266b38c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid SaveChecklistLog
		= new Guid("66800ba0-c129-400e-8c24-52c571e81229");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid SaveChecklistLogList
		= new Guid("7c6c7e46-febf-4c88-8fae-fe6255395101");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ChecklistLog\ChecklistLog.cs

	public static readonly Guid DeleteChecklistLog
		= new Guid("e90b8579-5fa6-4d8a-90fe-7080c8663d29");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid GetCompanyGeofence
		= new Guid("75f922de-225f-471b-9631-c8ff532fd3fb");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid GetCompanyGeofenceUpdateCount
		= new Guid("d1b03394-f149-4170-9585-bf5d85ef3fef");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid GetCompanyGeofenceListByTimeStamp
		= new Guid("0ad763b6-4157-44db-a32b-424a660995fe");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid GetCompanyGeofenceList
		= new Guid("3ef2ab6f-3806-40c2-87da-53d0ac824b68");

	public static readonly Guid GetAllCompanyGeofence
		= new Guid("52c3e09c-679d-421c-8226-8a8af1aeaea0");

	public static readonly Guid GetCompanyGeofenceCount
		= new Guid("c7e192b3-2f49-40a6-8fe5-7c58d12a76f8");

	public static readonly Guid GetCompanyGeofenceCommonCount
	= new Guid("2D89BD2B-DF25-4cff-A984-38C5AA84BDE8");

	public static readonly Guid GetCompanyGeofenceCommonListByVehiclePaging
	= new Guid("259F86CE-00EA-4667-9D56-01BF69F20FA9");

	public static readonly Guid InstallCompanyGeofenceCommonsToVehicle
	= new Guid("2622E0AB-AF53-457c-AC9F-6821BF7C703B");

	public static readonly Guid InstallCompanyGeofenceCommonsToFleet
	= new Guid("48F09315-315C-4735-B257-EBF2302EC774");

	public static readonly Guid DeleteCompanyGeofenceCommonListByAssetID
	= new Guid("baf16105-c15a-4480-ac84-b8b57173cec7");

	public static readonly Guid GetVehicleCommonCount
	= new Guid("5E13C21A-B2CF-42ea-A56A-042C0B119CC2");

	public static readonly Guid GetVehicleCommonListByLocationPaging
	= new Guid("5DA36012-14B3-458c-A6D6-4177024C77F8");

	public static readonly Guid InstallVehicleCommonsToLocation
	= new Guid("AF201FFC-0704-4975-8B3B-3E5B1332DF63");

	public static readonly Guid InstallVehicleCommonsToLocationGroup
	= new Guid("4E18B9AA-B4DC-4fbe-97DF-41F6522EDE2C");

	public static readonly Guid GetCompanyGeofenceByGroupCount
		= new Guid("8bd1054d-3844-48aa-b19f-79082cb8cdce");

	public static readonly Guid GetCompanyGeofenceGroupListByGeofenceID
		= new Guid("0F74172C-FD7D-48da-97ED-3D103671A7C0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid SaveCompanyGeofence
		= new Guid("e6bdf8f2-cf90-4bc1-b240-e9e1332740e1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid SaveCompanyGeofenceList
		= new Guid("cab61875-1c9c-426c-b98e-f18221ab752d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CompanyGeofence\CompanyGeofence.cs

	public static readonly Guid DeleteCompanyGeofence
		= new Guid("f12c041a-197c-4241-8139-50e90c134111");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid GetConex
		= new Guid("c89b9c74-0725-413a-9621-bdaa23537e87");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid GetConexUpdateCount
		= new Guid("e2b5e746-509c-4751-a3ba-da1d47bca1ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid GetConexListByTimeStamp
		= new Guid("02a5c98f-843b-4643-93bf-dd3ca9ccc523");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid GetConexList
		= new Guid("6ff1667a-8394-480d-b040-04f92289a6f6");

	public static readonly Guid GetConexListByUnitID
		= new Guid("456A2A2D-AF90-4d95-BF03-F07584A57CDA");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid SaveConex
		= new Guid("2edf628b-0c92-473e-b923-3add0b062754");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid SaveConexList
		= new Guid("06a48d3c-c412-45d2-af37-d0719acccc0e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Conex\Conex.cs

	public static readonly Guid DeleteConex
		= new Guid("a2eb1fe9-bd5d-4b02-ac96-ed01cab30823");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid GetDailyDistance
		= new Guid("7e412eb3-46bd-4ede-bd00-cbb2752c49a9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid GetDailyDistanceUpdateCount
		= new Guid("4e452461-1967-4427-8a33-7906e788ddda");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid GetDailyDistanceListByTimeStamp
		= new Guid("e94d76f3-7311-4dc4-a4e3-d11d6c8d018e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid GetDailyDistanceList
		= new Guid("fde74bff-5ad2-4d5d-98a0-422b2c21d9ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid SaveDailyDistance
		= new Guid("9126a18d-d28b-4eca-8b3e-8b2b79c0ebbd");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid SaveDailyDistanceList
		= new Guid("87a37503-7b5d-4357-8945-cc4cfe47b8fc");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DailyDistance\DailyDistance.cs

	public static readonly Guid DeleteDailyDistance
		= new Guid("214cd929-11c7-4fd2-9ade-7abd3b18cb64");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid GetDriverActivity
		= new Guid("6ee6c020-c2e1-468c-b5f5-4680e2114bd0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid GetDriverActivityUpdateCount
		= new Guid("754763c5-aa79-4775-a782-3166d187b5a5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid GetDriverActivityListByTimeStamp
		= new Guid("0b6457fe-65c5-4acb-876b-904f9bf2684f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid GetDriverActivityList
		= new Guid("ff590443-e8db-42c6-a966-96d104443dcb");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid SaveDriverActivity
		= new Guid("7842f70a-2b23-408c-85a4-4a70ba4c0973");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid SaveDriverActivityList
		= new Guid("d9dd1f5c-8d39-44c4-97c0-feac325a9902");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverActivity\DriverActivity.cs

	public static readonly Guid DeleteDriverActivity
		= new Guid("837f9167-8fa2-4c7a-9741-ef8a4906fc59");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid GetDriverFatigueBreach
		= new Guid("bb6591cf-cb0a-46ca-a5b9-869bbb6662fa");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid GetDriverFatigueBreachUpdateCount
		= new Guid("41824e80-b99b-4e7f-9a06-604e3d3e930b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid GetDriverFatigueBreachListByTimeStamp
		= new Guid("9d483f59-22f4-40e9-b157-d1d1b5bebee0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid GetDriverFatigueBreachList
		= new Guid("7ae5e37e-44c6-4d6c-9f0f-f72ec01c1aa0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid SaveDriverFatigueBreach
		= new Guid("90824b36-a037-4081-9635-e39015855365");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid SaveDriverFatigueBreachList
		= new Guid("88f892d6-8bc3-49ec-9c80-094b346bee8e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverFatigueBreach\DriverFatigueBreach.cs

	public static readonly Guid DeleteDriverFatigueBreach
		= new Guid("7dc6bb37-89bd-419d-86c0-cb8d2d495b60");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid GetDriverLog
		= new Guid("4b179ac7-3b50-4772-9f49-1ea623edc783");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid GetDriverLogUpdateCount
		= new Guid("47c25058-691c-4c0c-96bf-cbe11fe616b9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid GetDriverLogListByTimeStamp
		= new Guid("ae827d7f-6e71-40d3-a1d2-c27a9d37fd07");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid GetDriverLogList
		= new Guid("d86fee12-853c-4c88-9c21-afc54665d2e2");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid SaveDriverLog
		= new Guid("99eaffc4-c723-47a1-9068-5323204b043c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid SaveDriverLogList
		= new Guid("37eb54f3-5fbe-4b3c-890b-deccf764a8d0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\DriverLog\DriverLog.cs

	public static readonly Guid DeleteDriverLog
		= new Guid("d53231c7-0815-4fe0-9ddb-9079d133bf22");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid GetEventMessage
		= new Guid("884e1637-31f7-43ae-955d-c1cf29770eac");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid GetEventMessageUpdateCount
		= new Guid("0a46deb5-83b8-45d7-94f3-b506e15b8f69");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid GetEventMessageListByTimeStamp
		= new Guid("4ea6ab43-5dd4-487a-b3a9-45063c2fe3fa");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid GetEventMessageList
		= new Guid("c29e8400-1a89-4a6b-877b-bc0ebbbae09d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid SaveEventMessage
		= new Guid("b2c38fb9-cab4-474f-a450-322578736468");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid SaveEventMessageList
		= new Guid("552f66f3-277e-4c91-8e71-398533b49556");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessage\EventMessage.cs

	public static readonly Guid DeleteEventMessage
		= new Guid("8eb6cb67-1640-4a11-8af4-f58f75245395");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid GetEventMessageLog
		= new Guid("b9c5b623-2dc8-4a51-a0eb-ec6ed23f5c31");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid GetEventMessageLogUpdateCount
		= new Guid("cfa35c73-f664-4c15-b73f-908b70b6c68d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid GetEventMessageLogListByTimeStamp
		= new Guid("954ff0a8-5a43-4e7b-848b-10c56459d51d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid GetEventMessageLogList
		= new Guid("053bbe1f-7294-40db-8528-ecd52b32be8c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid SaveEventMessageLog
		= new Guid("f86d4e52-47aa-41e3-9729-75df90f85159");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid SaveEventMessageLogList
		= new Guid("93cbc68b-1dc2-4154-bf61-dd590f5c713b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageLog\EventMessageLog.cs

	public static readonly Guid DeleteEventMessageLog
		= new Guid("115e1350-7f1d-4b2b-a9af-f9d00115a73c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid GetEventMessageWaiting
		= new Guid("76e5b974-55c9-4fa8-aa20-7010e6a6aa72");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid GetEventMessageWaitingUpdateCount
		= new Guid("f3405301-b9f4-4a73-8a08-e20983bd3b33");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid GetEventMessageWaitingListByTimeStamp
		= new Guid("791c29f1-2dc0-4cc1-9aee-1dbba24a1ab1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid GetEventMessageWaitingList
		= new Guid("f8158f6d-4563-4339-a520-d601cd267786");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid SaveEventMessageWaiting
		= new Guid("803a2a1f-a83b-4552-bebb-d8efed6dacef");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid SaveEventMessageWaitingList
		= new Guid("21846516-9050-4a0e-a67c-7d8dd98cdbf1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventMessageWaiting\EventMessageWaiting.cs

	public static readonly Guid DeleteEventMessageWaiting
		= new Guid("fd4d4241-6c7f-44cc-901b-58dd83b98b59");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid GetEventType
		= new Guid("ceacc181-1e8f-4c6e-8d30-a612c9a588d8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid GetEventTypeUpdateCount
		= new Guid("b279de6b-40cf-456e-b535-d40053673e81");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid GetEventTypeListByTimeStamp
		= new Guid("c6723425-d316-481c-b7ed-f9b020b5928c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid GetEventTypeList
		= new Guid("79832c27-3826-48da-b546-79084bd12c32");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid SaveEventType
		= new Guid("84c401aa-780d-405c-83e0-68090af3ee17");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid SaveEventTypeList
		= new Guid("36398594-7239-49fd-95ed-20c75bc524d5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\EventType\EventType.cs

	public static readonly Guid DeleteEventType
		= new Guid("4207a763-aa94-46e5-a411-b72af3b6024a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid GetExceptionHistory
		= new Guid("ac3f6ec3-87f2-4e36-b945-0e5e4052cb51");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid GetExceptionHistoryUpdateCount
		= new Guid("e2a03cde-6973-418e-bb1b-ac8c00c87256");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid GetExceptionHistoryListByTimeStamp
		= new Guid("8031eaae-227b-4530-aa8c-9fa27f152d5d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid GetExceptionHistoryList
		= new Guid("5eae732b-814c-4742-abf4-22738e2d9de1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid SaveExceptionHistory
		= new Guid("86d69eff-1d3d-4e60-94b0-d8ec34d3adbe");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid SaveExceptionHistoryList
		= new Guid("5ab31e7d-ab06-4119-82f0-b79f996e993f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistory\ExceptionHistory.cs

	public static readonly Guid DeleteExceptionHistory
		= new Guid("434765f8-c36d-45af-9ca0-8460a1c4e76b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid GetExceptionHistoryBoral
		= new Guid("6af39455-9667-4659-af1e-49f3e697cf8c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid GetExceptionHistoryBoralUpdateCount
		= new Guid("eb022742-f85d-4b28-83cd-05669fbcfe73");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid GetExceptionHistoryBoralListByTimeStamp
		= new Guid("51b50873-0a25-4371-9eac-bb139ed733a8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid GetExceptionHistoryBoralList
		= new Guid("173ea6b4-69f8-4667-b3a5-2be7294ea7f7");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid SaveExceptionHistoryBoral
		= new Guid("b7c148c3-1579-495f-b843-9a3a7b235e48");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid SaveExceptionHistoryBoralList
		= new Guid("09f229ca-f4c5-4db3-8991-74892392da3a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionHistoryBoral\ExceptionHistoryBoral.cs

	public static readonly Guid DeleteExceptionHistoryBoral
		= new Guid("63e2cfe2-13f4-4b07-a879-762339ffad57");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid GetExceptionType
		= new Guid("94b14e8f-3283-4072-86d9-34b3a58a3515");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid GetExceptionTypeUpdateCount
		= new Guid("9e3c0a09-5519-4b58-8f7b-51018a480e4c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid GetExceptionTypeListByTimeStamp
		= new Guid("425dc714-3e6a-4da7-a5db-eb662c8a5820");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid GetExceptionTypeList
		= new Guid("a2eb3174-6fab-494c-8cfb-0c4ece6189f8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid SaveExceptionType
		= new Guid("5c1ae961-b3c2-462c-bb3c-dfd305f5a65f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid SaveExceptionTypeList
		= new Guid("044ec123-6b59-4920-9e3c-493b42d6b528");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ExceptionType\ExceptionType.cs

	public static readonly Guid DeleteExceptionType
		= new Guid("bce97edc-67fa-4d89-9815-ffc1eea48465");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid GetFatigueAccreditation
		= new Guid("85ef0c03-4604-4050-8cc7-c58a18d6a7b6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid GetFatigueAccreditationUpdateCount
		= new Guid("a892deac-2e14-4cd1-95dc-ea9d3811ee87");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid GetFatigueAccreditationListByTimeStamp
		= new Guid("4df13f70-9ee3-4f1f-a8b3-6e6ddfe7d766");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid GetFatigueAccreditationList
		= new Guid("291483cf-c5fa-4689-95b7-596e82c0c6ed");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid SaveFatigueAccreditation
		= new Guid("39405dc5-3464-44e3-ac99-19e2e3b65d6f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid SaveFatigueAccreditationList
		= new Guid("c0d102a6-756f-4bb5-b8aa-219b59d31cfa");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\FatigueAccreditation\FatigueAccreditation.cs

	public static readonly Guid DeleteFatigueAccreditation
		= new Guid("dc77f8e5-5f3c-4489-a480-20dd19a300ca");

	public static readonly Guid GetGatewayExtEventMessage
		= new Guid("b4e75068-b616-413b-bed2-425b2923d37a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid GetGatewayExtEventMessageUpdateCount
		= new Guid("339caf9a-9a1a-4d1e-aacf-050c903e2900");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid GetGatewayExtEventMessageListByTimeStamp
		= new Guid("a24e06a4-3a96-4894-bc06-c85b7a11b6cb");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid GetGatewayExtEventMessageList
		= new Guid("af78354b-6554-4fac-a065-7e123d5d50ce");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid SaveGatewayExtEventMessage
		= new Guid("99b55706-a1f0-4d09-b28f-c410e82e6c6c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid SaveGatewayExtEventMessageList
		= new Guid("8ed4ed31-96c0-49d3-9c39-8888173f1e4e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GatewayExtEventMessage\GatewayExtEventMessage.cs

	public static readonly Guid DeleteGatewayExtEventMessage
		= new Guid("9147dc12-b221-4887-8866-d795ef659ac3");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid GetGeofencePolygons
		= new Guid("c9b64533-49d4-4613-97e7-2f9fd9729e01");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid GetGeofencePolygonsUpdateCount
		= new Guid("ce0a5b31-834e-46d9-8fc3-0df40d57ad5d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid GetGeofencePolygonsListByTimeStamp
		= new Guid("728b1189-682c-46b5-8550-7a9ebc03a9ca");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid GetGeofencePolygonsList
		= new Guid("f2768c45-70fe-4ce8-b4fc-ccc7974009f8");

	public static readonly Guid GetGeofencePolygonPointsByCompany
		= new Guid("6CAE99ED-39FA-436f-862F-5B6E3102CB8C");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid SaveGeofencePolygons
		= new Guid("bf33aa46-60cd-4bdc-95cc-018cf7cf8250");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid SaveGeofencePolygonsList
		= new Guid("9e73cf85-a2b9-4abd-be1e-e5168bf573ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\GeofencePolygons\GeofencePolygons.cs

	public static readonly Guid DeleteGeofencePolygons
		= new Guid("f1645f23-0879-40d7-9bf0-2e1bef8a71d4");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid GetMapLocation
		= new Guid("a65f6a0b-5bda-461f-9f74-5f6d8c9ab709");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid GetMapLocationUpdateCount
		= new Guid("7242311c-75f3-4fd1-ab58-c0624ac22314");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid GetMapLocationListByTimeStamp
		= new Guid("92837bc6-e829-4a6d-9e83-7f345d3fbcfe");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid GetMapLocationList
		= new Guid("18b44b8b-cf70-4b97-a23d-742939fc5d23");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid SaveMapLocation
		= new Guid("3fb9f048-0cec-4717-ad87-83f8f1fecf48");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid SaveMapLocationList
		= new Guid("714196f0-c4ac-49df-b044-f9105cc5d91f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MapLocation\MapLocation.cs

	public static readonly Guid DeleteMapLocation
		= new Guid("d4f76c19-194e-4b19-b007-06fac1ecfd4d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid GetMessageCentre
		= new Guid("19e9052c-258f-4a99-9f4d-d4b85c16ddb6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid GetMessageCentreUpdateCount
		= new Guid("28803769-9f36-44f9-95a9-ad5e79939494");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid GetMessageCentreListByTimeStamp
		= new Guid("f8dfabe2-cf2f-4b25-8b0b-d5620a88d45e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid GetMessageCentreList
		= new Guid("f3195b29-0c1c-4c09-a36e-f5662315e2e1");

	public static readonly Guid GetMessageCentreListByThreadId
		= new Guid("4F4FF8CA-A54F-448d-A87A-F8F1232B0507");

	public static readonly Guid GetMessageCentreListRoot
		= new Guid("CF17A65C-9DB2-4359-8394-887DD2343CE5");

	public static readonly Guid GetMessageCentreListRecent
		= new Guid("D0327316-5C72-4C70-AFE1-E73C7888B8C8");

	public static readonly Guid GetMessageCentreListByTrackID
		= new Guid("C6EE7DA4-597E-4F4D-8C68-6CC8BA142C29");

	
	public static readonly Guid GetMessageCentreListByTrackIDs
		= new Guid("731c48da-7e31-48db-aeb9-07686171ce39");
	

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid SaveMessageCentre
		= new Guid("21c32ca9-6400-4563-9e5c-ddafa20de58a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid SaveMessageCentreList
		= new Guid("8a8d7cfa-4b01-4ad7-9fdc-783012a58a8c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.cs

	public static readonly Guid DeleteMessageCentre
		= new Guid("4d194d58-fbfd-4e50-8713-079f54eb000a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MessageCentre\MessageCentre.Extra.cs

	public static readonly Guid GetMessageBySendAndReceiveID
		= new Guid("30fadcf5-94b2-4099-bda6-8cb760e65438");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid GetMonthlyDistance
		= new Guid("120bd1cd-3c33-42e1-aa36-5cfa48974f73");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid GetMonthlyDistanceUpdateCount
		= new Guid("5f2af38d-97b6-4ac6-9d79-5beb131bbc34");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid GetMonthlyDistanceListByTimeStamp
		= new Guid("027df4d1-f016-4d96-9e88-c3f58c7d2af9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid GetMonthlyDistanceList
		= new Guid("6710d6df-b3d0-4ebd-9623-d46ddbbde461");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid SaveMonthlyDistance
		= new Guid("03dec73c-e314-48e9-b2dd-f92923530335");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid SaveMonthlyDistanceList
		= new Guid("1719b18a-c401-4b51-868d-6657a75820d5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\MonthlyDistance\MonthlyDistance.cs

	public static readonly Guid DeleteMonthlyDistance
		= new Guid("a57e944b-fb47-4a5a-a031-dd0eefd6787f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid GetProductType
		= new Guid("ccac2da7-2b7a-4010-a17e-06e216819f58");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid GetProductTypeUpdateCount
		= new Guid("467cd84c-5623-477f-a632-83eb053dff2e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid GetProductTypeListByTimeStamp
		= new Guid("bd41faf9-6a03-4f4e-b985-f0df4a8ad600");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid GetProductTypeList
		= new Guid("821c312b-e3cc-48f1-886c-d90a6dc1b324");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid SaveProductType
		= new Guid("77cafb6e-77b4-4ebc-9097-cef85af50d06");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid SaveProductTypeList
		= new Guid("299f7237-382a-449c-b23e-03a424534fe9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\ProductType\ProductType.cs

	public static readonly Guid DeleteProductType
		= new Guid("fb900ad4-cd5a-484b-8aa6-bbe73bee25cc");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid GetQueriedUnitGeofence
		= new Guid("ffa352a2-9408-4bcf-9a4e-14e68e8d8a58");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid GetQueriedUnitGeofenceUpdateCount
		= new Guid("f85650d2-5958-424d-a8a1-505df08f409b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid GetQueriedUnitGeofenceListByTimeStamp
		= new Guid("2ac96ebe-6e37-4806-89b1-5abed72ff9a6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid GetQueriedUnitGeofenceList
		= new Guid("43939e86-ef40-4cb6-9d5a-67f6d0a9c130");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid SaveQueriedUnitGeofence
		= new Guid("16447f16-d779-4d53-84ae-8c8f1633fdcf");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid SaveQueriedUnitGeofenceList
		= new Guid("3ca8ea43-45c7-4c2a-9bec-a5b44892da12");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\QueriedUnitGeofence\QueriedUnitGeofence.cs

	public static readonly Guid DeleteQueriedUnitGeofence
		= new Guid("ef45a735-a013-40b5-9414-d9cc4b185573");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid GetSMSLog
		= new Guid("75e1f744-0b69-4750-9a98-2ef0d38c8780");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid GetSMSLogUpdateCount
		= new Guid("9f0838c2-6e8c-4225-b6b4-b542d49cdaf3");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid GetSMSLogListByTimeStamp
		= new Guid("fbb8b301-fda0-4a73-97d4-521adb772276");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid GetSMSLogList
		= new Guid("5f450701-b945-4757-9aeb-3edb0380bc3d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid SaveSMSLog
		= new Guid("f1ef2166-79d3-41c5-93f7-c1267ba3693d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid SaveSMSLogList
		= new Guid("bb7a435c-dd75-4f7f-a9ab-6b2b8e2b7434");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SMSLog\SMSLog.cs

	public static readonly Guid DeleteSMSLog
		= new Guid("34a1431d-c000-4a25-b4d9-ee8e4f881ea9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid GetSystemStatus
		= new Guid("b88f103c-271a-4efb-b89d-68d38ce71373");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid GetSystemStatusUpdateCount
		= new Guid("5d58472e-8540-48cd-8bc9-59140a3c9f60");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid GetSystemStatusListByTimeStamp
		= new Guid("eab3a594-a9ac-40ff-a4f5-7912b3dd6e70");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid GetSystemStatusList
		= new Guid("243ef8d4-b553-48bd-9042-91542c2aebc0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid SaveSystemStatus
		= new Guid("fbbcd67e-7bd4-460f-8f2f-cb783f7d88a8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid SaveSystemStatusList
		= new Guid("81d0d029-0627-4d80-ac02-d9b281c2e163");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatus\SystemStatus.cs

	public static readonly Guid DeleteSystemStatus
		= new Guid("510432e2-feee-4aa6-b2b0-5bb98cfce08e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid GetSystemStatusLog
		= new Guid("b4718a4c-5368-4433-84ab-1a48dfddda6e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid GetSystemStatusLogUpdateCount
		= new Guid("414110c6-f3e3-48e1-9b5e-6395c3fd4480");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid GetSystemStatusLogListByTimeStamp
		= new Guid("655ad9f1-62f3-4433-89f5-1fec563f34aa");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid GetSystemStatusLogList
		= new Guid("1ba98646-c1c9-411d-a3bd-20a26ee071a8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid SaveSystemStatusLog
		= new Guid("07414564-502e-4543-ab31-7231a9314d1a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid SaveSystemStatusLogList
		= new Guid("5b77e8e2-c8ba-4005-b3ac-94342387d98b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\SystemStatusLog\SystemStatusLog.cs

	public static readonly Guid DeleteSystemStatusLog
		= new Guid("5f71b30a-5bc9-4db7-a305-3ac523bcfa82");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid GetTimeInterval
		= new Guid("005bad64-e540-4bbb-a989-032500c94fe0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid GetTimeIntervalUpdateCount
		= new Guid("4b880573-347c-46f0-aec6-d91e4cc4e18c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid GetTimeIntervalListByTimeStamp
		= new Guid("c8d35ca1-2232-4ab7-8f54-2b419378f186");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid GetTimeIntervalList
		= new Guid("0d29d8ac-61bc-459b-a90d-c30fae722b41");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid SaveTimeInterval
		= new Guid("8d306d5b-4762-43c8-9766-3e08a34783cb");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid SaveTimeIntervalList
		= new Guid("9d22d2e1-820c-4b7b-9386-0772b89a4f12");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TimeInterval\TimeInterval.cs

	public static readonly Guid DeleteTimeInterval
		= new Guid("8bb101cd-aa7b-4ab5-af3c-1be6d3c69fc5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetLastLocation
		= new Guid("5f7fedb0-21e6-485c-a846-fc0a6e94a99c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetTrackableList
		= new Guid("c8abae08-ccd4-4565-94ad-f543034209b0");

	public static readonly Guid GetTrackable
		= new Guid("8BDDB838-9648-4ff2-BC53-593E4D20E42A");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetTrackableListByGeofence
		= new Guid("66cc046f-3358-4957-8913-eb2d04b2d5d7");

	public static readonly Guid DeleteUnitOwnerListByOwner
		= new Guid("35caf0bf-f167-42fd-aa6c-4b467f8d2b73");

	public static readonly Guid GetTrackableListByGeofencePaging
		= new Guid("bd5a5ca6-676a-4b9d-9245-3eb4a6525346");

	public static readonly Guid GetTrackableListByGeofenceCount
		= new Guid("c6d904b8-6714-4871-adc6-9e898985b596");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid SaveGeofence
		= new Guid("e7010834-c8fa-4f2f-93e0-df6398fdfb67");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetGeofencePoints
		= new Guid("0d86d329-22c1-4d28-befb-3a5f212e369c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid DeleteGeofenceCascade
		= new Guid("4cb3cd92-f1c2-4cf1-b797-d5eacbbecdb8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetCompanyGeofenceListByUnit
		= new Guid("2fe28248-8591-40b0-a488-24a2d0d8621a");

	public static readonly Guid GetCompanyGeofenceListByUnitCount
		= new Guid("7d81f1a5-fd13-41da-9d34-9c2b2245bdfa");

	public static readonly Guid GetCompanyGeofenceListByUnitPaging
		= new Guid("9a537a5e-9042-4ce6-9b71-f6899cbcaf9a");

	public static readonly Guid GetUninstalledCompanyGeofenceListByUnitCount
		= new Guid("fae78a07-9341-41f2-8bc4-2e24675a526a");

	public static readonly Guid GetUninstalledCompanyGeofenceListByUnitPaging
		= new Guid("5b90d698-3418-4133-bce5-bc09fe73b38b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetCompanyGeofenceListByGFID
		= new Guid("55F21CE4-74B5-4067-A84D-69B7A9F9F406");

	public static readonly Guid GetCompanyGeofenceByID
		= new Guid("a29f18e6-a9bf-4a4a-b8f2-8167f61c619b");

	public static readonly Guid GetCompanyGeofenceByGFTitle
		= new Guid("65c7c6d2-56a7-45e4-bba2-534a5fe88453");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetCompanyGeofenceListByGoupID
		= new Guid("8347741a-b8a1-4356-afb0-43a7f258ef9d");

	public static readonly Guid InstallGeofencesToUnit
		= new Guid("af4ce830-2a8a-46b8-924b-a61249305816");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid InstallUnitsToGeofence
		= new Guid("9f5b5916-213e-427e-9537-958594c74807");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid GetGeofencePointsByUnitID
		= new Guid("4c8a025a-20c5-41be-b4ea-84e578e733d8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\TrackingManager\TrackingManager.cs

	public static readonly Guid InstallUnitsToVehicle
		= new Guid("03d7bea4-d798-4a49-885e-b48391c92c81");

	public static readonly Guid InstallUnitsToUser
		= new Guid("8B8A4F8F-93E0-45ff-97E1-18266ADA4D4B");

    public static readonly Guid GetExpiredCompanyGeofenceList
        = new Guid("B944947E-D181-426E-917E-415D15B36D51");

    public static readonly Guid GetGeofenceActivitiesByGeofence
        = new Guid("D0F1BA33-432A-4A1C-8BC5-BBF06B45B18A");

    public static readonly Guid GetGeofenceActivitiesByVehicle
        = new Guid("631C652A-2F6C-4056-AA35-89CE17C37D7E");
	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid GetUnit
		= new Guid("1783c786-6842-42f7-8934-f4a83c3e81fd");

	public static readonly Guid GetUnitByTrackID
		= new Guid("DEB3E6C6-954A-42cc-A603-05BC42D86253");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid GetUnitUpdateCount
		= new Guid("6a003c1c-b952-4f2c-bae1-f55d4ca5943c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid GetUnitListByTimeStamp
		= new Guid("a30b2476-db6f-422f-a3a6-8cd5cec961e8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid GetUnitList
		= new Guid("e3d2e707-1869-44ff-8f99-5c429cba4126");

	public static readonly Guid GetUnitListByMessageProfileID
		= new Guid("8DAE951B-A01D-443a-8247-60586B938425");

	public static readonly Guid GetUnitSeedList
		= new Guid("89618C32-7F53-4dd7-90C5-544642D9A4EA");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid SaveUnit
		= new Guid("2af8b34d-1d89-42bd-8fb7-3c313678b4e7");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid SaveUnitList
		= new Guid("57e3f195-53dd-4696-9fab-1fbda0d6e21e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Unit\Unit.cs

	public static readonly Guid DeleteUnit
		= new Guid("fc2b153d-f9aa-4dd5-a447-d523dd6fb90b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid GetUnitDailyDistance
		= new Guid("d3746b20-7def-47a9-8096-5558db1cb3ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid GetUnitDailyDistanceUpdateCount
		= new Guid("7e2b41e7-a26f-411d-b59f-8591f762145d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid GetUnitDailyDistanceListByTimeStamp
		= new Guid("0eb8aee6-b703-4253-8f60-499dc232c4f0");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid GetUnitDailyDistanceList
		= new Guid("4d90e56c-0e8a-4576-b088-29ae2e3db0c7");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid SaveUnitDailyDistance
		= new Guid("22f6a5e4-32f9-4c8d-baa2-0405f026224f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid SaveUnitDailyDistanceList
		= new Guid("4078b779-19c6-406b-8dd3-3ad9a5f84c7f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitDailyDistance\UnitDailyDistance.cs

	public static readonly Guid DeleteUnitDailyDistance
		= new Guid("7dad8762-7e66-4382-8070-9a951eded564");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid GetUnitGeofence
		= new Guid("6fa39591-0b13-4af7-b9a6-9db28fa147f4");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid GetUnitGeofenceUpdateCount
		= new Guid("2513b413-d3e8-4f43-a8dd-93db81939a40");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid GetUnitGeofenceListByTimeStamp
		= new Guid("e9253608-045a-4141-b931-7a477836d656");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid GetUnitGeofenceList
		= new Guid("dc2e21e8-85f6-43ee-a1bf-8e1d520a1185");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid SaveUnitGeofence
		= new Guid("d114c15a-0b10-4c1b-9eed-9c8602ff9e60");

	public static readonly Guid ConfirmUnitGeofenceListInstalled
		= new Guid("1c70c615-901f-4821-84c7-f9cfd611b1ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid SaveUnitGeofenceList
		= new Guid("993625c9-9dcb-4fca-ae82-a5afd0cc89f1");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitGeofence\UnitGeofence.cs

	public static readonly Guid DeleteUnitGeofence
		= new Guid("fa568d41-3291-4de2-8d57-9352380d52e6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid GetUnitLog
		= new Guid("0c29bfdb-2100-4829-bf52-fa517716d92a");

    public static readonly Guid GetUnitLogIdByVideoID
        = new Guid("8BE09E92-C474-42CA-BA14-F78C56D43A1D");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid GetUnitLogUpdateCount
		= new Guid("5b615735-b528-430c-ac57-7b3be2be7725");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid GetUnitLogListByTimeStamp
		= new Guid("f8331128-97fc-4fee-b01f-e629f3caeef5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid GetUnitLogList
		= new Guid("c5cb6c02-7b90-4e11-8cdf-93fb5a73da40");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid SaveUnitLog
		= new Guid("f5572c1b-8903-43a8-8bae-b79c2aa740d4");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid SaveUnitLogList
		= new Guid("7cac1d3b-b022-494b-ad78-70e372b13491");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLog\UnitLog.cs

	public static readonly Guid DeleteUnitLog
		= new Guid("735d83a9-c683-40fa-9489-ab4941887352");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid GetUnitLogType
		= new Guid("29677f73-3f1f-41c7-9296-c860d0ba6ea9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid GetUnitLogTypeUpdateCount
		= new Guid("5e666cbd-de20-4d4f-b95d-e803e866d7f2");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid GetUnitLogTypeListByTimeStamp
		= new Guid("62d941cc-2e05-426e-a306-de7ccb5f0d3a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid GetUnitLogTypeList
		= new Guid("b8a1d1d8-46c7-4651-aaaa-e94bb9f96de5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid SaveUnitLogType
		= new Guid("b8b420b3-9987-46ea-a2e6-1ddcb5108ac5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid SaveUnitLogTypeList
		= new Guid("89dcd191-0468-435f-b33a-f0996b794291");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitLogType\UnitLogType.cs

	public static readonly Guid DeleteUnitLogType
		= new Guid("63d94cd4-ec44-4581-8a91-66a0b22aa8df");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid GetUnitMonthlyDistance
		= new Guid("9d037eb5-d330-4761-a8aa-91d07d8e8498");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid GetUnitMonthlyDistanceUpdateCount
		= new Guid("2b125cbe-f3d1-4342-a811-569febd33dd2");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid GetUnitMonthlyDistanceListByTimeStamp
		= new Guid("4774aff5-0bd5-4009-a401-60238f3ace65");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid GetUnitMonthlyDistanceList
		= new Guid("f724ef88-854b-4b36-947c-8e3d06b43094");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid SaveUnitMonthlyDistance
		= new Guid("433a9807-2199-4525-8f3d-5e9ce45e34ec");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid SaveUnitMonthlyDistanceList
		= new Guid("763c4402-8657-4df1-95a8-a0482d1d6778");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitMonthlyDistance\UnitMonthlyDistance.cs

	public static readonly Guid DeleteUnitMonthlyDistance
		= new Guid("703eb711-719b-44a5-81ee-acf969dd0370");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid GetUnitNavman
		= new Guid("d4f94f76-5bc9-4a42-9ddf-231942f1d403");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid GetUnitNavmanUpdateCount
		= new Guid("e523811d-f458-4e75-bdae-826469b0fa5b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid GetUnitNavmanListByTimeStamp
		= new Guid("a555abf1-7749-4ba0-a867-5a7a7ad2e63b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid GetUnitNavmanList
		= new Guid("264a1246-b06b-4a8e-b17f-b5c5e8f92156");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid SaveUnitNavman
		= new Guid("e2ec481f-d8ad-4414-baca-d7aca1393a91");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid SaveUnitNavmanList
		= new Guid("753787e7-13df-4bd2-ba5d-d80cb7467c4d");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitNavman\UnitNavman.cs

	public static readonly Guid DeleteUnitNavman
		= new Guid("a1954690-4639-44e7-a17d-fa52c330c44e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid GetUnitPosition
		= new Guid("7e5bc10d-5766-41ae-acce-fc600429c2e2");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid GetUnitPositionUpdateCount
		= new Guid("163f4275-e2c5-4608-b405-30bdca54f88f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid GetUnitPositionListByTimeStamp
		= new Guid("f3f8f1ad-077e-4a49-8767-382dfb377e2a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid GetUnitPositionList
		= new Guid("241dc02f-6f64-481d-bd53-42c66bc450f3");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid SaveUnitPosition
		= new Guid("8067f4db-68f4-4b9e-a005-de86c1bebb2b");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid SaveUnitPositionList
		= new Guid("8b834457-da57-4be0-b05e-cab1b63985a9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitPosition\UnitPosition.cs

	public static readonly Guid DeleteUnitPosition
		= new Guid("52a181f8-4aa9-4213-8c52-82787f6d28e9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid GetUnitProlificx
		= new Guid("2a207203-712a-4fe9-b67e-774b94d007bc");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid GetUnitProlificxUpdateCount
		= new Guid("90395709-ad68-455c-9e1e-ab9e9c1b0644");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid GetUnitProlificxListByTimeStamp
		= new Guid("f1d1e91e-9a3f-4d9c-82ab-fd5af384ed84");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid GetUnitProlificxList
		= new Guid("50972923-6b32-4173-9670-414672ec08fb");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid SaveUnitProlificx
		= new Guid("5df87130-6f61-465e-816c-4e61db265bee");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid SaveUnitProlificxList
		= new Guid("37eb159c-2cfb-4e13-ab48-6a19283e0ca5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitProlificx\UnitProlificx.cs

	public static readonly Guid DeleteUnitProlificx
		= new Guid("d9e1a273-da94-4d14-acf5-7cff9d6e4efc");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid GetUnitTrace
		= new Guid("85831009-fa30-42b3-b2e2-e6d37d22d248");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid GetUnitTraceUpdateCount
		= new Guid("d637b28e-3fdf-4e16-a27b-15f75bfe7a2f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid GetUnitTraceListByTimeStamp
		= new Guid("e84e0a02-0f21-4878-924f-c66165edf8c4");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid GetUnitTraceList
		= new Guid("57192c25-ac44-4175-af8d-9fe6cb23969f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid SaveUnitTrace
		= new Guid("a914f553-11e0-4c3a-9f76-3105387027c5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid SaveUnitTraceList
		= new Guid("73aa9234-7860-45a2-a0d9-f3b9acf16957");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.cs

	public static readonly Guid DeleteUnitTrace
		= new Guid("6eda0b74-be9c-4294-acb2-73362720e6ee");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTrace\UnitTrace.Extra.cs

	public static readonly Guid GetIdsEventsByDateRange
	= new Guid("fbe600a7-9a4e-4b81-9ddb-2a2a0a9b00d3");

	public static readonly Guid GetUnitTraceListByDateRange
		= new Guid("8d116d02-95e1-443e-8586-e121c0b7a699");

	public static readonly Guid GetUnitTraceListByUnitDateEvent
		= new Guid("674aaf91-7655-4d5f-b052-3d51a8724cf5");
	public static readonly Guid GetUnitTraceCalibrationListByDate
		= new Guid("3f01157e-0612-42cf-8f4e-48d3cb84e49d");

	public static readonly Guid GetUnitTraceListByTopN
		= new Guid("4EEC34FD-D5EB-45f6-AA6A-3035FB874474");

	public static readonly Guid GetUnitTrailerTraceOpenList		//& IM-4564
		= new Guid("21F0CC4E-345E-4B2E-BE32-ADA2E8DEE97B");		//& IM-4564

	public static readonly Guid GetTraceEventLastLocationList
		= new Guid("7d23f63f-c08d-4af9-8a6d-51c0c960f339");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid GetUnitTraceArchive
		= new Guid("8d9bc00b-bbe3-4cc5-aaa2-278fd8862ae6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid GetUnitTraceArchiveUpdateCount
		= new Guid("677f1099-d90e-45f5-a532-8b0d342cef9a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid GetUnitTraceArchiveListByTimeStamp
		= new Guid("e9eab73c-bc18-485e-8849-6017c2dca1a6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid GetUnitTraceArchiveList
		= new Guid("22927f31-80a1-4f11-bb10-7c8213194f05");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid SaveUnitTraceArchive
		= new Guid("e6ea403a-270c-4a60-af6e-d8e6dd4c77ef");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid SaveUnitTraceArchiveList
		= new Guid("f7c4a983-ef8a-476c-99c7-adba444a4789");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.cs

	public static readonly Guid DeleteUnitTraceArchive
		= new Guid("9390a7cb-e93a-4b51-bc9b-87aec90ed5c5");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitTraceArchive\UnitTraceArchive.Extra.cs

	public static readonly Guid GetUnitTraceArchiveListByDateRange
		= new Guid("74094903-a975-4d7d-a899-c23e3ffe00ab");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid GetUnitType
		= new Guid("48bca13a-bef1-4648-903c-211f31e445d2");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid GetUnitTypeUpdateCount
		= new Guid("6978b811-b430-47f1-b281-66ee2a15a696");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid GetUnitTypeListByTimeStamp
		= new Guid("112c513b-84c3-4ffd-8f88-8b4ed660ea7e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid GetUnitTypeList
		= new Guid("19418ff6-80c0-495e-9829-abf827c9dd56");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid SaveUnitType
		= new Guid("4cce690c-3d23-4b1c-a8ce-2b78566fe51f");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid SaveUnitTypeList
		= new Guid("26b9b3ba-8256-4a2b-b079-e9004b84a879");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitType\UnitType.cs

	public static readonly Guid DeleteUnitType
		= new Guid("254a6de3-6472-44da-a629-bb066ca9db56");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid GetUnitVehicle
		= new Guid("faf97bb4-6a2d-4b82-8385-c61d975c2aad");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid GetUnitVehicleUpdateCount
		= new Guid("902a07f4-854d-4c8d-b29e-7667a4c65f12");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid GetUnitVehicleListByTimeStamp
		= new Guid("173aea7c-7346-45bc-b204-05070c52c005");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid GetUnitVehicleList
		= new Guid("ca379110-3b34-4e2c-99cb-006e57251b77");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid SaveUnitVehicle
		= new Guid("52943904-485d-4e3f-aab1-050f70ee85e4");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid SaveUnitVehicleList
		= new Guid("52b50797-6e9e-41b2-8b46-8ac365a8dc88");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnitVehicle\UnitVehicle.cs

	public static readonly Guid DeleteUnitVehicle
		= new Guid("cb02d749-b305-41b4-b4cc-3574b91a6251");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid GetUnprocessedData
		= new Guid("82fafe4c-1331-4550-9621-905b426b667e");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid GetUnprocessedDataUpdateCount
		= new Guid("a0840d52-dc4f-4c02-b726-f9df3cac83cd");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid GetUnprocessedDataListByTimeStamp
		= new Guid("ddc0a63d-0af7-45dc-bbd2-38ce6630fcdd");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid GetUnprocessedDataList
		= new Guid("cd86850a-f0ce-48a4-b707-17d7703175a6");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid SaveUnprocessedData
		= new Guid("b102feb2-6918-4783-b6a3-17900af53b34");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid SaveUnprocessedDataList
		= new Guid("9c3d8517-4fd1-4ec7-ac81-ff72d4d3e757");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\UnprocessedData\UnprocessedData.cs

	public static readonly Guid DeleteUnprocessedData
		= new Guid("ad1b2a3c-9e5c-472d-aceb-f7445046624a");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid GetCannedMessage
		= new Guid("969D6C82-C7F9-4b58-8064-DBA5674B5F57");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid GetCannedMessageUpdateCount
		= new Guid("256296E1-6D59-46da-A26F-03990D98C3E8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid GetCannedMessageListByTimeStamp
		= new Guid("7AE71133-E5FC-4c15-B0D7-B0FE107E0383");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid GetUserCannedMessageList
		= new Guid("fd9e854f-f2e5-4479-91d8-2358ca18b06a");

	public static readonly Guid GetCannedMessageList
		= new Guid("CDADA98E-FEE0-4729-B952-67662376684E");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid SaveCannedMessage
		= new Guid("3F069F84-0875-491c-91CD-5D9B8BE74490");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid SaveCannedMessageList
		= new Guid("ED2D751B-717E-4620-91CE-DC87ED23C5F7");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid DeleteCannedMessage
		= new Guid("B9FC5AE4-C775-4288-B22C-260FC4D753D9");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\CannedMessage\CannedMessage.cs

	public static readonly Guid GetCannedMessageListByProfileID
		= new Guid("5CC3EE2B-F795-4a50-8734-AB71560BB456");

	#region Marker

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid GetMarker
		= new Guid("8F93EE11-DFFF-44e1-B1F4-21B86332D89A");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid GetMarkerUpdateCount
		= new Guid("CCCC1634-761E-445e-84FF-459B15F0F4BD");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.Extra.cs

	public static readonly Guid ClearMarkers
		= new Guid("56D17CF6-5E95-46AA-8077-D76FB6DCB298");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid GetMarkerListByTimeStamp
		= new Guid("be2daac8-4a93-4f45-b5da-5e195f28bdb8");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid GetMarkerList
		= new Guid("ec30a951-a732-480e-ab20-964998970e2c");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid SaveMarker
		= new Guid("3d871e09-4963-4f14-8db6-777d3ae8b305");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid SaveMarkerList
		= new Guid("d440c0c1-b6e9-411b-845a-3ec4ff269164");

	// Imarda360Tracking\Imarda360TrackingBusiness\Control\Marker\Marker.cs

	public static readonly Guid DeleteMarker
		= new Guid("07a1625c-1508-47dc-b239-97ccc933843c");

	#endregion

	#endregion

	#region VehicleManagement

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs
	public static readonly Guid GetDriver
		= new Guid("9d36cae9-6b08-4c05-8781-b2553c39bf48");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid GetDriverUpdateCount
		= new Guid("787760da-f1d9-4fc1-a264-4c95798f44f1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid GetDriverListByTimeStamp
		= new Guid("c5cba35c-b881-4816-953a-84fcf16b0e5c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid GetDriverList
		= new Guid("30c1c165-ee19-4d2d-b4a6-cbaad61717c5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid SaveDriver
		= new Guid("f3a73f2c-e7cc-4dd0-9965-13c7952490b5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid SaveDriverList
		= new Guid("2ccb2691-dfd2-4519-a461-3c78c2b2c9e0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.cs

	public static readonly Guid DeleteDriver
		= new Guid("b74af1bb-9618-45a5-b6a9-c5b66a3c5b7c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.Extra.cs

	public static readonly Guid GetDriverListBySearch
		= new Guid("ef7318e5-1cf2-4082-af1a-304db7d27f80");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Driver\Driver.Extra.cs

	public static readonly Guid GetDriverListByDriverGroupID
		= new Guid("DBE5A8F5-CC3F-4be3-AEC9-950896B009CF");

	//-----
	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs
	public static readonly Guid GetDriverGroup
		= new Guid("cedebc1a-6637-48ff-bb9e-36c7394157d8");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid GetDriverGroupUpdateCount
		= new Guid("961fe04f-9b98-4da5-9fac-c197b3d830b0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid GetDriverGroupListByTimeStamp
		= new Guid("c0f35621-1887-4228-b79f-cb3be5631353");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid GetDriverGroupList
		= new Guid("bb71feb5-54d2-4776-8201-c08583382ddd");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid SaveDriverGroup
		= new Guid("d22804d5-88cf-4883-97f6-85a55f83d933");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid SaveDriverGroupList
		= new Guid("ada5bd43-5392-45aa-b316-3c262cbebb7b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.cs

	public static readonly Guid DeleteDriverGroup
		= new Guid("a5efbbcc-c654-42a9-b998-be03cd1a6f08");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.Extra.cs

	public static readonly Guid GetDriverGroupByDriverID
		= new Guid("18A29774-3245-4a0a-B343-4570A35D71E4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.Extra.cs

	public static readonly Guid AssignDriversToDriverGroup
		= new Guid("e97a12d2-f42c-42cb-b33f-9f1d1ad7353a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroup\DriverGroup.Extra.cs

	public static readonly Guid AssignDriverGroupsToDriver
		= new Guid("17c6eecd-dd91-44f9-af89-2bc6cc1185de");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs
	public static readonly Guid GetDriverGroupDriver
		= new Guid("7d3313a3-9f3f-4082-82cc-88db4cdb5b50");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid GetDriverGroupDriverUpdateCount
		= new Guid("eeb6bceb-0434-450f-a02d-f74883f2ed4a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid GetDriverGroupDriverListByTimeStamp
		= new Guid("b3336011-3cec-4f4f-acee-fe52dc98a5e4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid GetDriverGroupDriverList
		= new Guid("22bf5c66-39d8-4b14-b826-69c3105b10b2");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid SaveDriverGroupDriver
		= new Guid("0db5f332-aa67-4ae5-bc58-2481fe3e8121");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid SaveDriverGroupDriverList
		= new Guid("bc4bceac-a87f-4b5f-b69b-5a59e5e9cdcf");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverGroupDriver\DriverGroupDriver.cs

	public static readonly Guid DeleteDriverGroupDriver
		= new Guid("d79b1efc-76a9-4d61-80a3-292632708588");

	//-----

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverTrace
		= new Guid("27b6f4f6-5d37-40af-baaa-a9bd31ff8ab2");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverTraceByDriver
		= new Guid("9D724A5D-2D80-434d-B025-567E8817EAD7");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverTraceUpdateCount
		= new Guid("3fbf49d4-4758-4d4e-a85c-0de1a97d5911");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverTraceListByTimeStamp
		= new Guid("14cfd38e-f4f7-4d1f-9268-feaee9b1b723");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverTraceList
		= new Guid("2ad52bf2-be69-442c-a9d3-615a663a1178");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid GetDriverFatigueCounterList
		= new Guid("1E384938-C738-4ea7-9925-03516023F834");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid SaveDriverTrace
		= new Guid("cbcfd007-141d-4df0-a011-a5bd8780958e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid SaveDriverTraceList
		= new Guid("b0ca91a4-15d4-471b-98a3-b041d7fcb886");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\DriverTrace\DriverTrace.cs

	public static readonly Guid DeleteDriverTrace
		= new Guid("71ee2f49-9112-4696-954a-eeab9f8eb66d");

	/*checklist*/
	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Checklist\Checklist.cs

	public static readonly Guid GetChecklist
	= new Guid("B07DDECA-F615-40CE-9BDC-FBEA126AEBFC");

	public static readonly Guid GetChecklistGroup
	= new Guid("2463517A-5479-4FE4-9D60-272F9326A305");

	public static readonly Guid GetChecklistbyGroupID
	= new Guid("DC56DDA9-CFEE-4D2E-88AE-FDE734C4A307");

	public static readonly Guid GetChecklistPermissionsbyGroupID
	= new Guid("789EBD45-7B15-4E66-8689-60FA6A488443");

	public static readonly Guid GetChecklistList
	= new Guid("6aeb770d-5b48-4691-9cc8-e50baedad094");

	public static readonly Guid GetCompanyChecklistsWithInactive
	= new Guid("6777CDE6-CFD5-4D94-B79E-3756F3B96CDE");

	public static readonly Guid GetChecklistListByGroup
	= new Guid("9A2969D7-B621-4046-82B4-CC33E13B2599");

	public static readonly Guid GetChecklistListByGroupInactive
	= new Guid("E3DFFDBE-03E6-4E25-8A84-AA9D7872711E");

	public static readonly Guid SaveChecklist
	= new Guid("65e23fe0-a762-4b7e-91f5-8c85dcf85279");

	public static readonly Guid SaveChecklistItem
	= new Guid("18594FB4-469B-44A9-8B85-9B89E589CCB7");

	public static readonly Guid SaveChecklistItemList
	= new Guid("98AE8A8D-2EF9-4B2C-B73F-F4CBA35B3DD0");

	public static readonly Guid SaveChecklistImage
	= new Guid("EA6FB29F-4DF9-4076-8001-0BE0D4119211");

	public static readonly Guid SaveChecklistGroup
	= new Guid("2EF6AA79-015F-49DB-ABFC-EDA4983F1DA9");

	public static readonly Guid SaveChecklistUserOptions
	= new Guid("3F99CA49-1BC6-42B6-85A9-816733A1DBC2");

    public static readonly Guid SaveOutstandingItem
    = new Guid("993E9E4C-38E8-4C5D-B638-CD5B8069786F");

	public static readonly Guid GetChecklistUserOptions
	= new Guid("D7CFF85F-9378-4D50-9F46-C20948A42F33");

	public static readonly Guid GetSubmittedChecklistItemList
	= new Guid("32FD506D-8B35-49A8-A0C0-E152C52C721A");

	public static readonly Guid GetSubmittedChecklistInfo
	= new Guid("4EF5C8E0-C609-4884-9CE2-E2A58E515F1E");



	public static readonly Guid GetChecklistGroupList
	= new Guid("67C8F7E0-553F-42C2-8407-8ABB0D8E11E4");

	public static readonly Guid GetChecklistItemList
	= new Guid("5C3A72EE-9D73-40A5-AC2F-2A577F919D48");

	public static readonly Guid GetChecklistItem
	= new Guid("1BDF09AC-346C-4C94-9C59-1BF1FF3445F7");

	public static readonly Guid GetChecklistImage
	= new Guid("65700333-3584-464B-AA01-064A4698EB2E");

	public static readonly Guid GetChecklistIcons
	= new Guid("A4C4F105-AC8C-4D15-BE81-DF4847003851");

    public static readonly Guid GetSubmittedChecklistAttachmentList
    = new Guid("9842FAC5-C14F-4E6D-8759-E4A3DBB47415");
  
	public static readonly Guid SaveChecklistGroupPermissionList
	= new Guid("6468CA02-DB4C-48C6-B88A-D961ACA6CEFA");

	public static readonly Guid DeleteChecklistGroupPermission
	= new Guid("9CEC7FA4-993F-47FF-8A47-8E44CA421045");

	public static readonly Guid SaveChecklistGroupItemList
	= new Guid("0ECBB0BD-4896-4D73-AB6A-F7AD956951F0");

	public static readonly Guid DeleteChecklistGroupItem
	= new Guid("0CA0C032-0AAD-46D5-B194-D5813A426E19");
	
	/*checklist*/

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid GetFleet
		= new Guid("52d2cf69-799d-48e9-b6f4-0888e01082e7");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid GetFleetUpdateCount
		= new Guid("065ce84b-b6de-4bfa-b072-a2c8214707d6");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid GetFleetListByTimeStamp
		= new Guid("6eb0af96-c466-4a31-83d5-1062d49ab12d");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid GetFleetList
		= new Guid("6aeb770d-5b48-4691-9cc8-e50baedad094");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid SaveFleet
		= new Guid("65e23fe0-a762-4b7e-91f5-8c85dcf85279");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid SaveFleetList
		= new Guid("cd74125f-794e-42f9-8e3a-07809d098e46");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.cs

	public static readonly Guid DeleteFleet
		= new Guid("77e5475c-9f6e-49f7-99d4-d1d7cb1dfe14");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetInstalledVehicles
		= new Guid("7347e4b4-483d-4537-9b8e-a6c42a11ea94");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetUnInstalledVehicles
		= new Guid("ec99fe06-5865-4cdb-88c3-d85ed8eb2189");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetInstalledUsers
		= new Guid("991ABF1D-8FCC-4047-85ED-5CAAB46CA30B");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetUnInstalledUsers
		= new Guid("2EE45D50-7A57-4095-B5E4-22D31B1DE98E");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetInstalledAndUninstalledVehicles
		= new Guid("bb8e9f5f-f689-42de-a098-4c3b6fa93a36");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetInstalledAndUninstalledUsers
		= new Guid("F37DF511-3B4C-4114-AAF6-23CE775D30FD");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid InstallVehiclesToFleet
		= new Guid("ddf1f1d1-f05e-466c-8e4c-342104e37fb1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid InstallUsersToFleet
		= new Guid("DD2BC020-ADE1-4ea3-BED7-CD74C48D1FBA");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid GetService
		= new Guid("76698ccf-5d34-4c5e-9ad7-b1a0ef46741c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid GetServiceUpdateCount
		= new Guid("80ae5181-86d8-4bfd-9977-fce263f4daef");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid GetServiceListByTimeStamp
		= new Guid("16464261-d292-4443-8371-42affd317616");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid GetServiceList
		= new Guid("8e6520b4-7787-4926-bcba-8249b8b8cbab");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid SaveService
		= new Guid("8f8ef26e-e5fe-4e5c-894b-84f804da3c5a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid SaveServiceList
		= new Guid("dbdbeefc-0588-455d-8020-1053fb69c74c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.cs

	public static readonly Guid DeleteService
		= new Guid("fa84779d-c0cd-431b-ba34-17b437abee6f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service\Service.Extra.cs

	public static readonly Guid SetDeleteServiceList
		= new Guid("215d3443-6496-4663-bd78-3772c5a47daa");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid GetServiceItem
		= new Guid("064e5fd7-2aeb-4dd0-8898-f822cddafc84");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid GetServiceItemUpdateCount
		= new Guid("370112a5-a980-4553-a018-ce987828d7fc");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid GetServiceItemListByTimeStamp
		= new Guid("f99aa8e9-c164-470b-a41c-5d5066d75498");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid GetServiceItemList
		= new Guid("248605d3-71ff-4582-856e-bb57cca50639");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid SaveServiceItem
		= new Guid("c3ee1ced-3729-42a5-9c9d-4db164f19adc");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid SaveServiceItemList
		= new Guid("329dbf4f-42e6-44ca-b197-eeda2ebe5ac1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.cs

	public static readonly Guid DeleteServiceItem
		= new Guid("e79dd945-dcdc-4378-877b-6630ea424db4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.Extra.cs

	public static readonly Guid SetDeleteServiceItemList
		= new Guid("ceb0923c-e532-41be-aa8e-47ec59c968c5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceItem\ServiceItem.Extra.cs

	public static readonly Guid GetServiceItemListByServiceID
		= new Guid("e84a02f8-fa67-4166-a1de-328b9a3aaae8");

	public static readonly Guid GetServiceItemListByVehicleID
		= new Guid("f9c60f62-ed45-4a63-9428-f42ccbe3753f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid AddVehiclesToServiceManager
		= new Guid("94b49d9a-ad40-4883-8b42-dca9ac88ac1c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid AddServiceToServiceVehicle
		= new Guid("c87d1368-ca2e-4059-8f2b-89b7d7a22de1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid AddServiceItemsToServiceVehicle
		= new Guid("40e4f7ae-7e1e-47cf-a784-be0a1f98f624");

	public static readonly Guid GetNextServiceInvoiceNumber = new Guid("c8b3b909-48e1-4b12-a518-b120789c3277");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetVehiclesNotInServiceManager
		= new Guid("232bdb0a-16e1-4dd0-86e8-ef212c5b5755");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetVehiclesInServiceManager
		= new Guid("53dbc47a-13bf-487d-889a-8c4655445ebb");

	public static readonly Guid GetVehiclesNeedSendServiceReminder
		= new Guid("2f1e2e0f-c7a7-437a-9223-0b3626e795f4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetServiceItemsNearDue
		= new Guid("434967c8-9e2f-45b4-bf6c-f69a77dfa71b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid UpdateNextDueOdometer
		= new Guid("4a6a55ac-f52b-4014-8809-0ff56df3d9fb");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetServiceVehicleServiceItemsByVehicleID
		= new Guid("9a4d635e-ec07-4db2-bb53-8a4d8254706d");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid DeleteServiceVehicleServiceList
		= new Guid("f6630784-019b-4c67-84b0-2622dddab3ae");

	public static readonly Guid DeleteServiceVehicleServiceItemByServiceVehicleID
		= new Guid("fb8a4f4e-afab-47da-a950-d54f63b71bd3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid SaveServiceVehicleServiceItemsByServiceVehicle
		= new Guid("8aff22de-6abc-43ec-b9c2-336d6acad512");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetServiceVehicleByVehicleID
		= new Guid("7d1c40f2-e331-474f-b7be-af731cea3625");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetServicesByVehicleID
		= new Guid("867c7b49-0e72-4f29-99b1-6595bbd22abe");

	public static readonly Guid GetServiceVehicleServicesByVehicleID
		= new Guid("bc5b5dad-808b-435b-a5f8-70c1a3bb8ac3");

	public static readonly Guid OptoutService
		= new Guid("cc723434-604a-48a2-8f18-b734a36297ee");

	public static readonly Guid CleanServiceScheduleByCompanyID
		= new Guid("057d6c5f-1f19-4145-b6fb-f9658f9b56e2");

	public static readonly Guid CleanServiceScheduleByVehicleID
		= new Guid("9b60758b-df08-4f33-a088-707a64247d87");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid GetServicesNotInstalledByVehicleID
		= new Guid("9fa8b25d-c3c2-4e98-becc-af6e6d39da39");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceManager\ServiceManager.cs

	public static readonly Guid SaveServiceVehicleServiceByServiceVehicle
		= new Guid("e58f8d14-9da2-44fa-a997-b76c5a1149f0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid GetServiceNotification
		= new Guid("2684e161-eceb-4047-b577-bb81ef13a675");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid GetServiceNotificationUpdateCount
		= new Guid("cf2975d0-964d-45ef-8519-df9fad39eded");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid GetServiceNotificationListByTimeStamp
		= new Guid("6c5da8e1-1895-4ce9-826b-1b498bb59137");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid GetServiceNotificationListByServiceID
		= new Guid("bf0c4ff8-35d4-497b-ad20-5b30bdae6f28");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid GetServiceNotificationList
		= new Guid("b5f1a706-48d1-45d6-a727-e2cc43752f23");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid SaveServiceNotification
		= new Guid("7878496c-9fff-4fcd-ab62-303ef00d0581");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid SaveServiceNotificationList
		= new Guid("f7b33340-2dbb-44cb-96ef-705b3e7d7c35");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceNotification\ServiceNotification.cs

	public static readonly Guid DeleteServiceNotification
		= new Guid("cd096281-d9f2-4e26-8b5b-c948e1d1729d");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid GetServicePerformed
		= new Guid("09f6da32-03b9-4489-b913-29b403377416");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid GetServicePerformedUpdateCount
		= new Guid("c2fd0ca7-04de-430e-bb1d-f02df63e725b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid GetServicePerformedListByTimeStamp
		= new Guid("b529d5d5-3ff8-4f85-8be0-b76d9a328bf5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid GetServicePerformedList
		= new Guid("48c1e688-912e-4cd5-b73b-6b97fa1cb6d5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid SaveServicePerformed
		= new Guid("ca6d334e-7a47-46b5-ab04-a5ac0bac738f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid SaveServicePerformedList
		= new Guid("4210ae5b-4650-4234-aa87-7027351606d4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.cs

	public static readonly Guid DeleteServicePerformed
		= new Guid("56d9a310-37d3-4d51-b370-c7638cba2b04");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.Extra.cs

	public static readonly Guid GetServicePerformedListByVehicleID
		= new Guid("731ceb18-218a-4ff2-b950-fdd2e4d51573");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServicePerformed\ServicePerformed.Extra.cs

	public static readonly Guid GetServicePerformedListBySVSIID
		= new Guid("a4cb8d9f-cb8a-4495-aa22-3792ce5c2efe");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid GetServiceSchedule
		= new Guid("c35b13df-44f4-4f1d-8b17-6a8e3b93e22e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid GetServiceScheduleUpdateCount
		= new Guid("74db6871-357a-4c2a-b868-ec84daec51ec");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid GetServiceScheduleListByTimeStamp
		= new Guid("7a7bb3ed-88a6-41d3-89a0-04462e8ebf4b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid GetServiceScheduleList
		= new Guid("667f594d-7a6c-485c-8202-2b48fa197a96");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid SaveServiceSchedule
		= new Guid("34bd106f-555b-45d8-911f-874a9fc3429a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid SaveServiceScheduleList
		= new Guid("d4428a62-9fe7-481f-9549-355904277a70");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceSchedule\ServiceSchedule.cs

	public static readonly Guid DeleteServiceSchedule
		= new Guid("86e9d728-8cb7-4033-bccc-96811d8c0abd");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid GetServiceTemplate
		= new Guid("3f534213-7885-40f2-8629-f1ee774be121");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid GetServiceTemplateUpdateCount
		= new Guid("21ee5d5e-dc70-4ca2-b7a0-5a30d1122ebc");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid GetServiceTemplateListByTimeStamp
		= new Guid("953c1788-2c09-476f-8975-213619dc48c5");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid GetServiceTemplateList
		= new Guid("c9aa1f76-57e8-4179-abe8-15a935432c30");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid SaveServiceTemplate
		= new Guid("c697111b-3195-49c1-b525-0e61f673509c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid SaveServiceTemplateList
		= new Guid("7ad84267-00fe-4337-bf06-18bcddecc924");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplate\ServiceTemplate.cs

	public static readonly Guid DeleteServiceTemplate
		= new Guid("6c0b5b0e-d9b7-4d80-ac94-26a16949ff9b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid GetServiceTemplateItem
		= new Guid("935f41bb-1b7c-47a9-8b19-f9e90ffd78c3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid GetServiceTemplateItemUpdateCount
		= new Guid("a07e13b1-65b9-4ba8-910e-dd79acaa2412");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid GetServiceTemplateItemListByTimeStamp
		= new Guid("235b6b3e-fc05-49be-a684-2b4bffe64f26");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid GetServiceTemplateItemList
		= new Guid("8fbf5ce7-afab-4ff1-9d31-1e12e7f2932e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid SaveServiceTemplateItem
		= new Guid("e52d7c8b-c9b6-4304-afd0-28281898caae");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid SaveServiceTemplateItemList
		= new Guid("bd64abf3-12ca-4c36-8809-0bb33129ef7f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceTemplateItem\ServiceTemplateItem.cs

	public static readonly Guid DeleteServiceTemplateItem
		= new Guid("50634474-2d7b-4e4e-b591-c55ee4fea8a6");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid GetServiceType
		= new Guid("3a0fcbd8-a1a1-45a7-aaef-486798bcf585");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid GetServiceTypeUpdateCount
		= new Guid("363a6261-1682-4774-987d-710199415db1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid GetServiceTypeListByTimeStamp
		= new Guid("4a963da4-7e13-449b-bcf8-54db344147f1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid GetServiceTypeList
		= new Guid("0d7b9d16-7c6b-4ef1-8b5b-b554e338b128");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid SaveServiceType
		= new Guid("2e29cef3-4faa-424e-9181-2a6efdcfb6ce");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid SaveServiceTypeList
		= new Guid("53490eaf-18c2-4275-9562-0dcf01772236");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceType\ServiceType.cs

	public static readonly Guid DeleteServiceType
		= new Guid("3e4eca58-09cf-463c-959e-92f3d9854c36");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid GetServiceVehicle
		= new Guid("4ea0432c-74d4-4858-948c-79e376012a10");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid GetServiceVehicleUpdateCount
		= new Guid("a46d544f-680b-4929-9c35-9fa797616125");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid GetServiceVehicleListByTimeStamp
		= new Guid("0c3cb99a-f7cc-4bd8-a499-7f963b70b709");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid GetServiceVehicleList
		= new Guid("5b5374d5-6f87-4959-9bc0-f5f84285126b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid SaveServiceVehicle
		= new Guid("69ef64bf-0b4d-477b-ad05-eb846fd03447");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid SaveServiceVehicleList
		= new Guid("0378d242-9803-4327-bc53-72fa6bbbd429");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle\ServiceVehicle.cs

	public static readonly Guid DeleteServiceVehicle
		= new Guid("db70afc6-4b5e-4d7b-9988-dd5f55004aa7");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid GetServiceVehicle_Service
		= new Guid("95824471-7142-4d71-bf7a-8fd5100d0706");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid GetServiceVehicle_ServiceUpdateCount
		= new Guid("e7fc49e4-0297-44ee-817f-baca0955bfeb");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid GetServiceVehicle_ServiceListByTimeStamp
		= new Guid("23ae0922-dbfe-4b75-9640-4e658e2db647");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid GetServiceVehicle_ServiceList
		= new Guid("7fb6ea4e-3101-4c6d-ba73-9cc3e1d60558");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid SaveServiceVehicle_Service
		= new Guid("cceb884b-c943-4063-9def-301f2908cb2a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid SaveServiceVehicle_ServiceList
		= new Guid("967b112a-ceb2-4ff4-94f5-d8c168267858");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_Service\ServiceVehicle_Service.cs

	public static readonly Guid DeleteServiceVehicle_Service
		= new Guid("7ca9c865-a17c-4227-9804-d2ef3d59bd78");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid GetServiceVehicle_ServiceItem
		= new Guid("c91262d0-9776-459c-837a-9e088c69513c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid GetServiceVehicle_ServiceItemUpdateCount
		= new Guid("50401e16-f087-44b4-a4d4-14339dd0002e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid GetServiceVehicle_ServiceItemListByTimeStamp
		= new Guid("2fb049dd-42ca-4b42-b055-e44873ad20e7");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid GetServiceVehicle_ServiceItemList
		= new Guid("6908f611-a0c3-47c3-8cad-30646edadae1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid SaveServiceVehicle_ServiceItem
		= new Guid("7f43ebaa-526e-44fd-9a90-a73b1db19057");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid SaveServiceVehicle_ServiceItemList
		= new Guid("24d0d85c-4505-48d2-8295-08f1b7e67723");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\ServiceVehicle_ServiceItem\ServiceVehicle_ServiceItem.cs

	public static readonly Guid DeleteServiceVehicle_ServiceItem
		= new Guid("6930a309-defa-4afa-91a0-3af2acbd9563");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid GetService_ServiceItem
		= new Guid("0b31e5a9-8e60-403d-8f91-3a14bea1fcc3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid GetService_ServiceItemUpdateCount
		= new Guid("8bcbf52d-3115-48d6-becf-f0f6a992b75c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid GetService_ServiceItemListByTimeStamp
		= new Guid("c4545505-d9b6-4810-a4d5-a93a8d6099b0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid GetService_ServiceItemList
		= new Guid("2b250596-3d52-4443-8bb6-722b0b471da6");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid SaveService_ServiceItem
		= new Guid("38643761-e57c-4d2a-8137-9c8de25c6d6f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid SaveService_ServiceItemList
		= new Guid("7121f1b0-bc84-43cf-91b0-36f680559fe8");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.cs

	public static readonly Guid DeleteService_ServiceItem
		= new Guid("469b1672-2275-4339-bf78-f124ab6732be");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceItem\Service_ServiceItem.Extra.cs

	public static readonly Guid SaveService_ServiceItemListByServiceID
		= new Guid("312fc29b-d561-4ae0-8f80-8ce15e2faa72");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid GetService_ServiceVehicle
		= new Guid("f60ab893-f310-4982-843e-a9a8baff433e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid GetService_ServiceVehicleUpdateCount
		= new Guid("18dd02bd-df87-4fe1-bbfb-af9b5d6d52d7");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid GetService_ServiceVehicleListByTimeStamp
		= new Guid("c15f7f05-341d-4cd4-9831-7265b7912429");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid GetService_ServiceVehicleList
		= new Guid("1c767539-22ab-4ea0-a99b-14ab39bbaf07");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid SaveService_ServiceVehicle
		= new Guid("95120022-18ae-40f8-a4d0-e522d85bacb3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid SaveService_ServiceVehicleList
		= new Guid("c7c88a28-c508-464b-9f58-5ea250279e02");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Service_ServiceVehicle\Service_ServiceVehicle.cs

	public static readonly Guid DeleteService_ServiceVehicle
		= new Guid("97db83b7-0a90-417d-93a1-87748b7f10fe");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid GetUserFleet
		= new Guid("47cb2d20-da2c-4f9b-a303-0605fcc61932");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid GetUserFleetUpdateCount
		= new Guid("bedc48af-a5ec-4a72-8c69-2e1029ac434d");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid GetUserFleetListByTimeStamp
		= new Guid("8c451501-1600-4442-bbae-3712784087c9");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid GetUserFleetList
		= new Guid("dce7d5c4-a840-487d-a070-29e27645c763");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid SaveUserFleet
		= new Guid("1897e836-3d2f-4885-9f4c-5f3787ba0e7e");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid SaveUserFleetList
		= new Guid("3e099f47-df44-4937-9e1a-5481ef440120");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid InstallFleetsToUser
		= new Guid("39FFD222-D317-4146-BA36-6D13E0E7F3FD");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\UserFleet\UserFleet.cs

	public static readonly Guid DeleteUserFleet
		= new Guid("13f9c1bd-ecc7-4ccb-8b09-af3543ca8b5a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid GetVehicle
		= new Guid("4d706059-43e3-400e-994a-68889a1be4f4");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid GetVehicleUpdateCount
		= new Guid("70742109-5fdf-4a3f-9508-ea4ca57edcc1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid GetVehicleListByTimeStamp
		= new Guid("33e58076-de0c-44dd-b637-1d86b243df33");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid GetVehicleList
		= new Guid("0ed30924-e6ff-4c1e-997b-7a709884471b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid SaveVehicle
		= new Guid("9554c6ee-bdee-4452-8783-a6c10737bd85");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid SaveVehicleList
		= new Guid("a987ead5-f093-42b1-9876-328508c0e681");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.cs

	public static readonly Guid DeleteVehicle
		= new Guid("00844633-f711-49f4-8c56-9ff770451d84");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs
	public static readonly Guid GetVehicleSeedList
		= new Guid("C4DCB156-811E-4d0f-94BF-72006420EFF8");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs
	public static readonly Guid GetCO2Emission
		= new Guid("bf90ef1d-eacf-409d-a057-591c67c42fdd");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs
	public static readonly Guid GetInstalledDrivers
		= new Guid("bba96f11-7730-4594-ab98-b0c58a0ee0d3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetUnInstalledDrivers
		= new Guid("82e892dd-c47f-4379-9396-7f5adeaa2635");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetInstalledAndUninstalledDrivers
		= new Guid("2cab3eba-5909-4ccc-b28e-29f33847ba55");
	public static readonly Guid GetVehicleDrivers
		= new Guid("849F532D-B537-477a-A660-4FB6136E1E0F");
	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid InstallDriversToVehicle
		= new Guid("b853b03e-2f12-40f3-b8b1-a01ae3f6d769");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetInstalledFleets
		= new Guid("2f08a686-803a-492d-a848-46c4cf3dca60");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetUnInstalledFleets
		= new Guid("539eede1-2fe2-4a33-8bd7-f576c32576aa");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetInstalledAndUninstalledFleets
		= new Guid("01f049d2-ebfe-4630-90a7-a10b63982a12");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid InstallFleetsToVehicle
		= new Guid("a61f5ead-1f90-4dad-9c19-bede93ebab9f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetFleetFromVehicle
		= new Guid("3b703806-ac5f-489e-ae6a-a673bd15e25a");

	public static readonly Guid GetFleetsFromVehicle
		= new Guid("D0097CC0-FD46-4689-8B75-BBA3BF797CB9");

    public static readonly Guid GetPrimaryFleetFromVehicle
        = new Guid("E64AEB89-FFD4-4DDF-9B40-4F075FB987D2");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Vehicle\Vehicle.Extra.cs

	public static readonly Guid GetFleetListBySearch
		= new Guid("ae98fbb2-48c9-4b27-83e2-9d98ce8e68e0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid GetVehicleDriver
		= new Guid("802c2532-66d6-4ee1-b2ec-0602efbaa432");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid GetVehicleDriverUpdateCount
		= new Guid("107dae0d-0968-40af-afbe-189b68b8881c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid GetVehicleDriverListByTimeStamp
		= new Guid("761a87cf-662e-43f8-ae75-c5cafcbe7788");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid GetVehicleDriverList
		= new Guid("f847f314-0b20-4b32-a063-f65c99912378");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid SaveVehicleDriver
		= new Guid("fbb269b3-9f04-4140-9b22-88770262c1ce");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid SaveVehicleDriverList
		= new Guid("f4a2cd6a-7299-4b37-a88b-8adfd76f6f13");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleDriver\VehicleDriver.cs

	public static readonly Guid DeleteVehicleDriver
		= new Guid("0d1bfcae-ac89-452d-8cd8-73a1f8eaf9a1");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid GetVehicleFleet
		= new Guid("29c68e47-99bc-4bd5-a3cf-4a3d67aa3a52");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid GetVehicleFleetUpdateCount
		= new Guid("cf184fda-3ebd-4348-aec4-d4ffd1f09e1b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid GetVehicleFleetListByTimeStamp
		= new Guid("050badd4-49ce-4a9f-9e2c-03a66ff295ad");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid GetVehicleFleetList
		= new Guid("bcebb14d-427e-47af-8cc4-ce7a2f31d6cd");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid SaveVehicleFleet
		= new Guid("246de57d-f04f-48b5-9eb9-0f1ff711ad72");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid SaveVehicleFleetList
		= new Guid("371f5399-937a-473b-b8ba-fa52be317f94");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleFleet\VehicleFleet.cs

	public static readonly Guid DeleteVehicleFleet
		= new Guid("e0eea338-5768-488a-86d5-9c260d2ac812");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid GetVehicleImage
		= new Guid("4372d4d9-c564-4125-88ff-09ce79ba0d6c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid GetVehicleImageUpdateCount
		= new Guid("e72256f3-4786-45cb-a739-e3faf00a27c0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid GetVehicleImageListByTimeStamp
		= new Guid("a7c29de0-1efd-407f-bab3-24b380bb2e8c");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid GetVehicleImageList
		= new Guid("222ebbdd-a074-403a-8155-7e34150fe8c8");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid SaveVehicleImage
		= new Guid("14e17bb1-8251-48d7-8b05-9f3e01f4fb26");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid SaveVehicleImageList
		= new Guid("e8dc9396-4e4d-4655-bcd1-b05eeb52f55f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleImage\VehicleImage.cs

	public static readonly Guid DeleteVehicleImage
		= new Guid("03f6ada9-e6eb-470a-a5bb-a9f10ce7df90");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleManager\VehicleManager.cs

	public static readonly Guid GetTrackableVehicleListByFleet
		= new Guid("b0a89c30-0373-4452-a6d5-6b9e80b6438e");

	public static readonly Guid GetTrackableWithConexListByFleet
		= new Guid("ff5f17f7-bc25-4394-a4e7-f32fbefa57c1");

	public static readonly Guid GetTrackableVehicleListByFleetPaging
		= new Guid("46f4eece-66db-403a-a2bc-2fcb7cc6dfdc");

	public static readonly Guid GetTrackablesByFleetAndServicePaging
		= new Guid("f31e6fbe-f2b3-4108-b499-321f45df3c27");

	public static readonly Guid GetTrackableVehicleListByFleetCount
		= new Guid("be0dd7b9-27e0-4e51-87d1-d785f4db86ae");

	public static readonly Guid GetTrackableListByFleetCount
		= new Guid("86e9c261-3d73-41a6-98fc-ce6768b40a72");

	public static readonly Guid GetTrackablesByFleetAndServiceCount
		= new Guid("a92945ea-9173-47ea-992c-0f73b17f783d");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleManager\VehicleManager.cs

	public static readonly Guid GetUnitsInstalledInVehicle
		= new Guid("cdcf8079-95a2-4fbe-8ab8-57a2b5bdf7c0");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleManager\VehicleManager.cs

	public static readonly Guid GetUnitsInstalledToUser
		= new Guid("C2936936-89BC-4a9b-B3EA-8D34A4AA9484");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid GetVehicleType
		= new Guid("52ff262e-9cbb-4c38-b94f-6cbab39ef19b");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid GetVehicleTypeUpdateCount
		= new Guid("e2e5d48e-48da-4caa-91c4-51fcea9a3ddc");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid GetVehicleTypeListByTimeStamp
		= new Guid("4c3f4418-3418-4a15-a880-cd82baa2981a");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid GetVehicleTypeList
		= new Guid("a14035c2-2c15-4463-ace8-5f5f444ac729");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid SaveVehicleType
		= new Guid("227ba1f4-deaa-43eb-b661-8517f77232ea");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid SaveVehicleTypeList
		= new Guid("a893d125-6397-4a7b-b61c-77655ffb23ad");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\VehicleType\VehicleType.cs

	public static readonly Guid DeleteVehicleType
		= new Guid("c104aba9-b04b-4c4f-a805-5a987f944381");

	public static readonly Guid GetFatigueRuleSetByCodes
		= new Guid("B75DA92E-588B-4735-AAA8-9A48801A3DC9");

	public static readonly Guid GetFatigueRuleSetByRuleSetID
		= new Guid("f71524d7-cadc-46fb-aebc-16befeeb541d");		//& IM-3510

	public static readonly Guid GetFatigueRuleByRuleSetID
		= new Guid("11C26053-5CC0-424b-8110-9694564EB05D");

	public static readonly Guid GetFatigueRuleSetList
		= new Guid("{34C3A3A9-AA63-4d5e-B32C-0FCE2B3E9E42}");

	#region FleetAccess

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid GetFleetAccess
		= new Guid("51124cd6-4466-4031-aeb6-7d8b2f68c695");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid GetFleetAccessUpdateCount
		= new Guid("9ee44249-5848-4b0f-b906-321c5de5eb48");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid GetFleetAccessListByTimeStamp
		= new Guid("2db4454b-effb-4e2b-921a-69030f12a27f");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid GetFleetAccessList
		= new Guid("afd75682-220a-4d13-b932-ba8971fab7dd");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid SaveFleetAccess
		= new Guid("3cdd7437-00e6-430d-b61c-05cdb08591ec");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid SaveFleetAccessList
		= new Guid("1cf114e5-4b0b-4993-925a-598e7bc309a3");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\FleetAccess\FleetAccess.cs

	public static readonly Guid DeleteFleetAccess
		= new Guid("d8454330-1c93-462f-ab85-ceb1cba10b05");

	// Imarda360VehicleManagement\Imarda360VehicleManagementBusiness\Control\Fleet\Fleet.Extra.cs

	public static readonly Guid GetFleetsByUserID
		= new Guid("260fa344-e16c-4ff4-bc2c-eadbbc3f180f");

	public static readonly Guid GetFleetsOnCrmID
		= new Guid("A47E2FD6-F664-4c03-B3EC-A5B4CAC9B6CA");

	#endregion

	#endregion

	#region Provisioning

	// ImardaProvisioningBusiness\Control\ImardaProvisioning.cs

	public static readonly Guid SaveEntityList
		= new Guid("bda2b58f-e063-4964-89b6-e999c811ceac");

	// ImardaProvisioningBusiness\Control\ImardaProvisioning.cs

	public static readonly Guid SaveEntity
		= new Guid("f452d32a-a5c9-4ec2-81d0-cd7ef2a60d5d");

	// ImardaProvisioningBusiness\Control\ImardaProvisioning.cs

	public static readonly Guid GetEntityList
		= new Guid("3af4dad9-560d-4b56-820c-02ef52559af7");

	// ImardaProvisioningBusiness\Control\UCompany.cs

	public static readonly Guid GetUCompanyExtent
		= new Guid("8eb05370-02de-4c21-b0d0-2ef2211380eb");

	// ImardaProvisioningBusiness\Control\UCompany.cs

	public static readonly Guid GetUCompany
		= new Guid("10395a1b-82fe-49d7-ba95-2aec08fb05aa");

	// ImardaProvisioningBusiness\Control\UCompany.cs

	public static readonly Guid SaveUCompany
		= new Guid("415e689e-7187-468b-ba5e-5f78e25d47e8");

	// ImardaProvisioningBusiness\Control\UCompanyLocation.cs

	public static readonly Guid GetUCompanyLocation
		= new Guid("022247e0-b361-4498-ad37-20ee0a016b03");

	// ImardaProvisioningBusiness\Control\UCompanyLocation.cs

	public static readonly Guid GetUCompanyLocationListByCompanyID
		= new Guid("c3a0d04e-9e1c-4cd2-b873-af9f587aa82c");

	// ImardaProvisioningBusiness\Control\UConfigGroup.cs

	public static readonly Guid GetUConfigGroup
		= new Guid("c0e30529-2a6d-4335-92a6-e24e061a445d");

	// ImardaProvisioningBusiness\Control\UEmailGroup.cs

	public static readonly Guid GetUEmailGroup
		= new Guid("1afbbb2e-867e-4e64-b316-6902f4de3026");

	// ImardaProvisioningBusiness\Control\UEmailGroup.cs

	public static readonly Guid GetUEmailGroupListByCompanyID
		= new Guid("198b28ee-e51a-4c87-b010-19565341a3d3");

	// ImardaProvisioningBusiness\Control\UPerson.cs

	public static readonly Guid GetUPersonExtent
		= new Guid("4ff2fb19-122d-4c5b-bfbb-b7613405b514");

	// ImardaProvisioningBusiness\Control\UPerson.cs

	public static readonly Guid GetUPerson
		= new Guid("6f4ab468-345c-4169-b72b-25c656901d7f");

	// ImardaProvisioningBusiness\Control\UUser.cs

	public static readonly Guid GetUUserExtent
		= new Guid("db10fcb7-94bc-4c43-be5c-07152d933896");

	// ImardaProvisioningBusiness\Control\UUser.cs

	public static readonly Guid GetUUser
		= new Guid("b2d1709e-e96d-4ecb-a51d-2789a0b0dd18");

	// ImardaProvisioningBusiness\Control\UUser.cs

	public static readonly Guid GetUUserListByCompanyID
		= new Guid("b764c450-2172-41be-8e3c-adf3174b16b9");

	// ImardaProvisioningBusiness\Control\UReportExtent.cs

	public static readonly Guid GetUReportExtent
		= new Guid("66125873-6280-47b4-a73c-d326d8abb78c");

	#endregion

	// Other

	public static readonly Guid GetChartDataListByTimeStamp
		= new Guid("f17886f0-a631-4276-bac5-5333289d09c8");

	public static readonly Guid GetChartDataList
		= new Guid("4e2a5500-58ca-48b9-8f9f-acac233bc3dd");

	public static readonly Guid SaveChartDataList
		= new Guid("2026dffa-5b0d-4566-8789-59e51ae0f9af");

	public static readonly Guid SaveChartData
		= new Guid("7c9c0444-06ac-442a-a909-e77535fc5fe6");

	public static readonly Guid DeleteChartData
		= new Guid("d311ed18-0284-4534-9909-8a29cc8ea00c");

	public static readonly Guid GetChartData
		= new Guid("00b10f22-4c7a-4352-b5c5-8bb63a73a716");

	public static readonly Guid GetChartDataUpdateCount
		= new Guid("35157f90-4203-4dce-8197-4e9b98beafc8");

	public static readonly Guid GetChartDataset
		= new Guid("e445f2b3-bd0c-4079-ae67-1baee5ac0517");

	public static readonly Guid GetUnitLogListByUnitIDAndDateRange
		= new Guid("61669e8d-ae67-4c2c-93b7-adbe96896c6f");

	public static readonly Guid GetDriverPosition
		= new Guid("61669e8d-ae67-4c2c-93b7-adbe96896c6f");

	public static readonly Guid GetUnitGeofenceListByTrackID
		= new Guid("014AE88E-BC4E-411e-9D89-4CCC4C086FCD");

	public static readonly Guid DeleteGeofencePolygonsByGeofenceID
		= new Guid("473E1F95-9547-4f17-9E4C-140B7478A0B8");

	public static readonly Guid GetMessageCentreItemByReceiverIDAndMsgNum
		= new Guid("0718B702-3249-46f4-888A-E869C2BFC6FC");

	public static readonly Guid GetTrackableListByVehicleID
		= new Guid("A54C3AAA-83DD-497d-A1F6-B04B836FD382");

	#region CompanyGeofenceGroup

	public static readonly Guid GetCompanyGeofenceGroup
		= new Guid("69C9862F-80B6-4da7-9357-EB51AC14FF75");

	public static readonly Guid GetCompanyGeofenceGroupUpdateCount
		= new Guid("BB8800FA-ACAE-4cc8-9BEC-E4AB9951C6BE");

	public static readonly Guid GetCompanyGeofenceGroupListByTimeStamp
		= new Guid("9613D6FC-31D7-42fe-A468-49D02A88A360");

	public static readonly Guid GetCompanyGeofenceGroupList
		= new Guid("4420634C-DC4A-4fc7-BD8C-C30657667160");

	public static readonly Guid SaveCompanyGeofenceGroup
		= new Guid("4D887AB5-7BF0-40b2-A22A-B462E0034D63");

	public static readonly Guid SaveCompanyGeofenceGroupList
		= new Guid("6AE02BA6-D0AD-4477-A4A3-2532C21122B4");

	public static readonly Guid DeleteCompanyGeofenceGroup
		= new Guid("171B4E07-97F2-41b7-9364-9655FA73F568");

	public static readonly Guid GetCompanyGeofenceInstalledOnGroup
		= new Guid("4A427AF2-B3BB-4287-BF61-31A358D68E3B");

	public static readonly Guid DeleteCompanyGeofenceMapByGroupID
		= new Guid("309E36AD-65CB-4a15-9623-04594D864AE8");

	public static readonly Guid GetCompanyGeofenceListByGroup
		= new Guid("2442B4B5-B4CD-47c8-B5F1-225F1F3FD505");

	public static readonly Guid GetCompanyGeofenceGroupSeedList
		= new Guid("21F05C61-44D7-44b3-82D8-3ECACC2D6A78");

	public static readonly Guid GetUnitCommand
		= new Guid("1BEC0D46-7568-42d2-94DF-A97CCA032D44");

	public static readonly Guid GetUnitCommandList
		= new Guid("495B3EAB-40DD-4e68-B55B-1404AD639BCD");

	public static readonly Guid GetUnitCommandListByUnitType
		= new Guid("4322DB32-11BF-4e6b-AFE1-65B1F7A145D6");

	public static readonly Guid GetUnitCommandUpdateCount
		= new Guid("424D9170-FFEE-4309-BB66-EAD91DE54D90");

	public static readonly Guid GetUnitCommandListByTimeStamp
		= new Guid("5C3AB8A0-552B-457c-A120-4094C0F52F03");

	public static readonly Guid SaveUnitCommand
		= new Guid("816FFD32-BD13-4406-A31F-61CB0C54A90E");

	public static readonly Guid SaveUnitCommandList
		= new Guid("5373F54E-E0B1-4015-BE89-18635CF62297");

	public static readonly Guid DeleteUnitCommand
		= new Guid("BE590C7D-B4B5-427b-B4FC-DA31E1687F5E");

	public static readonly Guid GetActionPlanTriggerMapList
		= new Guid("81E87CF1-0501-4aa2-BE07-E4D1AB0E54E5");

	public static readonly Guid SaveActionPlanTriggerMapList
		= new Guid("85EE5EB2-F5C3-46df-8B97-2A1553CC5B21");

	public static readonly Guid GetActionPlanTriggerMapListByActionPlanID
		= new Guid("9AAC2733-7EBE-4c56-AB3A-3863970144A1");

	#endregion

	#region CompanyGeofenceMap

	public static readonly Guid GetCompanyGeofenceMap
		= new Guid("A44A407C-267F-4c55-BB2C-55A47014C678");

	public static readonly Guid GetCompanyGeofenceMapUpdateCount
		= new Guid("03AEB769-B898-403e-A4B9-17B55A1A5A37");

	public static readonly Guid GetCompanyGeofenceMapListByTimeStamp
		= new Guid("9E78AC4C-604C-4a81-87F9-DCE5346DD7BB");

	public static readonly Guid GetCompanyGeofenceMapList
		= new Guid("0C31D7C1-F207-4d37-B3EE-46689207092F");

	public static readonly Guid SaveCompanyGeofenceMap
		= new Guid("0C31D7C1-F207-4d37-B3EE-46689207092F");

	public static readonly Guid SaveCompanyGeofenceMapList
		= new Guid("DDC47A4D-77E4-4d6e-B918-5A925E02AB39");

	public static readonly Guid DeleteCompanyGeofenceMap
		= new Guid("A884501B-C1E7-4c29-BE7E-4B11D9664987");

	public static readonly Guid UpdateGeofenceMapList
		= new Guid("C095F575-375A-43DF-9CC7-53E5BFC21E50");

	#endregion

	#region UnitGeoEntityMap

	public static readonly Guid GetUnitGeoEntityMap
		= new Guid("AB5A1611-D75E-4583-AEE2-B9789B6D81A7");

	public static readonly Guid GetUnitGeoEntityMapUpdateCount
		= new Guid("02F39553-6F0C-442f-BFAC-2B1A5183F080");

	public static readonly Guid GetUnitGeoEntityMapListByTimeStamp
		= new Guid("9D1FC4A2-AE3A-4cbd-88FA-1385C1A4A6C2");

	public static readonly Guid GetUnitGeoEntityMapList
		= new Guid("13A65A9D-CDAB-4745-B2EC-2595BCA61F1A");

	public static readonly Guid SaveUnitGeoEntityMap
		= new Guid("EDAEB0CF-9C9A-4b98-A8CB-7B546A5F85DE");

	public static readonly Guid SaveUnitGeoEntityMapList
		= new Guid("020D31B0-DC02-464d-9C82-0F80C6F0F36C");

	public static readonly Guid DeleteUnitGeoEntityMap
		= new Guid("CB2286F4-FDE7-454b-8A44-3C869823A2CE");

	#endregion

	#region CannedMessageProfile

	public static readonly Guid SaveCannedMessageProfile
		= new Guid("568EC0F8-69BF-4783-8DFD-ED91690735D1");

	public static readonly Guid SaveCannedMessageProfileList
		= new Guid("08A1AC44-0A9F-4783-9E43-2C3496EE8DDE");

	#endregion

	#region Alerting

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid GetAction
		= new Guid("4b4e9bc6-9b84-4f88-91c0-74323800522a");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid GetActionUpdateCount
		= new Guid("bb6ad731-3141-4447-a0e1-f0511a1cee8c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid GetActionListByTimeStamp
		= new Guid("f58aafd4-c3e6-46fa-bf6e-0623c9212df1");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid GetActionList
		= new Guid("acaec4d3-1657-4371-bbe0-78a5b4930376");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid SaveAction
		= new Guid("4edb02c0-2acb-47c4-b512-1a873de602ff");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid SaveActionList
		= new Guid("ee1f183c-feed-46be-872a-09b6943f3a79");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Action\Action.cs

	public static readonly Guid DeleteAction
		= new Guid("b7b2dd4f-511f-48b8-9f10-499359f34376");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid GetActionPlan
		= new Guid("a764190c-0e9d-4cb7-aa67-d863d53476eb");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid GetActionPlanUpdateCount
		= new Guid("266d0733-395b-4a18-ad7d-ee304b2a140c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid GetActionPlanListByTimeStamp
		= new Guid("390339de-4838-4dd3-b097-35f472c0a0ca");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid GetActionPlanList
		= new Guid("36842090-0491-4377-b70c-7d8522908cdc");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid SaveActionPlan
		= new Guid("bb04ff5d-0b90-45cc-a4f1-e8d757dbbc29");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid SaveActionPlanList
		= new Guid("d1d4b5d1-9004-4e77-9315-294c0b4904f7");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlan\ActionPlan.cs

	public static readonly Guid DeleteActionPlan
		= new Guid("da2b25e5-664b-4ac0-beee-c7365cd2e8a9");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid GetActionPlanAction
		= new Guid("3b2ad055-fd8b-4110-bb1b-e10ce47177c3");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid GetActionPlanActionUpdateCount
		= new Guid("8d388288-473c-49b7-9251-28803797dbb8");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid GetActionPlanActionListByTimeStamp
		= new Guid("80f5ec05-3d4d-45d6-9f28-35293bccfb94");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid GetActionPlanActionList
		= new Guid("a35ef0c8-3c28-4b1b-9fd0-1ba4f7c36ed2");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid SaveActionPlanAction
		= new Guid("95e21d77-90ba-497b-89a6-373f7d0e385a");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid SaveActionPlanActionList
		= new Guid("daabb3d1-a291-4f9b-8919-2abeda9ed3c4");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\ActionPlanAction\ActionPlanAction.cs

	public static readonly Guid DeleteActionPlanAction
		= new Guid("e3546b26-2b6a-40c6-851c-66b639b1f868");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid GetAlertPlan
		= new Guid("3490be5d-7cdd-4b36-8cf6-cd5a498c4f0e");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid GetAlertPlanUpdateCount
		= new Guid("a5e8003a-1585-48a9-995d-b2e396235450");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid GetAlertPlanListByTimeStamp
		= new Guid("e8e224a4-869a-48b1-906d-19ecfd88e9a8");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid GetAlertPlanList
		= new Guid("d77509ec-7afd-4d13-a0d0-9c16b74e865c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid SaveAlertPlan
		= new Guid("b82fb7a9-3267-4c94-8241-fbfaaeb01291");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid SaveAlertPlanList
		= new Guid("abc7c234-a17c-4588-97bb-ca04e4b78995");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlan\AlertPlan.cs

	public static readonly Guid DeleteAlertPlan
		= new Guid("f760b7c2-1b2a-49ee-bd4c-b5406f9492b7");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid GetAlertPlanAction
		= new Guid("992bcaf2-7f74-4f6c-9e87-bfade81f2c9d");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid GetAlertPlanActionUpdateCount
		= new Guid("e22f27a9-4a16-44b4-8ff3-5a8be7646ff4");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid GetAlertPlanActionListByTimeStamp
		= new Guid("03730013-7d0e-4c84-8df7-bb0029cd5553");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid GetAlertPlanActionList
		= new Guid("7f11a7fb-29a9-4a05-8981-f5a4d11bf12c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid SaveAlertPlanAction
		= new Guid("55fbdab2-0976-4438-8036-f1b6c28879d1");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid SaveAlertPlanActionList
		= new Guid("e51003c1-cae6-439f-b2d5-58fe204f8b35");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanAction\AlertPlanAction.cs

	public static readonly Guid DeleteAlertPlanAction
		= new Guid("78b41a76-e0a7-4e30-8b73-2954670d16d8");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid GetAlertPlanEvent
		= new Guid("b152a402-e87f-467a-8d83-925ac9c33556");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid GetAlertPlanEventUpdateCount
		= new Guid("f8cec6dd-d379-458a-b73b-f8c5838fc977");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid GetAlertPlanEventListByTimeStamp
		= new Guid("dc830be3-931d-463a-b898-a545b7115375");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid GetAlertPlanEventList
		= new Guid("60b272be-9d4b-4a4c-9dab-0f3efd8cc42d");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid SaveAlertPlanEvent
		= new Guid("36b32c01-7e31-4fa5-9e14-dc03fab63173");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid SaveAlertPlanEventList
		= new Guid("9c2ffaa9-693d-4e3f-902b-4c7772297e21");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanEvent\AlertPlanEvent.cs

	public static readonly Guid DeleteAlertPlanEvent
		= new Guid("d149c8cb-e554-42f6-814e-3788b9e110e7");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid GetAlertPlanNotificationPlan
		= new Guid("aa3efa89-300e-499d-93f7-b72777d5679f");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid GetAlertPlanNotificationPlanUpdateCount
		= new Guid("88ed574a-f496-4ec9-ac25-ac2ec3acdc5c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid GetAlertPlanNotificationPlanListByTimeStamp
		= new Guid("b9df92a9-cafa-445e-8eed-d6362a001bb9");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid GetAlertPlanNotificationPlanList
		= new Guid("37397117-4337-45a9-95d5-1bb5d9ec4d27");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid SaveAlertPlanNotificationPlan
		= new Guid("940ad037-9185-4e3f-8697-72584fb7cfa4");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid SaveAlertPlanNotificationPlanList
		= new Guid("db279833-2069-4b8d-b908-9ea78a4775b9");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanNotificationPlan\AlertPlanNotificationPlan.cs

	public static readonly Guid DeleteAlertPlanNotificationPlan
		= new Guid("639eaaac-502e-4183-b233-94bb6692f519");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid GetAlertPlanRule
		= new Guid("99f3f7ca-cc44-4bba-8990-bd22ffdc059c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid GetAlertPlanRuleUpdateCount
		= new Guid("6a93d989-5dae-4d26-a53b-f46d1a66b263");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid GetAlertPlanRuleListByTimeStamp
		= new Guid("7ca9140d-cc69-4e90-ad15-330115a18679");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid GetAlertPlanRuleList
		= new Guid("923172ac-9239-44e2-b60a-5c8253511142");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid SaveAlertPlanRule
		= new Guid("d3c66b64-a941-4871-b2b1-e1629d14e434");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid SaveAlertPlanRuleList
		= new Guid("ed34607b-1336-4c0c-9e46-823009cffc88");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\AlertPlanRule\AlertPlanRule.cs

	public static readonly Guid DeleteAlertPlanRule
		= new Guid("4b8d6229-5d39-41bb-a488-b36e7d4286aa");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid GetEvent
		= new Guid("9c9c8b41-d207-409a-9c8e-5288362598eb");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid GetEventUpdateCount
		= new Guid("460a67b6-e943-493c-981c-1f631f521d85");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid GetEventListByTimeStamp
		= new Guid("ad80d3b9-304c-4df5-a5a5-312ba83e0a6b");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid GetEventList
		= new Guid("83717044-357e-4802-a59b-64e81c37557b");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid SaveEvent
		= new Guid("c9f9d4c8-0e29-400f-a148-bfad1524b268");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid SaveEventList
		= new Guid("0f355501-23d2-4ca6-91a8-2529aa5f340c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Event\Event.cs

	public static readonly Guid DeleteEvent
		= new Guid("8652c86c-de51-4a58-a53c-30ba0cf3bdd0");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid GetEventPlan
		= new Guid("aaddf73e-cf3a-4e11-abe5-6ec06322bc44");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid GetEventPlanUpdateCount
		= new Guid("adf9d400-a4c1-44ee-9f4b-29c4df8c633d");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid GetEventPlanListByTimeStamp
		= new Guid("29c30b6b-0b5d-46ed-b7fe-76027fdb5343");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid GetEventPlanList
		= new Guid("7789b970-c7f0-48ea-90b9-05f165d3777c");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid SaveEventPlan
		= new Guid("c7126fb7-9c0a-4873-b140-16b99a3d0d3a");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid SaveEventPlanList
		= new Guid("8887b3e6-d694-4b05-b570-cf551ca9c497");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlan\EventPlan.cs

	public static readonly Guid DeleteEventPlan
		= new Guid("d0c260ca-eba4-435a-9662-1b66825b1711");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid GetEventPlanEvent
		= new Guid("171fb9e1-6a77-422c-9974-d5e0b8a29818");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid GetEventPlanEventUpdateCount
		= new Guid("dcda2245-2f8b-49fc-aa22-5c86d54ff823");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid GetEventPlanEventListByTimeStamp
		= new Guid("ead941b4-bad5-48a0-b6d0-c3b269613924");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid GetEventPlanEventList
		= new Guid("11cd646e-e169-4b38-bd38-c516d510ec94");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid SaveEventPlanEvent
		= new Guid("dcefaa2d-042d-48c6-969b-1b6e897b1a77");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid SaveEventPlanEventList
		= new Guid("46a6102c-eb23-4805-9f20-09a38604a2ad");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventPlanEvent\EventPlanEvent.cs

	public static readonly Guid DeleteEventPlanEvent
		= new Guid("e62892e6-da86-468b-a949-145ac96fc3f8");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid GetEventLog
		= new Guid("5c1966dc-4f82-4a70-a48c-91786492b766");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid GetEventLogUpdateCount
		= new Guid("96d410fc-2da1-43da-87e8-3287de4a79c1");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid GetEventLogListByTimeStamp
		= new Guid("7edb6e2d-6c31-4bd1-aff3-7adda06a8645");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.Extra.cs

	public static readonly Guid GetLatestEvents
		= new Guid("ee095551-e607-4a13-bc1d-d28e7673956d");

		
	public static readonly Guid SetEventLogViewed
			= new Guid("8aa674f7-4d5c-4d6f-9616-9be37c3997c9");
	

	public static readonly Guid GetEventLogByOwnerID
		= new Guid("{3375CB18-B4F8-4362-9838-6240C45F8D1E}");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid GetEventLogList
		= new Guid("720fefd7-5101-4ba0-bf65-5b8dc4f113cd");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid SaveEventLog
		= new Guid("607ccb50-f14d-4823-a6ab-b855f4c16d90");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid SaveEventLogList
		= new Guid("0d7d13fb-f2b5-40c3-bbfb-b9f0aa7650e9");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\EventLog\EventLog.cs

	public static readonly Guid DeleteEventLog
		= new Guid("af111286-b96a-40a0-9866-e6956889d601");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid GetRule
		= new Guid("f51ae96f-e92c-49e2-90d2-626440649194");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid GetRuleUpdateCount
		= new Guid("5ea68857-e9c8-43cf-ac59-7f6f43460225");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid GetRuleListByTimeStamp
		= new Guid("efa6cd82-2fe3-4b51-adf2-8f23edb79d64");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid GetRuleList
		= new Guid("86ec71f1-bfda-467e-99e6-3e7803306755");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid SaveRule
		= new Guid("1e3f6ea9-3f5c-4d52-a3eb-4011d0ad98ac");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid SaveRuleList
		= new Guid("458f29f7-ddaa-4c16-9967-9166e70320f4");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\Rule\Rule.cs

	public static readonly Guid DeleteRule
		= new Guid("99b2119a-88d0-4e62-99a2-1bf0427a3f81");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid GetRulePlan
		= new Guid("3a445b62-567c-4538-b7ae-c024aa74b3e7");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid GetRulePlanUpdateCount
		= new Guid("b0d59f57-e698-4239-beb5-e86c1a479182");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid GetRulePlanListByTimeStamp
		= new Guid("7043564e-b80c-47b5-893a-65b18ecf8db9");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid GetRulePlanList
		= new Guid("73afde90-69f4-4019-9c3a-b2ebca08d11b");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid SaveRulePlan
		= new Guid("14a5936c-60b0-4e91-be1d-90d13b8ae960");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid SaveRulePlanList
		= new Guid("e64e58e1-cfd1-4e38-a886-7be76bdbe587");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlan\RulePlan.cs

	public static readonly Guid DeleteRulePlan
		= new Guid("9cdd324a-466b-4e24-b53b-103ec9e44a12");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid GetRulePlanRule
		= new Guid("fb91e540-c48a-4b40-b7c5-36a43df98e52");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid GetRulePlanRuleUpdateCount
		= new Guid("39c2398d-6e63-4670-bf8b-473f577a037e");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid GetRulePlanRuleListByTimeStamp
		= new Guid("0077b72d-f80b-4f8d-9741-d4a5c81556f8");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid GetRulePlanRuleList
		= new Guid("286391e5-e55c-43a4-9a4c-f339374786e6");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid SaveRulePlanRule
		= new Guid("0880d267-0a86-41ef-a941-464d56e546fe");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid SaveRulePlanRuleList
		= new Guid("80d38368-76ca-45b7-8b7c-14ff5310bad5");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\RulePlanRule\RulePlanRule.cs

	public static readonly Guid DeleteRulePlanRule
		= new Guid("b51c2d29-1037-42cd-8720-62843247d8eb");

	//Extra
	public static readonly Guid GetActionListByAlertPlanID
		= new Guid("60c11c43-2e5b-4b94-aa1b-c864ab814ab6");

	public static readonly Guid GetEventListByAlertPlanID
		= new Guid("723a011c-6816-4d7d-afd1-da71adb85366");

	public static readonly Guid GetRuleListByAlertPlanID
		= new Guid("94617076-44c9-4514-b016-d2eccbf10b23");

	public static readonly Guid GetActionPlanListByAlertPlanID
		= new Guid("2c1b9f78-f96b-45a7-ac3f-33b20e7a2228");

	public static readonly Guid GetEventPlanListByAlertPlanID
		= new Guid("af50a954-934c-49cd-b99e-3635508f8dfc");

	public static readonly Guid GetRulePlanListByAlertPlanID
		= new Guid("c7581014-4962-4642-a1cd-883250dc08dc");

	public static readonly Guid GetActionListByActionPlanID
		= new Guid("446eba13-8995-4d77-8687-4d7a2fb70c74");

	public static readonly Guid GetEventListByEventPlanID
		= new Guid("c049d982-c3a7-45e7-8338-3d9959a5a88e");

	public static readonly Guid GetRuleListByRulePlanID
		= new Guid("c9195f3c-7eb5-4848-b61b-8cff21968f6b");

	public static readonly Guid GetActionTemplateList
		= new Guid("c2450514-71f0-4539-a6a8-6db926d0194b");

	public static readonly Guid GetEventTemplateList
		= new Guid("edc796e4-8dba-425a-90e3-d11c650842ba");

	public static readonly Guid GetRuleTemplateList
		= new Guid("d89fc1fa-07db-42e6-8838-a475003a3404");

	public static readonly Guid InstallActionsToActionPlan
		= new Guid("aa2e73cf-3e51-47bb-9cef-bc363c0feaa3");

	public static readonly Guid InstallEventsToEventPlan
		= new Guid("9c85b68d-7a35-42f7-8e0a-72ded5404a63");

	public static readonly Guid InstallRulesToRulePlan
		= new Guid("ea4e01de-269c-4524-935b-9ae0990ab3b4");

	public static readonly Guid InstallActionsToAlertPlan
		= new Guid("9a90a5b8-8d91-4dd4-a0cb-0b10895cbdea");

	public static readonly Guid InstallEventsToAlertPlan
		= new Guid("3e5fb686-f014-4126-be70-f59ca5789f3b");

	public static readonly Guid InstallNotificationPlansToAlertPlan
		= new Guid("a5fb9492-0c5b-4f40-85e7-1f892c2a818a");

	public static readonly Guid InstallRulesToAlertPlan
		= new Guid("78ABF314-986E-409f-B7ED-85800D74DA03");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid GetIXLParameter
		= new Guid("14940f2e-1421-4b16-8691-d7e425596a8e");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid GetIXLParameterUpdateCount
		= new Guid("2a25da9d-a0a2-4251-a7ea-c988ba15438b");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid GetIXLParameterListByTimeStamp
		= new Guid("eb7ee29c-a716-4a02-b993-b218290577db");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid GetIXLParameterList
		= new Guid("33cce9c4-5089-4d7e-a77e-bffa1641818d");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid SaveIXLParameter
		= new Guid("3e050d61-eaa0-4acf-b671-e3ecfe8d2ce6");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid SaveIXLParameterList
		= new Guid("344ee282-5998-42e5-9581-a5635a1ffac2");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.cs

	public static readonly Guid DeleteIXLParameter
		= new Guid("cff39dc1-028d-47e2-affd-e6948585bb5f");

	// Imarda360Alerting\Imarda360AlertingBusiness\Control\IXLParameter\IXLParameter.Extra.cs

	public static readonly Guid GetUserAttributeListByCategoryID
		= new Guid("60ca7c04-d4cf-4af0-99cd-93c692d48a21");

	#endregion

	#region UI Menu

	//UI Menu
	public static readonly Guid VehicleManagerMenu
		= new Guid("4517274b-117a-4b84-93a6-047a1dc473fa");

	public static readonly Guid ChecklistManagerMenu
		= new Guid("3E68FE5A-E561-439A-ADCF-C39EC84169A3");

	public static readonly Guid DriverManagerMenu
		= new Guid("0b74f2a5-639e-4d12-b32b-6dd97cac5989");
    
	public static readonly Guid GeofenceManagerMenu
		= new Guid("444a0b69-821a-47e0-958b-0bbef2afed90");

	public static readonly Guid ServiceManagerMenu
		= new Guid("c8077554-cf0f-4dc2-a074-f4bb6597423d");

	public static readonly Guid UserManagerMenu
		= new Guid("6d3b0e4d-17d2-48bc-b181-d3e0a6e92178");

	public static readonly Guid FleetManagerMenu
		= new Guid("950c4df3-1592-49c3-bee1-b8878cbdde1d");

	public static readonly Guid DeviceManagerMenu
		= new Guid("179c3470-61af-4042-81f6-28fbbc94de9c");

	public static readonly Guid MessageCentreMenu
		= new Guid("d930a823-5a7f-4352-aab1-5cf872ed5799");
	
	public static readonly Guid SendMessage
		= new Guid("B8CD985C-A5C5-42E5-B39A-ECCDB7225E35");

	public static readonly Guid DashboardMenu
		= new Guid("F59C06E1-E9BF-482e-B7D1-2CBDEAE7B08F");

	public static readonly Guid AlertingManagerMenu
		= new Guid("A717F4BF-53F8-4448-9938-D56C34F996AE");

	public static readonly Guid ReportsMenu
		=// new Guid("84B5D3CD-9DE4-4610-BEE8-4E72BA4B716F");
		new Guid("7813e7b2-d641-4d17-b0a3-489e338b4f8d");//in authority tree view it's called report manager

	public static readonly Guid DiagnosticsMenu
		= new Guid("78C02561-1432-43F7-B0B6-73344F1A73D9");

	public static readonly Guid AssignFleetTab
		= new Guid("9C185A64-F013-471b-A692-B850650EE068");

	public static readonly Guid AssignPermissionTab
	= new Guid("221d3b8a-f85f-488c-b8f2-3edb2c6ecd84");

	public static readonly Guid EditUserTab
	= new Guid("ffff77b5-814b-4988-8970-1a08856a85fd");

	public static readonly Guid JobManagerMenu
		= new Guid("d1af0611-347a-49dd-88aa-1db7b9c094f7");

	public static readonly Guid DisplayManagerMenu
		= new Guid("53341e84-0fa9-4f33-aecf-ee137f6f02cd");

	public static readonly Guid ScheduledReport
		= new Guid("07213d95-8c17-44a3-93a2-8fe91cb7704f");

	public static readonly Guid AccountManager
			= new Guid("34e1c5ab-34ce-47ca-b976-30c7fdfe7a56");

	public static readonly Guid VehicleFeature
			= new Guid("18ff9ac7-1302-4c8a-95b6-dced670b39bf");

	public static readonly Guid FleetFeature
			= new Guid("c239ff17-4e3d-4614-92db-e5a152555214");

	public static readonly Guid DeviceFeature
			= new Guid("c6deca1e-68e0-41ed-9110-938546cf098d");

	public static readonly Guid DriverFeature
			= new Guid("43a4740f-ae47-4a75-bfd0-d7570c22ae9e");

	public static readonly Guid LocationFeature
			= new Guid("dabaf71b-a9d8-4295-af23-cc5daa1e70ac");

	public static readonly Guid ProfitLossManager
			= new Guid("e88bed5d-a164-42b1-b14b-697d1ef109da");

	public static readonly Guid AssignDevice
			= new Guid("8293eee7-810a-4639-a443-2de56041a8d3");

	public static readonly Guid EditVehicle
			= new Guid("693846e8-87ad-4f31-b353-14e2122fc82a");
	public static readonly Guid ChannelSummaryReport
			= new Guid("58404fbe-a1f1-43a4-82ad-266ad556dc5b");
	public static readonly Guid StateMileageReport
			= new Guid("1f32b91c-074f-4ba4-80d9-28aabde4936a");
	public static readonly Guid FuelPurchaseReport
			= new Guid("6017c288-c180-495e-afa0-4599c5e992c2");
	public static readonly Guid LocationScheduledVisitsReport
			= new Guid("D52DA1EF-1CAB-4A4F-9B7D-52983B4B1774");
    public static readonly Guid LocationViewOnly
    = new Guid("854f8908-8b98-43f5-989b-c9c4ba542039");

    public static readonly Guid DriverFatigueView
   = new Guid("0a5a5375-4b7d-4266-b9dc-40d3657679e4");

    public static readonly Guid LogOffDriver
   = new Guid("9cde54a9-940f-4e41-879f-7f12855a2999");

#if NewPermission

	public static readonly Guid `PermName` 
			= new Guid("`PermGuid`");
#endif

	#endregion

	public static readonly Guid ImardaAdminServiceGroup
		= new Guid("996cde25-d396-460c-b599-cf3f92913d2c");

	public static readonly Guid ImardaAdminServiceLogin
		= new Guid("63620B0E-AFF5-402D-B1F1-37B61ADCE26B");

	public static readonly Guid GetTraceEventList
		= new Guid("80D87423-204B-43BB-916B-5FD884CE3AAD");

	public static readonly Guid CopyUnits
		= new Guid("BE2481C4-9DEB-4BD9-A9B1-CAAD4B4FE2B7");

	public static readonly Guid UpdateSensor
		= new Guid("7CFB3D4E-7BAB-4872-8C53-04F9D2E84E60");

	public static readonly Guid ClearConfigCache
		= new Guid("38202760-e928-4eac-ab0d-defce29e6c12");

	public static readonly Guid SetDeletedSecurityEntityByCRMID
		= new Guid("F8A683D2-BC0C-49F2-B3DD-6044CC2D1ADE");

	#region Reports

	public static readonly Guid BreachReport
		= new Guid("72F1C4D1-F794-4D9F-BF18-07AAD3B8560D");

	public static readonly Guid OutstandingSafetyChecklistReport
		= new Guid("F7E16BF9-85C5-4854-A56F-1490D840718F");

	public static readonly Guid TripSummaryReport
		= new Guid("5FB72236-25D0-41F5-862B-1BEA1B04386A");

	public static readonly Guid LogoffNotCompletedReport
		= new Guid("1799F0DC-BA51-4D0B-AE38-28F5FBF68778");

	public static readonly Guid SensorReport
		= new Guid("A9ADE47E-EBBA-45A3-97E5-2C5ED6623469");

	public static readonly Guid FatigueSummaryReport
		= new Guid("C430D366-7226-43A3-B342-2F583BBC284C");

	public static readonly Guid VisitedStreetsReport
		= new Guid("BB4EEAEF-F91A-4405-962A-32CD1AF7D257");

	public static readonly Guid TotalDistanceReport
		= new Guid("43024929-0A31-42AD-9CA9-3510E1901AF8");

	public static readonly Guid DetailedVehicleEfficiencyReport
		= new Guid("FF3A2E72-05AD-4A92-82B4-41E586F789FA");

	public static readonly Guid DetailedDriverEfficiencyReport
		= new Guid("5c75ea5e-457e-496b-a64b-db51e3ed557c");

	public static readonly Guid DriverEfficiencyReport
		= new Guid("FFC7FE51-0E4C-4267-8AF1-470E18E1A3E7");

	public static readonly Guid TopSpeedReport
		= new Guid("4ECC5F2F-1E71-4C15-B81C-503335AEA7C2");

	public static readonly Guid VisitedSuburbReport
		= new Guid("B4FAC076-49E1-4D54-8B94-5E03E5CC56A9");

	public static readonly Guid StopReport
		= new Guid("ABDF4C63-F68B-41D3-B6A4-676E9D47AAFF");

	public static readonly Guid LocationToLocationReport
		= new Guid("F3D91535-62A6-44D2-9E19-7F47C783E182");

	public static readonly Guid DailyUtilisationReport
		= new Guid("78734D48-E2D1-4065-9E9C-84FB26676AB6");

	public static readonly Guid DistanceReport
		= new Guid("D8E65A19-56DD-4318-96E8-8857A6B65C8A");

	public static readonly Guid TravelReport
		= new Guid("2F6EAC22-4838-4C4F-ACC8-885945F133CF");

	public static readonly Guid LocationTravelReport
		= new Guid("785A987F-E1A0-41D0-8F0C-9D98A78D38C8");

	public static readonly Guid OverSpeedReport
		= new Guid("66594A7B-5A61-48A3-AA4E-A72F4F2645E6");

	public static readonly Guid LocationSummaryReport
		= new Guid("3F1AE056-E333-40B9-89CF-ABAB05072549");

    public static readonly Guid LocationVehicleSpeedReport
		= new Guid("517F506C-7E6C-4577-8A73-96B48F360D2B");

	public static readonly Guid FatigueManagementGraphReport
		= new Guid("367774CB-511B-437D-8C9C-B0FB5B573FC7");

	public static readonly Guid VehicleEfficiencyReport
		= new Guid("DC17A60B-A5DE-4CE8-932F-B7E65D1A1055");

	public static readonly Guid DriverLoginReport
		= new Guid("60A31E13-8755-4711-97E0-B95527E72B08");

	public static readonly Guid DriverActivityGraphReport
		= new Guid("AD999CE1-ED30-4E21-A748-BDC781377624");

	public static readonly Guid VehicleStressReport
		= new Guid("8C1703BB-301C-4102-8BC7-C36CBA34B73C");

	public static readonly Guid DriverActivityReport
		= new Guid("9BEFD70C-FCBA-481B-AACC-C77E2A19D71D");

	public static readonly Guid WeeklyUtilisationReport
		= new Guid("86AECCCA-BAED-4DB1-8336-D468F9A7303F");

	public static readonly Guid GTPEfficiencyReport
		= new Guid("E7B59FB9-7624-49CB-A6C1-D7AF02E0F3C9");

    public static readonly Guid GTPSpeedReport
        = new Guid("E0C6A00E-BF66-4115-B9E4-14FEB92C2471");

	public static readonly Guid ExcessiveIdleTimeReport
		= new Guid("742654F2-94F2-4C2D-A68E-EA5929973F21");

	public static readonly Guid LoadSwitchRecalibrationReport
		= new Guid("12C037A2-FAFD-41F6-9D37-F0E5DFFF2487");

	public static readonly Guid DeviceReport
		= new Guid("C84AF806-EF2F-4776-BA5B-F8E4C3A4338E");

	public static readonly Guid LocationReport
		= new Guid("F5C1326C-D678-461A-8A84-FC276D472089");

	public static readonly Guid FleetLocationReport
		= new Guid("55F5F743-1A10-4e3a-AD8B-EFD16CA05116");

	public static readonly Guid LocationCycleReport
		= new Guid("e6b4ef54-ef75-4e0b-93bd-ebe463941686");

	public static readonly Guid FleetLocationCycleReport
		= new Guid("C1D31A52-7055-45ef-97BF-929702E33916");

	//added for fatigue II reports 2011-05-09 Q.C.
	public static readonly Guid DriverFatigueSummaryReport
		= new Guid("111a9f92-93ea-48e2-b09e-1cda0fe6a63e");

	public static readonly Guid DriverExpiredCertificationReport
		= new Guid("50db5144-e40f-427e-9e0a-aa6d8d8e682d");

	public static readonly Guid DriverFatigueReport
		= new Guid("eac4081e-9cf5-412d-95d7-7f6ede0a6fa2");

	public static readonly Guid DriverRecertificationReport
		= new Guid("6441a57e-ebfe-43fd-863f-576cfa6ef232");

	public static readonly Guid DriverFatigueRuleReport
		= new Guid("748092c3-7f3f-4ac9-9295-2d70c1829b8f");

	//added new reports 2011-07-05 Q.C.
	public static readonly Guid VehicleJobCompletedReport
			= new Guid("4cf446fe-b116-4a5b-be71-2445385d5242");

	public static readonly Guid DriverJobCompletedReport
			= new Guid("5e115608-7f7f-46a6-a74d-cea239567dc6");

	public static readonly Guid ServiceStatusReport
			= new Guid("47487e0f-f5a0-4a4b-bf78-77489ba4bbbf");
    //IM-5683
    public static readonly Guid ServiceModuleReport
            = new Guid("E5507F22-F4EA-495C-8D35-CDD980BA966A");

    //IM-5661
    public static readonly Guid LoadedKMReport
            = new Guid("BB5965E4-D6D1-4BF0-899E-1748CFF0FAA0");

    public static readonly Guid UnauthorisedDoorOpenReport
            = new Guid("50C9246B-F395-4A83-BBD7-0D2B1D7392BF");

    public static readonly Guid VehicleOverspeedingAlert
            = new Guid("C61E5DFE-B355-48DC-9D9F-C8FA93E143B8");


	//added 2011-07-21 Q.C.
	public static readonly Guid FleetProfitReport
		= new Guid("5b3cee45-1fa8-49ea-aa39-5b42f3e664a1");

	public static readonly Guid VehicleProfitDetailReport
			= new Guid("759836c1-4de8-4780-8494-c952c72c2e25");

    public static readonly Guid DailyDriverWorksheetReport //IM-5366
            = new Guid("2F301F1E-9CA1-4B21-8B7B-253ECD2E5CC3");

    public static readonly Guid LogOffExceptionReport //IM-5366
        = new Guid("37F154BF-4CC0-455D-89EF-4192751347C3");

    public static readonly Guid BoralEAMReport //IM-5366
        = new Guid("D02A9C31-0750-4E91-BF81-786158D17419"); 

	#endregion

	#region Managements

	public static readonly Guid DeviceManagement
		= new Guid("BA6A31E8-E47C-423A-AB7E-2B43499ADFA6");

	public static readonly Guid DriverManagement
		= new Guid("75633827-E49F-4966-8AB4-35B63A750670");

	public static readonly Guid LocationManagement
		= new Guid("844819C9-417B-4B9F-ADFD-39591591BC81");

	public static readonly Guid UserManagement
		= new Guid("22F6642D-1644-4558-8309-7BC09DAB5AF6");

	public static readonly Guid ChecklistManagement
		= new Guid("D2D34671-F69C-4DF6-A07C-70D8405BCBEB");

	public static readonly Guid FleetManagement
		= new Guid("F2A0038F-DAE9-446B-B53D-8EFEF606E2E9");

	public static readonly Guid ServiceManagement
		= new Guid("5EBC7485-F6B8-41CD-8541-A0CB4901CE0E");

	public static readonly Guid AlertingManagement
		= new Guid("9402861C-59EB-4665-B53A-B453E47A858F");

	public static readonly Guid VehicleManagement
		= new Guid("D9D493D0-00AD-4190-BBCA-B8023AF133B3");

	public static readonly Guid ReportManagement
		= new Guid("AF9A2D90-3AF0-493E-919F-BD2C1ADEF2FF");

	public static readonly Guid AccountManagement
		= new Guid("18ffa9af-6666-4a89-ace9-74db0dbaa94d");

	public static readonly Guid JobManagement
		= new Guid("42d93507-882f-4e9b-bb59-6edd626695ca");

	public static readonly Guid ProfitLossManagement
		= new Guid("7ced7f13-635a-4fb0-8444-088ef1f5e8d3");
	#endregion

	public static readonly Guid GetJobActivityListByTimeStamp
		= new Guid("34457d93-50eb-490f-be2a-6dc3b98c0eb2");

	public static readonly Guid GetJobActivityList
		= new Guid("62248e70-da6f-4881-a7d3-31441a00d3df");

	public static readonly Guid SaveJobActivityList
		= new Guid("b5b98cdd-6e99-49e6-80ec-8fad1b302fef");

	public static readonly Guid SaveJobActivity
		= new Guid("c2b2ebd6-41eb-4004-9bfc-ce32681eed87");

	public static readonly Guid DeleteJobActivity
		= new Guid("81222808-7f0e-4979-93a1-a97abd88f4e1");

	public static readonly Guid GetJobActivity
		= new Guid("f75f76c1-fd9a-4274-8b60-438b86e69e3b");

	public static readonly Guid GetJobActivityUpdateCount
		= new Guid("b6a6723a-bc73-48ad-a716-686ed4326742");

	public static readonly Guid GetJobListByTimeStamp
		= new Guid("20a6e62f-a46b-4ced-991c-580d2de1dcb5");

	public static readonly Guid GetJobList
		= new Guid("4f508924-1b1b-410a-8fb6-7b9c5be84796");

    public static readonly Guid DispatchJobs
        = new Guid("A809371B-B5DD-4FEB-8212-235AF482C24D");

	public static readonly Guid GetJobListByOwnerID
		= new Guid("4743f55e-86f4-405a-9964-f1c41a389df3");

	public static readonly Guid SaveJobList
		= new Guid("1697dfef-cc78-47f4-a749-3cdb0d4ebd4d");

	public static readonly Guid SaveJob
		= new Guid("1e2abb6d-7770-4b1c-9786-c9c2ebd96c7f");

	public static readonly Guid DeleteJob
		= new Guid("e7bc46e8-a82a-46d3-86b7-61a8f1bfbedb");

	public static readonly Guid GetJob
		= new Guid("d2dfc705-ab0a-4904-88e7-fbc00fdc03f6");

	public static readonly Guid GetJobUpdateCount
		= new Guid("02aa6895-8d96-4fa8-9b5a-1b355e826278");

	public static readonly Guid GetJobStatusListByTimeStamp
	= new Guid("583b9850-3056-40b4-827c-12b58ed9d731");

	public static readonly Guid GetJobStatusList
		= new Guid("66864c76-a9bd-457d-92c0-65842f4658c4");

	public static readonly Guid SaveJobStatusList
		= new Guid("4ce9f778-4aad-4f46-b9fe-681f4fd4ce45");

	public static readonly Guid SaveJobStatus
		= new Guid("ca351516-67f4-4299-9cd9-bab53ed1e5dd");

	public static readonly Guid DeleteJobStatus
		= new Guid("3081d6b7-104d-4bb1-9ae3-300ecb4d25fc");

	public static readonly Guid GetJobStatus
		= new Guid("3eefb1b4-3a7e-4b05-9899-be710581384b");

	public static readonly Guid GetJobStatusUpdateCount
		= new Guid("7af2a264-1eeb-4872-afc4-1a77bd82df37");

	//public static readonly Guid GetJobListByTimeStamp
	//    = new Guid("d369fc34-4532-4c00-924e-e6f46fe45949");

	public static readonly Guid ResetUnitLocation
		= new Guid("49b2c5cb-0d15-4d61-ad45-f7d2ddaec959");

	public static readonly Guid GetAppTaskList
		= new Guid("78f8b7a1-2fd2-4d90-aa6c-feb8ce20b3c2");

	public static readonly Guid GetReportTaskList
		= new Guid("75b4d018-67c7-4292-85f5-27f2e84d2b2a");

	public static readonly Guid ScheduleAppTask
		= new Guid("8e59bd20-bae5-47e0-8907-35964c736478");

	public static readonly Guid CancelScheduledAppTask
		= new Guid("42024599-91c8-48f5-80ec-e76e02ca242c");

	public static readonly Guid GetAppTask
		= new Guid("4e6e7628-ab44-4de0-89d4-a13ec7a8aab8");


	public static readonly Guid GetAlertPlanOwnerListByTimeStamp
		= new Guid("218dc616-2c50-4055-90dc-c9a066b7c006");

	public static readonly Guid GetAlertPlanOwnerList
		= new Guid("072ba982-d82a-4467-8d2a-8f27e7985f1d");

	public static readonly Guid SaveAlertPlanOwnerList
		= new Guid("ec6e1506-3646-46b2-81bd-cef9e37bf6b3");

	public static readonly Guid SaveAlertPlanOwner
		= new Guid("7eacb1b2-a161-4348-925b-c5fcfcd0e853");

	public static readonly Guid DeleteAlertPlanOwner
		= new Guid("5b0c3e11-2c24-41c4-915e-45b1f4eb25f8");

	public static readonly Guid GetAlertPlanOwner
		= new Guid("41846504-767c-405b-a0be-5b7b11ce4623");

	public static readonly Guid GetAlertPlanOwnerUpdateCount
		= new Guid("8a0ef837-95b0-46ad-8ef7-f063acde888d");

	public static readonly Guid GetTripListByTimeStamp
		= new Guid("9876d963-e293-47f9-b4d2-1fac69f9bacf");

	public static readonly Guid GetTripList
		= new Guid("638c0b7a-d4e9-4221-9391-447f857b2ef5");

	public static readonly Guid SaveTripList
		= new Guid("f15bd900-2db3-4709-9106-a47fcb3a9e9c");

	public static readonly Guid SaveTrip
		= new Guid("e28a0d12-ddf5-458f-ae2d-1fb126b2ebf7");

	public static readonly Guid DeleteTrip
		= new Guid("bcff15b8-2e2f-4822-9565-e74d25a287da");

	public static readonly Guid GetTrip
		= new Guid("c38887b5-9d00-406e-a86b-c6b95ec9104a");

	public static readonly Guid GetTripUpdateCount
		= new Guid("0111d333-4bc0-4bb8-91e7-d12d25709889");

	public static readonly Guid GetPersonNameList
		= new Guid("6a2587f7-f691-40f1-b198-6d7f4d485dc0");

	public static readonly Guid GetJobTypeListByTimeStamp
			= new Guid("d397bfe6-04f3-4ea1-ba8c-ac90816ca0ad");
	public static readonly Guid GetJobTypeList
			= new Guid("c089e3e2-b97c-4bdd-929c-290f194227c9");
	public static readonly Guid SaveJobTypeList
			= new Guid("2ac5f244-f4bb-4178-8c5e-dfc28fb3230a");
	public static readonly Guid SaveJobType
			= new Guid("4a2f6937-d564-44c3-bfc7-0dc26f915395");
	public static readonly Guid DeleteJobType
			= new Guid("8ae5352e-341c-485b-940e-532a5a3f0769");
	public static readonly Guid GetJobType
			= new Guid("c00df939-2729-4398-868b-751e684ea677");
	public static readonly Guid GetJobTypeUpdateCount
			= new Guid("b11f5075-f701-44b3-904e-9fcedecd2389");

	public static readonly Guid GetApplicationPlanListByTimeStamp
			= new Guid("3e5a3da4-baf2-44b2-948b-667814374d22");
	public static readonly Guid GetApplicationPlanList
			= new Guid("19b7cea9-0667-4ea2-b520-838cc9bee953");
	public static readonly Guid SaveApplicationPlanList
			= new Guid("565cd93d-9037-409c-8f05-3e3b13e43791");
	public static readonly Guid SaveApplicationPlan
			= new Guid("275a6fc5-e367-454d-ad56-4bb5652b262f");
	public static readonly Guid DeleteApplicationPlan
			= new Guid("00ae62cf-17c2-4566-9d4c-49c385cac0d6");
	public static readonly Guid GetApplicationPlan
			= new Guid("a357a232-cd85-436d-844d-68af6c609720");
	public static readonly Guid GetApplicationPlanUpdateCount
			= new Guid("6789112b-0af4-4cef-b71f-c581ecb82f7c");

	public static readonly Guid GetApplicationFeatureCategoryListByTimeStamp
			= new Guid("e68caf1b-e5b7-49eb-967e-e2bc55de13e7");
	public static readonly Guid GetApplicationFeatureCategoryList
			= new Guid("bd405a18-5364-4aa2-af5c-351f1d063ea7");
	public static readonly Guid SaveApplicationFeatureCategoryList
			= new Guid("a8a480cf-d69c-43a9-b140-70d3f2ad001b");
	public static readonly Guid SaveApplicationFeatureCategory
			= new Guid("5559bc96-8198-4e51-bf71-e63f7d07ee66");
	public static readonly Guid DeleteApplicationFeatureCategory
			= new Guid("18963305-8591-4642-b43b-af69bc1ca340");
	public static readonly Guid GetApplicationFeatureCategory
			= new Guid("e2767707-b5fc-4bac-b850-aaa11db33815");
	public static readonly Guid GetApplicationFeatureCategoryUpdateCount
			= new Guid("f37079b7-c683-4097-a569-e9abc785c28f");

	public static readonly Guid GetApplicationFeatureListByTimeStamp
			= new Guid("179c91a6-f788-4570-b078-8a218ade7272");
	public static readonly Guid GetApplicationFeatureList
			= new Guid("abf685b9-4643-4885-b6b9-67a7f6f8c705");
	public static readonly Guid SaveApplicationFeatureList
			= new Guid("d55a75e6-4d0c-4807-bbff-5a062c8a1488");
	public static readonly Guid SaveApplicationFeature
			= new Guid("291aa068-56c6-4857-9a19-af5e7dc23496");
	public static readonly Guid DeleteApplicationFeature
			= new Guid("940856b3-9777-44d3-80b3-3dbb60b6c323");
	public static readonly Guid GetApplicationFeature
			= new Guid("a41e4122-1e75-4033-8df6-d83dcfcc33bd");
	public static readonly Guid GetApplicationFeatureUpdateCount
			= new Guid("3853a82c-4290-4f99-991c-de8ca7006f5d");

	public static readonly Guid GetApplicationPlanFeatureListByTimeStamp
			= new Guid("29b96ae0-0dcf-4ee1-a5c7-d5695673a277");
	public static readonly Guid GetApplicationPlanFeatureList
			= new Guid("0b79ebf1-52dc-4ac0-90e5-a84f4e46865c");
	public static readonly Guid SaveApplicationPlanFeatureList
			= new Guid("c107b843-cd0f-4d2b-b709-80bae49f6954");
	public static readonly Guid SaveApplicationPlanFeature
			= new Guid("f7627b29-99f2-4b9b-9cff-9f6626bbdf8c");
	public static readonly Guid DeleteApplicationPlanFeature
			= new Guid("5d66cb11-bfcd-4e3d-9565-863b943d6689");
	public static readonly Guid GetApplicationPlanFeature
			= new Guid("95ad5802-f1bc-4675-a958-981aa767c295");
	public static readonly Guid GetApplicationPlanFeatureUpdateCount
			= new Guid("61618804-45b6-4600-8d3d-26f384c22087");

	public static readonly Guid GetFeatureSupportListByTimeStamp
			= new Guid("b77a3916-99d5-44d4-b91d-01fdc3109342");
	public static readonly Guid GetFeatureSupportList
			= new Guid("d35dd779-3563-4ca7-b2d6-7b8e154a3d63");
	public static readonly Guid SaveFeatureSupportList
			= new Guid("dc8acb66-401a-4512-9e52-7f6b2b34cb18");
	public static readonly Guid SaveFeatureSupport
			= new Guid("a8e3dde5-f38d-4ad9-b5a9-a5a7e7b3e7cb");
	public static readonly Guid DeleteFeatureSupport
			= new Guid("31867181-93bc-4b2e-9061-c6f908732761");
	public static readonly Guid GetFeatureSupport
			= new Guid("6bef3aed-cb58-42cc-a081-99e38a43916c");
	public static readonly Guid GetFeatureSupportUpdateCount
			= new Guid("0c08a5c3-e496-4b8e-a7ba-c9fe7c142d14");

	public static readonly Guid GetApplicationFeatureOwnerListByTimeStamp
			= new Guid("793802bc-6ebf-45a5-9d7f-a5d93435df18");
	public static readonly Guid GetApplicationFeatureOwnerList
			= new Guid("4e64db01-9c0f-4a49-8cc5-38a46ce84df0");
	public static readonly Guid SaveApplicationFeatureOwnerList
			= new Guid("0df5ee23-1b68-4814-b7a2-bf0f5a8b5d0d");
	public static readonly Guid SaveApplicationFeatureOwner
			= new Guid("c698df6e-7d6f-4b35-b49f-f0022cc96ad8");
	public static readonly Guid DeleteApplicationFeatureOwner
			= new Guid("a3778525-308b-4623-9e8c-c63e43c09be3");
	public static readonly Guid GetApplicationFeatureOwner
			= new Guid("c021045c-f7ce-4876-908e-e9582b27b4af");
	public static readonly Guid GetApplicationFeatureOwnerUpdateCount
			= new Guid("511fff76-2f81-4fa6-b85b-cb76bf6f472d");

	public static readonly Guid GetIXLParamUsageListByTimeStamp
			= new Guid("78fce7f7-d81f-4a8e-a9a2-81e9de3f7764");
	public static readonly Guid GetIXLParamUsageList
			= new Guid("b6751a92-47d9-452f-870b-8d29f8835004");
	public static readonly Guid SaveIXLParamUsageList
			= new Guid("b4278327-4aa4-4c5a-b3a8-35eaf785903a");
	public static readonly Guid SaveIXLParamUsage
			= new Guid("b827fbb1-522e-467d-871a-5d1e34b58104");
	public static readonly Guid DeleteIXLParamUsage
			= new Guid("e3f4fab0-6362-42aa-83a0-a3d3b94aee33");
	public static readonly Guid GetIXLParamUsage
			= new Guid("ee0cc1f7-a4bc-4f04-acdb-b778447c3d86");
	public static readonly Guid GetIXLParamUsageUpdateCount
			= new Guid("c6a5a1f0-e6d1-4731-95e7-bc816e6bbbe3");

	public static readonly Guid GetUnitMessagePlanListByTimeStamp
			= new Guid("f57865eb-098b-456b-a619-60a5472b98d5");
	public static readonly Guid GetUnitMessagePlanList
			= new Guid("13679db3-7e3a-4a3e-98aa-5cf5a5cff353");
	public static readonly Guid SaveUnitMessagePlanList
			= new Guid("23dbd1df-2a55-466b-bdb8-1e8c703c5a0b");
	public static readonly Guid SaveUnitMessagePlan
			= new Guid("58ec23f2-8b87-48f0-a3d2-c7fed50dcfbe");
	public static readonly Guid DeleteUnitMessagePlan
			= new Guid("4b3733ab-3f85-4ddf-ad4d-544b41caf438");
	public static readonly Guid GetUnitMessagePlan
			= new Guid("001e42fb-4ca6-48e9-8c50-b49a9cdca033");
	public static readonly Guid GetUnitMessagePlanUpdateCount
			= new Guid("51ae3a85-c326-44a5-aaa5-6088c15671e7");

	public static readonly Guid GetUnitMessagePlanCmdListByTimeStamp
			= new Guid("0b2fd924-37a2-40e2-bc7f-0cce949e3154");
	public static readonly Guid GetUnitMessagePlanCmdListByPlanID
			= new Guid("22ff5f70-a269-43e4-9633-9da4122b9858");
	public static readonly Guid GetUnitMessagePlanCmdList
			= new Guid("6596aed5-3e6a-4595-98bb-f9a06ffcaf3d");
	public static readonly Guid SaveUnitMessagePlanCmdList
			= new Guid("4a71a69a-e528-4ef8-8986-f35b0b5e1d80");
	public static readonly Guid SaveUnitMessagePlanCmd
			= new Guid("09ddd0af-916c-4909-8803-c12baecd4cfd");
	public static readonly Guid DeleteUnitMessagePlanCmd
			= new Guid("10cc95ab-118f-47cd-b97f-e7b0ff32cb87");
	public static readonly Guid GetUnitMessagePlanCmd
			= new Guid("4308b435-97fc-4779-89fd-50b133378496");
	public static readonly Guid GetUnitMessagePlanCmdUpdateCount
			= new Guid("b677416d-3e16-4756-a111-893b0b94974a");

	public static readonly Guid GetReportTaskListFiltered
		= new Guid("b72a9592-5184-43f3-9e3f-bb9a7155cf7a");

	public static readonly Guid GetEventListByCategoryID
		= new Guid("67153635-bdd2-43c9-9952-6c37c7048cc9");

	public static readonly Guid GetJobActivityListByJobID
		= new Guid("53ae49a9-0b47-4f6f-b4a4-f9d2e859de2c");

	public static readonly Guid AddJobActivity
		= new Guid("e77e4b5d-0941-4700-a668-048cfd58e505");

	public static readonly Guid GetOwnedUnits
		= new Guid("9d4e4c54-0a89-49cc-9dd2-0ac3a40a1c0c");

	public static readonly Guid SaveAlertGrid
		= new Guid("624c0265-3d91-4e5a-a6b9-d74e195e49b7");

	public static readonly Guid GetAlertGrid
		= new Guid("1738f245-f81f-4818-adc5-a0735042a71b");

	public static readonly Guid DeleteAlertGrid
		= new Guid("360fc1a5-2e7b-41fe-b5d8-b07558f24e16");

	public static readonly Guid GetAlertPlanListByEventCategoryID
		= new Guid("66dc447b-f62e-408a-b174-c619554e35be");

	public static readonly Guid GetAlertPlanOwnerListByOwnerID
		= new Guid("47fc4a4b-33c4-46a1-bce7-439089a85707");

	public static readonly Guid SendDeviceTimers
		= new Guid("9311520a-69c0-4812-a5ad-7f523aa5817e");

	public static readonly Guid GetSecurityEntityByLoginUserName
		= new Guid("81a2db57-4d1e-407f-8ac2-d1f0029f7417");

	public static readonly Guid GetUserSecurityEntity
		= new Guid("e758b6fa-ffbc-4d83-aed3-04c91cb6ed0a");

    public static readonly Guid GetUserConfigSettings
        = new Guid("5103C226-E534-4017-83BF-B1941A19C390");

    public static readonly Guid SaveUserConfigSettings
        = new Guid("CED85C9C-7306-43e7-8D86-ED5E3306E622");

	public static readonly Guid GetTopNLogonLogListBySecurityEntityID
		= new Guid("1c3dd623-3749-4081-8ffd-9c25f3811fc0");

	public static readonly Guid EnqueueGetStatus
		= new Guid("9ac61dad-1008-455a-a7ad-c85d66f25cad");

	public static readonly Guid EnqueueGetDriverIDFatigue
		= new Guid("3df5adf1-f019-4b41-87ad-a5b8185174a8");

	public static readonly Guid AssignJob
		= new Guid("97b8f35a-087d-4b48-a767-392131c81c1b");

	public static readonly Guid GetMetrics
		= new Guid("5dbff449-0aff-4768-b159-d3130db8f0f9");

	public static readonly Guid GetDriversWithExpiredAccreditation
		= new Guid("7fda7a5c-f82d-4fc5-9fb9-5be6c077614a");

	public static readonly Guid Poll
			= new Guid("fd6021fb-44ae-47e1-b355-30c9cd019b76");

	public static readonly Guid GenericCommand
			= new Guid("060074cb-a9a7-4357-9dbc-0734139d5240");

	public static readonly Guid GetEnrolledVehicleByServiceCount
		= new Guid("64cf93f6-2d62-4000-917a-23e62df3ae5c");

	public static readonly Guid GetServiceByPlanPaging
		= new Guid("1116d7bd-e340-4074-8268-baa362dc6fa2");

	public static readonly Guid GetVehicleServices
			= new Guid("c5d0bd24-865f-447a-9d7f-9c2cc044dd0c");

	public static readonly Guid VehicleOptoutService
			= new Guid("7b88f552-3529-47a3-8382-e2cf2e4de2d7");

	public static readonly Guid GetServiceVehicleServiceByServiceVehicleID
			= new Guid("48e4f1d1-6b42-4d69-b09b-827afc28aa1d");
	public static readonly Guid SaveIXLParameterByCategoryID
			= new Guid("2c8024e5-2eeb-4d0f-b71f-c016b90cf264");
	public static readonly Guid GetAttributeListByCategoryID
			= new Guid("ec8a2e24-fb1f-4dd6-a823-40bcbef7f355");

	public static readonly Guid GetServicePerformedByID
			= new Guid("4fa42336-30e0-4855-addd-0c4a95d3287b");

	public static readonly Guid SendNotification
			= new Guid("3d8a2d8b-cdcb-4aca-9276-5e4319434880");

	public static readonly Guid GetApplicationFeatureListByCategoryID
			= new Guid("4623df93-c2c3-4fc9-88c9-e3fc96c10e2d");

	public static readonly Guid GetApplicationFeatureListByOwnerID
			= new Guid("399c638e-e6bc-43e1-873d-0d5584d35ede");

	public static readonly Guid GetAssetListByTimeStamp
			= new Guid("73747938-c808-48e3-b0b7-1b6d9308b1bf");
	public static readonly Guid GetAssetList
			= new Guid("03acb6eb-3285-4756-b68c-01827facd23c");
	public static readonly Guid SaveAssetList
			= new Guid("1c09f83d-3862-48e7-929b-baf419df735b");
	public static readonly Guid SaveAsset
			= new Guid("2d2cfa12-8115-46f5-afdb-531af1354119");
	public static readonly Guid DeleteAsset
			= new Guid("9432bd41-dbee-4bee-84d2-010b81691e1d");
	public static readonly Guid GetAsset
			= new Guid("6dbd1583-9e9e-42ec-8f95-bff25ff80db5");
	public static readonly Guid GetAssetUpdateCount
			= new Guid("2483393a-44d0-493c-88e0-e907aca13fd3");

	public static readonly Guid GetInstalledUnits
			= new Guid("27173a76-08c4-4cf9-a08a-d4f3ba530429");

	public static readonly Guid CheckInvoiceExist
			= new Guid("5752a6a9-90f8-45b2-8bbe-2733e321aa82");

	public static readonly Guid GetJobListCount
			= new Guid("a752e198-e684-490e-9587-82d529bfe6f0");

	public static readonly Guid CompleteOnDueJobs
			= new Guid("a2efb0f4-b5d5-4598-921a-17406180f95f");

	public static readonly Guid SearchContactList
			= new Guid("b65343c1-6f13-4f3c-99ff-34d02047c995");

	public static readonly Guid GetContactListByParentId
			= new Guid("A538A2D7-5AEA-40e2-92E5-E97B2296A444");

	public static readonly Guid CalibrateUnitOdometer
			= new Guid("d12da87b-8739-44f8-ad81-f8bca1ae3190");

    public static readonly Guid CalibrateUnitEngine
            = new Guid("87683F38-3EFF-44A2-9186-8E9D32381A08");

	public static readonly Guid ActivateUnits
			= new Guid("00408899-2eaf-471a-b27e-372689d71770");

	public static readonly Guid DeactivateUnits
			= new Guid("95452302-c3b7-4a1f-952c-64749190c391");

	public static readonly Guid GetProfitLogListByOwnerID
			= new Guid("22044871-b298-4cd2-823b-1cab66507b0d");
	public static readonly Guid SaveIXLParam
			= new Guid("1a57ac1e-cd60-4f8b-bc21-5e9be715a3d9");

	public static readonly Guid GetAttributeListByIXLSourceID
			= new Guid("6be14466-7e78-4456-8eef-e8b10503540e");

	public static readonly Guid ValidateRule
			= new Guid("4e03b7f1-c42a-45c6-a9f9-94a53855e582");

	public static readonly Guid SearchTrackableVehicleListByFleet
			= new Guid("8c556d4f-ab9c-4356-aa1e-0a0c166a45b2");

	public static readonly Guid ClearLastLocation
			= new Guid("c900f69b-02b0-46fb-8f69-a5156f8da999");

	public static readonly Guid SaveIXLParamUsageChanges
			= new Guid("d95903e8-3f2e-491d-9b70-6649c1db201c");

	public static readonly Guid TransferUser
			= new Guid("7eb49889-202b-4aeb-8791-6e4a7b2d1c3e");

	public static readonly Guid GetDefaultFleet
			= new Guid("cdb365f5-671a-4bc8-ad0a-bfc7ff973567");

	public static readonly Guid SetUnitVehicleName
			= new Guid("7E4B1C9E-464C-464C-BDAA-1AF9D14C389A");

	public static readonly Guid SaveContactMap
			= new Guid("c670b254-3bca-4ddd-8e97-7bd17455ea00");

	public static readonly Guid DeleteContactMapByContactNPerson
			= new Guid("04555b85-da70-48f1-90dc-e2234bea98a2");
	public static readonly Guid GetTripListByDateRange
			= new Guid("6fc39036-0515-4498-8e30-e6e503d108f0");

	public static readonly Guid GetEventsForTrip
			= new Guid("e77977b7-6db8-4ffd-855d-736a6054f955");

	public static readonly Guid GetCountryCodes
			= new Guid("50b4b95d-588d-4f4b-94c9-898b73f310ed");

    public static readonly Guid FindLatLong
            = new Guid("7d11bc0c-7790-487e-bc3a-760cdc86eed9");

	public static readonly Guid GetCountries
			= new Guid("93ca6071-2325-45e6-8040-d4e9eaf86e08");
	public static readonly Guid GetDriverFatigue
			= new Guid("e55c972d-89f2-4e16-9896-13054714cb40");
	public static readonly Guid GetTrackableListByFleet
		= new Guid("60d68529-d89b-40f6-ac82-693fc344d694");
	public static readonly Guid GetDeletedTrackableListByFilter
		= new Guid("2489B30C-739E-4C9F-87E0-E8E9409347F9");
	//public static readonly Guid GetTrackableListByFleetTimeStamp
	//    = new Guid("0bc0650f-dea6-455e-bb2a-6779e9a5154e");
	public static readonly Guid GetTemplateHtmlListByUnitIDs
		= new Guid("c5aae607-3c65-4fb3-bf63-51d28a26ba1e");

	//& IM-4181
	public static readonly Guid GetTemplateForEachTrackable
		= new Guid("79D4C854-0C5A-419D-B103-F082DA00CAE4");

	public static readonly Guid GetLatestAlertIcon
		= new Guid("B5ACBFFF-B970-4689-B34E-A24BA06AD2E8");

	//SRR IM-4181

	public static readonly Guid GetLocationCountForVehicle
			= new Guid("d1618be2-8aab-4b34-b89b-f52145c53bec");
	public static readonly Guid InstallFleetAccessToUser
			= new Guid("1f5a7e0c-50aa-46c9-b42f-61ea37ecccab");
	public static readonly Guid GetMetricChannelListForReport
			= new Guid("531de971-2088-4c00-a6e5-7352d03ebd3d");
	public static readonly Guid Diagnostics
					= new Guid("779C11C6-B3E7-4013-A9AC-D2CA8B2E69B6");
	public static readonly Guid SaveUserSecurityEntity 
			= new Guid("aed669f4-0056-4bb3-bb11-91f605e3ea5c");
	public static readonly Guid ClearTaskQueueCache 
			= new Guid("6cea7156-ab09-4dce-8e93-0347291eb427");
	public static readonly Guid GetScheduledTaskList 
			= new Guid("9d77f02f-ee5b-4751-9cbf-6721e734dc55");
	public static readonly Guid GetTaskQueueCache 
			= new Guid("e55bafd5-e896-456a-8677-934d37cd3911");

	
	public static readonly Guid ValidateExpression
			= new Guid("34736ca7-2a99-485d-a95a-1ceee513be0d");
	

	public static readonly Guid ShowAlertsOnTrackingPage
			= new Guid("355198fa-088f-4309-a6f1-1248405b0fa8");

	public static Guid UpdateVehicleServiceAlertFlags
			= new Guid("38372dd6-02d5-4270-8747-3309ce052338");

	public static readonly Guid SetTrackable
		= new Guid("d7f20c90-544f-43f4-b1ec-026dab2c54aa");

	
	public static readonly Guid GetUnitListByIDs 
			= new Guid("e5add3e2-0c2c-429d-9d42-32dc70302df5");
	
	public static readonly Guid GetLiteUnitTrace 
			= new Guid("16d44c78-95ba-4d8a-a869-fb1796724930");

    //& IM-4562
    public static readonly Guid GetLiteUnitTraceLastOdo
            = new Guid(" 9e3e4654-39c8-4680-9dd6-e83dedf8e9ac");
   
	public static readonly Guid IsNextScheduledDateInRange
		= new Guid("3e48fd3c-2f65-4614-941a-dc0e974e627f");

	//& IM-3222
	public static readonly Guid GetJobActivityRefsByID 
			= new Guid("cde33e0a-8ab0-46f6-8fa8-ffd7c892711c");
	//. IM-3222
	//& IM-3222
	public static readonly Guid SaveJobActivityNotesList 
			= new Guid("54aefc9c-4246-4c90-a7ee-3198df23e94a");
	//. IM-3222
	//& IM-3612
	public static readonly Guid ClearUnitLocation 
			= new Guid("d4e7e406-7294-4ff1-a2c1-2c08fa55f1b1");
	//. IM-3612
    //& IM-3488
    public static readonly Guid GetGeofenceHistoryList
            = new Guid("E79FBA3A-651C-4197-A71E-F983D87CC342");
    //. IM-3488 
	//& ipdad service
	public static readonly Guid GetDirections 
			= new Guid("a10ed6bc-5148-450a-89f9-f929fda0d95b");
	//. ipdad service
	//& Ipad Service
	public static readonly Guid GetLoadInfo 
			= new Guid("3375f7ac-47dc-4f26-a4c5-7a055aaa7321");
	//. Ipad Service
	//& Ipad Service
	public static readonly Guid GetStopsByLoadId 
			= new Guid("cf7d5511-891b-4512-baa5-5b9032cf5ee8");
	//. Ipad Service
	//& UI Tracking grid
	public static readonly Guid GetWayPointGeofence 
			= new Guid("0fe84ee2-148c-47f5-80c7-5bdb743a42d3");
	//. UI Tracking grid
	public static readonly Guid HistoricalLoadReport
			= new Guid("c37d7d3e-69a4-454d-a53e-710828748cc5");
	public static readonly Guid EquipmentLocationReport
			= new Guid("e97ac9ac-2c82-4d8c-961c-65dfd5bb4c9e");
	public static readonly Guid OnTimeDeliveryReport
			= new Guid("82e8a808-9272-44a1-aa31-eb1cde2ff10e");
	public static readonly Guid TrailerUtilisationReport
			= new Guid("d2127ed1-fd76-4bf2-89da-3e9065673bf8");
	public static readonly Guid ReturningTrailersReport
			= new Guid("72705943-a997-4fc5-bf64-065ff602d86c");
	public static readonly Guid RecentlyUnusedAssetReport
			= new Guid("01125667-3ead-45b1-a6ed-f8096b82f430");
	public static readonly Guid KPIReport
			= new Guid("b9181f09-c0d4-4982-8954-4b3bb1d9a3ae");
    public static readonly Guid VehicleDeviceMappingReport //IM-4612
            = new Guid("36910550-10ab-4209-b94e-1722f068e994");
      public static readonly Guid KronosReport //IM-4742
            = new Guid("727b5e95-404e-42dd-a0d7-bba94827d748");
	public static readonly Guid LastLocationReport
			= new Guid("679e5297-f8fd-4f75-a3df-251705e6d3eb");
	public static readonly Guid DormantVehiclesReport
			= new Guid("0739b60d-abe9-487d-b23b-dbd66cb57ba5");
	public static readonly Guid TravelTimeReport
            = new Guid("1a92bc19-cfd7-4f30-8546-af433fff1ae5");
	public static readonly Guid GetMessageListByUser
			= new Guid("16A9F6FC-295E-4777-A042-CA1B50F648FB");
	public static readonly Guid SaveMessageViewedByUser
			= new Guid("97B58C9A-552E-4880-AD54-12F11F3AB20C");
	public static readonly Guid GetUtilisationInfo
			= new Guid("F4ACC134-56C5-42AA-9715-E27279ABBE6B");
	public static readonly Guid GetScoreCardInfo
			= new Guid("B1E60049-5EB4-4FD0-ADFE-33BA4DE4A60F");
	public static readonly Guid LocationDashBoard
			= new Guid("1F44B059-31B3-45C3-9D68-2E12384FAB93");
	public static readonly Guid ComplianceDashBoard
			= new Guid("DA3BE047-8418-4E0B-A742-C1E3D08CC9E5");
	public static readonly Guid GetScoreCardGraphInfo
			= new Guid("C693B394-F07F-4757-9B12-9BE5CC1A827B");
	//& IM-4163
	public static readonly Guid GetFleetWithOutLocation 
			= new Guid("8ba74f26-be6f-4e18-ae12-525a44a51469");
	//. IM-4163
    public static readonly Guid BoralConcreteVehicleDeviceImport
        = new Guid("0e385532-8bb9-460c-bf80-56649873af15");
    //IM-5501 
    public static readonly Guid ChecklistException 
        = new Guid("ae364ad7-16ba-4f21-9be4-4d74ab13ff00");
    //IM-5502 
    public static readonly Guid ChecklistAudit
        = new Guid("d49e5a23-a4ae-4689-9233-2c9a174f9aac");

	//& SC-APP
	public static readonly Guid GetSubmittedChecklistByVehicleID 
			= new Guid("777e0a0c-37d6-4d99-b01c-340653119c94");

    public static readonly Guid GetSubmittedChecklistByOwnerIdAndType
            = new Guid("B3544E91-D1C6-46FA-BD75-D90A49D8ECC7");

    public static readonly Guid GetSubmittedChecklistForSerivceMgr
           = new Guid("D562110E-EE59-4C4D-AF3C-092B502672B3");
	//. SC-APP
	//& OutstandingItem
	public static readonly Guid GetOutStandingItemByOwnerID 
			= new Guid("17bc03ee-7c28-4120-8f4c-86943d5acbc1");
	//. OutstandingItem
	//& API
	public static readonly Guid GetCheckListGroupsByDriverID 
			= new Guid("e5345196-7b2c-4efa-bbc8-6d5b9c698f0e");
	//. API
	
	//& API
	public static readonly Guid GetCheckListByDriverID 
			= new Guid("d2032c06-64c4-47cf-bf42-c6542345dc7d");
	//. API
	//& checklist API
	public static readonly Guid GetResolvedCheckListItems 
			= new Guid("059a2926-933d-45cb-850c-513c5e37879d");
	//. checklist API
	//& checklist
	public static readonly Guid GetSubmittedChecklistByChecklistID 
			= new Guid("5c621426-1978-49b7-a14d-f0688914ab81");
	//. checklist
	//& ChecklistDriver
	public static readonly Guid GetVehicleByDriverID 
			= new Guid("fe587d5f-84f2-48c7-ab15-d346e47803fa");
    public static readonly Guid GetUsernameifExists
            = new Guid("27F37457-6748-475B-AD64-EEF98032EA0A");
	//. ChecklistDriver
	public static readonly Guid EditChecklists
			= new Guid("75994616-325e-4bfe-bad6-7dbecfaa1219");
    public static readonly Guid ViewChecklists
            = new Guid("2738D364-C102-4B7F-A3AD-7CAE32F70653");
	public static readonly Guid ViewCompletedChecklists
			= new Guid("7F81777A-270F-4882-A1A2-3B7532DA797E");
#if NewAuthToken

	//& `jira`
	public static readonly Guid `authtoken` 
			= new Guid("`newguid`");
	//. `jira`
#endif

	/// <summary>
	/// Gives access to IDS Incident Reports in UI
	/// </summary>
	public static readonly Guid GetIdsReports
				= new Guid("05e9f722-ec97-49eb-8c82-f6dca79a0b5a");

    public static readonly Guid ChangeVideoStatus
                = new Guid("1EB9B9E3-C0E3-40DD-A172-21CE44764E14");

    public static readonly Guid DownloadVideo
                = new Guid("2F1F35B7-AE2D-45E8-A225-7F0D5D0E49CC");
    

    public static readonly Guid AttachTrailer
        = new Guid("78F6C8A9-1D30-44C7-ACF5-C54528D5EF89");

    public static readonly Guid DetachTrailer
        = new Guid("51FF9FB1-919F-4518-9F84-57A3A9558612");
    public static Guid SaveSubmittedChecklistAttachment
        = new Guid("1AE910DB-F353-4330-A848-289D7CE7D0F1");

    public static readonly Guid GetAttributeValueList = new Guid("D6DC2821-9B34-48E3-AB23-EB879A88E050");
    public static readonly Guid GetAttributeValue = new Guid("7D7DFA09-7B64-4C42-94E3-30A151F6D928");
    public static readonly Guid GetAttributeValueByID = new Guid("30A4A82C-B83A-468F-BEB8-691F722C39F9");
    public static readonly Guid GetAttributeValueByEntityType_EntityID_Name = new Guid("4B1903FB-98F7-4E5B-B777-044E717C31AB");
}
