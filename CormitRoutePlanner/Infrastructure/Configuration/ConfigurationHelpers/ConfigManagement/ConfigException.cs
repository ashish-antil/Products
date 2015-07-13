using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// Thrown by the configuration system. 
	/// </summary>
	public class ConfigException : Exception
	{
		public ConfigException() { }
		public ConfigException(string message) : base(message) { }
		public ConfigException(string message, Exception inner) : base(message, inner) { }
		public Guid ID { get; set; }
		public int ValueType { get; set; }
		public object Value { get; set; }
	}
}
