using System;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Net.FTP
{

    

	#region "FTP file info class"
	/// <summary>
	/// Represents a file or directory entry from an FTP listing
	/// </summary>
	/// <remarks>
	/// This class is used to parse the results from a detailed
	/// directory list from FTP. It supports most formats of
	/// 
	/// v1.1 fixed bug in Fullname/path
	/// </remarks>
	public class FTPfileInfo
	{
        static System.IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

       // log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		//Stores extended info about FTP file
		
		#region "Properties"
		public string FullName
		{
			get
			{
				return Path + Filename;
			}
		}
		public string Filename
		{
			get
			{
				return _filename;
			}
		}
		/// <summary>
		/// Path of file (always ending in /)
		/// </summary>
		/// <remarks>
		/// 1.1: Modifed to ensure always ends in / - with thanks to jfransella for pointing this out
		/// </remarks>
		public string Path
		{
			get
			{
				return _path + (_path.EndsWith("/") ? "" : "/");
			}
		}
		public DirectoryEntryTypes FileType
		{
			get
			{
				return _fileType;
			}
		}
		public long Size
		{
			get 
			{
				return _size;
			}
		}
		public DateTime FileDateTime
		{
			get
			{
                //if (_fileDateTime < DateTime.Parse("1901-1-1"))
                //{
                //    _fileDateTime = DateTime.Now;
                //}

				return _fileDateTime;
			}
			internal set
			{
				_fileDateTime = value;
			}

		}
		public string Permission
		{
			get
			{
				return _permission;
			}
		}
		public string Extension
		{
			get
			{
				int i = this.Filename.LastIndexOf(".");
				if (i >= 0 && i <(this.Filename.Length - 1))
				{
					return this.Filename.Substring(i + 1);
				}
				else
				{
					return "";
				}
			}
		}
		public string NameOnly
		{
			get
			{
				int i = this.Filename.LastIndexOf(".");
				if (i > 0)
				{
					return this.Filename.Substring(0, i);
				}
				else
				{
					return this.Filename;
				}
			}
		}
		private string _filename;
		private string _path;
		private DirectoryEntryTypes _fileType;
		private long _size;
		private DateTime _fileDateTime;
		private string _permission;
		
		#endregion
		
		/// <summary>
		/// Identifies entry as either File or Directory
		/// </summary>
		public enum DirectoryEntryTypes
		{
			File,
			Directory
		}
		
		/// <summary>
		/// Constructor taking a directory listing line and path
		/// </summary>
		/// <param name="line">The line returned from the detailed directory list</param>
		/// <param name="path">Path of the directory</param>
        /// <param name="isUnix">If the ftp server runs on Unix, the datetime format will be different. </param>
		/// <remarks></remarks>
		public FTPfileInfo(string line, string path, bool isUnix)
		{
			//parse line
			Match m = GetMatchingRegex(line);

            //log
           // Logger.Error(string.Format("Parse Line: {0}",line));

            if (m == null)
            {
                //failed
                if (!line.StartsWith("total"))
                    throw (new ApplicationException("Unable to parse line: " + line));
            }
            else
            {
                _filename = m.Groups["name"].Value;
                _path = path;

                Int64.TryParse(m.Groups["size"].Value, out _size);
                //_size = System.Convert.ToInt32(m.Groups["size"].Value);

                _permission = m.Groups["permission"].Value;
                string _dir = m.Groups["dir"].Value;
                if (_dir != "" && _dir != "-")
                {
                    _fileType = DirectoryEntryTypes.Directory;
                }
                else
                {
                    _fileType = DirectoryEntryTypes.File;
                }

                try
                {
                    if (isUnix)
                    {
                        if (m.Groups["timestamp"].Value.ToCharArray()[4] == ' ')
                        {
                            _fileDateTime = DateTime.ParseExact(m.Groups["timestamp"].Value, "MMM  d HH:mm", culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        else
                        {
                            _fileDateTime = DateTime.ParseExact(m.Groups["timestamp"].Value, "MMM d HH:mm", culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                    }
                    else
                    {

                       // Regex reg=new Regex(@"[0-9]{2}-[0-9]{2}-[0-9]{2}\s{2}")

                        _fileDateTime = DateTime.Parse(m.Groups["timestamp"].Value);
                    }
                }
                catch (Exception)
                {
                    _fileDateTime = Convert.ToDateTime(null);
                }

            }
		}
		
		private Match GetMatchingRegex(string line)
		{
			Regex rx;
			Match m;
			for (int i = 0; i <= _ParseFormats.Length - 1; i++)
			{
				rx = new Regex(_ParseFormats[i]);
				m = rx.Match(line);
				if (m.Success)
				{
					return m;
				}
			}
			return null;
		}
		
		#region "Regular expressions for parsing LIST results"
		/// <summary>
		/// List of REGEX formats for different FTP server listing formats
		/// </summary>
		/// <remarks>
		/// The first three are various UNIX/LINUX formats, fourth is for MS FTP
		/// in detailed mode and the last for MS FTP in 'DOS' mode.
		/// I wish VB.NET had support for Const arrays like C# but there you go
        /// 
        /// Modify at 20116-13, Convert 'd+' to 'd?' in '{3})\\s+\\d+\\s'
		/// </remarks>
		private static string[] _ParseFormats = new string[] { 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d?\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d?\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d?\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d?\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)", 
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)",
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{4}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)"
        };
		#endregion
	}
	#endregion
	
	
}

