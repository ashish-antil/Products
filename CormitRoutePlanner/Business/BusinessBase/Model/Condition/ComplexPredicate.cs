using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FernBusinessBase.Model
{
	/// <summary>
	/// A ComplexPredicate holds a number of conditions and/or subpredicates and a logical operator between them
	/// Only one logical operation can be applied between the conditions on same level
	/// To apply multiple logical operators create subpredicates (sublevels)
	/// So A AND B OR C cannot be defined, instead define either (A AND B) OR C or A AND (B OR C)
	/// where the brackets depict the subPredicate 
	/// </summary>
	[DataContract]
	[Serializable]
	public class ComplexPredicate
	{
		#region Instant Variables
		private List<SingleArgCondition> _conditions = new List<SingleArgCondition>();
		private List<ComplexPredicate> _subPredicates = new List<ComplexPredicate>();
		private LogicalOperator _logicalOperator;
		#endregion

		#region Constructor
		public ComplexPredicate()
		{
			_conditions = new List<SingleArgCondition>();
			_subPredicates = new List<ComplexPredicate>();
			_logicalOperator = LogicalOperator.AND; //default
		}

		public ComplexPredicate(LogicalOperator logicalOperator)
		{
			_conditions = new List<SingleArgCondition>();
			_subPredicates = new List<ComplexPredicate>();
			_logicalOperator = logicalOperator;
		}

		#endregion

		#region Properties
		[DataMember]
		public LogicalOperator LogicalOperator
		{
			get { return _logicalOperator; }
			set { _logicalOperator = value; }
		}

		[DataMember]
		public List<SingleArgCondition> Conditions
		{
			get { return _conditions; }
			set { _conditions = value; }
		}

		[DataMember]
		public List<ComplexPredicate> SubPredicates
		{
			get { return _subPredicates; }
			set { _subPredicates = value; }
		}

		#endregion

		#region Public Methods
		public void Add(SingleArgCondition condtion)
		{
			_conditions.Add(condtion);
		}

		public void Add(ComplexPredicate subPredicate)
		{
			_subPredicates.Add(subPredicate);
		}

		#endregion
	}
}
