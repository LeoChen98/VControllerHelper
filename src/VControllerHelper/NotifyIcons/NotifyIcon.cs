using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using Prism.Commands;


namespace VControllerHelper.NotifyIcons
{
    internal class NotifyIcon
    {
        #region Private Fields

        private static NotifyIcon _Instance;

        private TaskbarIcon NI;

        #endregion Private Fields

        #region Public Constructors

        public NotifyIcon()
        {
            NI = new TaskbarIcon()
            {
                Icon = new Icon("favicon.ico"),
#if DEBUG
                ToolTipText = "VControllerHelper - 调试模式",
#else
                ToolTipText = "VControllerHelper",
#endif
                Visibility = Visibility.Visible,
                ContextMenu = new ContextMenu(),
                MenuActivation = PopupActivationMode.RightClick
            };

            MenuItem menu_ShowMainWindow = new MenuItem { Header = "显示主窗口", FontWeight = FontWeights.Bold };
            menu_ShowMainWindow.Click += MI_ShowMainWindow_Click;
            NI.ContextMenu.Items.Add(menu_ShowMainWindow);
            NI.ContextMenu.Items.Add(new Separator());
            MenuItem menu_Exit = new MenuItem { Header = "退出" };
            menu_Exit.Click += MI_Exit_Click;
            NI.ContextMenu.Items.Add(menu_Exit);

            NI.DoubleClickCommand =new DelegateCommand(() =>
            {
                //View.WindowManager.Instance.GetWindow<View.MainWindow>().Show();
            });
        }

        #endregion Public Constructors

        #region Public Properties

        public static NotifyIcon Instance
        {
            get
            {
                if (_Instance is null)
                {
                    _Instance = new NotifyIcon();
                }

                return _Instance;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 气泡提示
        /// </summary>
        /// <param name="msg">必选，消息</param>
        /// <param name="title">可选，标题</param>
        /// <param name="icon">可选，图标</param>
        public void ShowToolTip(string msg, string title = "VControllerHelper", BalloonIcon icon = BalloonIcon.Info)
        {
            NI.ShowBalloonTip(title,msg,icon);
        }

        #endregion Public Methods

        #region Private Methods

        private void MI_Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MI_ShowMainWindow_Click(object sender, EventArgs e)
        {
            //View.WindowManager.Instance.GetWindow<View.MainWindow>().Show();
        }

        #endregion Private Methods
    }
}