using System;

namespace InstagramPhotos.Utility.QuickJson
{
    internal sealed class QuickStringReader : IDisposable
    {
        #region 静态

        /// <summary>
        /// 转义字母
        /// </summary>
        readonly static bool[] EscapeFlag_Switch = new bool[char.MaxValue];

        /// <summary>
        /// [0-9]
        /// </summary>
        readonly static bool[] Numbers_Switch = new bool[char.MaxValue];

        static QuickStringReader()
        {
            EscapeFlag_Switch['0'] = true;
            EscapeFlag_Switch['t'] = true;
            EscapeFlag_Switch['r'] = true;
            EscapeFlag_Switch['n'] = true;
            EscapeFlag_Switch['r'] = true;
            for (int i = '0'; i <= '9'; i++)
            {
                Numbers_Switch[i] = true;
            }
        }

        #endregion

        #region 字段

        int _Position = 0; //当前位置

        int _End = 0; //结束位置

        bool[] _FlagSwitch; //标识开关

        char[] _CharArray;//字符串数组

        QuickStringWriter _Buff; //缓冲区

        readonly int _StringLength; //字符串长度

        #endregion

        /// <summary>
        /// 当前位置
        /// </summary>
        public int Position
        {
            get { return _Position; }
            set
            {
                if (_Position >= _StringLength)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _Position = value;
                _Position = 0;
                Current = _CharArray[0];
            }
        }

        public QuickStringReader(string str)
        {
            _StringLength = str.Length;
            _CharArray = str.ToCharArray();
            _FlagSwitch = new bool[char.MaxValue];
            _Buff = new QuickStringWriter(_StringLength);
            _End = _StringLength - 1;
            _Position = 0;
            Current = _CharArray[0];
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _FlagSwitch = null;
            _CharArray = null;
        }

        /// <summary>
        /// 当前字符
        /// </summary>
        public char Current { get; private set; }

        /// <summary>
        /// 移动到下一个可读字符
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (IsEnd())
            {
                return false;
            }
            else
            {
                _Position++;
                Current = _CharArray[_Position];
                return true;
            }
        }

        /// <summary>
        /// 移动到结尾
        /// </summary>
        public void MoveToEnd()
        {
            _Position = _StringLength - 1;
        }

        /// <summary>
        /// 移动到下一个不是空格或回车的字符
        /// </summary>
        /// <returns></returns>
        public bool MoveToNotWhiteSpaceEnter()
        {
            do
            {
                if (char.IsWhiteSpace(Current) == false)
                {
                    return true;
                }
            } while (MoveNext());
            return false;
        }

        /// <summary>
        /// 移动到下一个不是空格的字符
        /// </summary>
        /// <returns></returns>
        public bool MoveToNotWhiteSpace()
        {
            do
            {
                switch (Current)
                {
                    case '\0':
                    case ' ':
                    case '\t':
                        continue;
                    default:
                        return true;
                }
            } while (MoveNext());
            return false;
        }

