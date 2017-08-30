using System;
using System.Data.Common;
using System.Linq;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration.MappingOperations;
using InstagramPhotos.Utility.Data.MappingConfigs;

namespace InstagramPhotos.Utility.Data
{
	 static partial class CommandBuilder
	{
		public static DbCommand BuildInsertCommand(
			this DbCommand cmd,
			object obj,
            string tableName,
            DbSettings dbSettings, Boolean isOutGuid, String outKey
		)
		{
            return BuildInsertCommand(cmd, obj, tableName, dbSettings, null, null, isOutGuid, outKey);
		}

		public static DbCommand BuildInsertCommand(
			this DbCommand cmd,
			object obj,
			string tableName,
			DbSettings dbSettings,
			string[] includeFields,
            string[] excludeFields, Boolean isOutGuid, String outKey
		)
		{
			IMappingConfigurator config = new AddDbCommandsMappingConfig(
					dbSettings,
					includeFields,
					excludeFields,
					"insertop_inc_" + includeFields.ToCSV("_") + "_exc_" + excludeFields.ToCSV("_")
			);

			var mapper = ObjectMapperManager.DefaultInstance.GetMapperImpl(
				obj.GetType(), 
				typeof(DbCommand), 
				config
			);

			string[] fields = mapper.StroredObjects.OfType<SrcReadOperation>().Select(m => m.Source.MemberInfo.Name).ToArray();
            var sb = new StringBuilder();
		    if (isOutGuid)
		    {
		        sb.Append("DECLARE @RESULT TABLE (pkey UNIQUEIDENTIFIER)");
		        sb.Append("INSERT INTO " + tableName + "(" + fields.Select(dbSettings.GetEscapedName).ToCSV(",") + ") ");
		        sb.AppendFormat("OUTPUT INSERTED.{0} INTO @RESULT ", outKey);
		        sb.Append("VALUES (" + fields.Select(dbSettings.GetParamName).ToCSV(",") + @")");
                sb.Append("SELECT pkey FROM @RESULT");
		    }
		    else
		    {
		        sb.Append("INSERT INTO " + tableName + "(" + fields.Select(dbSettings.GetEscapedName).ToCSV(",") + ") ");
		        sb.Append("VALUES (" + fields.Select(dbSettings.GetParamName).ToCSV(",") + @") SELECT @@IDENTITY");
		    }
            cmd.CommandText = sb.ToString();
			cmd.CommandType = System.Data.CommandType.Text;

			mapper.Map(obj, cmd, null);
			return cmd;
		}
	}
}
