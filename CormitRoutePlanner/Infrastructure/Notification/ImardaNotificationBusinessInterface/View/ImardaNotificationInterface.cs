/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/02/2010 3:40 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaNotificationBusiness 
{

	[ServiceContract]
	public partial interface IImardaNotification : IServerFacadeBase
	{
		[OperationContract]
		GetUpdateCountResponseList GetUpdateObjectList(GetUpdateCountRequestList request);
	}
}
