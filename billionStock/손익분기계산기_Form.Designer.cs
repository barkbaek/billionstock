namespace billionStock
{
    partial class 손익분기계산기_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(손익분기계산기_Form));
            this.데이터입력_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.손익분기가격_label = new System.Windows.Forms.Label();
            this.호가단위_label = new System.Windows.Forms.Label();
            this.손익분기호가_label = new System.Windows.Forms.Label();
            this.손익분기가결과_label = new System.Windows.Forms.Label();
            this.호가단위결과_label = new System.Windows.Forms.Label();
            this.손익분기호가결과_label = new System.Windows.Forms.Label();
            this.수량_label = new System.Windows.Forms.Label();
            this.수량결과_label = new System.Windows.Forms.Label();
            this.손절_dataGridView = new System.Windows.Forms.DataGridView();
            this.손절률_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.손절틱간격_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.손절호가_DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.익절_dataGridView = new System.Windows.Forms.DataGridView();
            this.익절률_dataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.익절틱간격_dataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.익절호가_dataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.매수금액_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.계산_button = new System.Windows.Forms.Button();
            this.시장_comboBox = new System.Windows.Forms.ComboBox();
            this.시장_label = new System.Windows.Forms.Label();
            this.체결가_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.매수금액_label = new System.Windows.Forms.Label();
            this.데이터입력_tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.손절_dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.익절_dataGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.매수금액_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.체결가_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // 데이터입력_tableLayoutPanel
            // 
            this.데이터입력_tableLayoutPanel.ColumnCount = 2;
            this.데이터입력_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.데이터입력_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.데이터입력_tableLayoutPanel.Controls.Add(this.손익분기가격_label, 0, 1);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.호가단위_label, 0, 2);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.손익분기호가_label, 0, 3);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.손익분기가결과_label, 1, 1);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.호가단위결과_label, 1, 2);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.손익분기호가결과_label, 1, 3);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.수량_label, 0, 0);
            this.데이터입력_tableLayoutPanel.Controls.Add(this.수량결과_label, 1, 0);
            this.데이터입력_tableLayoutPanel.Location = new System.Drawing.Point(410, 12);
            this.데이터입력_tableLayoutPanel.Name = "데이터입력_tableLayoutPanel";
            this.데이터입력_tableLayoutPanel.RowCount = 6;
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.데이터입력_tableLayoutPanel.Size = new System.Drawing.Size(241, 150);
            this.데이터입력_tableLayoutPanel.TabIndex = 0;
            // 
            // 손익분기가격_label
            // 
            this.손익분기가격_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.손익분기가격_label.AutoSize = true;
            this.손익분기가격_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.손익분기가격_label.Location = new System.Drawing.Point(3, 24);
            this.손익분기가격_label.Name = "손익분기가격_label";
            this.손익분기가격_label.Size = new System.Drawing.Size(114, 24);
            this.손익분기가격_label.TabIndex = 4;
            this.손익분기가격_label.Text = "손익분기가";
            this.손익분기가격_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 호가단위_label
            // 
            this.호가단위_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.호가단위_label.AutoSize = true;
            this.호가단위_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.호가단위_label.Location = new System.Drawing.Point(3, 48);
            this.호가단위_label.Name = "호가단위_label";
            this.호가단위_label.Size = new System.Drawing.Size(114, 24);
            this.호가단위_label.TabIndex = 3;
            this.호가단위_label.Text = "호가단위";
            this.호가단위_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 손익분기호가_label
            // 
            this.손익분기호가_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.손익분기호가_label.AutoSize = true;
            this.손익분기호가_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.손익분기호가_label.Location = new System.Drawing.Point(3, 72);
            this.손익분기호가_label.Name = "손익분기호가_label";
            this.손익분기호가_label.Size = new System.Drawing.Size(114, 24);
            this.손익분기호가_label.TabIndex = 32;
            this.손익분기호가_label.Text = "손익분기호가";
            this.손익분기호가_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 손익분기가결과_label
            // 
            this.손익분기가결과_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.손익분기가결과_label.AutoSize = true;
            this.손익분기가결과_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.손익분기가결과_label.Location = new System.Drawing.Point(123, 24);
            this.손익분기가결과_label.Name = "손익분기가결과_label";
            this.손익분기가결과_label.Size = new System.Drawing.Size(115, 24);
            this.손익분기가결과_label.TabIndex = 30;
            this.손익분기가결과_label.Text = "0";
            this.손익분기가결과_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 호가단위결과_label
            // 
            this.호가단위결과_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.호가단위결과_label.AutoSize = true;
            this.호가단위결과_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.호가단위결과_label.Location = new System.Drawing.Point(123, 48);
            this.호가단위결과_label.Name = "호가단위결과_label";
            this.호가단위결과_label.Size = new System.Drawing.Size(115, 24);
            this.호가단위결과_label.TabIndex = 31;
            this.호가단위결과_label.Text = "0";
            this.호가단위결과_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 손익분기호가결과_label
            // 
            this.손익분기호가결과_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.손익분기호가결과_label.AutoSize = true;
            this.손익분기호가결과_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.손익분기호가결과_label.Location = new System.Drawing.Point(123, 72);
            this.손익분기호가결과_label.Name = "손익분기호가결과_label";
            this.손익분기호가결과_label.Size = new System.Drawing.Size(115, 24);
            this.손익분기호가결과_label.TabIndex = 33;
            this.손익분기호가결과_label.Text = "0";
            this.손익분기호가결과_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 수량_label
            // 
            this.수량_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.수량_label.AutoSize = true;
            this.수량_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.수량_label.Location = new System.Drawing.Point(3, 0);
            this.수량_label.Name = "수량_label";
            this.수량_label.Size = new System.Drawing.Size(114, 24);
            this.수량_label.TabIndex = 34;
            this.수량_label.Text = "수량";
            this.수량_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 수량결과_label
            // 
            this.수량결과_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.수량결과_label.AutoSize = true;
            this.수량결과_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.수량결과_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.수량결과_label.Location = new System.Drawing.Point(123, 0);
            this.수량결과_label.Name = "수량결과_label";
            this.수량결과_label.Size = new System.Drawing.Size(115, 24);
            this.수량결과_label.TabIndex = 35;
            this.수량결과_label.Text = "0";
            this.수량결과_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.수량결과_label.Click += new System.EventHandler(this.수량결과_label_Click);
            // 
            // 손절_dataGridView
            // 
            this.손절_dataGridView.AllowUserToAddRows = false;
            this.손절_dataGridView.AllowUserToDeleteRows = false;
            this.손절_dataGridView.AllowUserToResizeRows = false;
            this.손절_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.손절_dataGridView.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.손절_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.손절_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.손절_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.손절률_DataGridViewTextBoxColumn,
            this.손절틱간격_DataGridViewTextBoxColumn,
            this.손절호가_DataGridViewTextBoxColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.손절_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.손절_dataGridView.Location = new System.Drawing.Point(9, 162);
            this.손절_dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.손절_dataGridView.Name = "손절_dataGridView";
            this.손절_dataGridView.ReadOnly = true;
            this.손절_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.손절_dataGridView.RowHeadersVisible = false;
            this.손절_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.손절_dataGridView.RowTemplate.Height = 23;
            this.손절_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.손절_dataGridView.Size = new System.Drawing.Size(325, 737);
            this.손절_dataGridView.TabIndex = 8;
            this.손절_dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.손절_dataGridView_CellClick);
            // 
            // 손절률_DataGridViewTextBoxColumn
            // 
            this.손절률_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.손절률_DataGridViewTextBoxColumn.HeaderText = "손절률";
            this.손절률_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.손절률_DataGridViewTextBoxColumn.Name = "손절률_DataGridViewTextBoxColumn";
            this.손절률_DataGridViewTextBoxColumn.ReadOnly = true;
            this.손절률_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 손절틱간격_DataGridViewTextBoxColumn
            // 
            this.손절틱간격_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.손절틱간격_DataGridViewTextBoxColumn.HeaderText = "손절틱간격";
            this.손절틱간격_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.손절틱간격_DataGridViewTextBoxColumn.Name = "손절틱간격_DataGridViewTextBoxColumn";
            this.손절틱간격_DataGridViewTextBoxColumn.ReadOnly = true;
            this.손절틱간격_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 손절호가_DataGridViewTextBoxColumn
            // 
            this.손절호가_DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.손절호가_DataGridViewTextBoxColumn.HeaderText = "손절호가";
            this.손절호가_DataGridViewTextBoxColumn.MinimumWidth = 100;
            this.손절호가_DataGridViewTextBoxColumn.Name = "손절호가_DataGridViewTextBoxColumn";
            this.손절호가_DataGridViewTextBoxColumn.ReadOnly = true;
            this.손절호가_DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 익절_dataGridView
            // 
            this.익절_dataGridView.AllowUserToAddRows = false;
            this.익절_dataGridView.AllowUserToDeleteRows = false;
            this.익절_dataGridView.AllowUserToResizeRows = false;
            this.익절_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.익절_dataGridView.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.익절_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.익절_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.익절_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.익절률_dataGridViewTextBoxColumn,
            this.익절틱간격_dataGridViewTextBoxColumn,
            this.익절호가_dataGridViewTextBoxColumn});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkViolet;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.익절_dataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            this.익절_dataGridView.Location = new System.Drawing.Point(350, 162);
            this.익절_dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.익절_dataGridView.Name = "익절_dataGridView";
            this.익절_dataGridView.ReadOnly = true;
            this.익절_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.익절_dataGridView.RowHeadersVisible = false;
            this.익절_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.익절_dataGridView.RowTemplate.Height = 23;
            this.익절_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.익절_dataGridView.Size = new System.Drawing.Size(325, 737);
            this.익절_dataGridView.TabIndex = 9;
            this.익절_dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.익절_dataGridView_CellClick);
            // 
            // 익절률_dataGridViewTextBoxColumn
            // 
            this.익절률_dataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.익절률_dataGridViewTextBoxColumn.HeaderText = "익절률";
            this.익절률_dataGridViewTextBoxColumn.MinimumWidth = 100;
            this.익절률_dataGridViewTextBoxColumn.Name = "익절률_dataGridViewTextBoxColumn";
            this.익절률_dataGridViewTextBoxColumn.ReadOnly = true;
            this.익절률_dataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 익절틱간격_dataGridViewTextBoxColumn
            // 
            this.익절틱간격_dataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.익절틱간격_dataGridViewTextBoxColumn.HeaderText = "익절틱간격";
            this.익절틱간격_dataGridViewTextBoxColumn.MinimumWidth = 100;
            this.익절틱간격_dataGridViewTextBoxColumn.Name = "익절틱간격_dataGridViewTextBoxColumn";
            this.익절틱간격_dataGridViewTextBoxColumn.ReadOnly = true;
            this.익절틱간격_dataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 익절호가_dataGridViewTextBoxColumn
            // 
            this.익절호가_dataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.익절호가_dataGridViewTextBoxColumn.HeaderText = "익절호가";
            this.익절호가_dataGridViewTextBoxColumn.MinimumWidth = 100;
            this.익절호가_dataGridViewTextBoxColumn.Name = "익절호가_dataGridViewTextBoxColumn";
            this.익절호가_dataGridViewTextBoxColumn.ReadOnly = true;
            this.익절호가_dataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.매수금액_numericUpDown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.계산_button, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.시장_comboBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.시장_label, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.체결가_numericUpDown, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.매수금액_label, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 15);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(241, 150);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // 매수금액_numericUpDown
            // 
            this.매수금액_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.매수금액_numericUpDown.Location = new System.Drawing.Point(123, 3);
            this.매수금액_numericUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.매수금액_numericUpDown.Name = "매수금액_numericUpDown";
            this.매수금액_numericUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.매수금액_numericUpDown.Size = new System.Drawing.Size(115, 21);
            this.매수금액_numericUpDown.TabIndex = 22;
            this.매수금액_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.매수금액_numericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // 계산_button
            // 
            this.계산_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.계산_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.계산_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.계산_button.ForeColor = System.Drawing.Color.Gold;
            this.계산_button.Location = new System.Drawing.Point(123, 72);
            this.계산_button.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.계산_button.Name = "계산_button";
            this.계산_button.Size = new System.Drawing.Size(115, 24);
            this.계산_button.TabIndex = 9;
            this.계산_button.Text = "계산";
            this.계산_button.UseVisualStyleBackColor = true;
            this.계산_button.Click += new System.EventHandler(this.계산_button_Click);
            // 
            // 시장_comboBox
            // 
            this.시장_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.시장_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.시장_comboBox.FormattingEnabled = true;
            this.시장_comboBox.Items.AddRange(new object[] {
            "코스피",
            "코스닥"});
            this.시장_comboBox.Location = new System.Drawing.Point(123, 51);
            this.시장_comboBox.Name = "시장_comboBox";
            this.시장_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.시장_comboBox.Size = new System.Drawing.Size(115, 20);
            this.시장_comboBox.TabIndex = 29;
            // 
            // 시장_label
            // 
            this.시장_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.시장_label.AutoSize = true;
            this.시장_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.시장_label.Location = new System.Drawing.Point(3, 48);
            this.시장_label.Name = "시장_label";
            this.시장_label.Size = new System.Drawing.Size(114, 24);
            this.시장_label.TabIndex = 1;
            this.시장_label.Text = "시장";
            this.시장_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 체결가_numericUpDown
            // 
            this.체결가_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.체결가_numericUpDown.Location = new System.Drawing.Point(123, 27);
            this.체결가_numericUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.체결가_numericUpDown.Name = "체결가_numericUpDown";
            this.체결가_numericUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.체결가_numericUpDown.Size = new System.Drawing.Size(115, 21);
            this.체결가_numericUpDown.TabIndex = 21;
            this.체결가_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.체결가_numericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.체결가_numericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.체결가_numericUpDown_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 24);
            this.label1.TabIndex = 30;
            this.label1.Text = "체결가";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 매수금액_label
            // 
            this.매수금액_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.매수금액_label.AutoSize = true;
            this.매수금액_label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.매수금액_label.Location = new System.Drawing.Point(3, 0);
            this.매수금액_label.Name = "매수금액_label";
            this.매수금액_label.Size = new System.Drawing.Size(114, 24);
            this.매수금액_label.TabIndex = 0;
            this.매수금액_label.Text = "매수금액(만원)";
            this.매수금액_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 손익분기계산기_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(684, 911);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.익절_dataGridView);
            this.Controls.Add(this.손절_dataGridView);
            this.Controls.Add(this.데이터입력_tableLayoutPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "손익분기계산기_Form";
            this.Text = "손익분기계산기";
            this.Load += new System.EventHandler(this.손익분기계산기_Form_Load);
            this.데이터입력_tableLayoutPanel.ResumeLayout(false);
            this.데이터입력_tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.손절_dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.익절_dataGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.매수금액_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.체결가_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel 데이터입력_tableLayoutPanel;
        private System.Windows.Forms.Label 손익분기가격_label;
        private System.Windows.Forms.Label 호가단위_label;
        private System.Windows.Forms.Label 손익분기가결과_label;
        private System.Windows.Forms.Label 호가단위결과_label;
        private System.Windows.Forms.DataGridView 손절_dataGridView;
        private System.Windows.Forms.DataGridView 익절_dataGridView;
        private System.Windows.Forms.Label 손익분기호가_label;
        private System.Windows.Forms.Label 손익분기호가결과_label;
        private System.Windows.Forms.DataGridViewTextBoxColumn 손절률_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 손절틱간격_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 손절호가_DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 익절률_dataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 익절틱간격_dataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 익절호가_dataGridViewTextBoxColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button 계산_button;
        private System.Windows.Forms.ComboBox 시장_comboBox;
        private System.Windows.Forms.Label 시장_label;
        private System.Windows.Forms.Label 매수금액_label;
        private System.Windows.Forms.NumericUpDown 체결가_numericUpDown;
        private System.Windows.Forms.Label 수량_label;
        private System.Windows.Forms.Label 수량결과_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown 매수금액_numericUpDown;
    }
}