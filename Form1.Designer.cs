using System;
using HalconDotNet;
using System.Windows.Forms;

namespace PlaceInspect
{
    partial class Form1 : Form
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
            this.bnEnum = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bnTriggerExec = new System.Windows.Forms.Button();
            this.cbSoftTrigger = new System.Windows.Forms.CheckBox();
            this.bnStopGrab = new System.Windows.Forms.Button();
            this.bnStartGrab = new System.Windows.Forms.Button();
            this.bnTriggerMode = new System.Windows.Forms.RadioButton();
            this.bnContinuesMode = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bnCloseCamera = new System.Windows.Forms.Button();
            this.bnOpen = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbDeviceList = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbTriggerDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bnSetParam = new System.Windows.Forms.Button();
            this.bnGetParam = new System.Windows.Forms.Button();
            this.tbGain = new System.Windows.Forms.TextBox();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bnCloseForm = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCaliRadius = new System.Windows.Forms.TextBox();
            this.cbCaliEnable = new System.Windows.Forms.CheckBox();
            this.bnTriggerCali = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cbSmallblackspot = new System.Windows.Forms.CheckBox();
            this.cbBigblackspot = new System.Windows.Forms.CheckBox();
            this.Result_label = new System.Windows.Forms.Label();
            this.Result_listView = new System.Windows.Forms.ListView();
            this.Number = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bnUpdateParam = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bnEnum
            // 
            this.bnEnum.Location = new System.Drawing.Point(20, 38);
            this.bnEnum.Name = "bnEnum";
            this.bnEnum.Size = new System.Drawing.Size(107, 23);
            this.bnEnum.TabIndex = 0;
            this.bnEnum.Text = "查找设备";
            this.bnEnum.UseVisualStyleBackColor = true;
            this.bnEnum.Click += new System.EventHandler(this.bnEnum_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bnTriggerExec);
            this.groupBox2.Controls.Add(this.cbSoftTrigger);
            this.groupBox2.Controls.Add(this.bnStopGrab);
            this.groupBox2.Controls.Add(this.bnStartGrab);
            this.groupBox2.Controls.Add(this.bnTriggerMode);
            this.groupBox2.Controls.Add(this.bnContinuesMode);
            this.groupBox2.Location = new System.Drawing.Point(170, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(177, 150);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集图像";
            // 
            // bnTriggerExec
            // 
            this.bnTriggerExec.Enabled = false;
            this.bnTriggerExec.Location = new System.Drawing.Point(96, 99);
            this.bnTriggerExec.Name = "bnTriggerExec";
            this.bnTriggerExec.Size = new System.Drawing.Size(75, 23);
            this.bnTriggerExec.TabIndex = 5;
            this.bnTriggerExec.Text = "软触发一次";
            this.bnTriggerExec.UseVisualStyleBackColor = true;
            this.bnTriggerExec.Click += new System.EventHandler(this.bnTriggerExec_Click);
            // 
            // cbSoftTrigger
            // 
            this.cbSoftTrigger.AutoSize = true;
            this.cbSoftTrigger.Enabled = false;
            this.cbSoftTrigger.Location = new System.Drawing.Point(10, 104);
            this.cbSoftTrigger.Name = "cbSoftTrigger";
            this.cbSoftTrigger.Size = new System.Drawing.Size(60, 16);
            this.cbSoftTrigger.TabIndex = 4;
            this.cbSoftTrigger.Text = "软触发";
            this.cbSoftTrigger.UseVisualStyleBackColor = true;
            this.cbSoftTrigger.CheckedChanged += new System.EventHandler(this.cbSoftTrigger_CheckedChanged);
            // 
            // bnStopGrab
            // 
            this.bnStopGrab.Enabled = false;
            this.bnStopGrab.Location = new System.Drawing.Point(96, 53);
            this.bnStopGrab.Name = "bnStopGrab";
            this.bnStopGrab.Size = new System.Drawing.Size(75, 23);
            this.bnStopGrab.TabIndex = 3;
            this.bnStopGrab.Text = "停止采集";
            this.bnStopGrab.UseVisualStyleBackColor = true;
            this.bnStopGrab.Click += new System.EventHandler(this.bnStopGrab_Click);
            // 
            // bnStartGrab
            // 
            this.bnStartGrab.Enabled = false;
            this.bnStartGrab.Location = new System.Drawing.Point(9, 53);
            this.bnStartGrab.Name = "bnStartGrab";
            this.bnStartGrab.Size = new System.Drawing.Size(75, 23);
            this.bnStartGrab.TabIndex = 2;
            this.bnStartGrab.Text = "开始采集";
            this.bnStartGrab.UseVisualStyleBackColor = true;
            this.bnStartGrab.Click += new System.EventHandler(this.bnStartGrab_Click);
            // 
            // bnTriggerMode
            // 
            this.bnTriggerMode.AutoSize = true;
            this.bnTriggerMode.Enabled = false;
            this.bnTriggerMode.Location = new System.Drawing.Point(100, 31);
            this.bnTriggerMode.Name = "bnTriggerMode";
            this.bnTriggerMode.Size = new System.Drawing.Size(71, 16);
            this.bnTriggerMode.TabIndex = 1;
            this.bnTriggerMode.TabStop = true;
            this.bnTriggerMode.Text = "触发模式";
            this.bnTriggerMode.UseVisualStyleBackColor = true;
            this.bnTriggerMode.CheckedChanged += new System.EventHandler(this.bnTriggerMode_CheckedChanged);
            // 
            // bnContinuesMode
            // 
            this.bnContinuesMode.AutoSize = true;
            this.bnContinuesMode.Enabled = false;
            this.bnContinuesMode.Location = new System.Drawing.Point(10, 31);
            this.bnContinuesMode.Name = "bnContinuesMode";
            this.bnContinuesMode.Size = new System.Drawing.Size(71, 16);
            this.bnContinuesMode.TabIndex = 0;
            this.bnContinuesMode.TabStop = true;
            this.bnContinuesMode.Text = "连续模式";
            this.bnContinuesMode.UseVisualStyleBackColor = true;
            this.bnContinuesMode.CheckedChanged += new System.EventHandler(this.bnContinuesMode_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.bnCloseCamera);
            this.groupBox1.Controls.Add(this.bnOpen);
            this.groupBox1.Controls.Add(this.bnEnum);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 150);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "初始化";
            // 
            // bnCloseCamera
            // 
            this.bnCloseCamera.Enabled = false;
            this.bnCloseCamera.Location = new System.Drawing.Point(81, 89);
            this.bnCloseCamera.Name = "bnCloseCamera";
            this.bnCloseCamera.Size = new System.Drawing.Size(65, 23);
            this.bnCloseCamera.TabIndex = 2;
            this.bnCloseCamera.Text = "关闭设备";
            this.bnCloseCamera.UseVisualStyleBackColor = true;
            this.bnCloseCamera.Click += new System.EventHandler(this.bnClose_Click);
            // 
            // bnOpen
            // 
            this.bnOpen.Location = new System.Drawing.Point(8, 89);
            this.bnOpen.Name = "bnOpen";
            this.bnOpen.Size = new System.Drawing.Size(65, 23);
            this.bnOpen.TabIndex = 1;
            this.bnOpen.Text = "打开设备";
            this.bnOpen.UseVisualStyleBackColor = true;
            this.bnOpen.Click += new System.EventHandler(this.bnOpen_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(853, 714);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // cbDeviceList
            // 
            this.cbDeviceList.FormattingEnabled = true;
            this.cbDeviceList.Location = new System.Drawing.Point(867, 5);
            this.cbDeviceList.Name = "cbDeviceList";
            this.cbDeviceList.Size = new System.Drawing.Size(464, 20);
            this.cbDeviceList.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbTriggerDelay);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.bnSetParam);
            this.groupBox4.Controls.Add(this.bnGetParam);
            this.groupBox4.Controls.Add(this.tbGain);
            this.groupBox4.Controls.Add(this.tbExposure);
            this.groupBox4.Location = new System.Drawing.Point(6, 163);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(158, 163);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "相机参数";
            // 
            // tbTriggerDelay
            // 
            this.tbTriggerDelay.Enabled = false;
            this.tbTriggerDelay.Location = new System.Drawing.Point(79, 91);
            this.tbTriggerDelay.Name = "tbTriggerDelay";
            this.tbTriggerDelay.Size = new System.Drawing.Size(69, 21);
            this.tbTriggerDelay.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "触发延时(us)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "曝光(us)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "增益dB";
            // 
            // bnSetParam
            // 
            this.bnSetParam.Enabled = false;
            this.bnSetParam.Location = new System.Drawing.Point(81, 123);
            this.bnSetParam.Name = "bnSetParam";
            this.bnSetParam.Size = new System.Drawing.Size(67, 23);
            this.bnSetParam.TabIndex = 7;
            this.bnSetParam.Text = "设置参数";
            this.bnSetParam.UseVisualStyleBackColor = true;
            this.bnSetParam.Click += new System.EventHandler(this.bnSetParam_Click);
            // 
            // bnGetParam
            // 
            this.bnGetParam.Enabled = false;
            this.bnGetParam.Location = new System.Drawing.Point(6, 123);
            this.bnGetParam.Name = "bnGetParam";
            this.bnGetParam.Size = new System.Drawing.Size(67, 23);
            this.bnGetParam.TabIndex = 6;
            this.bnGetParam.Text = "获取参数";
            this.bnGetParam.UseVisualStyleBackColor = true;
            this.bnGetParam.Click += new System.EventHandler(this.bnGetParam_Click);
            // 
            // tbGain
            // 
            this.tbGain.Enabled = false;
            this.tbGain.Location = new System.Drawing.Point(77, 61);
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(71, 21);
            this.tbGain.TabIndex = 1;
            // 
            // tbExposure
            // 
            this.tbExposure.Enabled = false;
            this.tbExposure.Location = new System.Drawing.Point(77, 25);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(71, 21);
            this.tbExposure.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(1029, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(360, 688);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bnCloseForm);
            this.tabPage1.Controls.Add(this.rtbLog);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(352, 662);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "相机操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bnCloseForm
            // 
            this.bnCloseForm.Location = new System.Drawing.Point(87, 515);
            this.bnCloseForm.Name = "bnCloseForm";
            this.bnCloseForm.Size = new System.Drawing.Size(143, 40);
            this.bnCloseForm.TabIndex = 18;
            this.bnCloseForm.Text = "退出";
            this.bnCloseForm.UseVisualStyleBackColor = true;
            this.bnCloseForm.Click += new System.EventHandler(this.bnCloseForm_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rtbLog.Location = new System.Drawing.Point(6, 383);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(314, 96);
            this.rtbLog.TabIndex = 17;
            this.rtbLog.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 342);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "通讯测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbCaliRadius);
            this.groupBox3.Controls.Add(this.cbCaliEnable);
            this.groupBox3.Controls.Add(this.bnTriggerCali);
            this.groupBox3.Location = new System.Drawing.Point(170, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 162);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "标定参数设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "标定圆半径(mm)";
            // 
            // tbCaliRadius
            // 
            this.tbCaliRadius.Location = new System.Drawing.Point(95, 52);
            this.tbCaliRadius.Name = "tbCaliRadius";
            this.tbCaliRadius.Size = new System.Drawing.Size(62, 21);
            this.tbCaliRadius.TabIndex = 12;
            // 
            // cbCaliEnable
            // 
            this.cbCaliEnable.AutoSize = true;
            this.cbCaliEnable.Enabled = false;
            this.cbCaliEnable.Location = new System.Drawing.Point(9, 95);
            this.cbCaliEnable.Name = "cbCaliEnable";
            this.cbCaliEnable.Size = new System.Drawing.Size(72, 16);
            this.cbCaliEnable.TabIndex = 11;
            this.cbCaliEnable.Text = "使能校准";
            this.cbCaliEnable.UseVisualStyleBackColor = true;
            this.cbCaliEnable.CheckedChanged += new System.EventHandler(this.cbCaliEnable_CheckedChanged);
            // 
            // bnTriggerCali
            // 
            this.bnTriggerCali.Enabled = false;
            this.bnTriggerCali.Location = new System.Drawing.Point(84, 93);
            this.bnTriggerCali.Name = "bnTriggerCali";
            this.bnTriggerCali.Size = new System.Drawing.Size(75, 23);
            this.bnTriggerCali.TabIndex = 6;
            this.bnTriggerCali.Text = "拍摄校准";
            this.bnTriggerCali.UseVisualStyleBackColor = true;
            this.bnTriggerCali.Click += new System.EventHandler(this.bnTriggerCali_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.bnUpdateParam);
            this.tabPage3.Controls.Add(this.cbSmallblackspot);
            this.tabPage3.Controls.Add(this.cbBigblackspot);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(352, 662);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "检测标准";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cbSmallblackspot
            // 
            this.cbSmallblackspot.AutoSize = true;
            this.cbSmallblackspot.Checked = true;
            this.cbSmallblackspot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSmallblackspot.Location = new System.Drawing.Point(21, 80);
            this.cbSmallblackspot.Name = "cbSmallblackspot";
            this.cbSmallblackspot.Size = new System.Drawing.Size(186, 16);
            this.cbSmallblackspot.TabIndex = 4;
            this.cbSmallblackspot.Text = "0.5~1平方毫米的斑点超过四个";
            this.cbSmallblackspot.UseVisualStyleBackColor = true;
            this.cbSmallblackspot.CheckedChanged += new System.EventHandler(this.cbSmallblackspot_CheckedChanged);
            // 
            // cbBigblackspot
            // 
            this.cbBigblackspot.AutoSize = true;
            this.cbBigblackspot.Checked = true;
            this.cbBigblackspot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBigblackspot.Location = new System.Drawing.Point(21, 31);
            this.cbBigblackspot.Name = "cbBigblackspot";
            this.cbBigblackspot.Size = new System.Drawing.Size(138, 16);
            this.cbBigblackspot.TabIndex = 3;
            this.cbBigblackspot.Text = "大于1平方毫米的斑点";
            this.cbBigblackspot.UseVisualStyleBackColor = true;
            this.cbBigblackspot.CheckedChanged += new System.EventHandler(this.cbBigblackspot_CheckedChanged);
            // 
            // Result_label
            // 
            this.Result_label.BackColor = System.Drawing.Color.Blue;
            this.Result_label.Font = new System.Drawing.Font("宋体", 27.75F, System.Drawing.FontStyle.Bold);
            this.Result_label.ForeColor = System.Drawing.Color.White;
            this.Result_label.Location = new System.Drawing.Point(864, 610);
            this.Result_label.Name = "Result_label";
            this.Result_label.Size = new System.Drawing.Size(159, 109);
            this.Result_label.TabIndex = 14;
            this.Result_label.Text = "NG";
            this.Result_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Result_listView
            // 
            this.Result_listView.BackColor = System.Drawing.Color.DarkGray;
            this.Result_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Number,
            this.Result});
            this.Result_listView.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Result_listView.GridLines = true;
            this.Result_listView.Location = new System.Drawing.Point(867, 30);
            this.Result_listView.Name = "Result_listView";
            this.Result_listView.Size = new System.Drawing.Size(156, 567);
            this.Result_listView.TabIndex = 13;
            this.Result_listView.UseCompatibleStateImageBehavior = false;
            this.Result_listView.View = System.Windows.Forms.View.Details;
            // 
            // Number
            // 
            this.Number.Text = "编号";
            // 
            // Result
            // 
            this.Result.Text = "检测结果";
            this.Result.Width = 140;
            // 
            // bnUpdateParam
            // 
            this.bnUpdateParam.Location = new System.Drawing.Point(60, 228);
            this.bnUpdateParam.Name = "bnUpdateParam";
            this.bnUpdateParam.Size = new System.Drawing.Size(75, 23);
            this.bnUpdateParam.TabIndex = 5;
            this.bnUpdateParam.Text = "更新参数";
            this.bnUpdateParam.UseVisualStyleBackColor = true;
            this.bnUpdateParam.Click += new System.EventHandler(this.bnUpdateParam_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 1042);
            this.Controls.Add(this.Result_label);
            this.Controls.Add(this.Result_listView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbDeviceList);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "家得宝餐盘检测系统V1.0";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bnEnum;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bnTriggerExec;
        private System.Windows.Forms.CheckBox cbSoftTrigger;
        private System.Windows.Forms.Button bnStopGrab;
        private System.Windows.Forms.Button bnStartGrab;
        private System.Windows.Forms.RadioButton bnTriggerMode;
        private System.Windows.Forms.RadioButton bnContinuesMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bnCloseCamera;
        private System.Windows.Forms.Button bnOpen;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cbDeviceList;
        private GroupBox groupBox4;
        private Button bnSetParam;
        private Button bnGetParam;
        private Label label2;
        private TextBox tbGain;
        private TextBox tbExposure;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label1;
        private Label Result_label;
        private ListView Result_listView;
        private ColumnHeader Number;
        private ColumnHeader Result;
        private TextBox tbTriggerDelay;
        private Label label3;
        private TabPage tabPage3;
        private CheckBox cbSmallblackspot;
        private CheckBox cbBigblackspot;
        private CheckBox cbCaliEnable;
        private Button bnTriggerCali;
        private Label label4;
        private TextBox tbCaliRadius;
        private GroupBox groupBox3;
        private Button button1;
        private RichTextBox rtbLog;
        private Button bnCloseForm;
        private Button bnUpdateParam;
    }
}

