using InstagramPhotos.DomainModel;
using InstagramPhotos.Media.DomainModel;
using InstagramPhotos.Utility.Data;
using InstagramPhotos.Utility.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Repository
{
    /// <summary>
    /// 媒体数据存取操作
    /// </summary>
    public class MediaRepository : RepositoryBase
    {
        #region Ctor. && Init

        public MediaRepository()
        {
            GetDBKey(ExtendDBType.InstagramPhotos);
        }

        #endregion

        #region Auto Repository

        #region auto Media

        /// <summary>
        ///     新增Media信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public void AddMedia(MediaDO entity, DbTransaction tran = null)
        {
            var data = new[]
            {
                MediaDO.ColumnEnum.MediaID.ToString(),
                MediaDO.ColumnEnum.MediaCode.ToString(),
                MediaDO.ColumnEnum.MediaName.ToString(),
                MediaDO.ColumnEnum.TagId.ToString(),
                MediaDO.ColumnEnum.FromInsUser.ToString(),
                MediaDO.ColumnEnum.Url.ToString(),
                MediaDO.ColumnEnum.RelativeAddress.ToString(),
                MediaDO.ColumnEnum.PhycialAddress.ToString(),
                MediaDO.ColumnEnum.Size.ToString(),
                MediaDO.ColumnEnum.Download_Start.ToString(),
                MediaDO.ColumnEnum.Download_End.ToString(),
                MediaDO.ColumnEnum.Download_Ok.ToString(),
                MediaDO.ColumnEnum.SortValue.ToString(),
                MediaDO.ColumnEnum.Disabled.ToString(),
                MediaDO.ColumnEnum.Rec_CreateBy.ToString(),
                MediaDO.ColumnEnum.Rec_CreateTime.ToString(),
                MediaDO.ColumnEnum.Rec_ModifyBy.ToString(),
                MediaDO.ColumnEnum.Rec_ModifyTime.ToString()
            };
            if (tran == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.InsertObject(conn, entity, GetTableName(MediaDO.TableName), db, data);
                }
            }
            else
                DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName(MediaDO.TableName), db, tran, data);
        }

        /// <summary>
        ///     批量新增Media信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void AddMediaList(List<MediaDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"INSERT INTO {0} SELECT
				T.ts.value('@MediaID', 'uniqueidentifier') as MediaID,
				T.ts.value('@MediaCode', 'nvarchar(100)') as MediaCode,
				T.ts.value('@MediaName', 'nvarchar(200)') as MediaName,
				CASE WHEN T.ts.value('@TagId', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@TagId', 'uniqueidentifier') END as TagId,
				CASE WHEN T.ts.value('@FromInsUser', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@FromInsUser', 'uniqueidentifier') END as FromInsUser,
				T.ts.value('@Url', 'varchar(1000)') as Url,
				T.ts.value('@RelativeAddress', 'varchar(1000)') as RelativeAddress,
				T.ts.value('@PhycialAddress', 'varchar(1000)') as PhycialAddress,
				CASE WHEN T.ts.value('@Size', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Size', 'bigint') END as Size,
				CASE WHEN T.ts.value('@Download_Start', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_Start', 'datetime') END as Download_Start,
				CASE WHEN T.ts.value('@Download_End', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_End', 'datetime') END as Download_End,
				CASE WHEN T.ts.value('@Download_Ok', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_Ok', 'int') END as Download_Ok,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts)", GetTableName(MediaDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }

        /// <summary>
        ///    更新Media信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public Boolean UpdateMedia(MediaDO entity, DbTransaction tran = null)
        {
            try
            {
                if (tran == null)
                    using (DbConnection conn = GetConn())
                    {
                        DBTools.UpdateObject(conn, entity, GetTableName(MediaDO.TableName), new[] { MediaDO.IdName }, db);
                    }
                else
                    DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName(MediaDO.TableName), new[] { MediaDO.IdName }, db, tran);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量更新Media信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void UpdateMediaList(List<MediaDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"DECLARE @TBL TABLE(
				[MediaID] uniqueidentifier NOT NULL ,
				[MediaCode] nvarchar(100) NULL ,
				[MediaName] nvarchar(200) NULL ,
				[TagId] uniqueidentifier NULL ,
				[FromInsUser] uniqueidentifier NULL ,
				[Url] varchar(1000) NULL ,
				[RelativeAddress] varchar(1000) NULL ,
				[PhycialAddress] varchar(1000) NULL ,
				[Size] bigint NULL ,
				[Download_Start] datetime NULL ,
				[Download_End] datetime NULL ,
				[Download_Ok] int NULL ,
				[SortValue] nvarchar(36) NULL ,
				[Disabled] int NULL ,
				[Rec_CreateBy] uniqueidentifier NULL ,
				[Rec_CreateTime] datetime NULL ,
				[Rec_ModifyBy] uniqueidentifier NULL ,
				[Rec_ModifyTime] datetime NULL )


				INSERT INTO @TBL SELECT
				T.ts.value('@MediaID', 'uniqueidentifier') as MediaID,
				T.ts.value('@MediaCode', 'nvarchar(100)') as MediaCode,
				T.ts.value('@MediaName', 'nvarchar(200)') as MediaName,
				CASE WHEN T.ts.value('@TagId', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@TagId', 'uniqueidentifier') END as TagId,
				CASE WHEN T.ts.value('@FromInsUser', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@FromInsUser', 'uniqueidentifier') END as FromInsUser,
				T.ts.value('@Url', 'varchar(1000)') as Url,
				T.ts.value('@RelativeAddress', 'varchar(1000)') as RelativeAddress,
				T.ts.value('@PhycialAddress', 'varchar(1000)') as PhycialAddress,
				CASE WHEN T.ts.value('@Size', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Size', 'bigint') END as Size,
				CASE WHEN T.ts.value('@Download_Start', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_Start', 'datetime') END as Download_Start,
				CASE WHEN T.ts.value('@Download_End', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_End', 'datetime') END as Download_End,
				CASE WHEN T.ts.value('@Download_Ok', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Download_Ok', 'int') END as Download_Ok,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts);


				UPDATE {0} SET  
				[MediaCode] = B.MediaCode,
				[MediaName] = B.MediaName,
				[TagId] = B.TagId,
				[FromInsUser] = B.FromInsUser,
				[Url] = B.Url,
				[RelativeAddress] = B.RelativeAddress,
				[PhycialAddress] = B.PhycialAddress,
				[Size] = B.Size,
				[Download_Start] = B.Download_Start,
				[Download_End] = B.Download_End,
				[Download_Ok] = B.Download_Ok,
				[SortValue] = B.SortValue,
				[Disabled] = B.Disabled,
				[Rec_CreateBy] = B.Rec_CreateBy,
				[Rec_CreateTime] = B.Rec_CreateTime,
				[Rec_ModifyBy] = B.Rec_ModifyBy,
				[Rec_ModifyTime] = B.Rec_ModifyTime
				FROM {0} A,@TBL B WHERE A.MediaID=B.MediaID ", GetTableName(MediaDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }
        /// <summary>
        ///    获取Media信息
        /// </summary>
        /// <param name="MediaID">主键</param>
        public MediaDO GetMedia(Guid MediaID)
        {
            var cmd = new CmdParams { { "@MediaID", MediaID } };
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE MediaID=@MediaID", GetTableName(MediaDO.TableName));
            using (DbConnection conn = GetConn())
            {
                return DBTools.ExecuteReader<MediaDO>(conn, sql, cmd);
            }
        }

        /// <summary>
        ///    根据MediaIDs数组获取Media信息列表
        /// </summary>
        /// <param name="MediaIDs">主键集合</param>
        /// <returns>Media信息列表</returns>
        public Dictionary<Guid, MediaDO> GetMedias(IEnumerable<Guid> MediaIDs)
        {
            Guid[] mediaids = MediaIDs as Guid[] ?? MediaIDs.ToArray();
            if (!mediaids.Any()) return null;
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE MediaID in ('{1}')",
                GetTableName(MediaDO.TableName), mediaids.Distinct().ToCSV("','"));
            using (DbConnection conn = GetConn())
            {
                IEnumerable<MediaDO> result = DBTools.ReadCollection<MediaDO>(conn, sql, null);
                return result.ToDictionary(i => i.MediaID);
            }
        }

        #endregion

        #region auto MediaTask

        /// <summary>
        ///     新增MediaTask信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public void AddMediatask(MediaTaskDO entity, DbTransaction tran = null)
        {
            var data = new[]
            {
                MediaTaskDO.ColumnEnum.MediaTaskId.ToString(),
                MediaTaskDO.ColumnEnum.Url.ToString(),
                MediaTaskDO.ColumnEnum.FileFullName.ToString(),
                MediaTaskDO.ColumnEnum.MetaTypeList.ToString(),
                MediaTaskDO.ColumnEnum.RegexList.ToString(),
                MediaTaskDO.ColumnEnum.Disabled.ToString(),
                MediaTaskDO.ColumnEnum.Rec_CreateBy.ToString(),
                MediaTaskDO.ColumnEnum.Rec_CreateTime.ToString(),
                MediaTaskDO.ColumnEnum.Rec_ModifyBy.ToString(),
                MediaTaskDO.ColumnEnum.Rec_ModifyTime.ToString()
            };
            if (tran == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.InsertObject(conn, entity, GetTableName(MediaTaskDO.TableName), db, data);
                }
            }
            else
                DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName(MediaTaskDO.TableName), db, tran, data);
        }

        /// <summary>
        ///     批量新增MediaTask信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void AddMediataskList(List<MediaTaskDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"INSERT INTO {0} SELECT
				T.ts.value('@MediaTaskId', 'uniqueidentifier') as MediaTaskId,
				T.ts.value('@Url', 'nvarchar(200)') as Url,
				T.ts.value('@FileFullName', 'nvarchar(200)') as FileFullName,
				T.ts.value('@MetaTypeList', 'nvarchar(200)') as MetaTypeList,
				T.ts.value('@RegexList', 'nvarchar(200)') as RegexList,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts)", GetTableName(MediaTaskDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }

        /// <summary>
        ///    更新MediaTask信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public Boolean UpdateMediatask(MediaTaskDO entity, DbTransaction tran = null)
        {
            try
            {
                if (tran == null)
                    using (DbConnection conn = GetConn())
                    {
                        DBTools.UpdateObject(conn, entity, GetTableName(MediaTaskDO.TableName), new[] { MediaTaskDO.IdName }, db);
                    }
                else
                    DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName(MediaTaskDO.TableName), new[] { MediaTaskDO.IdName }, db, tran);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量更新MediaTask信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void UpdateMediataskList(List<MediaTaskDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"DECLARE @TBL TABLE(
				[MediaTaskId] uniqueidentifier NOT NULL ,
				[Url] nvarchar(200) NULL ,
				[FileFullName] nvarchar(200) NULL ,
				[MetaTypeList] nvarchar(200) NULL ,
				[RegexList] nvarchar(200) NULL ,
				[Disabled] int NULL ,
				[Rec_CreateBy] uniqueidentifier NULL ,
				[Rec_CreateTime] datetime NULL ,
				[Rec_ModifyBy] uniqueidentifier NULL ,
				[Rec_ModifyTime] datetime NULL )


				INSERT INTO @TBL SELECT
				T.ts.value('@MediaTaskId', 'uniqueidentifier') as MediaTaskId,
				T.ts.value('@Url', 'nvarchar(200)') as Url,
				T.ts.value('@FileFullName', 'nvarchar(200)') as FileFullName,
				T.ts.value('@MetaTypeList', 'nvarchar(200)') as MetaTypeList,
				T.ts.value('@RegexList', 'nvarchar(200)') as RegexList,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts);


				UPDATE {0} SET  
				[Url] = B.Url,
				[FileFullName] = B.FileFullName,
				[MetaTypeList] = B.MetaTypeList,
				[RegexList] = B.RegexList,
				[Disabled] = B.Disabled,
				[Rec_CreateBy] = B.Rec_CreateBy,
				[Rec_CreateTime] = B.Rec_CreateTime,
				[Rec_ModifyBy] = B.Rec_ModifyBy,
				[Rec_ModifyTime] = B.Rec_ModifyTime
				FROM {0} A,@TBL B WHERE A.MediaTaskId=B.MediaTaskId ", GetTableName(MediaTaskDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }
        /// <summary>
        ///    获取MediaTask信息
        /// </summary>
        /// <param name="MediaTaskId">主键</param>
        public MediaTaskDO GetMediatask(Guid MediaTaskId)
        {
            var cmd = new CmdParams { { "@MediaTaskId", MediaTaskId } };
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE MediaTaskId=@MediaTaskId", GetTableName(MediaTaskDO.TableName));
            using (DbConnection conn = GetConn())
            {
                return DBTools.ExecuteReader<MediaTaskDO>(conn, sql, cmd);
            }
        }

        /// <summary>
        ///    根据MediaTaskIds数组获取MediaTask信息列表
        /// </summary>
        /// <param name="MediaTaskIds">主键集合</param>
        /// <returns>MediaTask信息列表</returns>
        public Dictionary<Guid, MediaTaskDO> GetMediatasks(IEnumerable<Guid> MediaTaskIds)
        {
            Guid[] mediataskids = MediaTaskIds as Guid[] ?? MediaTaskIds.ToArray();
            if (!mediataskids.Any()) return null;
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE MediaTaskId in ('{1}')",
                GetTableName(MediaTaskDO.TableName), mediataskids.Distinct().ToCSV("','"));
            using (DbConnection conn = GetConn())
            {
                IEnumerable<MediaTaskDO> result = DBTools.ReadCollection<MediaTaskDO>(conn, sql, null);
                return result.ToDictionary(i => i.MediaTaskId);
            }
        }

        #endregion

        #region auto Download

        /// <summary>
        ///     新增Download信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public void AddDownload(DownloadDO entity, DbTransaction tran = null)
        {
            var data = new[]
            {
                DownloadDO.ColumnEnum.DownloadId.ToString(),
                DownloadDO.ColumnEnum.HttpUrl.ToString(),
                DownloadDO.ColumnEnum.DirName.ToString(),
                DownloadDO.ColumnEnum.SortValue.ToString(),
                DownloadDO.ColumnEnum.Disabled.ToString(),
                DownloadDO.ColumnEnum.Rec_CreateBy.ToString(),
                DownloadDO.ColumnEnum.Rec_CreateTime.ToString(),
                DownloadDO.ColumnEnum.Rec_ModifyBy.ToString(),
                DownloadDO.ColumnEnum.Rec_ModifyTime.ToString()
            };
            if (tran == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.InsertObject(conn, entity, GetTableName(DownloadDO.TableName), db, data);
                }
            }
            else
                DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName(DownloadDO.TableName), db, tran, data);
        }

        /// <summary>
        ///     批量新增Download信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void AddDownloadList(List<DownloadDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"INSERT INTO {0} SELECT
				T.ts.value('@DownloadId', 'uniqueidentifier') as DownloadId,
				T.ts.value('@HttpUrl', 'nvarchar(400)') as HttpUrl,
				T.ts.value('@DirName', 'nvarchar(400)') as DirName,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts)", GetTableName(DownloadDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }

        /// <summary>
        ///    更新Download信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public Boolean UpdateDownload(DownloadDO entity, DbTransaction tran = null)
        {
            try
            {
                if (tran == null)
                    using (DbConnection conn = GetConn())
                    {
                        DBTools.UpdateObject(conn, entity, GetTableName(DownloadDO.TableName), new[] { DownloadDO.IdName }, db);
                    }
                else
                    DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName(DownloadDO.TableName), new[] { DownloadDO.IdName }, db, tran);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量更新Download信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void UpdateDownloadList(List<DownloadDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"DECLARE @TBL TABLE(
				[DownloadId] uniqueidentifier NOT NULL ,
				[HttpUrl] nvarchar(400) NULL ,
				[DirName] nvarchar(400) NULL ,
				[SortValue] nvarchar(36) NULL ,
				[Disabled] int NULL ,
				[Rec_CreateBy] uniqueidentifier NULL ,
				[Rec_CreateTime] datetime NULL ,
				[Rec_ModifyBy] uniqueidentifier NULL ,
				[Rec_ModifyTime] datetime NULL )


				INSERT INTO @TBL SELECT
				T.ts.value('@DownloadId', 'uniqueidentifier') as DownloadId,
				T.ts.value('@HttpUrl', 'nvarchar(400)') as HttpUrl,
				T.ts.value('@DirName', 'nvarchar(400)') as DirName,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts);


				UPDATE {0} SET  
				[HttpUrl] = B.HttpUrl,
				[DirName] = B.DirName,
				[SortValue] = B.SortValue,
				[Disabled] = B.Disabled,
				[Rec_CreateBy] = B.Rec_CreateBy,
				[Rec_CreateTime] = B.Rec_CreateTime,
				[Rec_ModifyBy] = B.Rec_ModifyBy,
				[Rec_ModifyTime] = B.Rec_ModifyTime
				FROM {0} A,@TBL B WHERE A.DownloadId=B.DownloadId ", GetTableName(DownloadDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }
        /// <summary>
        ///    获取Download信息
        /// </summary>
        /// <param name="DownloadId">主键</param>
        public DownloadDO GetDownload(Guid DownloadId)
        {
            var cmd = new CmdParams { { "@DownloadId", DownloadId } };
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE DownloadId=@DownloadId", GetTableName(DownloadDO.TableName));
            using (DbConnection conn = GetConn())
            {
                return DBTools.ExecuteReader<DownloadDO>(conn, sql, cmd);
            }
        }

        /// <summary>
        ///    根据DownloadIds数组获取Download信息列表
        /// </summary>
        /// <param name="DownloadIds">主键集合</param>
        /// <returns>Download信息列表</returns>
        public Dictionary<Guid, DownloadDO> GetDownloads(IEnumerable<Guid> DownloadIds)
        {
            Guid[] downloadids = DownloadIds as Guid[] ?? DownloadIds.ToArray();
            if (!downloadids.Any()) return null;
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE DownloadId in ('{1}')",
                GetTableName(DownloadDO.TableName), downloadids.Distinct().ToCSV("','"));
            using (DbConnection conn = GetConn())
            {
                IEnumerable<DownloadDO> result = DBTools.ReadCollection<DownloadDO>(conn, sql, null);
                return result.ToDictionary(i => i.DownloadId);
            }
        }

        #endregion

        #region auto DownloadLog

        /// <summary>
        ///     新增DownloadLog信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public void AddDownloadlog(DownloadLogDO entity, DbTransaction tran = null)
        {
            var data = new[]
            {
                DownloadLogDO.ColumnEnum.LogId.ToString(),
                DownloadLogDO.ColumnEnum.Message.ToString(),
                DownloadLogDO.ColumnEnum.Level.ToString(),
                DownloadLogDO.ColumnEnum.SortValue.ToString(),
                DownloadLogDO.ColumnEnum.Disabled.ToString(),
                DownloadLogDO.ColumnEnum.Rec_CreateBy.ToString(),
                DownloadLogDO.ColumnEnum.Rec_CreateTime.ToString(),
                DownloadLogDO.ColumnEnum.Rec_ModifyBy.ToString(),
                DownloadLogDO.ColumnEnum.Rec_ModifyTime.ToString()
            };
            if (tran == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.InsertObject(conn, entity, GetTableName(DownloadLogDO.TableName), db, data);
                }
            }
            else
                DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName(DownloadLogDO.TableName), db, tran, data);
        }

        /// <summary>
        ///     批量新增DownloadLog信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void AddDownloadlogList(List<DownloadLogDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"INSERT INTO {0} SELECT
				T.ts.value('@LogId', 'uniqueidentifier') as LogId,
				CASE WHEN T.ts.value('@Message', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Message', 'uniqueidentifier') END as Message,
				CASE WHEN T.ts.value('@Level', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Level', 'int') END as Level,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts)", GetTableName(DownloadLogDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }

        /// <summary>
        ///    更新DownloadLog信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public Boolean UpdateDownloadlog(DownloadLogDO entity, DbTransaction tran = null)
        {
            try
            {
                if (tran == null)
                    using (DbConnection conn = GetConn())
                    {
                        DBTools.UpdateObject(conn, entity, GetTableName(DownloadLogDO.TableName), new[] { DownloadLogDO.IdName }, db);
                    }
                else
                    DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName(DownloadLogDO.TableName), new[] { DownloadLogDO.IdName }, db, tran);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量更新DownloadLog信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void UpdateDownloadlogList(List<DownloadLogDO> entities, DbTransaction trans = null)
        {
            var cmd = new ComplexParams
            {
                new ComplexParameter
                {
                    Key = "@model_list",
                    DbType = DbType.Xml,
                    Value = ConvertUtils.ConvertModelListToXML("e", entities)
                }
            };
            var sql = string.Format(@"DECLARE @TBL TABLE(
				[LogId] uniqueidentifier NOT NULL ,
				[Message] uniqueidentifier NULL ,
				[Level] int NULL ,
				[SortValue] nvarchar(36) NULL ,
				[Disabled] int NULL ,
				[Rec_CreateBy] uniqueidentifier NULL ,
				[Rec_CreateTime] datetime NULL ,
				[Rec_ModifyBy] uniqueidentifier NULL ,
				[Rec_ModifyTime] datetime NULL )


				INSERT INTO @TBL SELECT
				T.ts.value('@LogId', 'uniqueidentifier') as LogId,
				CASE WHEN T.ts.value('@Message', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Message', 'uniqueidentifier') END as Message,
				CASE WHEN T.ts.value('@Level', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Level', 'int') END as Level,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts);


				UPDATE {0} SET  
				[Message] = B.Message,
				[Level] = B.Level,
				[SortValue] = B.SortValue,
				[Disabled] = B.Disabled,
				[Rec_CreateBy] = B.Rec_CreateBy,
				[Rec_CreateTime] = B.Rec_CreateTime,
				[Rec_ModifyBy] = B.Rec_ModifyBy,
				[Rec_ModifyTime] = B.Rec_ModifyTime
				FROM {0} A,@TBL B WHERE A.LogId=B.LogId ", GetTableName(DownloadLogDO.TableName));

            if (trans == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.ExecuteNonQuery(conn, sql, cmd);
                }
            }
            else
                DBTools.ExecuteNonQuery(trans.Connection, trans, sql, cmd);
        }
        /// <summary>
        ///    获取DownloadLog信息
        /// </summary>
        /// <param name="LogId">主键</param>
        public DownloadLogDO GetDownloadlog(Guid LogId)
        {
            var cmd = new CmdParams { { "@LogId", LogId } };
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE LogId=@LogId", GetTableName(DownloadLogDO.TableName));
            using (DbConnection conn = GetConn())
            {
                return DBTools.ExecuteReader<DownloadLogDO>(conn, sql, cmd);
            }
        }

        /// <summary>
        ///    根据LogIds数组获取DownloadLog信息列表
        /// </summary>
        /// <param name="LogIds">主键集合</param>
        /// <returns>DownloadLog信息列表</returns>
        public Dictionary<Guid, DownloadLogDO> GetDownloadlogs(IEnumerable<Guid> LogIds)
        {
            Guid[] logids = LogIds as Guid[] ?? LogIds.ToArray();
            if (!logids.Any()) return null;
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE LogId in ('{1}')",
                GetTableName(DownloadLogDO.TableName), logids.Distinct().ToCSV("','"));
            using (DbConnection conn = GetConn())
            {
                IEnumerable<DownloadLogDO> result = DBTools.ReadCollection<DownloadLogDO>(conn, sql, null);
                return result.ToDictionary(i => i.LogId);
            }
        }

        #endregion


        #endregion

        #region Members
        private static readonly DbSettings db = DbSettings.MSSQL;
        #endregion
    }
}
