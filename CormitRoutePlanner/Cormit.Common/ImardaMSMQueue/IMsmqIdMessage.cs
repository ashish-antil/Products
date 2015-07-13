using System;

// ReSharper disable once CheckNamespace
namespace Imarda.Lib
{
	public interface IMsmqIdMessage
	{
		string MessageId { get; set; }
	}
}