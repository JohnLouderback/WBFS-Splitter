namespace WbfsFileSplitter;

partial class WbfsFileSplitterWindow {
  /// <summary>
  ///  Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;


  /// <summary>
  ///  Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing) {
    if (disposing && (components != null)) {
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
    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WbfsFileSplitterWindow));
    openFileDialog1 = new OpenFileDialog();
    btnSelectFile = new Button();
    groupBox1 = new GroupBox();
    btnSplitFiles = new Button();
    groupBox3 = new GroupBox();
    lblFile2 = new Label();
    lblFile1 = new Label();
    textBox1 = new TextBox();
    groupBox2 = new GroupBox();
    progressBar = new DarkModeForms.FlatProgressBar();
    lblStatus = new Label();
    groupBox1.SuspendLayout();
    groupBox3.SuspendLayout();
    groupBox2.SuspendLayout();
    SuspendLayout();
    // 
    // openFileDialog1
    // 
    openFileDialog1.FileName = "openFileDialog1";
    // 
    // btnSelectFile
    // 
    btnSelectFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    btnSelectFile.Location = new Point(435, 21);
    btnSelectFile.Margin = new Padding(2);
    btnSelectFile.Name = "btnSelectFile";
    btnSelectFile.Size = new Size(78, 22);
    btnSelectFile.TabIndex = 1;
    btnSelectFile.Text = "Select File";
    btnSelectFile.UseVisualStyleBackColor = true;
    btnSelectFile.Click += btnSelectFile_Click;
    // 
    // groupBox1
    // 
    groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    groupBox1.Controls.Add(btnSplitFiles);
    groupBox1.Controls.Add(groupBox3);
    groupBox1.Controls.Add(textBox1);
    groupBox1.Controls.Add(btnSelectFile);
    groupBox1.Location = new Point(8, 7);
    groupBox1.Margin = new Padding(2);
    groupBox1.Name = "groupBox1";
    groupBox1.Padding = new Padding(6, 4, 6, 4);
    groupBox1.Size = new Size(522, 156);
    groupBox1.TabIndex = 2;
    groupBox1.TabStop = false;
    groupBox1.Text = "Input WBFS File";
    groupBox1.UseCompatibleTextRendering = true;
    // 
    // btnSplitFiles
    // 
    btnSplitFiles.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    btnSplitFiles.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    btnSplitFiles.Enabled = false;
    btnSplitFiles.Location = new Point(9, 121);
    btnSplitFiles.Name = "btnSplitFiles";
    btnSplitFiles.Size = new Size(504, 26);
    btnSplitFiles.TabIndex = 6;
    btnSplitFiles.Text = "Split Files";
    btnSplitFiles.UseVisualStyleBackColor = true;
    btnSplitFiles.Click += btnSplitFiles_Click;
    // 
    // groupBox3
    // 
    groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    groupBox3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    groupBox3.Controls.Add(lblFile2);
    groupBox3.Controls.Add(lblFile1);
    groupBox3.Location = new Point(9, 48);
    groupBox3.Name = "groupBox3";
    groupBox3.Size = new Size(504, 67);
    groupBox3.TabIndex = 5;
    groupBox3.TabStop = false;
    groupBox3.Text = "Files to be Created";
    // 
    // lblFile2
    // 
    lblFile2.AutoSize = true;
    lblFile2.Location = new Point(5, 32);
    lblFile2.Margin = new Padding(2, 0, 2, 0);
    lblFile2.Name = "lblFile2";
    lblFile2.Size = new Size(91, 14);
    lblFile2.TabIndex = 4;
    lblFile2.Text = "                     ";
    // 
    // lblFile1
    // 
    lblFile1.AutoSize = true;
    lblFile1.Location = new Point(5, 18);
    lblFile1.Margin = new Padding(2, 0, 2, 0);
    lblFile1.Name = "lblFile1";
    lblFile1.Size = new Size(91, 14);
    lblFile1.TabIndex = 3;
    lblFile1.Text = "                     ";
    // 
    // textBox1
    // 
    textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    textBox1.Location = new Point(8, 21);
    textBox1.Margin = new Padding(2);
    textBox1.Name = "textBox1";
    textBox1.Size = new Size(424, 22);
    textBox1.TabIndex = 2;
    textBox1.TextChanged += textBox1_TextChanged;
    // 
    // groupBox2
    // 
    groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    groupBox2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    groupBox2.Controls.Add(progressBar);
    groupBox2.Controls.Add(lblStatus);
    groupBox2.Location = new Point(8, 167);
    groupBox2.Margin = new Padding(2);
    groupBox2.Name = "groupBox2";
    groupBox2.Padding = new Padding(6, 4, 6, 4);
    groupBox2.Size = new Size(522, 83);
    groupBox2.TabIndex = 3;
    groupBox2.TabStop = false;
    groupBox2.Text = "Progress";
    // 
    // progressBar
    // 
    progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    progressBar.Location = new Point(8, 22);
    progressBar.Name = "progressBar";
    progressBar.ProgressBarColor = Color.Green;
    progressBar.Size = new Size(505, 23);
    progressBar.TabIndex = 2;
    // 
    // lblStatus
    // 
    lblStatus.AutoSize = true;
    lblStatus.Location = new Point(8, 48);
    lblStatus.Margin = new Padding(2, 0, 2, 0);
    lblStatus.Name = "lblStatus";
    lblStatus.Size = new Size(91, 14);
    lblStatus.TabIndex = 1;
    lblStatus.Text = "                     ";
    lblStatus.Click += lblStatus_Click;
    // 
    // WbfsFileSplitterWindow
    // 
    AllowDrop = true;
    AutoScaleDimensions = new SizeF(7F, 14F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(537, 261);
    Controls.Add(groupBox2);
    Controls.Add(groupBox1);
    Icon = (Icon)resources.GetObject("$this.Icon");
    Margin = new Padding(2);
    MinimumSize = new Size(291, 300);
    Name = "WbfsFileSplitterWindow";
    SizeGripStyle = SizeGripStyle.Show;
    StartPosition = FormStartPosition.CenterScreen;
    Text = "WBFS File Splitter";
    DragDrop += WbfsFileSplitterWindow_DragDrop;
    DragEnter += WbfsFileSplitterWindow_DragEnter;
    groupBox1.ResumeLayout(false);
    groupBox1.PerformLayout();
    groupBox3.ResumeLayout(false);
    groupBox3.PerformLayout();
    groupBox2.ResumeLayout(false);
    groupBox2.PerformLayout();
    ResumeLayout(false);
  }
  #endregion

  private OpenFileDialog openFileDialog1;
  private Button btnSelectFile;
  private GroupBox groupBox1;
  private TextBox textBox1;
  private GroupBox groupBox2;
  private Label lblStatus;
  private DarkModeForms.FlatProgressBar progressBar;
  private GroupBox groupBox3;
  private Label lblFile2;
  private Label lblFile1;
  private Button btnSplitFiles;
}
