using Bll.Utils;
using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraSplashScreen;
using System;
using System.Windows.Forms;

namespace Bll.Common
{
    /// <summary>
    /// Manager class để quản lý việc hiển thị UserControl trong FluentDesignFormContainer
    /// </summary>
    public class UserControlManager(FluentDesignFormContainer container)
    {
        public DevExpress.XtraEditors.XtraUserControl TopmostUserControl { get; private set; }

        public void ShowControl(DevExpress.XtraEditors.XtraUserControl instance, string title)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(VnsSplashScreen));

                if (!container.Controls.Contains(instance))
                {
                    container.Controls.Add(instance);
                    instance.Dock = DockStyle.Fill;
                    instance.BringToFront();
                }
                else
                    instance.BringToFront();

                // Set the window title
                container.Parent.Text = title;

                TopmostUserControl = instance;
                var fluentFrm = container.Parent as FluentDesignForm;
                SetLocationOfFluentButton(fluentFrm, title);

                if (SplashScreenManager.Default == null) return;

                if (SplashScreenManager.Default.IsSplashFormVisible)
                    SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                if (SplashScreenManager.Default != null)
                    if (SplashScreenManager.Default.IsSplashFormVisible)
                        SplashScreenManager.CloseForm();
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Set vị trí khi click vào nút của Fluent Form
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="location"></param>
        private static void SetLocationOfFluentButton(FluentDesignForm frm, string location)
        {
            var parentForm = frm.ParentForm;

            // Lấy RibbonStatusBar từ Form cha
            if (parentForm?.Controls["ribbonStatusBar"] is not RibbonStatusBar ribbonStatusBar) return;

            foreach (DevExpress.XtraBars.BarItemLink link in ribbonStatusBar.ItemLinks)
            {
                if (link.Item is not DevExpress.XtraBars.BarStaticItem barStaticItem) continue;

                switch (barStaticItem)
                {
                    case { Name: "lblViTri" }:
                        barStaticItem.Caption = $@"{frm.Text} >><color='red'> {location} </color>";
                        barStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        break;
                    case { Name: "lblHanhDong" }:
                        barStaticItem.Caption = $@"";
                        barStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        break;
                }
            }
        }
    }
}