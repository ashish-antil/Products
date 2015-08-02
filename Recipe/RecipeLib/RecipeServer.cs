using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bend.Util;
using System.IO;
using System.Threading;

namespace RecipeLib
{
	internal class RecipeServer : HttpServer
	{
		private const string ContentType = "mime";
		private const string Response = "post";
		private readonly Recipe _Engine;
		private readonly string _Script;

		internal RecipeServer(string host, int port, Recipe engine, string recipe)
			: base(host, port)
		{
			_Engine = engine;
			_Script = recipe;
		}

		private void CopyHeaders(HttpProcessor p, Recipe engine)
		{
			foreach (string key in p.HttpHeaders.Keys)
			{
				engine.SetMacro("q_" + key, p.HttpHeaders[key]);
			}
			int i = p.HttpUrl.IndexOf('?');
			string path;
			if (i == -1)
			{
				path = Unescape(p.HttpUrl);
			}
			else
			{
				path = Unescape(p.HttpUrl.Substring(0, i));

				var query = p.HttpUrl.Substring(i + 1); // skip '?'
				string[] pairs = query.Split('&');
				foreach (string s in pairs)
				{
					string u = Unescape(s);
					string[] t = u.Split('=');
					string key = t[0];
					string value = t.Length == 1 ? "" : t[1];
					engine.SetMacro("q_" + key, value); // try to avoid clashing with existing macros by prefixing with q_
				}
			}
			engine.SetMacro("query", path);
		}

		private string Unescape(string s)
		{
			return Uri.UnescapeDataString(s).Replace('+', ' ');
		}

		public override void HandleGETRequest(HttpProcessor p)
		{
			//Console.WriteLine("request: {0}", p.HttpUrl);
			var engine = (Recipe)_Engine.Clone();
			CopyHeaders(p, engine);
			engine.SetMacro(Response, "");
			engine.Run(new LineReader(_Script, "GET"));
			WriteResponse(p, engine);
		}

		public override void HandlePOSTRequest(HttpProcessor p, StreamReader inputData)
		{
			//Console.WriteLine("POST request: {0}", p.HttpUrl);
			var engine = (Recipe)_Engine.Clone();
			CopyHeaders(p, engine);
			engine.SetMacro(Response, inputData.ReadToEnd());
			engine.Run(new LineReader(_Script, "POST"));
			WriteResponse(p, engine);
		}

		private void WriteResponse(HttpProcessor p, Recipe engine)
		{
			p.WriteSuccess(engine.GetMacro(ContentType));
			p.OutputStream.WriteLine(engine.GetMacro(Response));
			engine.RemoveMacro(Response);
		}

		public static void Test(string host, int port, Recipe engine, string recipe)
		{
			new Thread(new RecipeServer(host, port, engine, recipe).Listen).Start();
		}
	}
}