using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UIShell.PageFlowService;
using UIShell.iOpenWorks.Bootstrapper;
using UIShell.OSGi;
using UIShell.OSGi.Logging;
using UIShell.OSGi.Utility;

namespace AdvancedWpfShell1
{
    /// <summary>
    /// WPF startup class.
    /// </summary>
    public partial class App : Application
    {
        // Use object type to avoid load UIShell.OSGi.dll before update.
        private object _bundleRuntime;

        public App()
        {
            UpdateCore();
            StartBundleRuntime();
        }

        void UpdateCore() // Update Core Files, including BundleRepositoryOpenAPI, PageFlowService and OSGi Core assemblies.
        {
            if (AutoUpdateCoreFiles)
            {
                new CoreFileUpdater().UpdateCoreFiles(CoreFileUpdateCheckType.Daily);
            }
        }

        void StartBundleRuntime() // Start OSGi Core.
        {
            FileLogUtility.SetLogLevel(LogLevel);
            FileLogUtility.SetMaxFileSizeByMB(MaxLogFileSize);
            FileLogUtility.SetCreateNewFileOnMaxSize(CreateNewLogFileOnMaxSize);

            var bundleRuntime = new BundleRuntime();
            bundleRuntime.AddService<Application>(this);
            bundleRuntime.Start();

            Startup += App_Startup;
            Exit += App_Exit;
            _bundleRuntime = bundleRuntime;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            Application app = Application.Current;
            var bundleRuntime = _bundleRuntime as BundleRuntime;
            app.ShutdownMode = ShutdownMode.OnLastWindowClose;

            #region Get the main window
            var pageFlowService = bundleRuntime.GetFirstOrDefaultService<IPageFlowService>();
            if (pageFlowService == null)
            {
                throw new Exception("The page flow service is not installed.");
            }

            if (pageFlowService.FirstPageNode == null || string.IsNullOrEmpty(pageFlowService.FirstPageNode.Value))
            {
                throw new Exception("There is not first page node defined.");
            }

            var windowType = pageFlowService.FirstPageNodeOwner.LoadClass(pageFlowService.FirstPageNode.Value);
            if (windowType == null)
            {
                throw new Exception(string.Format("Can not load Window type '{0}' from Bundle '{1}'.", pageFlowService.FirstPageNode.Value, pageFlowService.FirstPageNodeOwner.SymbolicName));
            }

            app.MainWindow = System.Activator.CreateInstance(windowType) as Window;
            #endregion 

            app.MainWindow.Show();
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            if (_bundleRuntime != null)
            {
                var bundleRuntime = _bundleRuntime as BundleRuntime;
                bundleRuntime.Stop();
                _bundleRuntime = null;
            }
        }
        #region Settings
        /// <summary>
        /// 日志级别。
        /// </summary>
        private static LogLevel LogLevel
        {
            get
            {
                string level = ConfigurationSettings.AppSettings["LogLevel"];
                if (!string.IsNullOrEmpty(level))
                {
                    try
                    {
                        object result = Enum.Parse(typeof(LogLevel), level);
                        if (result != null)
                        {
                            return (LogLevel)result;
                        }
                    }
                    catch { }
                }
                return LogLevel.Debug;
            }
        }

        /// <summary>
        /// 日志文件限制的大小。
        /// </summary>
        private static int MaxLogFileSize
        {
            get
            {
                string size = ConfigurationSettings.AppSettings["MaxLogFileSize"];
                if (!string.IsNullOrEmpty(size))
                {
                    try
                    {
                        return int.Parse(size);
                    }
                    catch { }
                }

                return 10;
            }
        }

        /// <summary>
        /// 当日志大小超过限制时，是否新建一个。
        /// </summary>
        private static bool CreateNewLogFileOnMaxSize
        {
            get
            {
                string createNew = ConfigurationSettings.AppSettings["CreateNewLogFileOnMaxSize"];
                if (!string.IsNullOrEmpty(createNew))
                {
                    try
                    {
                        return bool.Parse(createNew);
                    }
                    catch { }
                }

                return false;
            }
        }

        /// <summary>
        /// 当日志大小超过限制时，是否新建一个。
        /// </summary>
        private static bool AutoUpdateCoreFiles
        {
            get
            {
                string autoUpdateCoreFiles = ConfigurationSettings.AppSettings["AutoUpdateCoreFiles"];
                if (!string.IsNullOrEmpty(autoUpdateCoreFiles))
                {
                    try
                    {
                        return bool.Parse(autoUpdateCoreFiles);
                    }
                    catch { }
                }

                return false;
            }
        }
        #endregion
    }
}
