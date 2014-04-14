namespace Calculon.Networking.UdpListener
{
    partial class Listener
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblPerson = new System.Windows.Forms.Label();
            this.lblServerLink = new System.Windows.Forms.LinkLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Currently at the door:";
            // 
            // lblPerson
            // 
            this.lblPerson.AutoSize = true;
            this.lblPerson.Location = new System.Drawing.Point(25, 42);
            this.lblPerson.Name = "lblPerson";
            this.lblPerson.Size = new System.Drawing.Size(0, 13);
            this.lblPerson.TabIndex = 1;
            // 
            // lblServerLink
            // 
            this.lblServerLink.AutoSize = true;
            this.lblServerLink.Location = new System.Drawing.Point(25, 110);
            this.lblServerLink.Name = "lblServerLink";
            this.lblServerLink.Size = new System.Drawing.Size(0, 13);
            this.lblServerLink.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Listener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 147);
            this.Controls.Add(this.lblServerLink);
            this.Controls.Add(this.lblPerson);
            this.Controls.Add(this.label1);
            this.Name = "Listener";
            this.Text = "Messages From Calculon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPerson;
        private System.Windows.Forms.LinkLabel lblServerLink;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

