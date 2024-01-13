namespace NewNovelTranslator
{
    partial class Form1
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
            text = new TextBox();
            start = new Button();
            textResult = new TextBox();
            SuspendLayout();
            // 
            // text
            // 
            text.Location = new Point(12, 12);
            text.Multiline = true;
            text.Name = "text";
            text.Size = new Size(532, 153);
            text.TabIndex = 0;
            // 
            // start
            // 
            start.Location = new Point(219, 180);
            start.Name = "start";
            start.Size = new Size(75, 23);
            start.TabIndex = 1;
            start.Text = "Start";
            start.UseVisualStyleBackColor = true;
            start.Click += start_Click;
            // 
            // textResult
            // 
            textResult.Location = new Point(7, 237);
            textResult.Multiline = true;
            textResult.Name = "textResult";
            textResult.Size = new Size(532, 175);
            textResult.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 424);
            Controls.Add(textResult);
            Controls.Add(start);
            Controls.Add(text);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox text;
        private Button start;
        private TextBox textResult;
    }
}
