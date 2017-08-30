using System.Net;
using System.Net.Sockets;

namespace InstagramPhotos.Utility.Net
{
    /// <summary>
    /// Network utilities.
    /// </summary>
    public static class NetUtility
    {
        #region HostName

        /// <summary>
        /// Localhost name.
        /// </summary>
        public static string HostName
        {
            get { return Dns.GetHostName(); }
        }

        #endregion

        #region HostAddress

        /// <summary>
        /// Localhost ip address.
        /// </summary>
        public static IPAddress HostAddress
        {
            get { return Dns.GetHostAddresses(HostName)[0]; }
        }

        #endregion

        #region GetSocketAddress

        /// <summary>
        /// Get ip address by socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string GetSocketAddress(Socket socket)
        {
            if (socket != null)
            {
                return ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            }
            return "";
        }

        #endregion

        #region GetSocketName

        /// <summary>
        /// Get host name by socket
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public static string GetSocketName(Socket socket)
        {
            if (socket != null)
            {
                return Dns.GetHostEntry(((IPEndPoint)socket.RemoteEndPoint).Address).HostName;
            }
            return "";
        }

        #endregion
    }
}
