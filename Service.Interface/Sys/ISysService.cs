using InstagramPhotos.QueryModel.Sys;
using InstagramPhotos.ViewModel;
using InstagramPhotos.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramPhotos.Service.Interface.Sys
{
    public interface ISysService
    {
        #region  Auto
        #region auto LogInfo

        /// <summary>
        ///     新增LogInfo信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void AddLoginfo(LogInfoEntity dto);

        /// <summary>
        ///     更新LogInfo信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void UpdateLoginfo(LogInfoEntity dto);

        /// <summary>
        ///    获取LogInfoEntity信息
        /// </summary>
        /// <param name="LogId">主键</param>
        LogInfoEntity GetLoginfo(Guid LogId);

        /// <summary>
        ///    根据LogIds数组获取LogInfo信息列表
        /// </summary>
        /// <param name="LogIds">主键集合</param>
        /// <returns>LogInfoEntity信息列表</returns>
        List<LogInfoEntity> GetLoginfos(Guid[] LogIds);

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>LogInfoEntity信息列表</returns>
        List<LogInfoEntity> GetLoginfoDtosByPara(LogInfoQO queryEntity, Boolean isCache);

        /// <summary>
        ///  分页通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="isCache"></param>
        /// <returns>LogInfoEntity信息列表</returns>
        List<LogInfoEntity> GetLoginfoDtosByParaForPage(LogInfoQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache);

        #endregion

        #region auto VersionInfo

        /// <summary>
        ///     新增VersionInfo信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void AddVersioninfo(VersionInfoEntity dto);

        /// <summary>
        ///     更新VersionInfo信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void UpdateVersioninfo(VersionInfoEntity dto);

        /// <summary>
        ///    获取VersionInfoEntity信息
        /// </summary>
        /// <param name="VersionId">主键</param>
        VersionInfoEntity GetVersioninfo(Guid VersionId);

        /// <summary>
        ///    根据VersionIds数组获取VersionInfo信息列表
        /// </summary>
        /// <param name="VersionIds">主键集合</param>
        /// <param name="isCache"></param>
        /// <returns>VersionInfoEntity信息列表</returns>
        List<VersionInfoEntity> GetVersioninfos(Guid[] VersionIds, Boolean isCache=true);

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>VersionInfoEntity信息列表</returns>
        List<VersionInfoEntity> GetVersioninfoDtosByPara(VersionInfoQO queryEntity, Boolean isCache);

        /// <summary>
        ///  分页通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="isCache"></param>
        /// <returns>VersionInfoEntity信息列表</returns>
        List<VersionInfoEntity> GetVersioninfoDtosByParaForPage(VersionInfoQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache);

        #endregion

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="appName"></param>
        /// <param name="classFullName"></param>
        /// <param name="methodName"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="responseTime"></param>
        /// <param name="opName"></param>
        /// <param name="clientIp"></param>
        void WriteInfoLog(SysEnums.SysAppType appType, string appName, string classFullName = "",
            string methodName = "", string input = "",
            string output = "", string responseTime = "", string opName = "", string clientIp = "");

        /// <summary>
        /// 写入LogError信息
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="appName"></param>
        /// <param name="classFullName"></param>
        /// <param name="methodName"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        /// <param name="responseTime"></param>
        /// <param name="opName"></param>
        /// <param name="clientIp"></param>
        void WriteErrorLog(SysEnums.SysAppType appType, string appName, string classFullName = "",
            string methodName = "", string input = "",
            string output = "", string message = "", string stackTrace = "", string responseTime = "",
            string opName = "", string clientIp = "");
        /// <summary>
        /// 清除历史过期日志
        /// </summary>
        /// <returns></returns>
        Result ClearHistoryLog();

        /// <summary>
        /// 清除日志信息
        /// </summary>
        /// <returns></returns>
        Task<Result> ClearHistoryLogAsync();
    }
}
