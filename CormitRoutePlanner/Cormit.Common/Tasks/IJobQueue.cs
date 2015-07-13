using System;

namespace Imarda.Lib
{
	public interface IJobQueue
	{
		string ID { get; }
		bool IsBusy { get; }
		string Category { get;  }
		DateTime EmptySince { get; }
	}
}
