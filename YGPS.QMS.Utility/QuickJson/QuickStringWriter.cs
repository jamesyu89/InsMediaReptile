using System;

namespace InstagramPhotos.Utility.QuickJson
{
    internal class QuickStringWriter : IDisposable
    {
        public QuickStringWriter() : this(2048) { }

        /// <summary>
        /// 实例化新的对象,并且指定初始容量
        /// </summary>
        /// <param name="capacity"></param>
        public QuickStringWriter(int capacity)
        {
            NumberBuff = new Char[64];
            Buff = new Char[capacity];
        }

        void SetCapacity(int capacity)
        {
            if (capacity > Buff.Length)
            {
                if (capacity > int.MaxValue / 32)
                {
                    throw new OutOfMemoryException();
                }

            }
            var newbuff = new char[capacity];
            Array.Copy(Buff, 0, newbuff, 0, Math.Min(Position, capacity));
            GC.ReRegisterForFinalize(Buff);
            Buff = newbuff;
            Position = Math.Min(Position, Buff.Length);
        }

        /// <summary>
        /// 当容量不足的时候,尝试翻倍空间
        /// </summary>
        void ToDouble()
        {
            SetCapacity(Math.Min(Buff.Length * 2, int.MaxValue / 2048));
        }

        Char[] NumberBuff;
        Char[] Buff;
        int Position;

        public void Dispose()
        {
            NumberBuff = null;
            Buff = null;
        }

        static readonly char[] ArrTrue = new[] { 't', 'r', 'u', 'e' };
        static readonly char[] ArrFalse = new[] { 'f', 'a', 'l', 's', 'e' };

        public QuickStringWriter Append(Boolean val)
        {
            if (val)
            {
                Check(4);
                Array.Copy(ArrTrue, 0, Buff, Position, 4);
                Position += 4;
            }
            else
            {
                Check(5);
                Array.Copy(ArrFalse, 0, Buff, Position, 5);
                Position += 5;
            }
            return this;
        }

        public QuickStringWriter Append(DateTime val)
        {
            Append(val.ToString());
            return this;
        }

        public QuickStringWriter Append(Guid val)
        {
            Append(val.ToString());
            return this;
        }

        public QuickStringWriter Append(DateTime val, string format)
        {

            Append(val.ToString(format));
            return this;
        }

        public QuickStringWriter Append(Guid val, string format)
        {
            Append(val.ToString(format));
            return this;
        }

        public QuickStringWriter Append(Decimal val)
        {
            Append(val.ToString());
            return this;
        }

        public QuickStringWriter Append(Double val)
        {
            Append(Convert.ToString(val));
            return this;
        }

        public QuickStringWriter Append(Single val)
        {
            Append(Convert.ToString(val));
            return this;
        }

        public QuickStringWriter Append(SByte val)
        {
            Append((Int64)val);
            return this;
        }

        public QuickStringWriter Append(Int16 val)
        {
            Append((Int64)val);
            return this;
        }

        public QuickStringWriter Append(Int32 val)
        {
            Append((Int64)val);
            return this;
        }

        public override string ToString()
        {
            return new string(Buff, 0, Position);
        }

        public QuickStringWriter Append(Int64 val)
        {
            if (val == 0)
            {
                Buff[Position++] = '0';
                return this;
            }

            var pos = 63;
            if (val < 0)
            {
                Buff[Position++] = '-';
                NumberBuff[pos] = (char)(~(val % 10) + '1');
                if (val < -10)
                {
                    val = val / -10;
                    NumberBuff[--pos] = (char)(val % 10 + '0');
                }
            }
            else
            {
                NumberBuff[pos] = (char)(val % 10 + '0');
            }
            while ((val = val / 10L) != 0L)
            {
                NumberBuff[--pos] = (char)(val % 10L + '0');
            }
            var length = 64 - pos;
            Check(length);
            Array.Copy(NumberBuff, pos, Buff, Position, length);
            Position += length;
            return this;
        }

        public QuickStringWriter Append(Char val)
        {
            Try();
            Buff[Position++] = val;
            return this;
        }

        public QuickStringWriter Append(String val)
        {
            if (val == null || val.Length == 0)
            {
                return this;
            }
            else if (val.Length == 1)
            {
                Try();
                Buff[Position++] = val[0];
                return this;
            }
            Check(val.Length);
            val.CopyTo(0, Buff, Position, val.Length);
            Position += val.Length;
            return this;
        }

        public QuickStringWriter Append(Byte val)
        {
            Append((UInt64)val);
            return this;
        }

        public QuickStringWriter Append(UInt16 val)
        {
            Append((UInt64)val);
            return this;
        }

        public QuickStringWriter Append(UInt32 val)
        {
            Append((UInt64)val);
            return this;
        }

        public QuickStringWriter Append(UInt64 val)
        {
            if (val == 0)
            {
                Buff[Position++] = '0';
                return this;
            }
            var pos = 63;

            NumberBuff[pos] = (char)(val % 10 + '0');

            while ((val = val / 10L) != 0L)
            {
                NumberBuff[--pos] = (char)(val % 10L + '0');
            }
            var length = 64 - pos;
            Check(length);
            Array.Copy(NumberBuff, pos, Buff, Position, length);
            Position += length;
            return this;
        }

        public QuickStringWriter Clear()
        {
            Position = 0;
            return this;
        }

        void Try()
        {
            if (Position >= Buff.Length)
            {
                ToDouble();
            }
        }

        void Check(int count)
        {
            var pre = Position + count;
            if (pre >= Buff.Length)
            {
                SetCapacity(pre * 2);
            }
        }
    }
}
