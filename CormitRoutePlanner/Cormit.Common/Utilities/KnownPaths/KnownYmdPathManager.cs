using System;
using System.Collections.Generic;
using System.Linq;
using Imarda.Lib.MVVM.Extensions;

namespace Imarda.Lib.Utilities.KnownPaths
{
	/// <summary>
	/// This class is used by the injectors to manage Year\Month\Date directory structure in S3
	/// </summary>
	public class KnownYmdPathManager : KnownPathManagerBase
	{
		private Dictionary<DateTime,string> _knownYmdPaths;

		public KnownYmdPathManager()
		{
			GetDateTimeFromPath = IoUtils.GetDateTimeFromYmdPath;
		}
		
		/// <summary>
		/// Clears paths older than n days from the known paths dictionary
		/// </summary>
		/// <param name="param"></param>
		public override void Flush(int param = 10)
		{
			//when creating new directories - should be only a few times a day - clear the old ones to free the cache
			var now = DateTime.UtcNow;
			now = now.AddDays(-param);
			var removables = KnownPaths.Where(entry => entry.Key < now).Select(pair => pair.Key).ToList();
			removables.ForEach(dtkey => KnownPaths.Remove(dtkey));
		}

		/// <summary>
		/// Default implementation expects paths of the form c:\blabla\2014\12\24
		/// </summary>
		public Func<string,DateTime> GetDateTimeFromPath { get; set; }

		protected new Dictionary<DateTime, string> KnownPaths
		{
			get { return _knownYmdPaths ?? (_knownYmdPaths = new Dictionary<DateTime, string>()); }
			set { _knownYmdPaths = value; }
		}

		public bool PathExists(DateTime dt)
		{
			return KnownPaths.ContainsKey(dt);
		}

		public override bool CreatePath(string path)
		{
			var dt = GetDateTimeFromPath(path);
			var result = Create(path);
			if (result && !PathExists(dt))
			{
				KnownPaths.Add(dt,path);
				Flush();
			}
			return result;
		}

	}
}
