using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace InstagramPhotos.Utility.WinService
{
    public class ServiceControl
    {
        #region DLLImport

        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);

        [DllImport("advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
            int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
            string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll")]
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);

        [DllImport("advapi32.dll")]
        public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);

        [DllImport("advapi32.dll")]
        public static extern int DeleteService(IntPtr SVHANDLE);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        [DllImport("advapi32.dll")]
        private static extern bool ChangeServiceConfig2(IntPtr hService, uint dwInfoLevel, ref string lpInfo);

        #endregion DLLImport

        #region Service Utility

        public static bool InstallService(string svcPath, string svcName, string svcDispName, string svcDesName)
        {
            #region Constants declaration.

            int SC_MANAGER_CREATE_SERVICE = 0x0002;
            int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003;
            int SERVICE_ERROR_NORMAL = 0x00000001;
            int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            int SERVICE_QUERY_CONFIG = 0x0001;
            int SERVICE_CHANGE_CONFIG = 0x0002;
            int SERVICE_QUERY_STATUS = 0x0004;
            int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            int SERVICE_START = 0x0010;
            int SERVICE_STOP = 0x0020;
            int SERVICE_PAUSE_CONTINUE = 0x0040;
            int SERVICE_INTERROGATE = 0x0080;
            int SERVICE_USER_DEFINED_CONTROL = 0x0100;
            int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG |
                                      SERVICE_QUERY_STATUS | SERVICE_ENUMERATE_DEPENDENTS | SERVICE_START | SERVICE_STOP |
                                      SERVICE_PAUSE_CONTINUE | SERVICE_INTERROGATE | SERVICE_USER_DEFINED_CONTROL);
            int SERVICE_AUTO_START = 0x00000002;
            uint dwInfoLevel = 1;

            #endregion Constants declaration.

            try
            {
                IntPtr sc_handle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
                if (sc_handle.ToInt32() != 0)
                {
                    IntPtr sv_handle = CreateService(sc_handle, svcName, svcDispName, SERVICE_ALL_ACCESS,
                        SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, svcPath, null, 0, null,
                        null, null);
                    if (sv_handle.ToInt32() == 0)
                    {
                        CloseServiceHandle(sc_handle);
                        return false;
                    }
                    bool flag = ChangeServiceConfig2(sv_handle, dwInfoLevel, ref svcDesName);

                    if (!flag)
                    {
                        return flag;
                    }

                    CloseServiceHandle(sc_handle);

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool UnInstallService(string svcName)
        {
            int GENERIC_WRITE = 0x40000000;
            IntPtr sc_hndl = OpenSCManager(null, null, GENERIC_WRITE);
            if (sc_hndl.ToInt32() != 0)
            {
                int DELETE = 0x10000;
                IntPtr svc_hndl = OpenService(sc_hndl, svcName, DELETE);
                if (svc_hndl.ToInt32() != 0)
                {
                    int i = DeleteService(svc_hndl);
                    if (i != 0)
                    {
                        CloseServiceHandle(sc_hndl);
                        return true;
                    }
                    CloseServiceHandle(sc_hndl);
                    return false;
                }
                return false;
            }
            return false;
        }

        public static void StartService(string servicename)
        {
            var sc = new ServiceController();
            sc.ServiceName = servicename;

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        public static void StopService(string servicename)
        {
            var sc = new ServiceController();
            sc.ServiceName = servicename;

            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }

        public static ServiceStatus GetServiceStatus(string servicename)
        {
            if (!CheckService(servicename))
            {
                return 0;
            }
            var sc = new ServiceController(servicename);
            return (ServiceStatus) sc.Status;
        }

        public static ServiceControllerStatus GetServiceControllerStatus(string servicename)
        {
            var sc = new ServiceController(servicename);
            return sc.Status;
        }

        public static bool CheckService(string servicename)
        {
            var sc = new ServiceController(servicename);

            try
            {
                string a = sc.ServiceName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    // 摘要:
    //     Indicates the current state of the service.
    public enum ServiceStatus
    {
        NotInstalled = 0,
        // 摘要:
        //     The service is not running. This corresponds to the Win32 SERVICE_STOPPED
        //     constant, which is defined as 0x00000001.
        Stopped = 1,
        //
        // 摘要:
        //     The service is starting. This corresponds to the Win32 SERVICE_START_PENDING
        //     constant, which is defined as 0x00000002.
        StartPending = 2,
        //
        // 摘要:
        //     The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING
        //     constant, which is defined as 0x00000003.
        StopPending = 3,
        //
        // 摘要:
        //     The service is running. This corresponds to the Win32 SERVICE_RUNNING constant,
        //     which is defined as 0x00000004.
        Running = 4,
        //
        // 摘要:
        //     The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING
        //     constant, which is defined as 0x00000005.
        ContinuePending = 5,
        //
        // 摘要:
        //     The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING
        //     constant, which is defined as 0x00000006.
        PausePending = 6,
        //
        // 摘要:
        //     The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant,
        //     which is defined as 0x00000007.
        Paused = 7,
    }
}