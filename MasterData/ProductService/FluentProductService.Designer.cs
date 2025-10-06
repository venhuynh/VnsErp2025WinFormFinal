using System.ComponentModel;
using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;

namespace MasterData.ProductService
{
    partial class FluentProductService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fluentDesignFormContainer1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.ProductServiceCategoryBtn = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ProductServiceListBtn = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ProductVariantBtn = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.ProductServiceImagesBtn = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // fluentDesignFormContainer1
            // 
            this.fluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer1.Location = new System.Drawing.Point(260, 33);
            this.fluentDesignFormContainer1.Name = "fluentDesignFormContainer1";
            this.fluentDesignFormContainer1.Size = new System.Drawing.Size(648, 698);
            this.fluentDesignFormContainer1.TabIndex = 0;
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.ProductServiceCategoryBtn,
            this.ProductServiceListBtn,
            this.ProductVariantBtn,
            this.ProductServiceImagesBtn});
            this.accordionControl1.Location = new System.Drawing.Point(0, 33);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(260, 698);
            this.accordionControl1.TabIndex = 1;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // ProductServiceCategoryBtn
            // 
            this.ProductServiceCategoryBtn.Name = "ProductServiceCategoryBtn";
            this.ProductServiceCategoryBtn.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ProductServiceCategoryBtn.Text = "Phân loại";
            // 
            // ProductServiceListBtn
            // 
            this.ProductServiceListBtn.Name = "ProductServiceListBtn";
            this.ProductServiceListBtn.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ProductServiceListBtn.Text = "Danh mục SPDV";
            this.ProductServiceListBtn.Click += new System.EventHandler(this.ProductServiceListBtn_Click);
            // 
            // ProductVariantBtn
            // 
            this.ProductVariantBtn.Name = "ProductVariantBtn";
            this.ProductVariantBtn.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ProductVariantBtn.Text = "Biến thể SPDV";
            this.ProductVariantBtn.Click += new System.EventHandler(this.ProductVariantBtn_Click);
            // 
            // ProductServiceImagesBtn
            // 
            this.ProductServiceImagesBtn.Name = "ProductServiceImagesBtn";
            this.ProductServiceImagesBtn.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.ProductServiceImagesBtn.Text = "Hình ảnh";
            this.ProductServiceImagesBtn.Click += new System.EventHandler(this.ProductServiceImagesBtn_Click);
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(908, 33);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.Form = this;
            // 
            // FluentProductService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 731);
            this.ControlContainer = this.fluentDesignFormContainer1;
            this.Controls.Add(this.fluentDesignFormContainer1);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.Name = "FluentProductService";
            this.NavigationControl = this.accordionControl1;
            this.Text = "FluentProductService";
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FluentDesignFormContainer fluentDesignFormContainer1;
        private AccordionControl accordionControl1;
        private FluentDesignFormControl fluentDesignFormControl1;
        private FluentFormDefaultManager fluentFormDefaultManager1;
        private AccordionControlElement ProductServiceCategoryBtn;
        private AccordionControlElement ProductServiceListBtn;
        private AccordionControlElement ProductVariantBtn;
        private AccordionControlElement ProductServiceImagesBtn;
    }
}