using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

//# gs-372
namespace FernBusinessBase
{
	/// <summary>
	/// Request to get the whole extent of a table filtered by optional criteria.
	/// </summary>
	[MessageContract]
	[Serializable]
	public class GetFilteredExtentRequest : ParameterMessageBase
	{
		public GetFilteredExtentRequest() { }

		[MessageBodyMember]
		public DateTime? CreatedAfter { get; set; }

		[MessageBodyMember]
		public DateTime? CreatedBefore { get; set; }

		[MessageBodyMember]
		public DateTime? ModifiedAfter { get; set; }

		[MessageBodyMember]
		public DateTime? ModifiedBefore { get; set; }

		[MessageBodyMember]
		public bool? Deleted { get; set; }

		[MessageBodyMember]
		public bool? Active { get; set; }

		[MessageBodyMember]
		public bool? Template { get; set; }

		[MessageBodyMember]
		public int? Scope { get; set; }

		[MessageBodyMember]
		public string Path { get; set; }

		[MessageBodyMember]
		public int? Limit { get; set; }

		[MessageBodyMember]
		public int? Offset { get; set; }

		[MessageBodyMember]
		public Guid? OwnerID { get; set; }

		[MessageBodyMember]
		public int? OwnerType { get; set; }

		[MessageBodyMember]
		public List<SortColumn> SortColumns = new List<SortColumn>();

		[MessageBodyMember]
		public List<Condition> Conditions = new List<Condition>();

		[MessageBodyMember]
		public ConditionLogicalOperator LogicalOperator { get; set; }
	}

	[Serializable]
	public class SortColumn
	{
		public string FieldName { get; set; }
		public bool SortDescending { get; set; }
	}

	[Serializable]
	public class Condition
	{
		public ConditionType ConditionType { get; set; }

		public string FieldName { get; set; }
		public ConditionOperator Operator { get; set; }
		public string SearchValue { get; set; }

		//for sub clauses
		public List<Condition> SubConditions { get; set; }
		public ConditionLogicalOperator SubLogicalOperator { get; set; }

