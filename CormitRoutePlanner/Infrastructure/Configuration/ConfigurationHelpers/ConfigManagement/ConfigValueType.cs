using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public static class ConfigValueType
	{
		public const int System = -1;
		public const int Unknown = 0;
		public const int StringLiteral = 1;
		public const int StringWithArgs = 2;
		public const int RichText = 3;
		public const int Bool = 4;
		public const int Integer = 5;
		public const int Decimal = 6;
		public const int Real = 7;
		public const int Measurement = 8;
		public const int Color = 9;
		public const int Font = 10;
		public const int Parameters = 11;
		public const int Uri = 20;
		public const int Xml = 30;
	}


}
