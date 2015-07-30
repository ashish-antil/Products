using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using Imarda.Lib;

namespace Cormit.Application.RouteApplication.Task
{
	[DataContract]
	public class ReportDef
	{
		/// <summary>
		/// Meaning depends on TimeScale. 0 = from begin of day/week/month until now.
		/// Value must be positive.
		/// </summary>
		[DataMember]
		public int Offset { get; set; }

		/// <summary>
		/// Hour, day, week or month report. Used in combination with Offset.
		/// </summary>
		[DataMember]
		public TimeScale TimeScale { get; set; }


		private DateTime _PeriodBegin;

		/// <summary>
		/// Calculated start time of the reporting period
		/// </summary>
		[DataMember]
		public DateTime PeriodBegin
		{
			get { return _PeriodBegin; }
			set
			{
				if (value.Kind != DateTimeKind.Unspecified) _PeriodBegin = DateTime.SpecifyKind(value, DateTimeKind.Unspecified); // using UTC timezone
				_PeriodBegin = value;
			}
		}

		private DateTime _PeriodEnd;

		/// <summary>
		/// Calculated end time of the reporting period
		/// </summary>
		[DataMember]
		public DateTime PeriodEnd
		{
			get { return _PeriodEnd; }
			set
			{
				if (value.Kind != DateTimeKind.Unspecified) _PeriodEnd = DateTime.SpecifyKind(value, DateTimeKind.Unspecified); // using UTC timezone
				_PeriodEnd = value;
			}
		}

		/// <summary>
		/// e.g. "en-NZ", "de-CH"
		/// </summary>
		[DataMember]
		public string Locale { get; set; }

		/// <summary>
		/// Use CultureHelper. Other valid values are: Guid.Empty for Company, and any other Guid for user specific.
		/// </summary>
		[DataMember(EmitDefaultValue = false)]
		public Guid UnitSystemID { get; set; }


		/// <summary>
		/// Override any preferences as defined by UnitSystemID.
		/// </summary>
		[DataMember(EmitDefaultValue = false)]
		public string PrefOverride { get; set; }


		/// <summary>
		/// Identifies report type and version.
		/// </summary>
		[DataMember]
		public Guid ReportTypeID { get; set; }

		/// <summary>
		/// Parameters other than local-dependent. E.g. time zone, unit system etc. is not part of this.
		/// </summary>
		[DataMember(Name = "Param")]
		public string ReportParameters { get; set; }


		/// <summary>
		/// Get the report context: all the parameters required by the report
		/// </summary>
		/// <param name="tzi">user's timezone </param>
		/// <param name="utc">report generation date/time in UTC </param>
		/// <param name="pref">k|v||k2|v2...  user preferences</param>
		/// <param name="companyID"></param>
		/// <returns></returns>
		public string ReportContext(Guid companyID, TimeZoneInfo tzi, DateTime utc, string pref)
		{
			string tzDisplayName;

			DateTime userNow = TimeZoneInfo.ConvertTimeFromUtc(utc, tzi);
			double offset = TimeUtils.CalcTimeZoneOffset(tzi, PeriodBegin, out tzDisplayName);

			string allParameters = ReportParameters.SetKeyValuePairs(
				"StartDate", PeriodBegin.ToString(Formats.JsDateTimeFmt),
				"EndDate", PeriodEnd.ToString(Formats.JsDateTimeFmt),
				"TimeZoneDisplayName", tzDisplayName,
				"OffsetToUtc", offset.ToString(),
				"DateCreated", userNow.ToString(Formats.JsDateTimeFmt),
				"Language", Locale,
				"UserPreferenceString", StringUtils.MergeNonArrayKeyValuePairs(pref, PrefOverride).Replace('|', '&'),
				"CompanyID", companyID.ToString(),
				"Company", companyID.ToString(),
        "TimeZoneID", tzi.Id
				);

			return allParameters;
		}

		[DataMember(Name = "Fmt")]
		public RenderFormat RenderFormat { get; set; }

		[DataMember(Name = "Desc", EmitDefaultValue = false)]
		public string Description;


		/// <summary>
		/// Calculate the period begin and end for user time zone.
		/// </summary>
		/// <param name="run">Has to be in user time zone</param>
		public void CalculatePeriod(DateTime run)
		{
			DateTime midnight = run.Date;
			DateTime begin = TimeUtils.MinValue;
			DateTime end = TimeUtils.MaxValue;
			switch (TimeScale)
			{
				case TimeScale.Hours:
					if (Offset == 0)
					{
						begin = new DateTime(run.Year, run.Month, run.Day, run.Hour, 0, 0, DateTimeKind.Unspecified);
						end = run;
					}
					else
					{
						end = new DateTime(run.Year, run.Month, run.Day, run.Hour, 0, 0, DateTimeKind.Unspecified);
						begin = end.AddHours(-Offset);
					}
					break;

				case TimeScale.Days:
					if (Offset == 0)
					{
						begin = midnight;
						end = run;
					}
					else
					{
						begin = midnight.AddDays(-Offset);
						end = midnight;
					}
					break;

				case TimeScale.Weeks:
					int dow = (int)midnight.DayOfWeek - 1;
					if (dow < 0) dow = 6; //Mon=0, Tue=1,... Sun=6
					if (Offset == 0)
					{
						begin = midnight.AddDays(-dow);
						end = run;
					}
					else
					{
						int n = 7 * Offset;
						begin = midnight.AddDays(-n - dow);
						end = begin.AddDays(n);
					}
					break;

				case TimeScale.Months:
					if (Offset == 0)
					{
						begin = new DateTime(run.Year, run.Month, 1);
						end = run;
					}
					else
					{
						end = new DateTime(run.Year, run.Month, 1);
						begin = end.AddMonths(-Offset);
					}
					break;
			}
			_PeriodBegin = begin;
			_PeriodEnd = end;

		}


		public override string ToString()
		{
			return string.Format("ReportDef({0}{1} rtid={2} \"" + Description + "\")", Offset, TimeScale, ReportTypeID);

		}
	}

	/// <summary>
	/// The renderformat is passed to the ReportingService after applying ToString()
	/// </summary>
	[DataContract]
	public enum RenderFormat
	{
		[EnumMember]
		None,
		[EnumMember]
		XML,
		[EnumMember]
		KML,
		[EnumMember]
		CSV,
		[EnumMember]
		PDF,
		[EnumMember]
		MHTML,
		[EnumMember]
		Excel,
		[EnumMember]
		Image,
		[EnumMember]
        Word,
        [EnumMember]
        Xls2Csv
	}

	[DataContract]
	public enum TimeScale
	{
		[EnumMember]
		Undefined,
		[EnumMember]
		Hours,
		[EnumMember]
		Days,
		[EnumMember]
		Weeks,
		[EnumMember]
		Months,
	}


}
