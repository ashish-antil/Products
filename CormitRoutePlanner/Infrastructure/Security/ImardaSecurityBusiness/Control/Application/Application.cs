using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		#region Gets an Application from a DataRow
		private Application GetApplicationFromData(IDataReader dr)
		{
			var app = new Application
			          {
			          	ID = (Guid) dr["ID"],
			          	Name = dr["ApplicationName"].ToString(),
			          	Active = (bool) dr["Active"],
			          	Deleted = (bool) dr["Deleted"],
			          	Updated = (DateTime) dr["DateModified"]
			          };
			app.Updated = (DateTime)dr["DateCreated"];
			return app;
		}
		#endregion

		#region GetApplicationList
		/// <summary>
		/// Gets a list of all Applications
		/// </summary>
		/// <returns></returns>
		public GetApplicationListResponse GetApplicationList()
		{
			GetApplicationListResponse response = new GetApplicationListResponse();
			response.ApplicationList = new List<Application>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetApplicationList"))
				{

					while (dr.Read())
					{
						response.ApplicationList.Add(GetApplicationFromData(dr));
					}
				}
				return response;
			}
			catch (Exception e)
			{
				response.Status = false;
				response.StatusMessage = "Could not get application list: " + e.Message + e.StackTrace;
				return response;
			}
		}
		#endregion

		#region DeleteApplication
		/// <summary>
		/// Marks the Application with the given ID as Deleted
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public BusinessMessageResponse DeleteApplication(DeleteApplicationRequest request)
		{
			BusinessMessageResponse response = new BusinessMessageResponse();
			try
			{

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
				db.ExecuteNonQuery("SPDeleteApplication", request.ID);
			}
			catch (Exception e)
			{
				response.Status = false;
				response.StatusMessage = "Couldn't delete Application: " + e.Message + e.StackTrace;
			}
			return response;
		}
		#endregion

		#region SaveApplication
		/// <summary>
		/// Saves the given Application to the database
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public BusinessMessageResponse SaveApplication(SaveApplicationRequest request)
		{
			BusinessMessageResponse response = new BusinessMessageResponse();
			try
			{

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetApplication", request.Application.ID))
				{
					Application app = request.Application;
					if (dr.Read())
					{
						db.ExecuteNonQuery("SPSaveApplication", app.ID, app.Name, app.Active, app.Deleted);
					}
					else
					{
						db.ExecuteNonQuery("SPAddApplication", app.ID, app.Name);
					}
					return response;
				}
			}
			catch (Exception e)
			{
				response.Status = false;
				response.StatusMessage = "Couldn't save Application: " + e.Message + e.StackTrace;
				return response;
			}
		}
		#endregion

	}
}
