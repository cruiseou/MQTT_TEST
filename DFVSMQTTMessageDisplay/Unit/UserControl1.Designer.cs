﻿namespace DFVSMQTTMessageDisplay.Unit
{
    partial class UserControl1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.alarmAndFiberRealTimeRecord1 = new DFVSMQTTMessageDisplay.Unit.AlarmAndFiberRealTimeRecord();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.alarmHistory1 = new DFVSMQTTMessageDisplay.Unit.AlarmAndFiberHistoryRecord();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(839, 771);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.alarmAndFiberRealTimeRecord1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(831, 745);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "警报实时记录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // alarmAndFiberRealTimeRecord1
            // 
            this.alarmAndFiberRealTimeRecord1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmAndFiberRealTimeRecord1.Location = new System.Drawing.Point(3, 3);
            this.alarmAndFiberRealTimeRecord1.Name = "alarmAndFiberRealTimeRecord1";
            this.alarmAndFiberRealTimeRecord1.Size = new System.Drawing.Size(825, 739);
            this.alarmAndFiberRealTimeRecord1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.alarmHistory1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(831, 745);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "警报历史记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // alarmHistory1
            // 
            this.alarmHistory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmHistory1.Location = new System.Drawing.Point(3, 3);
            this.alarmHistory1.Name = "alarmHistory1";
            this.alarmHistory1.Size = new System.Drawing.Size(825, 739);
            this.alarmHistory1.TabIndex = 0;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(839, 771);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private AlarmAndFiberRealTimeRecord alarmAndFiberRealTimeRecord1;
        private AlarmAndFiberHistoryRecord alarmHistory1;
    }
}
