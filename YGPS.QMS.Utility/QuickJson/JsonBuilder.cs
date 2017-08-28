using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace InstagramPhotos.Utility.QuickJson
{
    internal class JsonBuilder
    {
        private Dictionary<object, object> _LoopObject = new Dictionary<object, object>();//循环引用对象缓存区

        protected QuickStringWriter Buff = new QuickStringWriter(4096);//字符缓冲区

        public const char Quot = '"';

        public const char Colon = ':';

        public const char Comma = ',';

        public string ConvertToJsonString(object obj)
        {
            Buff.Clear();
            AppendObject(obj);
            return Buff.ToString();
        }

        //泛对象
        protected void AppendObject(object obj)
        {
            if (obj == null) Buff.Append("null");
            else if (obj is String) AppendString((String)obj);
            else if (obj is Int32) AppendInt32((Int32)obj);
            else if (obj is Boolean) AppendBoolean((Boolean)obj);
            else if (obj is DateTime) AppendDateTime((DateTime)obj);
            else if (obj is Double) AppendDouble((Double)obj);
            else if (obj is Enum) AppendEnum((Enum)obj);
            else if (obj is Decimal) AppendDecimal((Decimal)obj);
            else if (obj is Char) AppendChar((Char)obj);
            else if (obj is Single) AppendSingle((Single)obj);
            else if (obj is Guid) AppendGuid((Guid)obj);
            else if (obj is Byte) AppendByte((Byte)obj);
            else if (obj is Int16) AppendInt16((Int16)obj);
            else if (obj is Int64) AppendInt64((Int64)obj);
            else if (obj is SByte) AppendSByte((SByte)obj);
            else if (obj is UInt32) AppendUInt32((UInt32)obj);
            else if (obj is UInt64) AppendUInt64((UInt64)obj);
            else if (_LoopObject.ContainsKey(obj) == false)
            {
                _LoopObject.Add(obj, null);
                if (obj is IDictionary) AppendJson((IDictionary)obj);
                else if (obj is IEnumerable) AppendArray((IEnumerable)obj);
                else if (obj is DataSet) AppendDataSet((DataSet)obj);
                else if (obj is DataTable) AppendDataTable((DataTable)obj);
                else if (obj is DataView) AppendDataView((DataView)obj);
                else AppendOther(obj);
                _LoopObject.Remove(obj);
            }
            else
            {
                Buff.Append("undefined");
            }
        }

        protected virtual void AppendOther(object obj)
        {
            Type t = obj.GetType();
            Buff.Append('{');
            string fix = "";
            foreach (var p in t.GetProperties())
            {
                if (p.CanRead)
                {
                    Buff.Append(fix);
                    AppendKey(p.Name, false);
                    object value = p.GetValue(obj, null);
                    AppendObject(value);
                    fix = ",";
                }
            }
            Buff.Append('}');
        }

        /// <summary> 
        /// 追加Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="escape">key中是否有(引号,回车,制表符等)特殊字符,需要转义</param>
        protected virtual void AppendKey(string key, bool escape)
        {
            if (escape)
            {
                AppendString(key);
            }
            else
            {
                Buff.Append(Quot);
                Buff.Append(key);
                Buff.Append(Quot);
            }
            Buff.Append(Colon);
        }

        //基本类型转换Json字符串写入Buff
        protected virtual void AppendByte(Byte value) { AppendNumber(value); }

        protected virtual void AppendDecimal(Decimal value) { AppendNumber(value); }

        protected virtual void AppendInt16(Int16 value) { AppendNumber(value); }

        protected virtual void AppendInt32(Int32 value) { AppendNumber(value); }

        protected virtual void AppendInt64(Int64 value) { AppendNumber(value); }

        protected virtual void AppendSByte(SByte value) { AppendNumber(value); }

        protected virtual void AppendUInt16(UInt16 value) { AppendNumber(value); }

        protected virtual void AppendUInt32(UInt32 value) { AppendNumber(value); }

        protected virtual void AppendUInt64(UInt64 value) { AppendNumber(value); }

        protected virtual void AppendDouble(Double value) { AppendNumber(value); }

        protected virtual void AppendSingle(Single value) { AppendNumber(value); }

        protected virtual void AppendBoolean(Boolean value) { Buff.Append(value ? "true" : "false"); }

        protected virtual void AppendChar(Char value)
        {
            Buff.Append(Quot);
            switch (value)
            {
                case '\\':
                case '\n':
                case '\r':
                case '\t':
                case '"':
                    Buff.Append('\\');
                    break;
            }
            Buff.Append(value);
            Buff.Append(Quot);
        }

        protected virtual void AppendString(String value)
        {
            Buff.Append(Quot);

            for (int j = 0; j < value.Length; j++)
            {
                switch (value[j])
                {
                    case '\\':
                    case '\n':
                    case '\r':
                    case '\t':
                    case '"':
                        Buff.Append('\\');
                        break;
                }
                Buff.Append(value[j]);
            }

            Buff.Append(Quot);
        }

        protected virtual void AppendDateTime(DateTime value)
        {
            Buff.Append(Quot);
            if (value.Year < 1000)
            {
                if (value.Year < 100)
                {
                    if (value.Year < 10)
                    {
                        Buff.Append("000");
                    }
                    else
                    {
                        Buff.Append("00");
                    }
                }
                else
                {
                    Buff.Append("0");
                }
            }
            Buff.Append(value.Year)
                .Append('-');
            if (value.Month < 10)
            {
                Buff.Append('0');
            }
            Buff.Append(value.Month).Append('-');

            if (value.Day < 10)
            {
                Buff.Append('0');
            }
            Buff.Append(value.Day).Append(' ');

            if (value.Hour < 10)
            {
                Buff.Append('0');
            }
            Buff.Append(value.Hour).Append(Colon);

            if (value.Minute < 10)
            {
                Buff.Append('0');
            }
            Buff.Append(value.Minute).Append(Colon);

            if (value.Second < 10)
            {
                Buff.Append('0');
            }
            Buff.Append(value.Second).Append(Quot);
        }

        protected virtual void AppendGuid(Guid value)
        {
            Buff.Append(Quot).Append(value.ToString()).Append(Quot);
        }

        //枚举
        protected virtual void AppendEnum(Enum value)
        {
            Buff.Append(Quot).Append(value.ToString()).Append(Quot);
        }

        protected virtual void AppendNumber(IConvertible number)
        {
            Buff.Append(number.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
        }

        //转换数组对象
        protected virtual void AppendArray(IEnumerable array)
        {
            Buff.Append('[');
            var ee = array.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendObject(ee.Current);
                while (ee.MoveNext())
                {
                    Buff.Append(Comma);
                    AppendObject(ee.Current);
                }
            }
            Buff.Append(']');
        }

        //转换键值对对象
        protected virtual void AppendJson(IDictionary dict)
        {
            AppendJson(dict.Keys, dict.Values);
        }

        //分别有键值枚举的对象
        protected virtual void AppendJson(IEnumerable keys, IEnumerable values)
        {
            Buff.Append('{');
            var ke = keys.GetEnumerator();
            var ve = values.GetEnumerator();
            if (ke.MoveNext() && ve.MoveNext())
            {
                AppendKey(ke.Current + "", true);
                AppendObject(ve.Current);
                while (ke.MoveNext() && ve.MoveNext())
                {
                    Buff.Append(Comma);
                    AppendKey(ke.Current + "", true);
                    AppendObject(ve.Current);
                }
            }
            Buff.Append('}');
        }

        protected virtual void AppendArray(IEnumerable enumer, Converter<object, object> getVal)
        {
            Buff.Append('[');
            var ee = enumer.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendObject(getVal(ee.Current));
                while (ee.MoveNext())
                {
                    Buff.Append(Comma);
                    AppendObject(getVal(ee.Current));
                }
            }
            Buff.Append(']');
        }

        protected virtual void AppendJson(IEnumerable enumer,
            Converter<object, string> getKey, Converter<object, object> getVal, bool escapekey)
        {
            Buff.Append('{');

            var ee = enumer.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendKey(getKey(ee.Current), escapekey);
                AppendObject(getVal(ee.Current));
                while (ee.MoveNext())
                {
                    Buff.Append(Comma);
                    AppendKey(getKey(ee.Current), true);
                    AppendObject(getVal(ee.Current));
                }
            }
            Buff.Append('}');
        }

        protected virtual void AppendDataSet(DataSet dataset)
        {
            Buff.Append('{');
            var ee = dataset.Tables.GetEnumerator();
            if (ee.MoveNext())
            {
                DataTable table = (DataTable)ee.Current;
                AppendKey(table.TableName, true);
                AppendDataTable(table);
                while (ee.MoveNext())
                {
                    Buff.Append(Comma);
                    table = (DataTable)ee.Current;
                    AppendKey(table.TableName, true);
                    AppendDataTable(table);
                }
            }
            Buff.Append('}');
        }

        protected virtual void AppendDataTable(DataTable table)
        {
            Buff.Append("{\"columns\":");
            AppendArray(table.Columns, o => ((DataColumn)o).ColumnName);
            Buff.Append(",\"rows\":");
            AppendArray(table.Rows, o => ((DataRow)o).ItemArray);
            Buff.Append('}');
        }

        protected virtual void AppendDataView(DataView tableView)
        {
            Buff.Append("{\"columns\":");
            AppendArray(tableView.Table.Columns, o => ((DataColumn)o).ColumnName);
            Buff.Append(",\"rows\":");
            AppendArray(tableView, o => ((DataRowView)o).Row.ItemArray);
            Buff.Append('}');
        }
    }
}
