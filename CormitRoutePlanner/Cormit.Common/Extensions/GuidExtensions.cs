using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imarda.Lib.Extensions
{
    /// <summary>
    /// Extends a Guid with some extra methods
    /// </summary>
    public static class GuidExtensions
    {

		public static bool IsEmpty(this Guid me)
		{
			return Guid.Empty == me;
		}

        /// <summary>
        /// Works as a SQL IN operator
        /// </summary>
        /// <param name="me">Guid being extended</param>
        /// <param name="values">Guid values to compare the extended Guid against</param>
        /// <returns></returns>
        public static bool In(this Guid me, params Guid[] values)
        {
            return values.Any(value => me == value);
        }

        /// <summary>
        /// Works as a SQL NOT IN operator
        /// </summary>
        /// <param name="me">Guid being extended</param>
        /// <param name="value">Guid to compare the extended Guid agaisnt</param>
        /// <returns></returns>
        public static bool In(this Guid me, Guid value)
        {
            return me == value;
        }

        /// <summary>
        /// Works as a SQL NOT IN operator
        /// </summary>
        /// <param name="me">Guid being extended</param>
        /// <param name="values">Guid values to compare the extended Guid against</param>
        /// <returns></returns>
        public static bool NotIn(this Guid me, params Guid[] values)
        {
            return values.All(value => me != value);
        }

        /// <summary>
        /// Works as a SQL NOT IN operator
        /// </summary>
        /// <param name="me">Guid being extended</param>
        /// <param name="value">Guid to compare the extended Guid agaisnt</param>
        /// <returns></returns>
        public static bool NotIn(this Guid me, Guid value)
        {
            return me != value;
        }

        /// <summary>
        /// Indicates whether or not the Guid in question is empty
        /// </summary>
        /// <param name="me">Guid being extended</param>
        /// <returns></returns>
        public static bool NotEmpty(this Guid me)
        {
            return me != Guid.Empty;
        }
    }
}
