using System;
using System.Collections.Generic;
using System.Text;

namespace FernBusinessBase.Control {
	public static class DatabaseSafeCast {
		public static T Cast<T>(object fromDatatable) {
			if (fromDatatable is DBNull) {
				return default(T);
			}
			// is the value compatable with the requested type
			if (fromDatatable is T) {
				return (T)fromDatatable;
			}
			else {
				// not compatable
				return default(T);
			}
		}
	}
}
