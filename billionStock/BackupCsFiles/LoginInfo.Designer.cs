namespace billionStock
{
    partial class LoginInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginInfo));
            this.labelUserId = new System.Windows.Forms.Label();
            this.userId = new System.Windows.Forms.TextBox();
            this.userName = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.accountList = new System.Windows.Forms.TextBox();
            this.labelAccountList = new System.Windows.Forms.Label();
            this.keyboardSecurity = new System.Windows.Forms.TextBox();
            this.labelKeyboardSecurity = new System.Windows.Forms.Label();
            this.firewall = new System.Windows.Forms.TextBox();
            this.labelFirewall = new System.Windows.Forms.Label();
            this.server = new System.Windows.Forms.TextBox();
            this.labelServer = new System.Windows.Forms.Label();
            this.userIdPanel = new System.Windows.Forms.Panel();
            this.userNamePanel = new System.Windows.Forms.Panel();
            this.serverPanel = new System.Windows.Forms.Panel();
            this.accountListPanel = new System.Windows.Forms.Panel();
            this.firewallPanel = new System.Windows.Forms.Panel();
            this.keyboardSecurityPanel = new System.Windows.Forms.Panel();
            this.userIdPanel.SuspendLayout();
            this.userNamePanel.SuspendLayout();
            this.serverPanel.SuspendLayout();
            this.accountListPanel.SuspendLayout();
            this.firewallPanel.SuspendLayout();
            this.keyboardSecurityPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelUserId
            // 
            this.labelUserId.AutoSize = true;
            this.labelUserId.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelUserId.ForeColor = System.Drawing.Color.Gold;
            this.labelUserId.Location = new System.Drawing.Point(12, 38);
            this.labelUserId.Name = "labelUserId";
            this.labelUserId.Size = new System.Drawing.Size(18, 12);
            this.labelUserId.TabIndex = 0;
            this.labelUserId.Text = "ID";
            // 
            // userId
            // 
            this.userId.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userId.BackColor = System.Drawing.Color.Black;
            this.userId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userId.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.userId.ForeColor = System.Drawing.Color.Chartreuse;
            this.userId.Location = new System.Drawing.Point(8, 10);
            this.userId.Name = "userId";
            this.userId.ReadOnly = true;
            this.userId.Size = new System.Drawing.Size(165, 14);
            this.userId.TabIndex = 0;
            // 
            // userName
            // 
            this.userName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userName.BackColor = System.Drawing.Color.Black;
            this.userName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userName.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.userName.ForeColor = System.Drawing.Color.Chartreuse;
            this.userName.Location = new System.Drawing.Point(8, 10);
            this.userName.Name = "userName";
            this.userName.ReadOnly = true;
            this.userName.Size = new System.Drawing.Size(165, 14);
            this.userName.TabIndex = 0;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelUserName.ForeColor = System.Drawing.Color.Gold;
            this.labelUserName.Location = new System.Drawing.Point(12, 89);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(31, 12);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "이름";
            // 
            // accountList
            // 
            this.accountList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountList.BackColor = System.Drawing.Color.Black;
            this.accountList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.accountList.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.accountList.ForeColor = System.Drawing.Color.Chartreuse;
            this.accountList.Location = new System.Drawing.Point(8, 8);
            this.accountList.Multiline = true;
            this.accountList.Name = "accountList";
            this.accountList.ReadOnly = true;
            this.accountList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.accountList.Size = new System.Drawing.Size(165, 113);
            this.accountList.TabIndex = 0;
            // 
            // labelAccountList
            // 
            this.labelAccountList.AutoSize = true;
            this.labelAccountList.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAccountList.ForeColor = System.Drawing.Color.Gold;
            this.labelAccountList.Location = new System.Drawing.Point(12, 138);
            this.labelAccountList.Name = "labelAccountList";
            this.labelAccountList.Size = new System.Drawing.Size(57, 12);
            this.labelAccountList.TabIndex = 4;
            this.labelAccountList.Text = "보유계좌";
            // 
            // keyboardSecurity
            // 
            this.keyboardSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboardSecurity.BackColor = System.Drawing.Color.Black;
            this.keyboardSecurity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.keyboardSecurity.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.keyboardSecurity.ForeColor = System.Drawing.Color.Chartreuse;
            this.keyboardSecurity.Location = new System.Drawing.Point(7, 9);
            this.keyboardSecurity.Name = "keyboardSecurity";
            this.keyboardSecurity.ReadOnly = true;
            this.keyboardSecurity.Size = new System.Drawing.Size(165, 14);
            this.keyboardSecurity.TabIndex = 0;
            // 
            // labelKeyboardSecurity
            // 
            this.labelKeyboardSecurity.AutoSize = true;
            this.labelKeyboardSecurity.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelKeyboardSecurity.ForeColor = System.Drawing.Color.Gold;
            this.labelKeyboardSecurity.Location = new System.Drawing.Point(12, 286);
            this.labelKeyboardSecurity.Name = "labelKeyboardSecurity";
            this.labelKeyboardSecurity.Size = new System.Drawing.Size(75, 12);
            this.labelKeyboardSecurity.TabIndex = 6;
            this.labelKeyboardSecurity.Text = "키보드 보안";
            // 
            // firewall
            // 
            this.firewall.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firewall.BackColor = System.Drawing.Color.Black;
            this.firewall.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.firewall.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.firewall.ForeColor = System.Drawing.Color.Chartreuse;
            this.firewall.Location = new System.Drawing.Point(7, 9);
            this.firewall.Name = "firewall";
            this.firewall.ReadOnly = true;
            this.firewall.Size = new System.Drawing.Size(165, 14);
            this.firewall.TabIndex = 0;
            // 
            // labelFirewall
            // 
            this.labelFirewall.AutoSize = true;
            this.labelFirewall.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFirewall.ForeColor = System.Drawing.Color.Gold;
            this.labelFirewall.Location = new System.Drawing.Point(12, 336);
            this.labelFirewall.Name = "labelFirewall";
            this.labelFirewall.Size = new System.Drawing.Size(44, 12);
            this.labelFirewall.TabIndex = 8;
            this.labelFirewall.Text = "방화벽";
            // 
            // server
            // 
            this.server.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.server.BackColor = System.Drawing.Color.Black;
            this.server.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.server.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.server.ForeColor = System.Drawing.Color.Chartreuse;
            this.server.Location = new System.Drawing.Point(7, 9);
            this.server.Name = "server";
            this.server.ReadOnly = true;
            this.server.Size = new System.Drawing.Size(165, 14);
            this.server.TabIndex = 0;
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelServer.ForeColor = System.Drawing.Color.Gold;
            this.labelServer.Location = new System.Drawing.Point(12, 384);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(31, 12);
            this.labelServer.TabIndex = 10;
            this.labelServer.Text = "서버";
            // 
            // userIdPanel
            // 
            this.userIdPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userIdPanel.Controls.Add(this.userId);
            this.userIdPanel.Location = new System.Drawing.Point(135, 28);
            this.userIdPanel.Name = "userIdPanel";
            this.userIdPanel.Size = new System.Drawing.Size(181, 35);
            this.userIdPanel.TabIndex = 1;
            // 
            // userNamePanel
            // 
            this.userNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userNamePanel.Controls.Add(this.userName);
            this.userNamePanel.Location = new System.Drawing.Point(135, 79);
            this.userNamePanel.Name = "userNamePanel";
            this.userNamePanel.Size = new System.Drawing.Size(181, 35);
            this.userNamePanel.TabIndex = 3;
            // 
            // serverPanel
            // 
            this.serverPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverPanel.Controls.Add(this.server);
            this.serverPanel.Location = new System.Drawing.Point(135, 374);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(181, 35);
            this.serverPanel.TabIndex = 11;
            // 
            // accountListPanel
            // 
            this.accountListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accountListPanel.Controls.Add(this.accountList);
            this.accountListPanel.Location = new System.Drawing.Point(135, 129);
            this.accountListPanel.Name = "accountListPanel";
            this.accountListPanel.Size = new System.Drawing.Size(181, 129);
            this.accountListPanel.TabIndex = 5;
            // 
            // firewallPanel
            // 
            this.firewallPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.firewallPanel.Controls.Add(this.firewall);
            this.firewallPanel.Location = new System.Drawing.Point(135, 326);
            this.firewallPanel.Name = "firewallPanel";
            this.firewallPanel.Size = new System.Drawing.Size(181, 35);
            this.firewallPanel.TabIndex = 9;
            // 
            // keyboardSecurityPanel
            // 
            this.keyboardSecurityPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.keyboardSecurityPanel.Controls.Add(this.keyboardSecurity);
            this.keyboardSecurityPanel.Location = new System.Drawing.Point(135, 276);
            this.keyboardSecurityPanel.Name = "keyboardSecurityPanel";
            this.keyboardSecurityPanel.Size = new System.Drawing.Size(181, 35);
            this.keyboardSecurityPanel.TabIndex = 7;
            // 
            // LoginInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(328, 430);
            this.Controls.Add(this.keyboardSecurityPanel);
            this.Controls.Add(this.serverPanel);
            this.Controls.Add(this.userNamePanel);
            this.Controls.Add(this.firewallPanel);
            this.Controls.Add(this.accountListPanel);
            this.Controls.Add(this.userIdPanel);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.labelFirewall);
            this.Controls.Add(this.labelKeyboardSecurity);
            this.Controls.Add(this.labelAccountList);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.labelUserId);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginInfo";
            this.Opacity = 0.97D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "로그인 정보";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoginInfo_Load);
            this.userIdPanel.ResumeLayout(false);
            this.userIdPanel.PerformLayout();
            this.userNamePanel.ResumeLayout(false);
            this.userNamePanel.PerformLayout();
            this.serverPanel.ResumeLayout(false);
            this.serverPanel.PerformLayout();
            this.accountListPanel.ResumeLayout(false);
            this.accountListPanel.PerformLayout();
            this.firewallPanel.ResumeLayout(false);
            this.firewallPanel.PerformLayout();
            this.keyboardSecurityPanel.ResumeLayout(false);
            this.keyboardSecurityPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.TextBox userId;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox accountList;
        private System.Windows.Forms.Label labelAccountList;
        private System.Windows.Forms.TextBox keyboardSecurity;
        private System.Windows.Forms.Label labelKeyboardSecurity;
        private System.Windows.Forms.TextBox firewall;
        private System.Windows.Forms.Label labelFirewall;
        private System.Windows.Forms.TextBox server;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Panel userIdPanel;
        private System.Windows.Forms.Panel userNamePanel;
        private System.Windows.Forms.Panel serverPanel;
        private System.Windows.Forms.Panel accountListPanel;
        private System.Windows.Forms.Panel firewallPanel;
        private System.Windows.Forms.Panel keyboardSecurityPanel;
    }
}