		private string GetConditionSQL(Condition condition)
		{
			if (!string.IsNullOrWhiteSpace(condition.SearchValue)) //in some cases like IsNull and others it may be null
			{
				//replace "?" wildcard with sql equivalent 
				condition.SearchValue = condition.SearchValue.Replace("?", "_");
				condition.SearchValue = condition.SearchValue.Replace("*", "%");
				//booleans - to review and workout further
				if (condition.SearchValue.ToLower() == "true")
					condition.SearchValue = "1";
				else if (condition.SearchValue.ToLower() == "false")
					condition.SearchValue = "0";
			}
			switch (condition.Operator)
			{
				case ConditionOperator.Equal:
					if (condition.ConditionType == ConditionType.TextCondition)
					{
						if (condition.SearchValue.Contains("_") || condition.SearchValue.Contains("%"))
							return string.Format("{0} LIKE '{1}'", condition.FieldName, condition.SearchValue);
						else
							return string.Format("{0} = '{1}'", condition.FieldName, condition.SearchValue);
					}
					else
						return string.Format("{0} = {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.NotEqual:
					if (condition.ConditionType == ConditionType.TextCondition)
						return string.Format("{0} <> '{1}'", condition.FieldName, condition.SearchValue);
					else
						return string.Format("{0} <> {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.BeginsWith:
					return string.Format("{0} LIKE '{1}%'", condition.FieldName, condition.SearchValue);
				case ConditionOperator.EndsWith:
					return string.Format("{0} LIKE '%{1}'", condition.FieldName, condition.SearchValue);
				case ConditionOperator.Contains:
					return string.Format("{0} LIKE '%{1}%'", condition.FieldName, condition.SearchValue);
				case ConditionOperator.NotContains:
					return string.Format("NOT ({0} LIKE '%{1}%')", condition.FieldName, condition.SearchValue);
				case ConditionOperator.IsNull:
					return string.Format("{0} IS NULL", condition.FieldName);
				case ConditionOperator.NotIsNull:
					return string.Format("{0} IS NOT NULL", condition.FieldName);
				case ConditionOperator.IsEmpty:
					return string.Format("{0} = ''", condition.FieldName);
				case ConditionOperator.NotIsEmpty:
					return string.Format("{0} <> ''", condition.FieldName);
				case ConditionOperator.GreaterThan:
					if (condition.ConditionType == ConditionType.TextCondition) //e.g. for date
						return string.Format("{0} > '{1}'", condition.FieldName, condition.SearchValue);
					else
						return string.Format("{0} > {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.LessThan:
					if (condition.ConditionType == ConditionType.TextCondition) //e.g. for date
						return string.Format("{0} < '{1}'", condition.FieldName, condition.SearchValue);
					else
						return string.Format("{0} < {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.GreaterThanOrEqualTo:
					if (condition.ConditionType == ConditionType.TextCondition) //e.g. for date
						return string.Format("{0} >= '{1}'", condition.FieldName, condition.SearchValue);
					else
						return string.Format("{0} >= {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.LessThanOrEqualTo:
					if (condition.ConditionType == ConditionType.TextCondition) //e.g. for date
						return string.Format("{0} <= '{1}'", condition.FieldName, condition.SearchValue);
					else
						return string.Format("{0} <= {1}", condition.FieldName, condition.SearchValue);
				case ConditionOperator.Wildcard:
					{
						var res = "";
						///we may may have multiple parts e.g. *001 !*2001 !device11001 which must be ANDed together
						var con = "";
						var parts = condition.SearchValue.Split(' ');
						for (int i = 0; i < parts.Length; i++)
						{
							var part = parts[i];
							if (part.Contains("!"))
								res += con + string.Format("{0} NOT LIKE '{1}'", condition.FieldName, part.Replace("!", ""));
							else
								res += con + string.Format("{0} LIKE '{1}'", condition.FieldName, part);
							con = " AND ";
						}

						return res;
					}

                //IAC-146
                case ConditionOperator.In:
			    {
                    return string.Format("{0} IN {1}", condition.FieldName, condition.SearchValue); //SearchValue must include the brackets
			    }
			}
			//default to contains
			return string.Format("{0} LIKE '%{1}%'", condition.FieldName, condition.SearchValue);

		}

		public string GetSQL()
		{
			var sql = "";
			if (!string.IsNullOrEmpty(FieldName))
			{
				sql = GetConditionSQL(this);
			}

			var subClause = "";
			if (SubConditions != null && SubConditions.Count > 0)
			{

				var con = "";
				foreach (Condition condition in SubConditions)
				{
					var part = condition.GetSQL();
					subClause += con + part;
					if (SubLogicalOperator == ConditionLogicalOperator.And)
						con = " AND ";
					else
						con = " OR ";
				}
			}
			if (!string.IsNullOrEmpty(subClause))
			{
				if (string.IsNullOrEmpty(sql))
					sql = string.Format("({0})", subClause);
				else if (SubLogicalOperator == ConditionLogicalOperator.Or)
					sql = string.Format("{0} OR ({1})", sql, subClause);
				else
					sql = string.Format("{0} AND ({1})", sql, subClause);

			}
			return sql;
		}

		public static string GetSQL(Condition condition)
		{
			return condition.GetSQL();
		}
	}

	[Serializable]
	public enum ConditionType { TextCondition, ValueCondition }

	[Serializable]
	public enum ConditionOperator
	{
		Equal = 0,
		NotEqual = 1,
		GreaterThan = 2,
		LessThan = 3,
		GreaterThanOrEqualTo = 4,
		LessThanOrEqualTo = 5,
		Between = 6,
		NotBetween = 7,
		Contains = 8,
		NotContains = 9,
		BeginsWith = 10,
		EndsWith = 11,
		IsNull = 12,	 //Null or System.DBNull
		NotIsNull = 13,
		IsEmpty = 14,
		NotIsEmpty = 15,
		In = 16,
		NotIn = 17,
		Regex = 18,
		Wildcard = 19
	}

	[Serializable]
	public enum ConditionLogicalOperator
	{
		And = 0,
		Or = 1,
	}
}
