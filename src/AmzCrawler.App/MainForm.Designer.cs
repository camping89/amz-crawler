namespace AmzCrawler.App
{
    partial class MainForm
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
            this.BtnCrawling = new System.Windows.Forms.Button();
            this.BtnUpdatePrices = new System.Windows.Forms.Button();
            this.PgrUpdatePrices = new System.Windows.Forms.ProgressBar();
            this.PgrLabel = new System.Windows.Forms.Label();
            this.LbResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnCrawling
            // 
            this.BtnCrawling.BackColor = System.Drawing.Color.Green;
            this.BtnCrawling.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnCrawling.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.BtnCrawling.Location = new System.Drawing.Point(413, 377);
            this.BtnCrawling.Name = "BtnCrawling";
            this.BtnCrawling.Size = new System.Drawing.Size(174, 59);
            this.BtnCrawling.TabIndex = 2;
            this.BtnCrawling.Text = "Start Crawling";
            this.BtnCrawling.UseVisualStyleBackColor = false;
            this.BtnCrawling.Click += new System.EventHandler(this.StartCrawling);
            // 
            // BtnUpdatePrices
            // 
            this.BtnUpdatePrices.BackColor = System.Drawing.Color.Green;
            this.BtnUpdatePrices.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnUpdatePrices.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.BtnUpdatePrices.Location = new System.Drawing.Point(593, 377);
            this.BtnUpdatePrices.Name = "BtnUpdatePrices";
            this.BtnUpdatePrices.Size = new System.Drawing.Size(181, 59);
            this.BtnUpdatePrices.TabIndex = 2;
            this.BtnUpdatePrices.Text = "Update Prices";
            this.BtnUpdatePrices.UseVisualStyleBackColor = false;
            this.BtnUpdatePrices.Click += new System.EventHandler(this.UpdateProductPrices);
            // 
            // PgrUpdatePrices
            // 
            this.PgrUpdatePrices.Location = new System.Drawing.Point(30, 156);
            this.PgrUpdatePrices.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PgrUpdatePrices.Name = "PgrUpdatePrices";
            this.PgrUpdatePrices.Size = new System.Drawing.Size(745, 45);
            this.PgrUpdatePrices.TabIndex = 3;
            this.PgrUpdatePrices.Visible = false;
            // 
            // PgrLabel
            // 
            this.PgrLabel.AutoSize = true;
            this.PgrLabel.Location = new System.Drawing.Point(30, 120);
            this.PgrLabel.Name = "PgrLabel";
            this.PgrLabel.Size = new System.Drawing.Size(147, 20);
            this.PgrLabel.TabIndex = 4;
            this.PgrLabel.Text = "Start updating prices";
            this.PgrLabel.Visible = false;
            // 
            // LbResult
            // 
            this.LbResult.AutoSize = true;
            this.LbResult.Location = new System.Drawing.Point(280, 224);
            this.LbResult.Name = "LbResult";
            this.LbResult.Size = new System.Drawing.Size(0, 20);
            this.LbResult.TabIndex = 5;
            this.LbResult.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 451);
            this.Controls.Add(this.LbResult);
            this.Controls.Add(this.PgrLabel);
            this.Controls.Add(this.PgrUpdatePrices);
            this.Controls.Add(this.BtnUpdatePrices);
            this.Controls.Add(this.BtnCrawling);
            this.Name = "MainForm";
            this.Text = "Amazon Crawler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnCrawling;
        private System.Windows.Forms.Button BtnUpdatePrices;
        private System.Windows.Forms.ProgressBar PgrUpdatePrices;
        private System.Windows.Forms.Label PgrLabel;
        private System.Windows.Forms.Label LbResult;
    }
}

