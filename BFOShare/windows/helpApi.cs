/*
 * 由SharpDevelop创建。
 * 用户： cheng
 * 日期: 2019/11/5
 * 时间: 13:44
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace BFOShare.windows
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class helpApi
	{
		/**
		 * 引入user32.dll，声明FindWindow，查找窗体
		 */
		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		/**
		 * 引入user32.dll,声明FindWindowEx，查找控件
		 */
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
		/**
		 * 引入user32.dll,声明SendMessage，发送消息
		 */
		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, int wParam, string lParam);
		 
		
		/**
		 * 引入user32.dll，声明EnumChildWindows，遍历窗体列表
		 * 
		 */ 
		[DllImport("user32.dll")]
		public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
		
		[DllImport("user32", CharSet = CharSet.Unicode)]
		public static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
		
		
		[DllImport("user32.dll", EntryPoint = "keybd_event")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
		
		
		public delegate bool CallBack(IntPtr hwnd, int lParam);
		
		//移动鼠标
		const int MOUSEEVENTF_MOVE = 0x0001;
		//模拟鼠标左键按下
		const int MOUSEEVENTF_LEFTDOWN = 0x0002;
		//模拟鼠标左键抬起
		const int MOUSEEVENTF_LEFTUP = 0x0004;
		//模拟鼠标右键按下
		const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
		//模拟鼠标右键抬起
		const int MOUSEEVENTF_RIGHTUP = 0x0010;
		//模拟鼠标中键按下
		const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
		//模拟鼠标中键抬起
		const int MOUSEEVENTF_MIDDLEUP = 0x0040;
		//标示是否采用绝对坐标
		const int MOUSEEVENTF_ABSOLUTE = 0x8000;

		const int WM_GETTEXT = 0x000D;

		const int WM_SETTEXT = 0x000C;

		const int WM_CLICK = 0x00F5;
		
		//SendMessage参数
		const int WM_KEYDOWN = 0x0100;
		//普通按键按下
		const int WM_KEYUP = 0x0101;
		//普通按键放开
		const int WM_SYSKEYDOWN = 0x104;
		//系统按键按下
		const int WM_SYSKEYUP = 0x105;
		//系统按键按下放开
		const int WM_SYSCHAR = 0x0106;
		//发送单个字符
		const int WM_SETFOCUS = 0x0007;
		//设置焦点
		const int WM_KILLFOCUS = 0x0008;
		//移除焦点
		
		private InitConfig initConfig;
		public InitConfig InitConfig{
			get{
				return initConfig;
			}
			set{
				initConfig=value;
			}
		}
		public helpApi()
		{
			initConfig=new InitConfig();
		}
		
		public void SetUserNameAndPwd()
		{
			
			ThreadStart childref = new ThreadStart(CallToChildThread1);
			Console.WriteLine("In Main: Creating the Child thread");
			Thread childThread = new Thread(childref);
			childThread.Start();
		}
		public  void CallToChildThread1()
		{
			int k = 0;
			IntPtr win = FindWindow(null, "Sharep ALM Browser");
			
			IntPtr iResult = IntPtr.Zero;
			
			IntPtr iLoginName = IntPtr.Zero;
			IntPtr iLoginEditFrame = IntPtr.Zero;
			IntPtr iLoginEdit = IntPtr.Zero;
			
			IntPtr iLoginPwdName = IntPtr.Zero;
			IntPtr iLoginPwdEditFrame = IntPtr.Zero;
			IntPtr iLoginPwdEdit = IntPtr.Zero;
			
			
			while (k < 100) {
				int i = EnumChildWindows(win, (h, l) => {
					IntPtr f1 = FindWindowEx(h, IntPtr.Zero, null, "Login Name:");
					if (f1 == IntPtr.Zero)
						return true;
					else {
						iLoginName = f1;
						//找到用户名登录框
						iLoginEditFrame = FindWindowEx(h, iLoginName, "WindowsForms10.Window.8.app.0.33c0d9d", null);
						iLoginEdit = FindWindowEx(iLoginEditFrame, IntPtr.Zero, "WindowsForms10.EDIT.app.0.33c0d9d", null);
						return false;
					}
				}, 0);
				if (!iLoginEdit.Equals(IntPtr.Zero)) {
					Debug.WriteLine("找到用户框");
					//Console.WriteLine("找到用户框");
					break;
				} else {
					Thread.Sleep(1000);
					k++;
				}
			}
			k = 0;
			while (k < 100) {
				int i = EnumChildWindows(win, (h, l) => {
					IntPtr f1 = FindWindowEx(h, IntPtr.Zero, null, "Password:");
					if (f1 == IntPtr.Zero)
						return true;
					else {
						iLoginPwdName = f1;
						iLoginPwdEditFrame = FindWindowEx(h, iLoginPwdName, "WindowsForms10.Window.8.app.0.33c0d9d", null);
						iLoginPwdEdit = FindWindowEx(iLoginPwdEditFrame, IntPtr.Zero, "WindowsForms10.EDIT.app.0.33c0d9d", null);
						return false;
					}
				}, 0);
				
				if (!iLoginPwdEditFrame.Equals(IntPtr.Zero)) {
					Debug.WriteLine("找到密码框");
					//Console.WriteLine("找到密码框");
					break;
				} else {
					Thread.Sleep(1000);
					k++;
				}
			}
			
			//密码标签
			//iLoginPwdEditFrame=FindWindowEx(h,iLoginEditFrame,null,"Password:");
			
			
			
			//修改用户名：
			SendMessage(iLoginEdit, WM_SETFOCUS, 0, "");
			IntPtr resultEdit = SendMessage(iLoginEdit, WM_SETTEXT, 0, initConfig.UserName);
			SendMessage(iLoginEdit, WM_KILLFOCUS, 0, "");
			 
			SendMessage(iLoginPwdEditFrame, WM_SETFOCUS, 0, null);//设置焦点到密码框，发送tab键有时不能转到密码框
			SendMessage(iLoginPwdEditFrame, WM_KILLFOCUS, 0, null);//设置焦点到密码框，发送tab键有时不能转到密码框
			
			SendMessage(iLoginPwdEdit, WM_SETFOCUS, 0, "");
			IntPtr resultEdit1 = SendMessage(iLoginPwdEdit, WM_SETTEXT, 0, initConfig.PassWord);
			SendMessage(iLoginPwdEdit, WM_KILLFOCUS, 0, "");
			Thread.Sleep(1000);
			//认证按钮点击
			EnumChildWindows(win, (cd, l) => {
				IntPtr f1 = FindWindowEx(cd, IntPtr.Zero, null, "Authenticate");
				if (f1 == IntPtr.Zero)
					return true;
				else {
					Debug.WriteLine("找到认证框");
					SendMessage(f1, WM_SETFOCUS, 0, "");
					keybd_event((byte)Keys.Enter,0,0,0); //按下F10
					
					//SendMessage(f1, WM_CLICK, 0, null);
					SendMessage(f1, WM_KILLFOCUS, 0, "");
					return false;
				}
			}, 0);
			Thread.Sleep(1000);
			//登录按钮点击
			EnumChildWindows(win, (cd, l) => {
				IntPtr f1 = FindWindowEx(cd, IntPtr.Zero, null, "Login");
				if (f1 == IntPtr.Zero)
					return true;
				else {
					Debug.WriteLine("找到登录框");
					SendMessage(f1, WM_SETFOCUS, 0, "");
					keybd_event((byte)Keys.Enter,0,0,0); //按下F10
					
					//SendMessage(f1, WM_CLICK, 0, null);
					SendMessage(f1, WM_KILLFOCUS, 0, "");
					return false;
				}
			}, 0);
			Thread.Sleep(1000);
				
			
		}
	}
}
