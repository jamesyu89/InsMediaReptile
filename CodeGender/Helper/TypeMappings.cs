using System;
using System.Collections.Generic;

namespace InstagramPhotos.CodeGender.Helper
{
    public static class TypeMappings
    {
        static Dictionary<string, Type> mappings = new Dictionary<string, Type>();

        static TypeMappings()
        {
            mappings["varchar"] = typeof(string);
            mappings["varchar2"] = typeof(string);
            mappings["nvarchar"] = typeof(string);
            mappings["nvarchar2"] = typeof(string);
            mappings["char"] = typeof(string);
            mappings["nchar"] = typeof(string);
            mappings["text"] = typeof(string);
            mappings["longtext"] = typeof(string);
            mappings["ntext"] = typeof(string);
            mappings["string"] = typeof(string);
            mappings["date"] = typeof(DateTime);
            mappings["datetime"] = typeof(DateTime);
            mappings["smalldatetime"] = typeof(DateTime);
            mappings["timestamp"] = typeof(DateTime);
            mappings["smallint"] = typeof(int);
            mappings["int"] = typeof(int);
            mappings["integer"] = typeof(int);
            mappings["int identity"] = typeof(int);
            mappings["number"] = typeof(int);
            mappings["smallint"] = typeof(Int16);
            mappings["tinyint"] = typeof(Int16);
            mappings["bigint"] = typeof(Int64);
            mappings["float"] = typeof(decimal);
            mappings["numeric"] = typeof(decimal);
            mappings["decimal"] = typeof(decimal);
            mappings["money"] = typeof(decimal);
            mappings["smallmoney"] = typeof(decimal);
            mappings["real"] = typeof(decimal);
            mappings["double"] = typeof(decimal);
            mappings["bit"] = typeof(bool);
            mappings["binary"] = typeof(byte[]);
            mappings["varbinary"] = typeof(byte[]);
            mappings["image"] = typeof(byte[]);
            mappings["raw"] = typeof(byte[]);
            mappings["long"] = typeof(byte[]);
            mappings["long raw"] = typeof(byte[]);
            mappings["blob"] = typeof(byte[]);
            mappings["bfile"] = typeof(byte[]);
            mappings["uniqueidentifier"] = typeof(Guid);
        }

        public static Type GetType(string dbType)
        {
            if (!string.IsNullOrEmpty(dbType))
            {
                dbType = dbType.ToLower();
                if (mappings.ContainsKey(dbType))
                    return mappings[dbType];
            }
            return typeof(object);
        }

        public static void GetTypeName(string dbType, out string csType, out string covertFormat)
        {
            switch (dbType.ToLower())
            { 
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "nvarchar2":
                case "char":
                case "nchar":
                case "text":
                case "longtext":
                case "string":
                    csType = "string";
                    covertFormat = "Convert.ToString({0})";
                    break;

                case "date":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    csType = "DateTime";
                    covertFormat = "Convert.ToDateTime({0})";
                    break;

                case "int":
                case "number":
                case "smallint":
                case "tinyint":
                case "integer":
                    csType = "int";
                    covertFormat = "Convert.ToInt32({0})";
                    break;

                case "bigint":
                    csType = "Int64";
                    covertFormat = "Convert.ToInt64({0})";
                    break;

                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "real":
                case "double":
                    csType = "decimal";
                    covertFormat = "Convert.ToDecimal({0})";
                    break;

                case "bit":
                    csType = "bool"; 
                    covertFormat = "Convert.ToBoolean({0})";
                    break;

                case "binary":
                case "varbinary":
                case "image":
                case "raw":
                case "long":
                case "long raw":
                case "blob":
                case "bfile":
                    csType = "byte[]";
                    covertFormat = "(byte[]){0}";
                    break;

                case "uniqueidentifier":
                    csType = "Guid";
                    covertFormat = "(Guid){0}";
                    break;

                case "xml":
                    csType="string";
                    covertFormat = "Convert.ToString({0})";
                    break;
               
                default:
                    csType = "object";
                    covertFormat = "{0}";
                    break;
            }
        }

        public static void GetTypeDefault(string dbType, out string defaultValue)
        {
            switch (dbType.ToLower())
            {
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "nvarchar2":
                case "char":
                case "nchar":
                case "text":
                case "longtext":
                case "string":
                    defaultValue = "''";
                    break;

                case "date":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    defaultValue = "GETDATE()";
                    break;

                case "int":
                case "number":
                case "smallint":
                case "tinyint":
                case "integer":
                    defaultValue = "0";
                    break;

                case "bigint":
                    defaultValue = "0";
                    break;

                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "real":
                case "double":
                    defaultValue = "0";
                    break;

                case "bit":
                    defaultValue = "0";
                    break;

                case "binary":
                case "varbinary":
                case "image":
                case "raw":
                case "long":
                case "long raw":
                case "blob":
                case "bfile":
                    defaultValue = "0";
                    break;

                case "uniqueidentifier":
                    defaultValue = "NEWID()";
                    break;

                case "xml":
                    defaultValue = "''";
                    break;

                default:
                    defaultValue = "''";
                    break;
            }
        }
    }
}
