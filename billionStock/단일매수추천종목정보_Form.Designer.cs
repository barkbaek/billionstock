namespace billionStock
{
    partial class 단일매수추천종목정보_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(단일매수추천종목정보_Form));
            this.종목정보_label = new System.Windows.Forms.Label();
            this.단일매수추천종목정보_dataGridView = new System.Windows.Forms.DataGridView();
            this.순서_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.시간_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.등락율_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.돌파틱개수_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결개수_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결강도_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도호가_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매수호가_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.현재가_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도거래대금_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매수거래대금_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.세력순수익률_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.누적거래량_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.누적거래대금_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.새로고침_button = new System.Windows.Forms.Button();
            this.시장가매매_checkBox = new System.Windows.Forms.CheckBox();
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            ((System.ComponentModel.ISupportInitialize)(this.단일매수추천종목정보_dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // 종목정보_label
            // 
            this.종목정보_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.종목정보_label.AutoSize = true;
            this.종목정보_label.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.종목정보_label.Location = new System.Drawing.Point(12, 9);
            this.종목정보_label.Name = "종목정보_label";
            this.종목정보_label.Size = new System.Drawing.Size(132, 15);
            this.종목정보_label.TabIndex = 6;
            this.종목정보_label.Text = "종목을 선택하세요";
            this.종목정보_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 단일매수추천종목정보_dataGridView
            // 
            this.단일매수추천종목정보_dataGridView.AllowUserToAddRows = false;
            this.단일매수추천종목정보_dataGridView.AllowUserToDeleteRows = false;
            this.단일매수추천종목정보_dataGridView.AllowUserToResizeRows = false;
            this.단일매수추천종목정보_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.단일매수추천종목정보_dataGridView.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.단일매수추천종목정보_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.단일매수추천종목정보_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.단일매수추천종목정보_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.순서_DataGridViewTextBoxColumn,
            this.시간_DataGridViewTextBoxColumn,
            this.등락율_DataGridViewTextBoxColumn,
            this.돌파틱개수_DataGridViewTextBoxColumn,
            this.체결개수_DataGridViewTextBoxColumn,
            this.체결강도_DataGridViewTextBoxColumn,
            this.매도호가_DataGridViewTextBoxColumn,
            this.매수호가_DataGridViewTextBoxColumn,
            this.현재가_DataGridViewTextBoxColumn,
            this.매도거래대금_DataGridViewTextBoxColumn,
            this.매수거래대금_DataGridViewTextBoxColumn,
            this.세력순수익률_DataGridViewTextBoxColumn,
            this.누적거래량_DataGridViewTextBoxColumn,
            this.누적거래대금_DataGridViewTextBoxColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.단일매수추천종목정보_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.단일매수추천종목정보_dataGridView.Location = new System.Drawing.Point(9, 34);
            this.단일매수추천종목정보_dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.단일매수추천종목정보_dataGridView.Name = "단일매수추천종목정보_dataGridView";
            this.단일매수추천종목정보_dataGridView.ReadOnly = true;
            this.단일매수추천종목정보_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.단일매수추천종목정보_dataGridView.RowHeadersVisible = false;
            this.단일매수추천종목정보_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.단일매수추천종목정보_dataGridView.RowTemplate.Height = 23;
            this.단일매수추천종목정보_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.단일매수추천종목정보_dataGridView.Size = new System.Drawing.Size(1446, 544);
            this.단일매수추천종목정보_dataGridView.TabIndex = 7;
            // 
            // 순서_DataGridViewTextBoxColumn
            // 
            this.순서_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.순서_DataGridViewTextBoxColumn.HeaderText = "순서";
            this.순서_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.순서_DataGridViewTextBoxColumn.Name = "순서_DataGridViewTextBoxColumn";
            this.순서_DataGridViewTextBoxColumn.ReadOnly = true;
            this.순서_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 시간_DataGridViewTextBoxColumn
            // 
            this.시간_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.시간_DataGridViewTextBoxColumn.HeaderText = "시간";
            this.시간_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.시간_DataGridViewTextBoxColumn.Name = "시간_DataGridViewTextBoxColumn";
            this.시간_DataGridViewTextBoxColumn.ReadOnly = true;
            this.시간_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 등락율_DataGridViewTextBoxColumn
            // 
            this.등락율_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.등락율_DataGridViewTextBoxColumn.HeaderText = "등락율";
            this.등락율_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.등락율_DataGridViewTextBoxColumn.Name = "등락율_DataGridViewTextBoxColumn";
            this.등락율_DataGridViewTextBoxColumn.ReadOnly = true;
            this.등락율_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 돌파틱개수_DataGridViewTextBoxColumn
            // 
            this.돌파틱개수_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.돌파틱개수_DataGridViewTextBoxColumn.HeaderText = "돌파틱개수";
            this.돌파틱개수_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.돌파틱개수_DataGridViewTextBoxColumn.Name = "돌파틱개수_DataGridViewTextBoxColumn";
            this.돌파틱개수_DataGridViewTextBoxColumn.ReadOnly = true;
            this.돌파틱개수_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 체결개수_DataGridViewTextBoxColumn
            // 
            this.체결개수_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.체결개수_DataGridViewTextBoxColumn.HeaderText = "체결개수";
            this.체결개수_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.체결개수_DataGridViewTextBoxColumn.Name = "체결개수_DataGridViewTextBoxColumn";
            this.체결개수_DataGridViewTextBoxColumn.ReadOnly = true;
            this.체결개수_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 체결강도_DataGridViewTextBoxColumn
            // 
            this.체결강도_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.체결강도_DataGridViewTextBoxColumn.HeaderText = "체결강도";
            this.체결강도_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.체결강도_DataGridViewTextBoxColumn.Name = "체결강도_DataGridViewTextBoxColumn";
            this.체결강도_DataGridViewTextBoxColumn.ReadOnly = true;
            this.체결강도_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매도호가_DataGridViewTextBoxColumn
            // 
            this.매도호가_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.매도호가_DataGridViewTextBoxColumn.HeaderText = "매도호가";
            this.매도호가_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.매도호가_DataGridViewTextBoxColumn.Name = "매도호가_DataGridViewTextBoxColumn";
            this.매도호가_DataGridViewTextBoxColumn.ReadOnly = true;
            this.매도호가_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매수호가_DataGridViewTextBoxColumn
            // 
            this.매수호가_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.매수호가_DataGridViewTextBoxColumn.HeaderText = "매수호가";
            this.매수호가_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.매수호가_DataGridViewTextBoxColumn.Name = "매수호가_DataGridViewTextBoxColumn";
            this.매수호가_DataGridViewTextBoxColumn.ReadOnly = true;
            this.매수호가_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 현재가_DataGridViewTextBoxColumn
            // 
            this.현재가_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.현재가_DataGridViewTextBoxColumn.HeaderText = "체결가";
            this.현재가_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.현재가_DataGridViewTextBoxColumn.Name = "현재가_DataGridViewTextBoxColumn";
            this.현재가_DataGridViewTextBoxColumn.ReadOnly = true;
            this.현재가_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매도거래대금_DataGridViewTextBoxColumn
            // 
            this.매도거래대금_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.매도거래대금_DataGridViewTextBoxColumn.HeaderText = "매도거래대금";
            this.매도거래대금_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.매도거래대금_DataGridViewTextBoxColumn.Name = "매도거래대금_DataGridViewTextBoxColumn";
            this.매도거래대금_DataGridViewTextBoxColumn.ReadOnly = true;
            this.매도거래대금_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매수거래대금_DataGridViewTextBoxColumn
            // 
            this.매수거래대금_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.매수거래대금_DataGridViewTextBoxColumn.HeaderText = "매수거래대금";
            this.매수거래대금_DataGridViewTextBoxColumn.MinimumWidth = 120;
            this.매수거래대금_DataGridViewTextBoxColumn.Name = "매수거래대금_DataGridViewTextBoxColumn";
            this.매수거래대금_DataGridViewTextBoxColumn.ReadOnly = true;
            this.매수거래대금_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.매수거래대금_DataGridViewTextBoxColumn.Width = 120;
            // 
            // 세력순수익률_DataGridViewTextBoxColumn
            // 
            this.세력순수익률_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.세력순수익률_DataGridViewTextBoxColumn.HeaderText = "세력순수익률";
            this.세력순수익률_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.세력순수익률_DataGridViewTextBoxColumn.Name = "세력순수익률_DataGridViewTextBoxColumn";
            this.세력순수익률_DataGridViewTextBoxColumn.ReadOnly = true;
            this.세력순수익률_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 누적거래량_DataGridViewTextBoxColumn
            // 
            this.누적거래량_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.누적거래량_DataGridViewTextBoxColumn.HeaderText = "누적거래량";
            this.누적거래량_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.누적거래량_DataGridViewTextBoxColumn.Name = "누적거래량_DataGridViewTextBoxColumn";
            this.누적거래량_DataGridViewTextBoxColumn.ReadOnly = true;
            this.누적거래량_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 누적거래대금_DataGridViewTextBoxColumn
            // 
            this.누적거래대금_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.누적거래대금_DataGridViewTextBoxColumn.HeaderText = "누적거래대금";
            this.누적거래대금_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.누적거래대금_DataGridViewTextBoxColumn.Name = "누적거래대금_DataGridViewTextBoxColumn";
            this.누적거래대금_DataGridViewTextBoxColumn.ReadOnly = true;
            this.누적거래대금_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 새로고침_button
            // 
            this.새로고침_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.새로고침_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.새로고침_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.새로고침_button.ForeColor = System.Drawing.Color.Gold;
            this.새로고침_button.Location = new System.Drawing.Point(1335, 5);
            this.새로고침_button.Margin = new System.Windows.Forms.Padding(0);
            this.새로고침_button.Name = "새로고침_button";
            this.새로고침_button.Size = new System.Drawing.Size(96, 26);
            this.새로고침_button.TabIndex = 8;
            this.새로고침_button.Text = "새로고침";
            this.새로고침_button.UseVisualStyleBackColor = true;
            this.새로고침_button.Click += new System.EventHandler(this.새로고침_button_Click);
            // 
            // 시장가매매_checkBox
            // 
            this.시장가매매_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.시장가매매_checkBox.AutoSize = true;
            this.시장가매매_checkBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.시장가매매_checkBox.Checked = true;
            this.시장가매매_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.시장가매매_checkBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.시장가매매_checkBox.Location = new System.Drawing.Point(1234, 9);
            this.시장가매매_checkBox.Name = "시장가매매_checkBox";
            this.시장가매매_checkBox.Size = new System.Drawing.Size(84, 16);
            this.시장가매매_checkBox.TabIndex = 34;
            this.시장가매매_checkBox.Text = "시장가매매";
            this.시장가매매_checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.시장가매매_checkBox.UseVisualStyleBackColor = true;
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(1194, 0);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(100, 50);
            this.axKHOpenAPI1.TabIndex = 35;
            // 
            // 단일매수추천종목정보_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1464, 587);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Controls.Add(this.시장가매매_checkBox);
            this.Controls.Add(this.새로고침_button);
            this.Controls.Add(this.단일매수추천종목정보_dataGridView);
            this.Controls.Add(this.종목정보_label);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "단일매수추천종목정보_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "단일매수추천종목정보";
            this.Load += new System.EventHandler(this.단일매수추천종목정보_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.단일매수추천종목정보_dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label 종목정보_label;
        private System.Windows.Forms.DataGridView 단일매수추천종목정보_dataGridView;
        private System.Windows.Forms.Button 새로고침_button;
        private System.Windows.Forms.CheckBox 시장가매매_checkBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn 순서_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 시간_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 등락율_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 돌파틱개수_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결개수_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결강도_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도호가_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매수호가_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 현재가_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도거래대금_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매수거래대금_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 세력순수익률_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 누적거래량_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 누적거래대금_DataGridViewTextBoxColumn;
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
    }
}