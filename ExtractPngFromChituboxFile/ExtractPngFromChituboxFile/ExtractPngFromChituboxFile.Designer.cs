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
            progressBar1 = new ProgressBar();
            LabelProgress = new Label();
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
            // progressBar1
            // 
            progressBar1.Location = new Point(30, 210);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(246, 25);
            progressBar1.TabIndex = 2;
            // 
            // LabelProgress
            // 
            LabelProgress.AutoSize = true;
            LabelProgress.BackColor = Color.Transparent;
            LabelProgress.Location = new Point(127, 184);
            LabelProgress.Name = "LabelProgress";
            LabelProgress.Size = new Size(52, 15);
            LabelProgress.TabIndex = 3;
            LabelProgress.Text = "Progress";
            // 
            // ExtractPngFromChituboxFile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 256);
            Controls.Add(LabelProgress);
            Controls.Add(progressBar1);
            Controls.Add(ButtonSavePngs);
            Controls.Add(ButtonSelectCtb);
            Name = "ExtractPngFromChituboxFile";
            Text = "ExtractPngFromChituboxFile";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtonSelectCtb;
        private Button ButtonSavePngs;
        private ProgressBar progressBar1;
        private Label LabelProgress;
    }
}
