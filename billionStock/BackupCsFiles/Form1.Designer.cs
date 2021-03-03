namespace billionStock
{
    partial class BS_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BS_Main));
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.BS_ShowBox = new System.Windows.Forms.TextBox();
            this.BS_Menu = new System.Windows.Forms.MenuStrip();
            this.BS_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.BS_MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.로그인ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuLoginInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.BS_MenuChart = new System.Windows.Forms.ToolStripMenuItem();
            this.BS_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.BS_ProgressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.BS_BtnUpdateStocks = new System.Windows.Forms.Button();
            this.BS_ClockTimer = new System.Windows.Forms.Timer(this.components);
            this.BS_Clock = new System.Windows.Forms.Label();
            this.BS_BtnSearchStock = new System.Windows.Forms.Button();
            this.BS_InputSearchStock = new System.Windows.Forms.TextBox();
            this.BS_InputPanelSearchStock = new System.Windows.Forms.Panel();
            this.BS_ShowBoxPanel = new System.Windows.Forms.Panel();
            this.BS_LoginState = new System.Windows.Forms.Label();
            this.BS_LoginStatePanel = new System.Windows.Forms.Panel();
            this.BS_UpdateStock_BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.BS_Menu.SuspendLayout();
            this.BS_InputPanelSearchStock.SuspendLayout();
            this.BS_ShowBoxPanel.SuspendLayout();
            this.BS_LoginStatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(5, 603);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(1, 1);
            this.axKHOpenAPI1.TabIndex = 5;
            // 
            // BS_ShowBox
            // 
            this.BS_ShowBox.BackColor = System.Drawing.Color.Black;
            this.BS_ShowBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BS_ShowBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_ShowBox.ForeColor = System.Drawing.Color.Chartreuse;
            this.BS_ShowBox.Location = new System.Drawing.Point(6, 9);
            this.BS_ShowBox.Multiline = true;
            this.BS_ShowBox.Name = "BS_ShowBox";
            this.BS_ShowBox.ReadOnly = true;
            this.BS_ShowBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BS_ShowBox.Size = new System.Drawing.Size(434, 467);
            this.BS_ShowBox.TabIndex = 2;
            // 
            // BS_Menu
            // 
            this.BS_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BS_MenuFile,
            this.MenuLogin,
            this.BS_MenuChart});
            this.BS_Menu.Location = new System.Drawing.Point(0, 0);
            this.BS_Menu.Name = "BS_Menu";
            this.BS_Menu.Size = new System.Drawing.Size(1904, 24);
            this.BS_Menu.TabIndex = 5;
            this.BS_Menu.Text = "menuStrip1";
            // 
            // BS_MenuFile
            // 
            this.BS_MenuFile.BackColor = System.Drawing.SystemColors.Control;
            this.BS_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BS_MenuExit});
            this.BS_MenuFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BS_MenuFile.Name = "BS_MenuFile";
            this.BS_MenuFile.Size = new System.Drawing.Size(43, 20);
            this.BS_MenuFile.Text = "파일";
            // 
            // BS_MenuExit
            // 
            this.BS_MenuExit.Name = "BS_MenuExit";
            this.BS_MenuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.BS_MenuExit.Size = new System.Drawing.Size(180, 22);
            this.BS_MenuExit.Text = "종료";
            this.BS_MenuExit.Click += new System.EventHandler(this.MenuExit_OnClick);
            // 
            // MenuLogin
            // 
            this.MenuLogin.BackColor = System.Drawing.SystemColors.Control;
            this.MenuLogin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.로그인ToolStripMenuItem,
            this.MenuLoginInfo});
            this.MenuLogin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MenuLogin.Name = "MenuLogin";
            this.MenuLogin.Size = new System.Drawing.Size(55, 20);
            this.MenuLogin.Text = "사용자";
            // 
            // 로그인ToolStripMenuItem
            // 
            this.로그인ToolStripMenuItem.Name = "로그인ToolStripMenuItem";
            this.로그인ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.로그인ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.로그인ToolStripMenuItem.Text = "로그인";
            this.로그인ToolStripMenuItem.Click += new System.EventHandler(this.MenuLogin_OnClick);
            // 
            // MenuLoginInfo
            // 
            this.MenuLoginInfo.Name = "MenuLoginInfo";
            this.MenuLoginInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.MenuLoginInfo.Size = new System.Drawing.Size(180, 22);
            this.MenuLoginInfo.Text = "로그인정보";
            this.MenuLoginInfo.Click += new System.EventHandler(this.MenuLoginInfo_OnClick);
            // 
            // BS_MenuChart
            // 
            this.BS_MenuChart.BackColor = System.Drawing.SystemColors.Control;
            this.BS_MenuChart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BS_MenuChart.Name = "BS_MenuChart";
            this.BS_MenuChart.Size = new System.Drawing.Size(43, 20);
            this.BS_MenuChart.Text = "차트";
            // 
            // BS_ProgressBar
            // 
            this.BS_ProgressBar.BackColor = System.Drawing.Color.Black;
            this.BS_ProgressBar.ForeColor = System.Drawing.Color.Chartreuse;
            this.BS_ProgressBar.Location = new System.Drawing.Point(1583, 943);
            this.BS_ProgressBar.Name = "BS_ProgressBar";
            this.BS_ProgressBar.Size = new System.Drawing.Size(321, 33);
            this.BS_ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.BS_ProgressBar.TabIndex = 7;
            this.BS_ProgressBar.Value = 100;
            // 
            // BS_ProgressBarTimer
            // 
            this.BS_ProgressBarTimer.Interval = 10;
            this.BS_ProgressBarTimer.Tick += new System.EventHandler(this.BS_ProgressBarTimer_OnTick);
            // 
            // BS_BtnUpdateStocks
            // 
            this.BS_BtnUpdateStocks.BackColor = System.Drawing.Color.Black;
            this.BS_BtnUpdateStocks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BS_BtnUpdateStocks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BS_BtnUpdateStocks.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_BtnUpdateStocks.ForeColor = System.Drawing.Color.Gold;
            this.BS_BtnUpdateStocks.Location = new System.Drawing.Point(1774, 24);
            this.BS_BtnUpdateStocks.Name = "BS_BtnUpdateStocks";
            this.BS_BtnUpdateStocks.Size = new System.Drawing.Size(130, 33);
            this.BS_BtnUpdateStocks.TabIndex = 8;
            this.BS_BtnUpdateStocks.Text = "종목 다운로드";
            this.BS_BtnUpdateStocks.UseVisualStyleBackColor = false;
            this.BS_BtnUpdateStocks.Click += new System.EventHandler(this.BS_BtnUpdateStocks_OnClick);
            // 
            // BS_ClockTimer
            // 
            this.BS_ClockTimer.Enabled = true;
            this.BS_ClockTimer.Interval = 1000;
            this.BS_ClockTimer.Tick += new System.EventHandler(this.BS_ClockTimer_OnTick);
            // 
            // BS_Clock
            // 
            this.BS_Clock.AutoSize = true;
            this.BS_Clock.BackColor = System.Drawing.Color.White;
            this.BS_Clock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_Clock.ForeColor = System.Drawing.Color.Black;
            this.BS_Clock.Location = new System.Drawing.Point(1658, 6);
            this.BS_Clock.Name = "BS_Clock";
            this.BS_Clock.Size = new System.Drawing.Size(242, 12);
            this.BS_Clock.TabIndex = 9;
            this.BS_Clock.Text = "0000년 00월 00일 일요일 오전 00:00:00";
            // 
            // BS_BtnSearchStock
            // 
            this.BS_BtnSearchStock.BackColor = System.Drawing.Color.Black;
            this.BS_BtnSearchStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BS_BtnSearchStock.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BS_BtnSearchStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BS_BtnSearchStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_BtnSearchStock.ForeColor = System.Drawing.Color.Gold;
            this.BS_BtnSearchStock.Location = new System.Drawing.Point(1795, 426);
            this.BS_BtnSearchStock.Name = "BS_BtnSearchStock";
            this.BS_BtnSearchStock.Size = new System.Drawing.Size(109, 33);
            this.BS_BtnSearchStock.TabIndex = 10;
            this.BS_BtnSearchStock.Text = "종목 조회";
            this.BS_BtnSearchStock.UseVisualStyleBackColor = false;
            // 
            // BS_InputSearchStock
            // 
            this.BS_InputSearchStock.BackColor = System.Drawing.Color.Black;
            this.BS_InputSearchStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BS_InputSearchStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_InputSearchStock.ForeColor = System.Drawing.Color.Chartreuse;
            this.BS_InputSearchStock.Location = new System.Drawing.Point(8, 8);
            this.BS_InputSearchStock.Name = "BS_InputSearchStock";
            this.BS_InputSearchStock.Size = new System.Drawing.Size(318, 14);
            this.BS_InputSearchStock.TabIndex = 11;
            this.BS_InputSearchStock.Text = "종목을 입력하세요";
            // 
            // BS_InputPanelSearchStock
            // 
            this.BS_InputPanelSearchStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BS_InputPanelSearchStock.Controls.Add(this.BS_InputSearchStock);
            this.BS_InputPanelSearchStock.Location = new System.Drawing.Point(1458, 426);
            this.BS_InputPanelSearchStock.Name = "BS_InputPanelSearchStock";
            this.BS_InputPanelSearchStock.Size = new System.Drawing.Size(337, 33);
            this.BS_InputPanelSearchStock.TabIndex = 12;
            this.BS_InputPanelSearchStock.Enter += new System.EventHandler(this.BS_InputPanelSearchStock_OnEnter);
            this.BS_InputPanelSearchStock.Leave += new System.EventHandler(this.BS_InputPanelSearchStock_OnLeave);
            // 
            // BS_ShowBoxPanel
            // 
            this.BS_ShowBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BS_ShowBoxPanel.Controls.Add(this.BS_ShowBox);
            this.BS_ShowBoxPanel.Location = new System.Drawing.Point(1458, 459);
            this.BS_ShowBoxPanel.Name = "BS_ShowBoxPanel";
            this.BS_ShowBoxPanel.Size = new System.Drawing.Size(446, 485);
            this.BS_ShowBoxPanel.TabIndex = 13;
            // 
            // BS_LoginState
            // 
            this.BS_LoginState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BS_LoginState.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BS_LoginState.Location = new System.Drawing.Point(2, 1);
            this.BS_LoginState.Name = "BS_LoginState";
            this.BS_LoginState.Size = new System.Drawing.Size(120, 30);
            this.BS_LoginState.TabIndex = 14;
            this.BS_LoginState.Text = "로그인하세요";
            this.BS_LoginState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BS_LoginState.TextChanged += new System.EventHandler(this.BS_LoginState_OnTextChanged);
            this.BS_LoginState.Click += new System.EventHandler(this.BS_LoginState_OnClick);
            // 
            // BS_LoginStatePanel
            // 
            this.BS_LoginStatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BS_LoginStatePanel.Controls.Add(this.BS_LoginState);
            this.BS_LoginStatePanel.Location = new System.Drawing.Point(1458, 943);
            this.BS_LoginStatePanel.Name = "BS_LoginStatePanel";
            this.BS_LoginStatePanel.Size = new System.Drawing.Size(125, 33);
            this.BS_LoginStatePanel.TabIndex = 15;
            // 
            // BS_UpdateStock_BackgroundWorker
            // 
            this.BS_UpdateStock_BackgroundWorker.WorkerReportsProgress = true;
            this.BS_UpdateStock_BackgroundWorker.WorkerSupportsCancellation = true;
            this.BS_UpdateStock_BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BS_UpdateStock_BackgroundWorker_OnDoWork);
            this.BS_UpdateStock_BackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BS_UpdateStock_BackgroundWorker_OnProgressChanged);
            this.BS_UpdateStock_BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BS_UpdateStock_BackgroundWorker_OnCompleted);
            // 
            // BS_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1904, 1011);
            this.Controls.Add(this.BS_LoginStatePanel);
            this.Controls.Add(this.BS_ProgressBar);
            this.Controls.Add(this.BS_BtnUpdateStocks);
            this.Controls.Add(this.BS_ShowBoxPanel);
            this.Controls.Add(this.BS_InputPanelSearchStock);
            this.Controls.Add(this.BS_BtnSearchStock);
            this.Controls.Add(this.BS_Clock);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Controls.Add(this.BS_Menu);
            this.ForeColor = System.Drawing.Color.Chartreuse;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.BS_Menu;
            this.MaximizeBox = false;
            this.Name = "BS_Main";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "빌리언스탁 - 자동매매의 황금 손 (Alpha Version)";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BS_Main_OnKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.BS_Menu.ResumeLayout(false);
            this.BS_Menu.PerformLayout();
            this.BS_InputPanelSearchStock.ResumeLayout(false);
            this.BS_InputPanelSearchStock.PerformLayout();
            this.BS_ShowBoxPanel.ResumeLayout(false);
            this.BS_ShowBoxPanel.PerformLayout();
            this.BS_LoginStatePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.TextBox BS_ShowBox;
        private System.Windows.Forms.MenuStrip BS_Menu;
        private System.Windows.Forms.ToolStripMenuItem BS_MenuFile;
        private System.Windows.Forms.ToolStripMenuItem BS_MenuExit;
        private System.Windows.Forms.ToolStripMenuItem BS_MenuChart;
        private System.Windows.Forms.ProgressBar BS_ProgressBar;
        private System.Windows.Forms.Timer BS_ProgressBarTimer;
        private System.Windows.Forms.Button BS_BtnUpdateStocks;
        private System.Windows.Forms.Timer BS_ClockTimer;
        private System.Windows.Forms.Label BS_Clock;
        private System.Windows.Forms.Button BS_BtnSearchStock;
        private System.Windows.Forms.TextBox BS_InputSearchStock;
        private System.Windows.Forms.Panel BS_InputPanelSearchStock;
        private System.Windows.Forms.Panel BS_ShowBoxPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuLogin;
        private System.Windows.Forms.ToolStripMenuItem 로그인ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuLoginInfo;
        private System.Windows.Forms.Label BS_LoginState;
        private System.Windows.Forms.Panel BS_LoginStatePanel;
        private System.ComponentModel.BackgroundWorker BS_UpdateStock_BackgroundWorker;
    }
}
