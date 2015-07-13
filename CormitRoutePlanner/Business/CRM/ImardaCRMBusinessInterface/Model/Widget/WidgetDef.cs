using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Control;

namespace ImardaCRMBusiness
{
	[DataContract]
	public enum WidgetType { RealTime, Trend };

	public class WidgetDef : FullBusinessEntity
	{
		#region Constructor
		public WidgetDef()
			: base()
		{
		}
		#endregion

		#region Properties
		//[DataMember]
		//public Guid ID { get; set; }
		[DataMember]
		public string Title { get; set; }
		[DataMember]
		public WidgetType Type { get; set; }
		[DataMember]
		public int Row { get; set; }
		[DataMember]
		public int Column { get; set; }
		[DataMember]
		public int RowSpan { get; set; }
		[DataMember]
		public int ColumnSpan { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		#endregion

		#region Methods
		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ID = GetValue<Guid>(dr, "ID");
			Title = GetValue<string>(dr, "Title");
			Type = (WidgetType)GetValue<int>(dr, "Type");
			Row = GetValue<int>(dr, "Row");
			Column = GetValue<int>(dr, "Column");
			RowSpan = GetValue<int>(dr, "RowSpan");
			ColumnSpan = GetValue<int>(dr, "ColumnSpan");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif


		}

		#endregion

	}
}
