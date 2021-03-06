﻿/*
 * Created by SharpDevelop.
 * User: cheng
 * Date: 2018/12/21
 * Time: 9:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using BFOShare.windows;


namespace BFOShare
{

	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
	
       
		static string inifilepath = "localdata.ini";
		
		public MainForm()
		{
			InitializeComponent();
		}
		void NotifyIcon1MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (WindowState == FormWindowState.Minimized) {
				//还原窗体显示    
				WindowState = FormWindowState.Normal;
				//激活窗体并给予它焦点
				this.Activate();
				//任务栏区显示图标
				this.ShowInTaskbar = true;
				//托盘区图标隐藏
				notifyIcon1.Visible = true;
			}
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
			notifyIcon1.Visible = true;
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			//载入配置文件
			//先判断是否有配置，如果没有则创建
			//Localdata.ini
			
			if (!File.Exists(inifilepath)) {
				FileStream fs = File.Create(inifilepath);
				if (fs != null) {
					fs.Flush();
					fs.Close();
					IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "url", "https://www.djc8.cn", "页面url地址的");
					IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "width", "1300", "窗口宽度");
					IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "heigth", "700", "窗口高度");
					IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "title", "Sharep ALM Browser", "窗口标题");
				}
			}
			
			List<IniHelper.IniStruct> iniValues = IniHelper.IniHelper.ReadValues(inifilepath);
			string url = IniHelper.IniHelper.ReadValue(inifilepath, "url", "WinFormIni");
			string width = IniHelper.IniHelper.ReadValue(inifilepath, "width", "WinFormIni");
			string heigth = IniHelper.IniHelper.ReadValue(inifilepath, "heigth", "WinFormIni");
			string title = IniHelper.IniHelper.ReadValue(inifilepath, "title", "WinFormIni");
			string username=IniHelper.IniHelper.ReadValue(inifilepath,"username","WinFormIni");
			string password=IniHelper.IniHelper.ReadValue(inifilepath,"password","WinFormIni");
			if (width != "" && heigth != "") {
				this.Width = Int32.Parse(width);
				this.Height = Int32.Parse(heigth);
			}
			if (title != "") {
				this.Text = title;
				notifyIcon1.Text = title;
			}
			if (url != "") {
				this.wbForm.Url = new System.Uri(url);
			}
			
			if(username!=""){
				initConfig.UserName=username;
			}
			if(password!=""){
				initConfig.PassWord=password;
			}
			
			
		}
		private InitConfig initConfig = new InitConfig();
		void saveIni()
		{
			//IniHelper.IniHelper.Write(inifilepath,"WinFormIni","url","http://www.djc8.cn","页面url地址的");
			IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "width", this.Width.ToString(), "窗口宽度");
			IniHelper.IniHelper.Write(inifilepath, "WinFormIni", "heigth", this.Height.ToString(), "窗口高度");
			//IniHelper.IniHelper.Write(inifilepath,"WinFormIni","title","Sharep ALM Browser","窗口标题");
		}
		void MainFormSizeChanged(object sender, EventArgs e)
		{
			//判断是否选择的是最小化按钮
			if (WindowState == FormWindowState.Minimized) {
				//隐藏任务栏区图标
				this.ShowInTaskbar = false;
				//图标显示在托盘区
				notifyIcon1.Visible = true;
			} else if (WindowState == FormWindowState.Maximized) {
            	
			} else {
				//保存下当前配置
				saveIni();
			}
            
		}
		/**
		 * 显示
		 */
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized) {
				//还原窗体显示    
				WindowState = FormWindowState.Normal;
				//激活窗体并给予它焦点
				this.Activate();
				//任务栏区显示图标
				this.ShowInTaskbar = true;
				//托盘区图标隐藏
				notifyIcon1.Visible = true;
			}
		}
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				// 关闭所有的线程
				this.Dispose();
				this.Close();
			}
            
		}
		void ContextMenuStrip1Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
	
		}
		void TsmiRestartClick(object sender, EventArgs e)
		{ 
			Debug.WriteLine("1");
			if (MessageBox.Show("是否确认重启程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				this.Dispose();
				this.Close();
				//Application.Exit();
				System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
			
		}
		
		public void CallToChildThread()
		{
           
			helpApi api=new helpApi();
			api.InitConfig=initConfig;
			api.SetUserNameAndPwd();
		}
       
		void WbFormDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			ThreadStart childref = new ThreadStart(CallToChildThread);
			Console.WriteLine("In Main: Creating the Child thread");
			Thread childThread = new Thread(childref);
			childThread.Start();
		}
	}
}
