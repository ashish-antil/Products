using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaCRMBusiness;
using System.ServiceModel;
using Imarda.Lib;
namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{

		#region Get Contact
		public GetItemResponse<Contact> GetContact(IDRequest request)
		{
			try
			{
				GetItemResponse<Contact> response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.GetContact(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Contact>>(ex);
			}
		}
		#endregion
		#region GetContactUpdateCount


		public GetUpdateCountResponse GetContactUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				GetUpdateCountResponse response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.GetContactUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}
		#endregion
		#region GetContactListByTimeStamp


		public GetListResponse<Contact> GetContactListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				GetListResponse<Contact> response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.GetContactListByTimeStamp(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		#endregion
		#region GetContactList


		public GetListResponse<Contact> GetContactList(IDRequest request)
		{
			try
			{
				GetListResponse<Contact> response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.GetContactList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		#endregion
		#region Save Contact
		public BusinessMessageResponse SaveContact(SaveRequest<Contact> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.SaveContact(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region SaveContactList


		public BusinessMessageResponse SaveContactList(SaveListRequest<Contact> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{

					channel = service as IClientChannel;
					response = service.SaveContactList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region DeleteContact
		public BusinessMessageResponse DeleteContact(IDRequest request)
		{
			try
			{
				BusinessMessageResponse appResponse = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var response = service.DeleteContact(request);
					ErrorHandler.Check(response);
					
					if (!request.ContainsKey("contactType") || !((int)Contact.ContactType.ContactPerson).ToString().Equals(request["contactType"]))
					{
						response = service.DeleteContactRelatedPersons(request);
						ErrorHandler.Check(response);						  
					}
					appResponse = response;
				});
				 
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<BusinessMessageResponse>(ex);
			}
		}
		#endregion
	}
}
