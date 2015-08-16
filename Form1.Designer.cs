namespace MVcamview
{
    partial class Form1
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
            this.StartButton = new System.Windows.Forms.Button();
            this.CaptureCamera1 = new System.Windows.Forms.Button();
            this.AutoExposure1 = new System.Windows.Forms.CheckBox();
            this.lOpacityImage1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Horizontal1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Vertical1 = new System.Windows.Forms.CheckBox();
            this.WhiteBalancePush1 = new System.Windows.Forms.Button();
            this.Capture3 = new System.Windows.Forms.Button();
            this.ResoulationList1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.WhiteBalancePush2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectImage1OnTop = new System.Windows.Forms.RadioButton();
            this.ShowRightLine = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ResoulationList2 = new System.Windows.Forms.ComboBox();
            this.lOpacityImage2 = new System.Windows.Forms.Label();
            this.AutoExposure2 = new System.Windows.Forms.CheckBox();
            this.SelectImage2OnTop = new System.Windows.Forms.RadioButton();
            this.CaptureCamera2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Horizontal2 = new System.Windows.Forms.CheckBox();
            this.Vertical2 = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.ShowLeftLine = new System.Windows.Forms.CheckBox();
            this.LeftLineValue = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.RightLineValue = new System.Windows.Forms.NumericUpDown();
            this.ExpoValue1 = new System.Windows.Forms.NumericUpDown();
            this.ExpoValue2 = new System.Windows.Forms.NumericUpDown();
            this.TempValue1 = new System.Windows.Forms.NumericUpDown();
            this.TintValue1 = new System.Windows.Forms.NumericUpDown();
            this.TempValue2 = new System.Windows.Forms.NumericUpDown();
            this.TintValue2 = new System.Windows.Forms.NumericUpDown();
            this.OpacityImage1 = new System.Windows.Forms.NumericUpDown();
            this.OpacityImage2 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftLineValue)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RightLineValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpoValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpoValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TintValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TintValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityImage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityImage2)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StartButton.Location = new System.Drawing.Point(3, 551);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(69, 19);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.OnStart);
            // 
            // CaptureCamera1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CaptureCamera1, 2);
            this.CaptureCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptureCamera1.Location = new System.Drawing.Point(3, 519);
            this.CaptureCamera1.Name = "CaptureCamera1";
            this.CaptureCamera1.Size = new System.Drawing.Size(144, 26);
            this.CaptureCamera1.TabIndex = 1;
            this.CaptureCamera1.Text = "Capture";
            this.CaptureCamera1.UseVisualStyleBackColor = true;
            this.CaptureCamera1.Click += new System.EventHandler(this.OnSnap);
            // 
            // AutoExposure1
            // 
            this.AutoExposure1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.AutoExposure1, 2);
            this.AutoExposure1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoExposure1.Location = new System.Drawing.Point(3, 144);
            this.AutoExposure1.Name = "AutoExposure1";
            this.AutoExposure1.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.AutoExposure1.Size = new System.Drawing.Size(144, 27);
            this.AutoExposure1.TabIndex = 4;
            this.AutoExposure1.Text = "Auto Exposure";
            this.AutoExposure1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AutoExposure1.UseVisualStyleBackColor = true;
            this.AutoExposure1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lOpacityImage1
            // 
            this.lOpacityImage1.AutoSize = true;
            this.lOpacityImage1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lOpacityImage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOpacityImage1.Location = new System.Drawing.Point(3, 500);
            this.lOpacityImage1.Margin = new System.Windows.Forms.Padding(3);
            this.lOpacityImage1.Name = "lOpacityImage1";
            this.lOpacityImage1.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lOpacityImage1.Size = new System.Drawing.Size(69, 13);
            this.lOpacityImage1.TabIndex = 9;
            this.lOpacityImage1.Text = "Opacity";
            this.lOpacityImage1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 337);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 144);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label9, 2);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 270);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(144, 32);
            this.label9.TabIndex = 9;
            this.label9.Text = "Flip";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 254);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Tint";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Horizontal1
            // 
            this.Horizontal1.AutoSize = true;
            this.Horizontal1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Horizontal1.Location = new System.Drawing.Point(0, 0);
            this.Horizontal1.Name = "Horizontal1";
            this.Horizontal1.Size = new System.Drawing.Size(73, 26);
            this.Horizontal1.TabIndex = 26;
            this.Horizontal1.Text = "Horizontal";
            this.Horizontal1.UseVisualStyleBackColor = true;
            this.Horizontal1.CheckedChanged += new System.EventHandler(this.FlipCamera1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 222);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Temp";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Vertical1
            // 
            this.Vertical1.AutoSize = true;
            this.Vertical1.Dock = System.Windows.Forms.DockStyle.Right;
            this.Vertical1.Location = new System.Drawing.Point(83, 0);
            this.Vertical1.Name = "Vertical1";
            this.Vertical1.Size = new System.Drawing.Size(61, 26);
            this.Vertical1.TabIndex = 26;
            this.Vertical1.Text = "Vertical";
            this.Vertical1.UseVisualStyleBackColor = true;
            this.Vertical1.CheckedChanged += new System.EventHandler(this.FlipCamera1_CheckedChanged);
            // 
            // WhiteBalancePush1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.WhiteBalancePush1, 2);
            this.WhiteBalancePush1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WhiteBalancePush1.Location = new System.Drawing.Point(3, 177);
            this.WhiteBalancePush1.Name = "WhiteBalancePush1";
            this.WhiteBalancePush1.Size = new System.Drawing.Size(144, 26);
            this.WhiteBalancePush1.TabIndex = 8;
            this.WhiteBalancePush1.Text = "White Balance One Push";
            this.WhiteBalancePush1.UseVisualStyleBackColor = true;
            this.WhiteBalancePush1.Click += new System.EventHandler(this.OnWhiteBalanceOnePush1);
            // 
            // Capture3
            // 
            this.Capture3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Capture3.Location = new System.Drawing.Point(419, 548);
            this.Capture3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Capture3.Name = "Capture3";
            this.Capture3.Size = new System.Drawing.Size(127, 22);
            this.Capture3.TabIndex = 23;
            this.Capture3.Text = "Capture";
            this.Capture3.UseVisualStyleBackColor = false;
            this.Capture3.Click += new System.EventHandler(this.OnSnap3);
            // 
            // ResoulationList1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ResoulationList1, 2);
            this.ResoulationList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResoulationList1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResoulationList1.FormattingEnabled = true;
            this.ResoulationList1.Location = new System.Drawing.Point(3, 67);
            this.ResoulationList1.Name = "ResoulationList1";
            this.ResoulationList1.Size = new System.Drawing.Size(144, 21);
            this.ResoulationList1.TabIndex = 28;
            this.ResoulationList1.SelectedIndexChanged += new System.EventHandler(this.OnSelectResolution1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(818, 125);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Expo";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WhiteBalancePush2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.WhiteBalancePush2, 2);
            this.WhiteBalancePush2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WhiteBalancePush2.Location = new System.Drawing.Point(818, 177);
            this.WhiteBalancePush2.Name = "WhiteBalancePush2";
            this.WhiteBalancePush2.Size = new System.Drawing.Size(149, 26);
            this.WhiteBalancePush2.TabIndex = 8;
            this.WhiteBalancePush2.Text = "White Balance One Push";
            this.WhiteBalancePush2.UseVisualStyleBackColor = true;
            this.WhiteBalancePush2.Click += new System.EventHandler(this.OnWhiteBalanceOnePush2);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 125);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Expo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelectImage1OnTop
            // 
            this.SelectImage1OnTop.Checked = true;
            this.tableLayoutPanel1.SetColumnSpan(this.SelectImage1OnTop, 2);
            this.SelectImage1OnTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectImage1OnTop.Location = new System.Drawing.Point(2, 34);
            this.SelectImage1OnTop.Margin = new System.Windows.Forms.Padding(2);
            this.SelectImage1OnTop.Name = "SelectImage1OnTop";
            this.SelectImage1OnTop.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.SelectImage1OnTop.Size = new System.Drawing.Size(146, 28);
            this.SelectImage1OnTop.TabIndex = 27;
            this.SelectImage1OnTop.TabStop = true;
            this.SelectImage1OnTop.Text = "Top";
            this.SelectImage1OnTop.UseVisualStyleBackColor = true;
            this.SelectImage1OnTop.Click += new System.EventHandler(this.SelectImage1OnTop_CheckedChanged);
            // 
            // ShowRightLine
            // 
            this.ShowRightLine.AutoSize = true;
            this.ShowRightLine.Location = new System.Drawing.Point(3, 0);
            this.ShowRightLine.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ShowRightLine.Name = "ShowRightLine";
            this.ShowRightLine.Size = new System.Drawing.Size(74, 17);
            this.ShowRightLine.TabIndex = 27;
            this.ShowRightLine.Text = "Right Line";
            this.ShowRightLine.UseVisualStyleBackColor = true;
            this.ShowRightLine.CheckedChanged += new System.EventHandler(this.cbRightLIne_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(818, 222);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Temp";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label8, 2);
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(818, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 32);
            this.label8.TabIndex = 25;
            this.label8.Text = "Camera 2";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(818, 254);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Tint";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResoulationList2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ResoulationList2, 2);
            this.ResoulationList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResoulationList2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResoulationList2.FormattingEnabled = true;
            this.ResoulationList2.Location = new System.Drawing.Point(818, 67);
            this.ResoulationList2.Name = "ResoulationList2";
            this.ResoulationList2.Size = new System.Drawing.Size(149, 21);
            this.ResoulationList2.TabIndex = 21;
            this.ResoulationList2.SelectedIndexChanged += new System.EventHandler(this.OnSelectResolution2);
            // 
            // lOpacityImage2
            // 
            this.lOpacityImage2.AutoSize = true;
            this.lOpacityImage2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lOpacityImage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOpacityImage2.Location = new System.Drawing.Point(818, 500);
            this.lOpacityImage2.Margin = new System.Windows.Forms.Padding(3);
            this.lOpacityImage2.Name = "lOpacityImage2";
            this.lOpacityImage2.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lOpacityImage2.Size = new System.Drawing.Size(69, 13);
            this.lOpacityImage2.TabIndex = 18;
            this.lOpacityImage2.Text = "Opacity";
            this.lOpacityImage2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AutoExposure2
            // 
            this.AutoExposure2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.AutoExposure2, 2);
            this.AutoExposure2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoExposure2.Location = new System.Drawing.Point(818, 144);
            this.AutoExposure2.Name = "AutoExposure2";
            this.AutoExposure2.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.AutoExposure2.Size = new System.Drawing.Size(149, 27);
            this.AutoExposure2.TabIndex = 22;
            this.AutoExposure2.Text = "Auto Exposure";
            this.AutoExposure2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AutoExposure2.UseVisualStyleBackColor = true;
            this.AutoExposure2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // SelectImage2OnTop
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.SelectImage2OnTop, 2);
            this.SelectImage2OnTop.Location = new System.Drawing.Point(817, 34);
            this.SelectImage2OnTop.Margin = new System.Windows.Forms.Padding(2);
            this.SelectImage2OnTop.Name = "SelectImage2OnTop";
            this.SelectImage2OnTop.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            this.SelectImage2OnTop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SelectImage2OnTop.Size = new System.Drawing.Size(150, 26);
            this.SelectImage2OnTop.TabIndex = 1;
            this.SelectImage2OnTop.Text = "Top";
            this.SelectImage2OnTop.UseVisualStyleBackColor = true;
            this.SelectImage2OnTop.Click += new System.EventHandler(this.SelectImage2OnTop_CheckedChanged);
            // 
            // CaptureCamera2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CaptureCamera2, 2);
            this.CaptureCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptureCamera2.Location = new System.Drawing.Point(818, 519);
            this.CaptureCamera2.Name = "CaptureCamera2";
            this.CaptureCamera2.Size = new System.Drawing.Size(149, 26);
            this.CaptureCamera2.TabIndex = 23;
            this.CaptureCamera2.Text = "Capture";
            this.CaptureCamera2.UseVisualStyleBackColor = true;
            this.CaptureCamera2.Click += new System.EventHandler(this.OnSnap2);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label7, 2);
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 32);
            this.label7.TabIndex = 26;
            this.label7.Text = "Camera 1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 7, 10);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lOpacityImage2, 7, 11);
            this.tableLayoutPanel1.Controls.Add(this.CaptureCamera2, 7, 12);
            this.tableLayoutPanel1.Controls.Add(this.ResoulationList2, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 7, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 7, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.WhiteBalancePush2, 7, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 7, 3);
            this.tableLayoutPanel1.Controls.Add(this.ResoulationList1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Capture3, 4, 13);
            this.tableLayoutPanel1.Controls.Add(this.WhiteBalancePush1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lOpacityImage1, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.CaptureCamera1, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.StartButton, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 7, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 7, 9);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 13);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 5, 13);
            this.tableLayoutPanel1.Controls.Add(this.SelectImage1OnTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SelectImage2OnTop, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExpoValue1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.AutoExposure1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.AutoExposure2, 7, 4);
            this.tableLayoutPanel1.Controls.Add(this.ExpoValue2, 8, 3);
            this.tableLayoutPanel1.Controls.Add(this.TempValue1, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.TintValue1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.TempValue2, 8, 6);
            this.tableLayoutPanel1.Controls.Add(this.TintValue2, 8, 7);
            this.tableLayoutPanel1.Controls.Add(this.OpacityImage1, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.OpacityImage2, 8, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.67115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.85958F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.678754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.681736F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(970, 573);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox2, 2);
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(818, 337);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(149, 144);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 31;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox3, 5);
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(153, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox3, 13);
            this.pictureBox3.Size = new System.Drawing.Size(659, 542);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label10, 2);
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(818, 270);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 32);
            this.label10.TabIndex = 33;
            this.label10.Text = "Flip";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.Horizontal2);
            this.panel2.Controls.Add(this.Vertical2);
            this.panel2.Location = new System.Drawing.Point(818, 305);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 26);
            this.panel2.TabIndex = 36;
            // 
            // Horizontal2
            // 
            this.Horizontal2.AutoSize = true;
            this.Horizontal2.Dock = System.Windows.Forms.DockStyle.Left;
            this.Horizontal2.Location = new System.Drawing.Point(0, 0);
            this.Horizontal2.Name = "Horizontal2";
            this.Horizontal2.Size = new System.Drawing.Size(73, 26);
            this.Horizontal2.TabIndex = 34;
            this.Horizontal2.Text = "Horizontal";
            this.Horizontal2.UseVisualStyleBackColor = true;
            this.Horizontal2.CheckedChanged += new System.EventHandler(this.FlipCamera2_CheckedChanged);
            // 
            // Vertical2
            // 
            this.Vertical2.AutoSize = true;
            this.Vertical2.Dock = System.Windows.Forms.DockStyle.Right;
            this.Vertical2.Location = new System.Drawing.Point(88, 0);
            this.Vertical2.Name = "Vertical2";
            this.Vertical2.Size = new System.Drawing.Size(61, 26);
            this.Vertical2.TabIndex = 35;
            this.Vertical2.Text = "Vertical";
            this.Vertical2.UseVisualStyleBackColor = true;
            this.Vertical2.CheckedChanged += new System.EventHandler(this.FlipCamera2_CheckedChanged);
            // 
            // panel3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel3, 2);
            this.panel3.Controls.Add(this.Horizontal1);
            this.panel3.Controls.Add(this.Vertical1);
            this.panel3.Location = new System.Drawing.Point(3, 305);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(144, 26);
            this.panel3.TabIndex = 37;
            // 
            // flowLayoutPanel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 2);
            this.flowLayoutPanel2.Controls.Add(this.ShowLeftLine);
            this.flowLayoutPanel2.Controls.Add(this.LeftLineValue);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(153, 551);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(260, 19);
            this.flowLayoutPanel2.TabIndex = 39;
            // 
            // ShowLeftLine
            // 
            this.ShowLeftLine.AutoSize = true;
            this.ShowLeftLine.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ShowLeftLine.Location = new System.Drawing.Point(190, 0);
            this.ShowLeftLine.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ShowLeftLine.Name = "ShowLeftLine";
            this.ShowLeftLine.Size = new System.Drawing.Size(67, 17);
            this.ShowLeftLine.TabIndex = 0;
            this.ShowLeftLine.Text = "Left Line";
            this.ShowLeftLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ShowLeftLine.UseVisualStyleBackColor = true;
            this.ShowLeftLine.CheckedChanged += new System.EventHandler(this.ckbLeftLine_CheckedChanged);
            // 
            // LeftLineValue
            // 
            this.LeftLineValue.Location = new System.Drawing.Point(124, 0);
            this.LeftLineValue.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.LeftLineValue.Name = "LeftLineValue";
            this.LeftLineValue.Size = new System.Drawing.Size(60, 20);
            this.LeftLineValue.TabIndex = 1;
            this.LeftLineValue.ValueChanged += new System.EventHandler(this.ckbLeftLine_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.ShowRightLine);
            this.flowLayoutPanel1.Controls.Add(this.RightLineValue);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(552, 551);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(260, 19);
            this.flowLayoutPanel1.TabIndex = 38;
            // 
            // RightLineValue
            // 
            this.RightLineValue.Location = new System.Drawing.Point(83, 0);
            this.RightLineValue.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.RightLineValue.Name = "RightLineValue";
            this.RightLineValue.Size = new System.Drawing.Size(60, 20);
            this.RightLineValue.TabIndex = 28;
            this.RightLineValue.ValueChanged += new System.EventHandler(this.cbRightLIne_CheckedChanged);
            // 
            // ExpoValue1
            // 
            this.ExpoValue1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExpoValue1.Location = new System.Drawing.Point(78, 118);
            this.ExpoValue1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ExpoValue1.Name = "ExpoValue1";
            this.ExpoValue1.Size = new System.Drawing.Size(69, 20);
            this.ExpoValue1.TabIndex = 40;
            this.ExpoValue1.ValueChanged += new System.EventHandler(this.OnExpoValueChange);
            // 
            // ExpoValue2
            // 
            this.ExpoValue2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExpoValue2.Location = new System.Drawing.Point(893, 118);
            this.ExpoValue2.Name = "ExpoValue2";
            this.ExpoValue2.Size = new System.Drawing.Size(74, 20);
            this.ExpoValue2.TabIndex = 41;
            this.ExpoValue2.ValueChanged += new System.EventHandler(this.OnExpoValueChange2);
            // 
            // TempValue1
            // 
            this.TempValue1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TempValue1.Location = new System.Drawing.Point(78, 215);
            this.TempValue1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TempValue1.Name = "TempValue1";
            this.TempValue1.Size = new System.Drawing.Size(69, 20);
            this.TempValue1.TabIndex = 42;
            this.TempValue1.ValueChanged += new System.EventHandler(this.OnTempTintChanged1);
            // 
            // TintValue1
            // 
            this.TintValue1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TintValue1.Location = new System.Drawing.Point(78, 247);
            this.TintValue1.Name = "TintValue1";
            this.TintValue1.Size = new System.Drawing.Size(69, 20);
            this.TintValue1.TabIndex = 43;
            this.TintValue1.ValueChanged += new System.EventHandler(this.OnTempTintChanged1);
            // 
            // TempValue2
            // 
            this.TempValue2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TempValue2.Location = new System.Drawing.Point(893, 215);
            this.TempValue2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TempValue2.Name = "TempValue2";
            this.TempValue2.Size = new System.Drawing.Size(74, 20);
            this.TempValue2.TabIndex = 44;
            this.TempValue2.ValueChanged += new System.EventHandler(this.OnTempTintChanged2);
            // 
            // TintValue2
            // 
            this.TintValue2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TintValue2.Location = new System.Drawing.Point(893, 247);
            this.TintValue2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TintValue2.Name = "TintValue2";
            this.TintValue2.Size = new System.Drawing.Size(74, 20);
            this.TintValue2.TabIndex = 44;
            this.TintValue2.ValueChanged += new System.EventHandler(this.OnTempTintChanged2);
            // 
            // OpacityImage1
            // 
            this.OpacityImage1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OpacityImage1.Location = new System.Drawing.Point(78, 493);
            this.OpacityImage1.Name = "OpacityImage1";
            this.OpacityImage1.Size = new System.Drawing.Size(69, 20);
            this.OpacityImage1.TabIndex = 45;
            this.OpacityImage1.ValueChanged += new System.EventHandler(this.tbOpacityImage1_Scroll);
            // 
            // OpacityImage2
            // 
            this.OpacityImage2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OpacityImage2.Location = new System.Drawing.Point(893, 493);
            this.OpacityImage2.Name = "OpacityImage2";
            this.OpacityImage2.Size = new System.Drawing.Size(74, 20);
            this.OpacityImage2.TabIndex = 46;
            this.OpacityImage2.ValueChanged += new System.EventHandler(this.tbOpacityImage2_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 573);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(638, 510);
            this.Name = "Form1";
            this.Text = "MVcamview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftLineValue)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RightLineValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpoValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpoValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TintValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TintValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityImage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityImage2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button CaptureCamera1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lOpacityImage2;
        private System.Windows.Forms.Button CaptureCamera2;
        private System.Windows.Forms.RadioButton SelectImage2OnTop;
        private System.Windows.Forms.CheckBox AutoExposure2;
        private System.Windows.Forms.ComboBox ResoulationList2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ShowRightLine;
        private System.Windows.Forms.RadioButton SelectImage1OnTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WhiteBalancePush2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ResoulationList1;
        private System.Windows.Forms.Button Capture3;
        private System.Windows.Forms.Button WhiteBalancePush1;
        private System.Windows.Forms.CheckBox Vertical1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox Horizontal1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lOpacityImage1;
        private System.Windows.Forms.CheckBox AutoExposure1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox Horizontal2;
        private System.Windows.Forms.CheckBox Vertical2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox ShowLeftLine;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown ExpoValue1;
        private System.Windows.Forms.NumericUpDown ExpoValue2;
        private System.Windows.Forms.NumericUpDown TempValue1;
        private System.Windows.Forms.NumericUpDown TintValue1;
        private System.Windows.Forms.NumericUpDown TempValue2;
        private System.Windows.Forms.NumericUpDown TintValue2;
        private System.Windows.Forms.NumericUpDown LeftLineValue;
        private System.Windows.Forms.NumericUpDown RightLineValue;
        private System.Windows.Forms.NumericUpDown OpacityImage1;
        private System.Windows.Forms.NumericUpDown OpacityImage2;
    }
}

