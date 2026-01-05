namespace ExtractPngFromChituboxFile
{
    partial class ExtractPngFromChituboxFile
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ButtonSelectCtb = new Button();
            ButtonSavePngs = new Button();
            SuspendLayout();
            // 
            // ButtonSelectCtb
            // 
            ButtonSelectCtb.Location = new Point(30, 26);
            ButtonSelectCtb.Name = "ButtonSelectCtb";
            ButtonSelectCtb.Size = new Size(246, 60);
            ButtonSelectCtb.TabIndex = 0;
            ButtonSelectCtb.Text = "CTB 파일을 선택하세요.";
            ButtonSelectCtb.UseVisualStyleBackColor = true;
            ButtonSelectCtb.Click += ButtonSelectCtb_Click;
            // 
            // ButtonSavePngs
            // 
            ButtonSavePngs.Location = new Point(30, 108);
            ButtonSavePngs.Name = "ButtonSavePngs";
            ButtonSavePngs.Size = new Size(246, 60);
            ButtonSavePngs.TabIndex = 1;
            ButtonSavePngs.Text = "PNG 파일들을 저장하세요.";
            ButtonSavePngs.UseVisualStyleBackColor = true;
            ButtonSavePngs.Click += ButtonSavePngs_Click;
            // 
            // ExtractPngFromChituboxFile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 196);
            Controls.Add(ButtonSavePngs);
            Controls.Add(ButtonSelectCtb);
            Name = "ExtractPngFromChituboxFile";
            Text = "ExtractPngFromChituboxFile";
            ResumeLayout(false);
        }

        #endregion

        private Button ButtonSelectCtb;
        private Button ButtonSavePngs;
    }
}
