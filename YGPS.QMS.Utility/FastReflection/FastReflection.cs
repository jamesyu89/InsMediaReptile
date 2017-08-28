using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace InstagramPhotos.Utility.FastReflection
{
    /// <summary>
    ///     快速反射
    /// </summary>
    public static class FastReflection
    {
        #region 私有成员

        /// <summary>
        ///     反射缓存池.
        /// </summary>
        private static readonly Dictionary<Type, FastReflectionCacheOfType> _reflectionCacheList =
            new Dictionary<Type, FastReflectionCacheOfType>();

        /// <summary>
        ///     小锁锁一个... 大锁锁一窝
        /// </summary>
        private static readonly object _cacheLock = new object();

        /// <summary>
        ///     快速反射缓存信息
        /// </summary>
        private class FastReflectionCacheOfType
        {
            /// <summary>
            ///     代理方法
            /// </summary>
            public Dictionary<string, FastDelegateHandle> ProxyMethods;
        }

        #endregion

        #region 常规代码

        /// <summary>
        ///     方法调用委托
        /// </summary>
        /// <param name="target">对象</param>
        /// <param name="paramters">参数</param>
        /// <returns></returns>
        public delegate object FastDelegateHandle(object target, params object[] paramters);

        /// <summary>
        ///     获取属性的值.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"> </param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object FGetValue(object obj, string name)
        {
            return GetDelegate(obj, "get_" + name)(obj);
        }

        /// <summary>
        ///     设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void FSetValue(object obj, string name, object value)
        {
            GetDelegate(obj, "set_" + name)(obj, value);
        }

        /// <summary>
        ///     获取方法代理
        /// </summary>
        /// <returns></returns>
        public static FastDelegateHandle GetDelegate(object obj, string methodName)
        {
            Type ot = obj.GetType();

            //从缓存中查找委托信息.
            FastReflectionCacheOfType cache = _reflectionCacheList.ContainsKey(ot)
                ? _reflectionCacheList[ot]
                : _reflectionCacheList[ot] =
                    new FastReflectionCacheOfType {ProxyMethods = new Dictionary<string, FastDelegateHandle>()};

            if (cache.ProxyMethods.ContainsKey(methodName))
            {
                return cache.ProxyMethods[methodName];
            }

            //新建委托
            lock (_cacheLock)
            {
                MethodInfo method = ot.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                var dyMethod = new DynamicMethod(string.Empty, typeof (object),
                    new[] {typeof (object), typeof (object[])}, method.DeclaringType.Module);
                ILGenerator il = dyMethod.GetILGenerator();
                ParameterInfo[] ps = method.GetParameters();
                var pts = new Type[ps.Length];

                //copy psType to pts
                for (int i = 0; i < pts.Length; i++)
                {
                    pts[i] = ps[i].ParameterType;
                }

                //声明局部变量
                var locals = new LocalBuilder[pts.Length];
                for (int i = 0; i < pts.Length; i++)
                {
                    locals[i] = il.DeclareLocal(pts[i]);
                }

                //设置局部变量 

                #region MyRegion

                for (int i = 0; i < pts.Length; i++)
                {
                    //压栈
                    il.Emit(OpCodes.Ldarg_1);

                    //小范围(-1 - 8)压栈.
                    switch (i)
                    {
                        case -1:
                            il.Emit(OpCodes.Ldc_I4_M1);
                            break;
                        case 0:
                            il.Emit(OpCodes.Ldc_I4_0);
                            break;
                        case 1:
                            il.Emit(OpCodes.Ldc_I4_1);
                            break;
                        case 2:
                            il.Emit(OpCodes.Ldc_I4_2);
                            break;
                        case 3:
                            il.Emit(OpCodes.Ldc_I4_3);
                            break;
                        case 4:
                            il.Emit(OpCodes.Ldc_I4_4);
                            break;
                        case 5:
                            il.Emit(OpCodes.Ldc_I4_5);
                            break;
                        case 6:
                            il.Emit(OpCodes.Ldc_I4_6);
                            break;
                        case 7:
                            il.Emit(OpCodes.Ldc_I4_7);
                            break;
                        case 8:
                            il.Emit(OpCodes.Ldc_I4_8);
                            break;
                        default:
                            //中等范围.
                            if (i > -129 && i < 128)
                            {
                                il.Emit(OpCodes.Ldc_I4_S, (SByte) i);
                            }
                            else
                            {
                                il.Emit(OpCodes.Ldc_I4, i);
                            }
                            break;
                    }

                    //临时入栈顶
                    il.Emit(OpCodes.Ldelem_Ref);

                    //做个类型转换, 如果是值类型,则拆箱,否则,直接转换为所需要类 (引用类型是可以做类型转换的.)
                    il.Emit(pts[i].IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, pts[i]);

                    //出栈.并作为下标i+1
                    il.Emit(OpCodes.Stloc, locals[i]);
                }

                #endregion

                //压0栈 (对象引用)
                il.Emit(OpCodes.Ldarg_0);

                //压参数栈 (所传参数)
                for (int i = 0; i < pts.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, locals[i]);
                }

                //呼叫方法
                il.EmitCall(OpCodes.Call, method, null);

                //如果方法没有返回值, return null.
                if (method.ReturnType == typeof (void))
                    il.Emit(OpCodes.Ldnull);
                else
                    //如果是值类型,则装箱后return, 否则,直接return.
                    if (method.ReturnType.IsValueType)
                        il.Emit(OpCodes.Box, method.ReturnType);

                //返回值
                il.Emit(OpCodes.Ret);

                //创建委托.
                var invoder = (FastDelegateHandle) dyMethod.CreateDelegate(typeof (FastDelegateHandle));

                //缓存委托.
                cache.ProxyMethods.Add(methodName, invoder);

                return invoder;
            }
        }

        /// <summary>
        ///     调用方法
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="values">要传的参数值</param>
        /// <returns></returns>
        public static object CallMethod(object obj, string methodName, params object[] values)
        {
            return GetDelegate(obj, methodName)(obj, values);
        }

        #endregion

        #region 扩展方法

        /// <summary>
        ///     调用方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object FastCallMethod(this object obj, string methodName, params object[] values)
        {
            return GetDelegate(obj, methodName)(obj, values);
        }

        /// <summary>
        ///     获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object FastGetValue(this object obj, string name)
        {
            return FGetValue(obj, name);
        }

        /// <summary>
        ///     获取属性值 非快速反射
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, string name)
        {
            return
                obj.GetType()
                    .GetProperty(name,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    .GetValue(obj, null);
        }

        /// <summary>
        ///     设置属性值 非快速反射
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void SetValue(this object obj, string name, object value)
        {
            obj.GetType()
                .GetProperty(name,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .SetValue(obj, value, null);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void FastSetValue(this object obj, string name, object value)
        {
            FSetValue(obj, name, value);
        }

        #endregion
    }
}