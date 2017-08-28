/*----------------------------------------
Developed by:WayneChen
Date Created:2015-07-27
Last Updated:2015-07-27
Copyright:CCJoy
Description:通用查询逻辑处理层
----------------------------------------*/

using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.Data;
using InstagramPhotos.Utility.KVStore;
using InstagramPhotos.Utility.Security.Cryptography;

namespace InstagramPhotos.Utility.CommonQuery
{
    public class QueryHelper
    {
        #region 缓存

        /// <summary>
        ///     通用查询参数缓存(GUID)
        /// </summary>
        private readonly KVStoreEntityTable<string, List<Guid>> queryGuidIDsCache =
            KVStoreManager.GetKVStoreEntityTable<string, List<Guid>>("queryGuidIDsCache", 2);

        /// <summary>
        ///     通用查询参数缓存(INT)
        /// </summary>
        private readonly KVStoreEntityTable<string, List<int>> queryIntIDsCache =
            KVStoreManager.GetKVStoreEntityTable<string, List<int>>("queryIntIDsCache", 2);

        private readonly KVStoreEntityTable<string, Tuple<List<Int32>, Int32>> queryPageIntIdsCache = KVStoreManager.GetKVStoreEntityTable<string, Tuple<List<Int32>, Int32>>("queryPageIntIDsCache", 2);
        private readonly KVStoreEntityTable<string, Tuple<List<Guid>, Int32>> queryPageGuidIdsCache = KVStoreManager.GetKVStoreEntityTable<string, Tuple<List<Guid>, Int32>>("queryPageGuidIDsCache", 2);

        public const string CONST_DEFAULT_ENCRYPT_KEY = "exfresh.qms";
        #endregion

        #region Members

        private readonly RepositoryBase rb;

        #endregion

        #region Ctor.

        public QueryHelper(RepositoryBase repositoryBase)
        {
            rb = repositoryBase;
        }

        #endregion

        #region 通用条件查询

        /// <summary>
        ///     通用条件查询(INT)
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="cacheabel"></param>
        /// <returns>IDs</returns>
        public List<int> GetIntIDsByConditions<T>(T queryEntity,bool cacheabel) where T : IQueryEntity
        {
            if (rb.OrgDbPrefix != String.Empty)
                queryEntity.TableName = rb.OrgDbPrefix + queryEntity.TableName;

            List<int> ids;
            string key = MD5Encrypt(queryEntity.QueryString);
            if (!queryIntIDsCache.TryGetValue(key, out ids))
            {
                ids = CommonDataProvider.Instance.GetIDsByConditions<int>(queryEntity.QueryString, queryEntity.QueryPars,
                    rb.CreateSqlConnection());
                if (cacheabel)
                queryIntIDsCache.AddKeyValue(key, ids);
                return ids;
            }
            return ids;
        }

        /// <summary>
        ///     通用分页查询方法INT(根据页码和页大小查询)
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="isCache"></param>
        /// <returns>IDs</returns>
        public List<int> GetIntIDsByConditions<T>(T queryEntity, int pageIndex, int pageSize,
            out int totalCount,
            out int totalPageCount,Boolean isCache) where T : IQueryEntity
        {
            queryEntity.IsPage = true;
            queryEntity.PageIndex = pageIndex;
            queryEntity.PageSize = pageSize;
            if (rb.OrgDbPrefix != String.Empty)
                queryEntity.TableName = rb.OrgDbPrefix + queryEntity.TableName;

            Tuple<List<Int32>, Int32> result;
            string qs = queryEntity.QueryString;
            string strEncrypt = qs;
            queryEntity.QueryPars.ForEach(f => strEncrypt += f.QueryName + f.EQueryType + f.QueryValue);
            string key = MD5Encrypt(strEncrypt);
            if (!queryPageIntIdsCache.TryGetValue(key, out result))
            {
                var ids = CommonDataProvider.Instance.GetIDsByConditions<int>(qs, queryEntity.QueryPars,
                    rb.CreateSqlConnection(),out totalCount);
                result = new Tuple<List<int>, int>(ids, totalCount);
                if (isCache)
                    queryPageIntIdsCache.AddKeyValue(key, result);
            }
            totalPageCount = getTotalPageCount(pageIndex, pageSize, result.Item2);
            totalCount = result.Item2;
            return result.Item1;
        }

        /// <summary>
        ///     通用条件查询(GUID)
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="cacheabel"> </param>
        /// <returns>IDs</returns>
        public List<Guid> GetGuidIDsByConditions<T>(T queryEntity, bool cacheabel)
            where T : IQueryEntity
        {
            if (rb.OrgDbPrefix != String.Empty)
                queryEntity.TableName = rb.OrgDbPrefix + queryEntity.TableName;
            List<Guid> ids;
            string qs = queryEntity.QueryString;
            string strEncrypt = qs;
            queryEntity.QueryPars.ForEach(f => strEncrypt += f.QueryName + f.EQueryType + f.QueryValue);
            string key = MD5Encrypt(strEncrypt);
            if (!queryGuidIDsCache.TryGetValue(key, out ids))
            {
                ids = CommonDataProvider.Instance.GetIDsByConditions<Guid>(qs, queryEntity.QueryPars,
                    rb.CreateSqlConnection());
                if (cacheabel)
                    queryGuidIDsCache.AddKeyValue(key, ids);
                return ids;
            }
            return ids;
        }

        /// <summary>
        ///     通用分页查询方法GUID(根据页码和页大小查询)
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="isCache"> </param>
        /// <returns>IDs</returns>
        public List<Guid> GetGuidIDsByConditions<T>(T queryEntity, int pageIndex, int pageSize,
            out int totalCount,
            out int totalPageCount, Boolean isCache) where T : IQueryEntity
        {
            queryEntity.IsPage = true;
            queryEntity.PageIndex = pageIndex;
            queryEntity.PageSize = pageSize;
            if (rb.OrgDbPrefix != String.Empty)
                queryEntity.TableName = rb.OrgDbPrefix + queryEntity.TableName;

            Tuple<List<Guid>, Int32> result;
            string qs = queryEntity.QueryString;
            string strEncrypt = qs;
            queryEntity.QueryPars.ForEach(f => strEncrypt += f.QueryName + f.EQueryType + f.QueryValue);
            string key = MD5Encrypt(strEncrypt);
            if (!queryPageGuidIdsCache.TryGetValue(key, out result))
            {
                var ids = CommonDataProvider.Instance.GetIDsByConditions<Guid>(qs, queryEntity.QueryPars,
                    rb.CreateSqlConnection(), out totalCount);
                result = new Tuple<List<Guid>, int>(ids, totalCount);
                if (isCache)
                    queryPageGuidIdsCache.AddKeyValue(key, result);
            }
            totalPageCount = getTotalPageCount(pageIndex, pageSize, result.Item2);
            totalCount = result.Item2;
            return result.Item1;
        }

        #endregion

        #region Common

        /// <summary>
        ///     给一个字符串进行MD5加密
        /// </summary>
        /// <param name="strText">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string strText)
        {
            return EncryptHelper.EncryptString(strText, CONST_DEFAULT_ENCRYPT_KEY);
        }

        public static Int32 getTotalPageCount(Int32 pageIndex, Int32 pageSize, Int32 totalCount)
        {
            if (totalCount == 0)
            {
                return 0;
            }

            pageSize = pageSize == 0 ? 1 : pageSize;
            
            //总页数
            int pageCount = totalCount / pageSize;

            if ((totalCount % pageSize) > 0)
            {
                pageCount++;
            }
            return pageCount;
        }


        #endregion
    }
}