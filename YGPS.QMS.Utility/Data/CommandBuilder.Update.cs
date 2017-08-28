using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration.MappingOperations;
using InstagramPhotos.Utility.Data.MappingConfigs;

namespace InstagramPhotos.Utility.Data
{
    static partial class CommandBuilder
    {
        public static bool BuildUpdateOperator(
            this DbCommand cmd,
            object obj,
            string tableName,
            string[] idFieldNames,
            DbSettings dbSettings
            )
        {
            return BuildUpdateCommand(cmd, obj, tableName, idFieldNames, null, null, null, dbSettings);
        }

        public static bool BuildUpdateCommand(
            this DbCommand cmd,
            object obj,
            string tableName,
            IEnumerable<string> idFieldNames,
            IEnumerable<string> includeFields,
            IEnumerable<string> excludeFields,
            ObjectsChangeTracker changeTracker,
            DbSettings dbSettings
            )
        {
            if (idFieldNames == null)
            {
                idFieldNames = new string[0];
            }

            //idFieldNames = idFieldNames.Select(n => n.ToUpper()).ToArray(); //不Upper会怎样

            if (changeTracker != null)
            {
                ObjectsChangeTracker.TrackingMember[] changedFields = changeTracker.GetChanges(obj);
                if (changedFields != null)
                {
                    includeFields = includeFields == null ? changedFields.Select(c => c.name).Except(idFieldNames).ToArray() 
                        : includeFields.Intersect(changedFields.Select(c => c.name)).Except(idFieldNames).ToArray();
                }
            }
            if (includeFields != null)
            {
                includeFields = includeFields.Concat(idFieldNames);
            }
            IMappingConfigurator config = new AddDbCommandsMappingConfig(
                dbSettings,
                includeFields,
                excludeFields,
                "updateop_inc_" + includeFields.ToCSV("_") + "_exc_" + excludeFields.ToCSV("_")
                );

            ObjectsMapperBaseImpl mapper = ObjectMapperManager.DefaultInstance.GetMapperImpl(
                obj.GetType(),
                typeof (DbCommand),
                config
                );

            string[] fields = mapper
                .StroredObjects
                .OfType<SrcReadOperation>()
                .Select(m => m.Source.MemberInfo.Name)
                .Where(f => !idFieldNames.Contains(f))
                .Except(idFieldNames)
                .ToArray(); //2014-4-29 cannot change id.

            if (fields.Length == 0)
            {
                return false;
            }

            var enableCol = new string[] { "TableName", "IdName" };
            fields = fields.Where(c => !enableCol.Contains(c)).ToArray();

            string cmdStr =
                "UPDATE " +
                tableName +
                " SET " +
                fields.Select(
                        f => dbSettings.GetEscapedName(f) + "=" + dbSettings.GetParamName(f)
                    )
                    .ToCSV(",") +
                " WHERE " +
                idFieldNames.Select(fn => dbSettings.GetEscapedName(fn) + "=" + dbSettings.GetParamName(fn))
                    .ToCSV(" AND ")
                ;
            cmd.CommandText = cmdStr;
            cmd.CommandType = CommandType.Text;

            mapper.Map(obj, cmd, null);
            return true;
        }
    }
}