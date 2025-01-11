namespace Tennis
{
    partial class Tennis : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Tennis";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Tennis";
            this.ResumeLayout(false);
        }
    }
}
