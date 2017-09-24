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
    public class InsUserRepository: RepositoryBase
    {
        #region Members
        private static readonly DbSettings db = DbSettings.MSSQL;
        #endregion

        #region Ctor. && Init

        public InsUserRepository()
        {
            GetDBKey(ExtendDBType.InstagramPhotos);
        }

        #endregion

        #region Auto Repository

        #region auto InsUserType

        /// <summary>
        ///     新增InsUserType信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public void AddInsusertype(InsUserTypeDO entity, DbTransaction tran = null)
        {
            var data = new[]
            {
                InsUserTypeDO.ColumnEnum.InsUserTypeId.ToString(),
                InsUserTypeDO.ColumnEnum.TypeName.ToString(),
                InsUserTypeDO.ColumnEnum.TagGroupId.ToString(),
                InsUserTypeDO.ColumnEnum.TagGroupName.ToString(),
                InsUserTypeDO.ColumnEnum.SortValue.ToString(),
                InsUserTypeDO.ColumnEnum.Disabled.ToString(),
                InsUserTypeDO.ColumnEnum.Rec_CreateBy.ToString(),
                InsUserTypeDO.ColumnEnum.Rec_CreateTime.ToString(),
                InsUserTypeDO.ColumnEnum.Rec_ModifyBy.ToString(),
                InsUserTypeDO.ColumnEnum.Rec_ModifyTime.ToString()
            };
            if (tran == null)
            {
                using (DbConnection conn = GetConn())
                {
                    DBTools.InsertObject(conn, entity, GetTableName(InsUserTypeDO.TableName), db, data);
                }
            }
            else
                DBTools.InsertObjectWithTrans(tran.Connection, entity, GetTableName(InsUserTypeDO.TableName), db, tran, data);
        }

        /// <summary>
        ///     批量新增InsUserType信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void AddInsusertypeList(List<InsUserTypeDO> entities, DbTransaction trans = null)
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
				T.ts.value('@InsUserTypeId', 'uniqueidentifier') as InsUserTypeId,
				T.ts.value('@TypeName', 'nvarchar(100)') as TypeName,
				CASE WHEN T.ts.value('@TagGroupId', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@TagGroupId', 'uniqueidentifier') END as TagGroupId,
				T.ts.value('@TagGroupName', 'nvarchar(2000)') as TagGroupName,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts)", GetTableName(InsUserTypeDO.TableName));

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
        ///    更新InsUserType信息
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="tran">事物对象</param>
        public Boolean UpdateInsusertype(InsUserTypeDO entity, DbTransaction tran = null)
        {
            try
            {
                if (tran == null)
                    using (DbConnection conn = GetConn())
                    {
                        DBTools.UpdateObject(conn, entity, GetTableName(InsUserTypeDO.TableName), new[] { InsUserTypeDO.IdName }, db);
                    }
                else
                    DBTools.UpdateObjectWithTrans(tran.Connection, entity, GetTableName(InsUserTypeDO.TableName), new[] { InsUserTypeDO.IdName }, db, tran);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量更新InsUserType信息
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="trans">事物对象</param>
        public void UpdateInsusertypeList(List<InsUserTypeDO> entities, DbTransaction trans = null)
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
				[InsUserTypeId] uniqueidentifier NOT NULL ,
				[TypeName] nvarchar(100) NULL ,
				[TagGroupId] uniqueidentifier NULL ,
				[TagGroupName] nvarchar(2000) NULL ,
				[SortValue] nvarchar(36) NULL ,
				[Disabled] int NULL ,
				[Rec_CreateBy] uniqueidentifier NULL ,
				[Rec_CreateTime] datetime NULL ,
				[Rec_ModifyBy] uniqueidentifier NULL ,
				[Rec_ModifyTime] datetime NULL )


				INSERT INTO @TBL SELECT
				T.ts.value('@InsUserTypeId', 'uniqueidentifier') as InsUserTypeId,
				T.ts.value('@TypeName', 'nvarchar(100)') as TypeName,
				CASE WHEN T.ts.value('@TagGroupId', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@TagGroupId', 'uniqueidentifier') END as TagGroupId,
				T.ts.value('@TagGroupName', 'nvarchar(2000)') as TagGroupName,
				T.ts.value('@SortValue', 'nvarchar(36)') as SortValue,
				CASE WHEN T.ts.value('@Disabled', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Disabled', 'int') END as Disabled,
				CASE WHEN T.ts.value('@Rec_CreateBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateBy', 'uniqueidentifier') END as Rec_CreateBy,
				CASE WHEN T.ts.value('@Rec_CreateTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_CreateTime', 'datetime') END as Rec_CreateTime,
				CASE WHEN T.ts.value('@Rec_ModifyBy', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyBy', 'uniqueidentifier') END as Rec_ModifyBy,
				CASE WHEN T.ts.value('@Rec_ModifyTime', 'varchar(1)')='' THEN NULL ELSE T.ts.value('@Rec_ModifyTime', 'datetime') END as Rec_ModifyTime
				FROM @model_list.nodes('/es/e') T(ts);


				UPDATE {0} SET  
				[TypeName] = B.TypeName,
				[TagGroupId] = B.TagGroupId,
				[TagGroupName] = B.TagGroupName,
				[SortValue] = B.SortValue,
				[Disabled] = B.Disabled,
				[Rec_CreateBy] = B.Rec_CreateBy,
				[Rec_CreateTime] = B.Rec_CreateTime,
				[Rec_ModifyBy] = B.Rec_ModifyBy,
				[Rec_ModifyTime] = B.Rec_ModifyTime
				FROM {0} A,@TBL B WHERE A.InsUserTypeId=B.InsUserTypeId ", GetTableName(InsUserTypeDO.TableName));

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
        ///    获取InsUserType信息
        /// </summary>
        /// <param name="InsUserTypeId">主键</param>
        public InsUserTypeDO GetInsusertype(Guid InsUserTypeId)
        {
            var cmd = new CmdParams { { "@InsUserTypeId", InsUserTypeId } };
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE InsUserTypeId=@InsUserTypeId", GetTableName(InsUserTypeDO.TableName));
            using (DbConnection conn = GetConn())
            {
                return DBTools.ExecuteReader<InsUserTypeDO>(conn, sql, cmd);
            }
        }

        /// <summary>
        ///    根据InsUserTypeIds数组获取InsUserType信息列表
        /// </summary>
        /// <param name="InsUserTypeIds">主键集合</param>
        /// <returns>InsUserType信息列表</returns>
        public Dictionary<Guid, InsUserTypeDO> GetInsusertypes(IEnumerable<Guid> InsUserTypeIds)
        {
            Guid[] insusertypeids = InsUserTypeIds as Guid[] ?? InsUserTypeIds.ToArray();
            if (!insusertypeids.Any()) return null;
            string sql = String.Format("SELECT * FROM {0}(NOLOCK) WHERE InsUserTypeId in ('{1}')",
                GetTableName(InsUserTypeDO.TableName), insusertypeids.Distinct().ToCSV("','"));
            using (DbConnection conn = GetConn())
            {
                IEnumerable<InsUserTypeDO> result = DBTools.ReadCollection<InsUserTypeDO>(conn, sql, null);
                return result.ToDictionary(i => i.InsUserTypeId);
            }
        }

        #endregion

        #endregion
    }
}
