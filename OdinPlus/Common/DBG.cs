using System;

namespace OdinPlus.Common
{
	[Obsolete]
	public static class DBG
	{
		#region Debug
		public static void cprt(string s)
		{
			global::Console.instance.Print(s);
		}
		public static void InfoTL(string s)
		{
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, s, 0, null);
		}
		public static void InfoCT(string s)
		{
			Player.m_localPlayer.Message(MessageHud.MessageType.Center, s, 0, null);
		}
		public static void blogInfo(object o)
		{
			Log.Info(o);
		}
		public static void blogWarning(object o)
		{
      Log.Warning(o);
		}
		public static void blogError(object o)
		{
      Log.Error(o);
		}
		public static void a()
		{
			blogWarning("a");
		}
		public static void b()
		{
			blogWarning("b");
		}
		public static void c()
		{
			blogWarning("c");
		}
		#endregion Debug
	}
}
