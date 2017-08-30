using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace InstagramPhotos.Utility.Net.FTP
{

    #region "FTP client class"

    /// <summary>
    ///     A wrapper class for .NET 2.0 FTP protocol
    /// </summary>
    /// <remarks>
    ///     This class does not hold open an FTP connection but
    ///     instead is stateless: for each FTP request it
    ///     connects, performs the request and disconnects.
    ///     v1.0 - original version
    ///     v1.1 - added support for EnableSSL, UsePassive and Proxy connections
    ///     v1.2 - added support for downloading correct date/time from FTP server for
    ///     each file
    ///     Added FtpDirectoryExists function as FtpFileExists does not work as directory
    ///     exists check.
    ///     Amended all URI encoding to ensure special characters are encoded
    /// </remarks>
    public class FTPclient
    {
        #region "CONSTRUCTORS"

        /// <summary>
        ///     Blank constructor
        /// </summary>
        /// <remarks>Hostname, username and password must be set manually</remarks>
        public FTPclient()
        {
            IsUnix = false;
            Proxy = null;
            KeepAlive = false;
            EnableSSL = false;
        }

        /// <summary>
        ///     Constructor just taking the hostname
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <remarks></remarks>
        public FTPclient(string Hostname)
        {
            IsUnix = false;
            Proxy = null;
            KeepAlive = false;
            EnableSSL = false;
            _hostname = Hostname;
        }

        /// <summary>
        ///     Constructor taking hostname, username and password
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="Username">Leave blank to use 'anonymous' but set password to your email</param>
        /// <param name="Password"></param>
        /// <remarks></remarks>
        public FTPclient(string Hostname, string Username, string Password)
        {
            IsUnix = false;
            Proxy = null;
            KeepAlive = false;
            EnableSSL = false;
            _hostname = Hostname;
            _username = Username;
            this.Password = Password;
        }

        /// <summary>
        ///     Constructor taking hostname, username and password
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="isUnix"></param>
        /// <param name="Username">Leave blank to use 'anonymous' but set password to your email</param>
        /// <param name="Password"></param>
        /// <remarks></remarks>
        public FTPclient(string Hostname, bool isUnix, string Username, string Password)
        {
            Proxy = null;
            KeepAlive = false;
            EnableSSL = false;
            IsUnix = isUnix;
            _hostname = Hostname;
            _username = Username;
            this.Password = Password;
        }

        /// <summary>
        ///     Constructor taking hostname, username, password and KeepAlive property
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="Username">Leave blank to use 'anonymous' but set password to your email</param>
        /// <param name="Password">Password</param>
        /// <param name="KeepAlive">Set True to keep connection alive after each request</param>
        /// <remarks></remarks>
        public FTPclient(string Hostname, string Username, string Password, bool KeepAlive)
        {
            IsUnix = false;
            Proxy = null;
            EnableSSL = false;
            _hostname = Hostname;
            _username = Username;
            this.Password = Password;
            this.KeepAlive = KeepAlive;
        }

        #endregion

        #region "Directory functions"

        /// <summary>
        ///     Return a simple directory listing
        /// </summary>
        /// <param name="directory">Directory to list, e.g. /pub</param>
        /// <returns>A list of filenames and directories as a List(of String)</returns>
        /// <remarks>For a detailed directory listing, use ListDirectoryDetail</remarks>
        public List<string> ListDirectory(string directory)
        {
            //return a simple list of filenames in directory
            String URI = GetDirectory(directory);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to do simple list
            ftp.Method = WebRequestMethods.Ftp.ListDirectory;

            string str = GetStringResponse(ftp);
            //replace CRLF to CR, remove last instance
            str = str.Replace("\r\n", "\r").TrimEnd('\r');
            //split the string into a list
            var result = new List<string>();
            result.AddRange(str.Split('\r'));
            return result;
        }

        /// <summary>
        ///     List current directory
        /// </summary>
        /// <returns></returns>
        public FTPdirectory ListDirectoryDetail()
        {
            return ListDirectoryDetail("", false);
        }

        /// <summary>
        ///     List specified directory (do not obtain datetime stamps)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FTPdirectory ListDirectoryDetail(string directory)
        {
            return ListDirectoryDetail(directory, true);
        }

        /// <summary>
        ///     Return a detailed directory listing, and also download datetime stamps if specified
        /// </summary>
        /// <param name="directory">Directory to list, e.g. /pub/etc</param>
        /// <param name="doDateTimeStamp">Boolean: set to True to download the datetime stamp for files</param>
        /// <returns>An FTPDirectory object</returns>
        public FTPdirectory ListDirectoryDetail(string directory, bool doDateTimeStamp)
        {
            String URI = GetDirectory(directory);
            FtpWebRequest ftp = GetRequest(URI);
            // Set request to do simple list
            ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            string str = GetStringResponse(ftp);
            // replace CRLF to CR, remove last instance
            str = str.Replace("\r\n", "\r").TrimEnd('\r');
            // split the string into a list
            var dir = new FTPdirectory(str, _lastDirectory, IsUnix);

            // download timestamps if requested
            if (doDateTimeStamp)
            {
                foreach (FTPfileInfo fi in dir)
                {
                    fi.FileDateTime = GetDateTimestamp(fi);
                }
            }

            return dir;
        }

        #endregion

        #region "Upload: File transfer TO ftp server"

        /// <summary>
        ///     Copy a local file to the FTP server from local filename string
        /// </summary>
        /// <param name="localFilename">Full path of the local file</param>
        /// <param name="targetFilename">Target filename, if required</param>
        /// <returns></returns>
        /// <remarks>
        ///     If the target filename is blank, the source filename is used
        ///     (assumes current directory). Otherwise use a filename to specify a name
        ///     or a full path and filename if required.
        /// </remarks>
        public bool Upload(string localFilename, string targetFilename)
        {
            //1. check source
            if (!File.Exists(localFilename))
            {
                throw (new ApplicationException("File " + localFilename + " not found"));
            }
            //copy to FI
            var fi = new FileInfo(localFilename);
            return Upload(fi, targetFilename);
        }

        /// <summary>
        ///     Upload a local file to the FTP server
        /// </summary>
        /// <param name="fi">Source file</param>
        /// <param name="targetFilename">Target filename (optional)</param>
        /// <returns>
        ///     1.2 [HR] simplified checks on target
        /// </returns>
        public bool Upload(FileInfo fi, string targetFilename)
        {
            //copy the file specified to target file: target file can be full path or just filename (uses current dir)

            //1. check target
            string target;
            if (String.IsNullOrEmpty(targetFilename))
            {
                //Blank target: use source filename & current dir
                target = CurrentDirectory + fi.Name;
            }
            else
            {
                //otherwise use original
                target = targetFilename;
            }
            using (FileStream fs = fi.OpenRead())
            {
                try
                {
                    return Upload(fs, target);
                }
                finally
                {
                    //ensure file closed
                    fs.Close();
                }
            }
        }

        /// <summary>
        ///     Upload a local source strean to the FTP server
        /// </summary>
        /// <param name="sourceStream">Source Stream</param>
        /// <param name="targetFilename">Target filename</param>
        /// <returns>
        ///     1.2 [HR] added CreateURI
        /// </returns>
        public bool Upload(Stream sourceStream, string targetFilename)
        {
            // validate the target file
            if (string.IsNullOrEmpty(targetFilename))
            {
                throw new ApplicationException("Target filename must be specified");
            }

            //perform copy
            string URI = CreateURI(targetFilename);
            FtpWebRequest ftp = GetRequest(URI);

            //Set request to upload a file in binary
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;

            //Notify FTP of the expected size
            ftp.ContentLength = sourceStream.Length;

            //create byte array to store: ensure at least 1 byte!
            const int BufferSize = 2048;
            var content = new byte[BufferSize - 1 + 1];

            //open file for reading
            using (sourceStream)
            {
                try
                {
                    sourceStream.Position = 0;
                    //open request to send
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        int dataRead;
                        do
                        {
                            dataRead = sourceStream.Read(content, 0, BufferSize);
                            rs.Write(content, 0, dataRead);
                        } while (!(dataRead < BufferSize));
                        rs.Close();
                    }
                    return true;
                }
                finally
                {
                    //ensure file closed
                    sourceStream.Close();
                }
            }
        }

        #endregion

        #region "Download: File transfer FROM ftp server"

        /// <summary>
        ///     Copy a file from FTP server to local
        /// </summary>
        /// <param name="sourceFilename">Target filename, if required</param>
        /// <param name="localFilename">Full path of the local file</param>
        /// <param name="PermitOverwrite"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Target can be blank (use same filename), or just a filename
        ///     (assumes current directory) or a full path and filename
        ///     1.2 [HR] added CreateURI
        /// </remarks>
        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
        {
            //2. determine target file
            var fi = new FileInfo(localFilename);
            return Download(sourceFilename, fi, PermitOverwrite);
        }

        /// <summary>
        ///     Version taking an FtpFileInfo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localFilename"></param>
        /// <param name="PermitOverwrite"></param>
        /// <returns></returns>
        public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite)
        {
            return Download(file.FullName, localFilename, PermitOverwrite);
        }

        /// <summary>
        ///     Another version taking FtpFileInfo and FileInfo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localFI"></param>
        /// <param name="PermitOverwrite"></param>
        /// <returns></returns>
        public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite)
        {
            return Download(file.FullName, localFI, PermitOverwrite);
        }

        /// <summary>
        ///     Version taking string/FileInfo
        /// </summary>
        /// <param name="sourceFilename"></param>
        /// <param name="targetFI"></param>
        /// <param name="PermitOverwrite"></param>
        /// <returns></returns>
        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
        {
            //1. check target
            if (targetFI.Exists && !(PermitOverwrite))
            {
                return true;
                //throw (new ApplicationException("Target file already exists"));
            }

            //2. check source
            if (String.IsNullOrEmpty(sourceFilename))
            {
                throw (new ApplicationException("File not specified"));
            }

            //3. perform copy
            string URI = CreateURI(sourceFilename);
            FtpWebRequest ftp = GetRequest(URI);

            //Set request to download a file in binary mode
            ftp.Method = WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;

            //open request and get response stream
            using (var response = (FtpWebResponse) ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (FileStream fs = targetFI.OpenWrite())
                    {
                        try
                        {
                            var buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (read != 0);
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            targetFI.Delete();
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }


            return true;
        }

        #endregion

        #region "Other functions: Delete rename etc."

        /// <summary>
        ///     Delete remote file
        /// </summary>
        /// <param name="filename">filename or full path</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool FtpDelete(string filename)
        {
            //Determine if file or full path
            string URI = CreateURI(filename);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to delete
            ftp.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                //get response but ignore it
                string str = GetStringResponse(ftp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public void FtpDeleteAsyc(string filename)
        {
            //Determine if file or full path
            string URI = CreateURI(filename);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to delete
            ftp.Method = WebRequestMethods.Ftp.DeleteFile;

            ftp.BeginGetResponse(result => { }
                , null);
        }

        /// <summary>
        ///     Determine if file exists on remote FTP site
        /// </summary>
        /// <param name="filename">Filename (for current dir) or full path</param>
        /// <returns></returns>
        /// <remarks>Note this only works for files</remarks>
        public bool FtpFileExists(string filename)
        {
            //Try to obtain filesize: if we get error msg containing "550"
            //the file does not exist
            try
            {
                long size = GetFileSize(filename);
                return true;
            }
            catch (WebException webex)
            {
                if (webex.Message.Contains("550")) return false;
                // other errors not handled
                throw;
            }
        }

        /// <summary>
        ///     Determine if a directory exists on remote ftp server
        /// </summary>
        /// <param name="remoteDir">Directory path, e.g. /pub/test</param>
        /// <returns>True if directory exists, otherwise false</returns>
        /// <remarks></remarks>
        public bool FtpDirectoryExists(string remoteDir)
        {
            try
            {
                // attempt directory listing
                List<string> files = ListDirectory(remoteDir);
                return true;
            }
            catch (WebException webex)
            {
                // error should contain 550 if not found
                if (webex.Message.Contains("550")) return false;
                // other error - not handled
                throw;
            }
        }


        /// <summary>
        ///     Determine size of remote file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <remarks>Throws an exception if file does not exist</remarks>
        public long GetFileSize(string filename)
        {
            string URI = CreateURI(filename);

            FtpWebRequest ftp = GetRequest(URI);
            //Try to get info on file/dir?
            ftp.Method = WebRequestMethods.Ftp.GetFileSize;
            string tmp = GetStringResponse(ftp);
            return GetSize(ftp);
        }

        /// <summary>
        ///     Rename a remote file
        /// </summary>
        /// <param name="sourceFilename">Either partial or full filename</param>
        /// <param name="newName">Partial or full name to rename to</param>
        /// <returns></returns>
        public bool FtpRename(string sourceFilename, string newName)
        {
            //Does file exist?
            string source = GetFullPath(sourceFilename);
            if (!FtpFileExists(source))
            {
                throw (new FileNotFoundException("File " + source + " not found"));
            }

            //build target name, ensure it does not exist
            string target = GetFullPath(newName);
            if (target == source)
            {
                throw (new ApplicationException("Source and target are the same"));
            }
            if (FtpFileExists(target))
            {
                throw (new ApplicationException("Target file " + target + " already exists"));
            }

            //perform rename
            string URI = CreateURI(source);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to delete
            ftp.Method = WebRequestMethods.Ftp.Rename;
            ftp.RenameTo = target;
            try
            {
                //get response but ignore it
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                // do not handle error
                throw;
            }
            return true;
        }

        public bool FtpCreateDirectory(string dirpath)
        {
            //perform create
            string URI = CreateURI(dirpath);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to MkDir
            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                //get response but ignore it
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDeleteDirectory(string dirpath)
        {
            //perform remove
            string URI = CreateURI(dirpath);
            FtpWebRequest ftp = GetRequest(URI);
            //Set request to RmDir
            ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
            try
            {
                //get response but ignore it
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Obtain datetimestamp for remote file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetDateTimestamp(FTPfileInfo file)
        {
            DateTime result = GetDateTimestamp(file.Filename);
            file.FileDateTime = result;
            return result;
        }

        /// <summary>
        ///     Obtain datetimestamp for remote file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DateTime GetDateTimestamp(string filename)
        {
            string URI = CreateURI(filename);
            FtpWebRequest ftp = GetRequest(URI);
            ftp.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            return GetLastModified(ftp);
        }

        #endregion

        #region "private supporting fns"

        /// <summary>
        ///     stored credentials
        /// </summary>
        private NetworkCredential _credentials;

        private string _lastDirectory = "";

        /// <summary>
        ///     Ensure the data payload for URI is properly encoded
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string CreateURI(string filename)
        {
            string path;
            if (filename.Contains("/"))
            {
                path = AdjustDir(filename);
            }
            else
            {
                path = CurrentDirectory + filename;
            }
            // escape the path
            string escapedPath = GetEscapedPath(path);
            return Hostname + escapedPath;
        }

        //Get the basic FtpWebRequest object with the
        //common settings and security
        private FtpWebRequest GetRequest(string URI)
        {
            //create request
            var result = (FtpWebRequest) WebRequest.Create(URI);
            //Set the login details
            result.Credentials = GetCredentials();
            // support for EnableSSL
            result.EnableSsl = EnableSSL;
            //keep alive? (stateless mode)
            result.KeepAlive = KeepAlive;
            // support for passive connections 
            result.UsePassive = UsePassive;
            // support for proxy settings
            result.Proxy = Proxy;

            return result;
        }

        /// <summary>
        ///     Ensure chars in path are correctly escaped e.g. #
        /// </summary>
        /// <param name="path">path to escape</param>
        /// <returns></returns>
        private string GetEscapedPath(string path)
        {
            string[] parts;
            parts = path.Split('/');
            string result;
            result = "";
            foreach (string part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                    result += @"/" + Uri.EscapeDataString(part);
            }
            return result;
        }


        /// <summary>
        ///     Get the credentials from username/password
        /// </summary>
        /// <remarks>
        ///     Amended to store credentials on first use, for re-use
        ///     when using KeepAlive=true
        /// </remarks>
        private ICredentials GetCredentials()
        {
            if (_credentials == null)
                _credentials = new NetworkCredential(Username, Password);
            return _credentials;
        }

        /// <summary>
        ///     returns a full path using CurrentDirectory for a relative file reference
        /// </summary>
        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return AdjustDir(file);
            }
            return CurrentDirectory + file;
        }

        /// <summary>
        ///     Amend an FTP path so that it always starts with /
        /// </summary>
        /// <param name="path">Path to adjust</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string AdjustDir(string path)
        {
            return ((path.StartsWith("/")) ? "" : "/") + path;
        }

        private string GetDirectory(string directory)
        {
            string URI;
            if (directory == "")
            {
                //build from current
                URI = Hostname + CurrentDirectory;
                _lastDirectory = CurrentDirectory;
            }
            else
            {
                if (!directory.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                URI = Hostname + directory;
                _lastDirectory = directory;
            }
            return URI;
        }

        //stores last retrieved/set directory

        /// <summary>
        ///     Obtains a response stream as a string
        /// </summary>
        /// <param name="ftp">current FTP request</param>
        /// <returns>String containing response</returns>
        /// <remarks>
        ///     FTP servers typically return strings with CR and
        ///     not CRLF. Use respons.Replace(vbCR, vbCRLF) to convert
        ///     to an MSDOS string
        ///     1.1: modified to ensure accepts UTF8 encoding
        /// </remarks>
        private string GetStringResponse(FtpWebRequest ftp)
        {
            //Get the result, streaming to a string
            string result = "";
            using (var response = (FtpWebResponse) ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(datastream, Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return result;
        }

        /// <summary>
        ///     Gets the size of an FTP request
        /// </summary>
        /// <param name="ftp"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private long GetSize(FtpWebRequest ftp)
        {
            long size;
            using (var response = (FtpWebResponse) ftp.GetResponse())
            {
                size = response.ContentLength;
                response.Close();
            }

            return size;
        }

        /// <summary>
        ///     Internal function to get the modified datetime stamp via FTP
        /// </summary>
        /// <param name="ftp">connection to use</param>
        /// <returns>
        ///     DateTime of file, or throws exception
        /// </returns>
        private DateTime GetLastModified(FtpWebRequest ftp)
        {
            DateTime lastmodified;
            using (var response = (FtpWebResponse) ftp.GetResponse())
            {
                lastmodified = response.LastModified;
                response.Close();
            }
            return lastmodified;
        }

        #endregion

        #region Properties

        private string _currentDirectory = "/";
        private string _hostname;

        private string _username;

        /// <summary>
        ///     Hostname
        /// </summary>
        /// <value></value>
        /// <remarks>
        ///     Hostname can be in either the full URL format
        ///     ftp://ftp.myhost.com or just ftp.myhost.com
        /// </remarks>
        public string Hostname
        {
            get
            {
                if (_hostname.StartsWith("ftp://"))
                {
                    return _hostname;
                }
                return "ftp://" + _hostname;
            }
            set { _hostname = value; }
        }

        /// <summary>
        ///     Username property
        /// </summary>
        /// <value></value>
        /// <remarks>Can be left blank, in which case 'anonymous' is returned</remarks>
        public string Username
        {
            get { return (_username == "" ? "anonymous" : _username); }
            set { _username = value; }
        }

        /// <summary>
        ///     Password for username
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     The CurrentDirectory value
        /// </summary>
        /// <remarks>Defaults to the root '/'</remarks>
        public string CurrentDirectory
        {
            get
            {
                //return directory, ensure it ends with /
                return _currentDirectory + ((_currentDirectory.EndsWith("/")) ? "" : "/");
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                _currentDirectory = value;
            }
        }

        /// <summary>
        ///     Support for EnableSSL flag on FtpWebRequest class
        /// </summary>
        public bool EnableSSL { get; set; }

        /// <summary>
        ///     KeepAlive property for permanent connections
        /// </summary>
        /// <remarks>
        ///     KeepAlive is set False by default (no permanent connection)
        /// </remarks>
        public bool KeepAlive { get; set; }

        /// <summary>
        ///     Support for Passive mode
        /// </summary>
        public bool UsePassive { get; set; }

        /// <summary>
        ///     Support for Proxy settings
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        ///     If ftp server is Unix.
        /// </summary>
        public bool IsUnix { get; set; }

        #endregion
    }

    #endregion
}