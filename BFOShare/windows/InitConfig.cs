/*
 * 由SharpDevelop创建。
 * 用户： cheng
 * 日期: 2019/11/5
 * 时间: 14:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace BFOShare.windows
{
	/// <summary>
	/// Description of InitAttribute.
	/// </summary>
	public class InitConfig
	{
		public InitConfig()
		{
		}
		private string username;
		public string UserName {
			get {
				return username;
			}
			set {
				username = value;
			}
		}
		private string password;
		public string PassWord {
			get {
				return password;
			}
			set {
				password = value;
			}
		}
	}
}