        /// <summary>
        /// 是否已经到结尾
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return _Position >= _End;
        }

        /// <summary>
        /// 移动到下一个可读字符
        /// </summary>
        /// <returns></returns>
        public char Read()
        {
            if (MoveNext())
            {
                return Current;
            }
            else
            {
                return '\0';
            }
        }

        /// <summary>
        /// 一直读到某个字符出现或者字符串结束,返回已读取的字符
        /// </summary>
        /// <param name="fixChar"></param>
        /// <returns></returns>
        public string ReadToChar(Char fixChar)
        {
            if (IsEnd() || Current == fixChar)//已到结尾
            {
                return string.Empty;
            }
            _Buff.Clear();
            do
            {
                //判断当前字符是否等于转义符
                if (Current == '\\')
                {
                    //读取下一个字符
                    if (MoveNext())
                    {
                        if (EscapeFlag_Switch[Current])
                        {
                            _Buff.Append('\\');
                        }
                    }
                    else
                    {//读取失败
                        return _Buff.ToString();
                    }
                }
                _Buff.Append(Current);//将当前字符推入缓冲区

            } while (MoveNext() && Current != fixChar);//读取下一个字符

            return _Buff.ToString();
        }

        /// <summary>
        /// 一直读到特定字符(不包含)出现或者字符串结束,返回已读取的字符
        /// </summary>
        /// <param name="fixChars"></param>
        /// <returns></returns>
        public string ReadToChars(params Char[] fixChars)
        {
            //设置标识开关
            if (SetFlagSwitch(fixChars) == false)
            {
                return string.Empty;
            }
            string s = ReadToChars(_FlagSwitch);
            ClearFlagSwitch(fixChars);
            return s;
        }

        /// <summary>
        /// 一直读到特定字符(不包含)出现或者字符串结束,返回已读取的字符
        /// </summary>
        /// <param name="fixCharFlags"></param>
        /// <returns></returns>
        public string ReadToChars(bool[] fixCharFlags)
        {
            if (IsEnd()) //已到结尾
            {
                return string.Empty;
            }

            if (fixCharFlags[Current]) //第一个就是结束字符
            {
                return string.Empty;
            }

            _Buff.Clear();
            do
            {
                //判断当前字符是否等于转义符
                if (Current == '\\')
                {
                    //读取下一个字符
                    if (MoveNext())
                    {
                        if (EscapeFlag_Switch[Current])
                        {
                            _Buff.Append('\\');
                        }
                    }
                    else
                    {//字符串结束
                        return _Buff.ToString();
                    }
                }
                _Buff.Append(Current);
            } while (MoveNext() && fixCharFlags[Current] == false);//读取下一个字符
            return _Buff.ToString();
        }

        /// <summary>
        /// 读取连续的特定字符
        /// </summary>
        /// <param name="fixChars"></param>
        /// <returns></returns>
        public string ReadChars(params Char[] fixChars)
        {
            //设置标识开关
            if (SetFlagSwitch(fixChars))
            {
                var s = ReadChars(_FlagSwitch);
                ClearFlagSwitch(fixChars);
                return s;
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取连续的特定字符
        /// </summary>
        /// <param name="fixCharFlags"></param>
        /// <returns></returns>
        public string ReadChars(bool[] fixCharFlags)
        {
            if (IsEnd()) //已到结尾
            {
                return string.Empty;
            }

            if (fixCharFlags[Current] == false) //第一个不是指定字符
            {
                return string.Empty;
            }
            _Buff.Clear();
            do
            {
                _Buff.Append(Current);
            } while (MoveNext() && fixCharFlags[Current]);
            return _Buff.ToString();
        }

        /// <summary>
        /// 返回剩余的字符串,该方法不会更新Position
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new string(_CharArray, _Position, _StringLength - _Position);
        }

        /// <summary>
        /// 从当前位置读取到字符串结束
        /// </summary>
        /// <returns></returns>
        public string ReadToEnd()
        {
            MoveToEnd();
            return ToString();
        }

        /// <summary>
        /// 获取指定地址之后的数字对象
        /// </summary>
        /// <returns></returns>
        public double ReaderNumber()
        {
            if (IsEnd()) //已到结尾
            {
                return double.NaN;
            }

            bool f = false;
            if (Current == '-')
            {
                f = true;
                MoveNext();
            }
            else if (Current == '+')
            {
                MoveNext();
            }

            if (Numbers_Switch[Current] == false && Current != '.')
            {
                return double.NaN;
            }

            double d = 0d;
            double digit = 1d;

            do
            {
                if (Numbers_Switch[Current])
                {
                    if (digit < 1d)
                    {
                        d += (int)(Current - '0') * digit;
                        digit *= 0.1d;
                    }
                    else
                    {
                        d = d * 10 + (int)(Current - '0');
                    }
                }
                else if (Current == '.' && digit == 1d)
                {
                    digit = 0.1d;
                }
                else
                {
                    if (Current == '%')
                    {
                        d *= 0.01d;
                        MoveNext();
                    }
                    break;
                }
            } while (MoveNext());
            if (f)
            {
                d = d * -1;
            }
            return d;

        }

        #region 私有方法

        /// <summary>
        /// 设置标识开关
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        private bool SetFlagSwitch(char[] flags)
        {
            if (flags == null || flags.Length == 0)
            {
                return false;
            }
            foreach (var item in flags)
            {
                _FlagSwitch[item] = true;
            }
            return true;
        }

        /// <summary>
        /// 清空标识开关
        /// </summary>
        /// <param name="flags"></param>
        private void ClearFlagSwitch(char[] flags)
        {
            foreach (var item in flags)
            {
                _FlagSwitch[item] = false;
            }
        }

        #endregion
    }
}
