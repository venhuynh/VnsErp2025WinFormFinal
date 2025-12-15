using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using System.Windows.Forms;

namespace VersionAndUserManagement.ApplicationVersion
{
    partial class FrmApplicationVersionDtoAddEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.VersionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.applicationVersionDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReleaseDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.DescriptionTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ReleaseNoteTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForVersion = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReleaseDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReleaseNote = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VersionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationVersionDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseNoteTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReleaseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReleaseNote)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SaveBarButtonItem,
            this.CloseBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::VersionAndUserManagement.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::VersionAndUserManagement.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(599, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 323);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(599, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 299);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(599, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 299);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.VersionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ReleaseDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveCheckEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ReleaseNoteTextEdit);
            this.dataLayoutControl1.DataSource = this.applicationVersionDtoBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(599, 299);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // VersionTextEdit
            // 
            this.VersionTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationVersionDtoBindingSource, "Version", true));
            this.VersionTextEdit.Location = new System.Drawing.Point(86, 12);
            this.VersionTextEdit.MenuManager = this.barManager1;
            this.VersionTextEdit.Name = "VersionTextEdit";
            this.VersionTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.VersionTextEdit.Size = new System.Drawing.Size(313, 20);
            this.VersionTextEdit.StyleController = this.dataLayoutControl1;
            this.VersionTextEdit.TabIndex = 5;
            // 
            // applicationVersionDtoBindingSource
            // 
            this.applicationVersionDtoBindingSource.DataSource = typeof(DTO.VersionAndUserManagementDto.ApplicationVersionDto);
            // 
            // ReleaseDateDateEdit
            // 
            this.ReleaseDateDateEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationVersionDtoBindingSource, "ReleaseDate", true));
            this.ReleaseDateDateEdit.EditValue = null;
            this.ReleaseDateDateEdit.Location = new System.Drawing.Point(86, 36);
            this.ReleaseDateDateEdit.MenuManager = this.barManager1;
            this.ReleaseDateDateEdit.Name = "ReleaseDateDateEdit";
            this.ReleaseDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReleaseDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReleaseDateDateEdit.Size = new System.Drawing.Size(501, 20);
            this.ReleaseDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ReleaseDateDateEdit.TabIndex = 6;
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationVersionDtoBindingSource, "IsActive", true));
            this.IsActiveCheckEdit.Location = new System.Drawing.Point(403, 12);
            this.IsActiveCheckEdit.MenuManager = this.barManager1;
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            this.IsActiveCheckEdit.Properties.Caption = "Đang hoạt động";
            this.IsActiveCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveCheckEdit.Size = new System.Drawing.Size(184, 20);
            this.IsActiveCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsActiveCheckEdit.TabIndex = 8;
            // 
            // DescriptionTextEdit
            // 
            this.DescriptionTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationVersionDtoBindingSource, "Description", true));
            this.DescriptionTextEdit.Location = new System.Drawing.Point(86, 60);
            this.DescriptionTextEdit.MenuManager = this.barManager1;
            this.DescriptionTextEdit.Name = "DescriptionTextEdit";
            this.DescriptionTextEdit.Size = new System.Drawing.Size(501, 60);
            this.DescriptionTextEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionTextEdit.TabIndex = 7;
            // 
            // ReleaseNoteTextEdit
            // 
            this.ReleaseNoteTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.applicationVersionDtoBindingSource, "ReleaseNote", true));
            this.ReleaseNoteTextEdit.Location = new System.Drawing.Point(86, 124);
            this.ReleaseNoteTextEdit.MenuManager = this.barManager1;
            this.ReleaseNoteTextEdit.Name = "ReleaseNoteTextEdit";
            this.ReleaseNoteTextEdit.Size = new System.Drawing.Size(501, 79);
            this.ReleaseNoteTextEdit.StyleController = this.dataLayoutControl1;
            this.ReleaseNoteTextEdit.TabIndex = 9;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(599, 299);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForVersion,
            this.ItemForReleaseDate,
            this.ItemForIsActive,
            this.ItemForDescription,
            this.ItemForReleaseNote});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(579, 279);
            // 
            // ItemForVersion
            // 
            this.ItemForVersion.Control = this.VersionTextEdit;
            this.ItemForVersion.Location = new System.Drawing.Point(0, 0);
            this.ItemForVersion.Name = "ItemForVersion";
            this.ItemForVersion.Size = new System.Drawing.Size(391, 24);
            this.ItemForVersion.Text = "Phiên bản";
            this.ItemForVersion.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForReleaseDate
            // 
            this.ItemForReleaseDate.Control = this.ReleaseDateDateEdit;
            this.ItemForReleaseDate.Location = new System.Drawing.Point(0, 24);
            this.ItemForReleaseDate.Name = "ItemForReleaseDate";
            this.ItemForReleaseDate.Size = new System.Drawing.Size(579, 24);
            this.ItemForReleaseDate.Text = "Ngày phát hành";
            this.ItemForReleaseDate.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveCheckEdit;
            this.ItemForIsActive.Location = new System.Drawing.Point(391, 0);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(188, 24);
            this.ItemForIsActive.Text = "Đang hoạt động";
            this.ItemForIsActive.TextVisible = false;
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionTextEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 48);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(579, 64);
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(62, 13);
            // 
            // ItemForReleaseNote
            // 
            this.ItemForReleaseNote.Control = this.ReleaseNoteTextEdit;
            this.ItemForReleaseNote.Location = new System.Drawing.Point(0, 112);
            this.ItemForReleaseNote.Name = "ItemForReleaseNote";
            this.ItemForReleaseNote.Size = new System.Drawing.Size(579, 83);
            this.ItemForReleaseNote.Text = "Ghi chú phát hành";
            this.ItemForReleaseNote.TextSize = new System.Drawing.Size(62, 13);
            // 
            // FrmApplicationVersionDtoAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 323);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmApplicationVersionDtoAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VersionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationVersionDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReleaseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReleaseNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReleaseNoteTextEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
        private DataLayoutControl dataLayoutControl1;
        private TextEdit VersionTextEdit;
        private BindingSource applicationVersionDtoBindingSource;
        private DateEdit ReleaseDateDateEdit;
        private CheckEdit IsActiveCheckEdit;
        private MemoEdit DescriptionTextEdit;
        private MemoEdit ReleaseNoteTextEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForVersion;
        private LayoutControlItem ItemForReleaseDate;
        private LayoutControlItem ItemForIsActive;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem ItemForReleaseNote;
    }
}
