using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
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
            drawingPanel = new Panel();
            back_default = new Button();
            exportSvgButton = new Button();
            button_speed_magnet = new Button();
            button_speed = new Button();
            add_magnet = new Button();
            remove_magnet = new Button();
            ScenarioComboBox = new ComboBox();
            drawingPanel.SuspendLayout();
            SuspendLayout();
            // 
            // drawingPanel
            // 
            drawingPanel.Controls.Add(back_default);
            drawingPanel.Controls.Add(exportSvgButton);
            drawingPanel.Controls.Add(button_speed_magnet);
            drawingPanel.Controls.Add(button_speed);
            drawingPanel.Controls.Add(add_magnet);
            drawingPanel.Controls.Add(remove_magnet);
            drawingPanel.Controls.Add(ScenarioComboBox);
            drawingPanel.Dock = DockStyle.Fill;
            drawingPanel.Location = new Point(0, 0);
            drawingPanel.Margin = new Padding(3, 4, 3, 4);
            drawingPanel.Name = "drawingPanel";
            drawingPanel.Size = new Size(782, 553);
            drawingPanel.TabIndex = 0;
            drawingPanel.Paint += drawingPanel_Paint;
            drawingPanel.MouseClick += drawingPanel_MouseClick;
            drawingPanel.MouseDown += drawingPanel_MouseDown;
            drawingPanel.MouseMove += drawingPanel_MouseMove;
            drawingPanel.MouseUp += drawingPanel_MouseUp;
            drawingPanel.Resize += drawingPanel_Resize;
            // 
            // back_default
            // 
            back_default.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            back_default.BackColor = SystemColors.AppWorkspace;
            back_default.Cursor = Cursors.Hand;
            back_default.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            back_default.Location = new Point(618, 260);
            back_default.Name = "back_default";
            back_default.Size = new Size(152, 47);
            back_default.TabIndex = 6;
            back_default.Text = "Back to Default";
            back_default.UseVisualStyleBackColor = false;
            back_default.Click += back_default_Click;
            // 
            // exportSvgButton
            // 
            exportSvgButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            exportSvgButton.BackColor = SystemColors.AppWorkspace;
            exportSvgButton.Cursor = Cursors.Hand;
            exportSvgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            exportSvgButton.Location = new Point(615, 672);
            exportSvgButton.Name = "exportSvgButton";
            exportSvgButton.Size = new Size(164, 64);
            exportSvgButton.TabIndex = 2;
            exportSvgButton.Text = "Export to SVG";
            exportSvgButton.UseVisualStyleBackColor = false;
            exportSvgButton.Click += exportSvgButton_Click;
            // 
            // button_speed_magnet
            // 
            button_speed_magnet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_speed_magnet.BackColor = SystemColors.AppWorkspace;
            button_speed_magnet.Cursor = Cursors.Hand;
            button_speed_magnet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button_speed_magnet.Location = new Point(639, 84);
            button_speed_magnet.Name = "button_speed_magnet";
            button_speed_magnet.Size = new Size(131, 66);
            button_speed_magnet.TabIndex = 1;
            button_speed_magnet.Text = "Magnet Change Speed";
            button_speed_magnet.UseVisualStyleBackColor = false;
            button_speed_magnet.Click += button_speed_magnet_Click;
            // 
            // button_speed
            // 
            button_speed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_speed.BackColor = SystemColors.AppWorkspace;
            button_speed.Cursor = Cursors.Hand;
            button_speed.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button_speed.Location = new Point(639, 12);
            button_speed.Name = "button_speed";
            button_speed.Size = new Size(131, 66);
            button_speed.TabIndex = 0;
            button_speed.Text = "Probe Speed";
            button_speed.UseVisualStyleBackColor = false;
            button_speed.Click += button_speed_Click;
            // 
            // add_magnet
            // 
            add_magnet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            add_magnet.BackColor = SystemColors.AppWorkspace;
            add_magnet.Cursor = Cursors.Hand;
            add_magnet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            add_magnet.Location = new Point(691, 190);
            add_magnet.Name = "add_magnet";
            add_magnet.Size = new Size(79, 64);
            add_magnet.TabIndex = 4;
            add_magnet.Text = "Add Magnet";
            add_magnet.UseVisualStyleBackColor = false;
            add_magnet.Click += add_magnet_Click;
            // 
            // remove_magnet
            // 
            remove_magnet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            remove_magnet.BackColor = SystemColors.AppWorkspace;
            remove_magnet.Cursor = Cursors.Hand;
            remove_magnet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            remove_magnet.Location = new Point(618, 190);
            remove_magnet.Name = "remove_magnet";
            remove_magnet.Size = new Size(76, 64);
            remove_magnet.TabIndex = 5;
            remove_magnet.Text = "Remove Magnet";
            remove_magnet.UseVisualStyleBackColor = false;
            remove_magnet.Click += remove_magnet_Click;
            // 
            // ScenarioComboBox
            // 
            ScenarioComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ScenarioComboBox.BackColor = SystemColors.AppWorkspace;
            ScenarioComboBox.Cursor = Cursors.Hand;
            ScenarioComboBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ScenarioComboBox.FormattingEnabled = true;
            ScenarioComboBox.Items.AddRange(new object[] { "Scenario 1", "Scenario 2", "Scenario 3", "Scenario 4" });
            ScenarioComboBox.Location = new Point(618, 156);
            ScenarioComboBox.Name = "ScenarioComboBox";
            ScenarioComboBox.Size = new Size(152, 28);
            ScenarioComboBox.TabIndex = 3;
            ScenarioComboBox.Text = "Change Scenario";
            ScenarioComboBox.SelectedIndexChanged += ScenarioComboBox_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(782, 553);
            Controls.Add(drawingPanel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "<A23B0303P> - Semestrální práce KIV/UPG 2024/2025";
            drawingPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void DrawingPanel_MouseLeave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion


        private Panel drawingPanel;
        private Button button_speed;
        private Button button_speed_magnet;
        private Button exportSvgButton;
        private ComboBox ScenarioComboBox;
        private Button add_magnet;
        private Button remove_magnet;
        private Button back_default;
    }
}
