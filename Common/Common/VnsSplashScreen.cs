using System;
using DevExpress.XtraSplashScreen;

namespace Common.Common
{
    public partial class VnsSplashScreen : SplashScreen
    {
        public VnsSplashScreen()
        {
            InitializeComponent();
            this.labelCopyright.Text = @"Copyright © 2020-" + DateTime.Now.Year.ToString();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}