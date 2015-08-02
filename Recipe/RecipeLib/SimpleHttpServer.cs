using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;

// offered to the private domain for any use with no restriction
// and also with no warranty of any kind, please enjoy. - David Jeske. 

// simple HTTP explanation
// http://www.jmarshall.com/easy/http/

namespace Bend.Util
{

	public class HttpProcessor
	{
		private TcpClient _Socket;
		private HttpServer _Srv;

		public Stream InputStream;
		public StreamWriter OutputStream;

		public string HttpMethod;
		public string HttpUrl;
		public string HttpProtocolVersion;
		public Dictionary<string, string> HttpHeaders = new Dictionary<string, string>();


		private const int MaxPostSize = 10 * 1024 * 1024; // 10MB

		public HttpProcessor(TcpClient s, HttpServer srv)
		{
			_Socket = s;
			_Srv = srv;
		}


		private string StreamReadLine(Stream inputStream)
		{
			int nextChar;
			string data = "";
			while (true)
			{
				nextChar = inputStream.ReadByte();
				if (nextChar == '\n') { break; }
				if (nextChar == '\r') { continue; }
				if (nextChar == -1) { Thread.Sleep(1); continue; };
				data += Convert.ToChar(nextChar);
			}
			return data;
		}

		public void Process()
		{
			// we can't use a StreamReader for input, because it buffers up extra data on us inside it's
			// "processed" view of the world, and we want the data raw after the headers
			using (InputStream = new BufferedStream(_Socket.GetStream()))
			{

				// we probably shouldn't be using a streamwriter for all output from handlers either
				using (OutputStream = new StreamWriter(new BufferedStream(_Socket.GetStream())))
				{
					try
					{
						ParseRequest();
						ReadHeaders();
						if (HttpMethod.Equals("GET"))
						{
							HandleGETRequest();
						}
						else if (HttpMethod.Equals("POST"))
						{
							HandlePOSTRequest();
						}
					}
					catch (Exception)
					{
						//Console.WriteLine("Exception: " + e.ToString());
						WriteFailure();
					}
					OutputStream.Flush();
				}
			}
			InputStream = null; OutputStream = null;
			_Socket.Close();
		}

		private void ParseRequest()
		{
			string request = StreamReadLine(InputStream);
			string[] tokens = request.Split(' ');
			if (tokens.Length != 3)
			{
				throw new Exception("invalid http request line");
			}
			HttpMethod = tokens[0].ToUpper();
			HttpUrl = tokens[1];
			HttpProtocolVersion = tokens[2];

			//Console.WriteLine("starting: " + request);
		}

		private void ReadHeaders()
		{
			//Console.WriteLine("readHeaders()");
			string line;
			while ((line = StreamReadLine(InputStream)) != null)
			{
				if (line.Equals(""))
				{
					//Console.WriteLine("got headers");
					return;
				}

				int separator = line.IndexOf(':');
				if (separator == -1)
				{
					throw new Exception("invalid http header line: " + line);
				}
				string name = line.Substring(0, separator);
				int pos = separator + 1;
				while ((pos < line.Length) && (line[pos] == ' '))
				{
					pos++; // strip any spaces
				}

				string value = line.Substring(pos, line.Length - pos);
				//Console.WriteLine("header: {0}:{1}", name, value);
				HttpHeaders[name] = value;
			}
		}

		private void HandleGETRequest()
		{
			_Srv.HandleGETRequest(this);
		}

		private const int BufSize = 4096;
		private void HandlePOSTRequest()
		{
			// this post data processing just reads everything into a memory stream.
			// this is fine for smallish things, but for large stuff we should really
			// hand an input stream to the request processor. However, the input stream 
			// we hand him needs to let him see the "end of the stream" at this content 
			// length, because otherwise he won't know when he's seen it all! 

			//Console.WriteLine("get post data start");
			int contentLength = 0;
			var ms = new MemoryStream();
			if (HttpHeaders.ContainsKey("Content-Length"))
			{
				contentLength = Convert.ToInt32(HttpHeaders["Content-Length"]);
				if (contentLength > MaxPostSize)
				{
					throw new Exception(string.Format("POST Content-Length({0}) too big for this simple server", contentLength));
				}
				byte[] buf = new byte[BufSize];
				int toRead = contentLength;
				while (toRead > 0)
				{
					//Console.WriteLine("starting Read, to_read={0}", to_read);

					int numread = InputStream.Read(buf, 0, Math.Min(BufSize, toRead));
					//Console.WriteLine("read finished, numread={0}", numread);
					if (numread == 0)
					{
						if (toRead == 0)
						{
							break;
						}
						else
						{
							throw new Exception("client disconnected during post");
						}
					}
					toRead -= numread;
					ms.Write(buf, 0, numread);
				}
				ms.Seek(0, SeekOrigin.Begin);
			}
			//Console.WriteLine("get post data end");
			_Srv.HandlePOSTRequest(this, new StreamReader(ms));

		}

		public void WriteSuccess(string contentType)
		{
			OutputStream.WriteLine("HTTP/1.0 200 OK");
			OutputStream.WriteLine("Content-Type: " + contentType);
			OutputStream.WriteLine("Connection: close");
			OutputStream.WriteLine("");
		}

		public void WriteFailure()
		{
			OutputStream.WriteLine("HTTP/1.0 404 File not found");
			OutputStream.WriteLine("Connection: close");
			OutputStream.WriteLine("");
		}
	}

	public abstract class HttpServer
	{
		protected int _Port;
		protected string _Host;
		private TcpListener _Listener;
		private volatile bool _IsActive = true;

		public HttpServer(string host, int port)
		{
			_Host = host;
			_Port = port;
		}

		public void Listen()
		{
			_Listener = new TcpListener(IPAddress.Parse(_Host), _Port);
			_Listener.Start();
			while (_IsActive)
			{
				try
				{
					var client = _Listener.AcceptTcpClient();
					var processor = new HttpProcessor(client, this);
					var thread = new Thread(processor.Process);
					thread.Start();
					Thread.Sleep(1);
				}
				catch
				{
				}
			}
		}

		public void Stop()
		{
			_IsActive = false;
			_Listener.Stop();
			_Listener = null;
		}

		public abstract void HandleGETRequest(HttpProcessor p);
		public abstract void HandlePOSTRequest(HttpProcessor p, StreamReader inputData);
	}
}



