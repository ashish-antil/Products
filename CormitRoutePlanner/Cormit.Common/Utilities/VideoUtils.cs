using System.Collections.Generic;
using Imarda.Lib.MVVM.Extensions;
using MediaInfoNET;

namespace Imarda.Lib.Utilities
{
	public static class VideoInfoConstants
	{
		public const string FileName = "File Name";
		public const string Format = "Format";
		public const string Duration = "Duration";
		public const string Bitrate = "Bitrate";
		public const string AudioFormat = "Audio Format";
		public const string AudioBitrate = "Audio Bit Rate";
		public const string AudioChannels = "Audio Channels";
		public const string AudioSamplingRate = "Audio Sampling Rate";
		public const string VideoFormat = "Video Format";
		public const string VideoBitrate = "Video Bit Rate";
		public const string VideoFrameRate = "Video Frame Rate";
		public const string VideoFrameSize = "Video Frame Size";
	}

	public static class VideoUtils
	{
		public static Dictionary<string,string> GetVideoInfo(string fileName)
		{
			var dic = new Dictionary<string, string>();
			var mediaFile = new MediaFile(fileName);

			dic.Add(VideoInfoConstants.FileName, mediaFile.Name);
			dic.Add(VideoInfoConstants.Format, mediaFile.General.Format);
			dic.Add(VideoInfoConstants.Duration, mediaFile.General.DurationString);
			dic.Add(VideoInfoConstants.Bitrate, mediaFile.General.Bitrate.ToInvariantString());

			if (mediaFile.Audio.Count > 0)
			{

				dic.Add(VideoInfoConstants.AudioFormat, mediaFile.Audio[0].Format);
				dic.Add(VideoInfoConstants.AudioBitrate, mediaFile.Audio[0].Bitrate.ToInvariantString());
				dic.Add(VideoInfoConstants.AudioChannels, mediaFile.Audio[0].Channels.ToInvariantString());
				dic.Add(VideoInfoConstants.AudioSamplingRate, mediaFile.Audio[0].SamplingRate.ToInvariantString());
			}

			if (mediaFile.Video.Count > 0)
			{

				dic.Add(VideoInfoConstants.VideoFormat, mediaFile.Video[0].Format);
				dic.Add(VideoInfoConstants.VideoBitrate, mediaFile.Video[0].Bitrate.ToInvariantString());
				dic.Add(VideoInfoConstants.VideoFrameRate, mediaFile.Video[0].FrameRate.ToInvariantString());
				dic.Add(VideoInfoConstants.VideoFrameSize, mediaFile.Video[0].FrameSize);
			}

			return dic;
		}
	}
}
