using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CGF
{

    public interface IDebugerConsole
    {
        void Log(string msg, object context = null);
        void LogWarning(string msg, object context = null);

        void LogError(string msg, object context = null);
    }

    public class UnityDebugerConsole : IDebugerConsole
    {
        private object[] m_tmpArgs = new[] { "" };
        private MethodInfo m_miLog;

        private MethodInfo m_miLogWaring;

        private MethodInfo m_miLogError;

        public UnityDebugerConsole()
        {
            Type type = Type.GetType("UnityEngine.Debug, UnieyEngine");
            m_miLog = type.GetMethod("Log", new Type[] {typeof(object)});
            m_miLogWaring = type.GetMethod("LogWarning", new Type[] { typeof(object) });
            m_miLogError = type.GetMethod("LogError", new Type[] { typeof(object) });
        }

        public void Log(string msg, object context = null)
        {
            m_tmpArgs[0] = msg;
            m_miLog.Invoke(null, m_tmpArgs);
        }

        public void LogWarning(string msg, object context = null)
        {
            m_tmpArgs[0] = msg;
            m_miLogWaring.Invoke(null, m_tmpArgs);
        }

        public void LogError(string msg, object context = null)
        {
            m_tmpArgs[0] = msg;
            m_miLogError.Invoke(null, m_tmpArgs);
        }
    }

    public interface ILogTag
    {
        string LOG_TAG { get; }
    }


    public static class Debuger
    {
        public static bool EnableLog = true;
        public static bool EnableTime = true;
        public static bool EnableSave = true;
        public static bool EnableStack = false;
        public static string LogFileDir = "";
        public static string LogFileName = "";
        public static string Prefix = "> ";

        private static IDebugerConsole m_console;

        public static void Init(string logFileDir = null, IDebugerConsole console = null)
        {
            LogFileDir = logFileDir;
            m_console = console;
            if (string.IsNullOrEmpty(LogFileDir))
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                LogFileDir = path + "/DebugerLog/";
            }
        }

        public static void Internal_Log(string msg,object context = null)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }


            if (m_console != null)
            {
                m_console.Log(msg, context);
            }
            else
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(msg);
                Console.ForegroundColor = old;
            }

            LogToFile("[I]" + msg);
        }

        public static void Internal_LogWaring(string msg, object context = null)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }


            if (m_console != null)
            {
                m_console.LogWarning(msg, context);
            }
            else
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(msg);
                Console.ForegroundColor = old;
            }

            LogToFile("[W]" + msg);
        }

        public static void Internal_LogError(string msg, object context = null)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }


            if (m_console != null)
            {
                m_console.LogError(msg, context);
            }
            else
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ForegroundColor = old;
            }

            LogToFile("[E]" + msg);
        }

        [Conditional("ENABLE_LOG")]
        public static void Log(object obj)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), obj);
            Internal_Log(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void Log(string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogCaller(true),message);
            Internal_Log(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void Log(string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), string.Format(format,args));
            Internal_Log(Prefix + message);
        }


        [Conditional("ENABLE_LOG")]
        public static void Log(this ILogTag obj, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogTag(obj), GetLogCaller(), message);
            Internal_Log(Prefix + message);

        }

        [Conditional("ENABLE_LOG")]
        public static void Log(this ILogTag obj, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogTag(obj), GetLogCaller(), string.Format(format, args));
            Internal_Log(Prefix + message);

        }


        [Conditional("ENABLE_LOG")]
        public static void LogWaring(object obj = null)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), obj);
            Internal_LogWaring(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogWaring(string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogCaller(true), message);
            Internal_LogWaring(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogWaring(string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), string.Format(format, args));
            Internal_LogWaring(Prefix + message);
        }


        [Conditional("ENABLE_LOG")]
        public static void LogWaring(this ILogTag obj, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogTag(obj), GetLogCaller(), message);
            Internal_LogWaring(Prefix + message);

        }

        [Conditional("ENABLE_LOG")]
        public static void LogWaring(this ILogTag obj, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogTag(obj), GetLogCaller(), string.Format(format, args));
            Internal_LogWaring(Prefix + message);

        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(object obj = null)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), obj);
            Internal_LogError(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogCaller(true), message);
            Internal_LogError(Prefix + message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogCaller(true), string.Format(format, args));
            Internal_LogError(Prefix + message);
        }


        [Conditional("ENABLE_LOG")]
        public static void LogError(this ILogTag obj, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(GetLogTag(obj), GetLogCaller(), message);
            Internal_LogError(Prefix + message);

        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(this ILogTag obj, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(GetLogTag(obj), GetLogCaller(), string.Format(format, args));
            Internal_LogError(Prefix + message);
        }

        public static string GetLogText(string tag, string methodName,string message)
        {
            return tag + "::" + methodName + " () " + message;
        }

        public static string GetLogText(string caller, string message)
        {
            return caller + "() " + message;
        }


        public static string GetLogText(string caller, object message)
        {
            return caller + "()" + (message != null ? message.ToListString() : "null");
        }


        /// <summary>
        /// 将容器序列化成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string ToListString(this object obj)
        {
            if (obj is string)
            {
                return obj.ToString();
            }
            else
            {
                var objAsList = obj as IEnumerable;
                return objAsList == null ? obj.ToString() : objAsList.Cast<object>().ListToString();
            }
        }

        private static string ListToString<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return "null";
            }

            if (source.Count() == 0)
            {
                return "[]";
            }

            if (source.Count() == 1)
            {
                return "[" + source.First() + "]";
            }

            var s = "";

            s += source.ButFirst().Aggregate(s, (res, x) => res + ", " + x.ToListString());
            s = "[" + source.First().ToListString() + s + "]";

            return s;
        }

        private static IEnumerable<T> ButFirst<T>(this IEnumerable<T> source)
        {
            return source.Skip(1);
        }

        private static string GetLogTag(ILogTag obj)
        {
            return obj.LOG_TAG;
        }

        private static Assembly ms_Assembly;

        public static string GetLogCaller(bool bIncludeClassName = false)
        {
            StackTrace st = new StackTrace(2,false);
            if (st != null)
            {
                if (null == ms_Assembly)
                {
                    ms_Assembly = typeof(Debuger).Assembly;
                }

                int currentStackFrameIndex = 0;
                while (currentStackFrameIndex < st.FrameCount)
                {
                    StackFrame oneSf = st.GetFrame(currentStackFrameIndex);
                    MethodBase oneMethod = oneSf.GetMethod();

                    if (oneMethod.Module.Assembly != ms_Assembly)
                    {
                        if (bIncludeClassName)
                        {
                            return oneMethod.DeclaringType.Namespace + "::" + oneMethod.Name;
                        }
                        else
                        {
                            return oneMethod.Name;
                        }
                    }

                    currentStackFrameIndex++;
                }
            }

            return "";
        }


        public static void LogToFile(string msg)
        {

        }
    }
}