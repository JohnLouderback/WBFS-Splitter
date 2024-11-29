using System.Media;
using CommunityToolkit.Common;
using DarkModeForms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace WbfsFileSplitter;

public partial class WbfsFileSplitterWindow : Form {
  private const long splitPosition = 0xFFFF8000; // 4,294,934,528 bytes

  private static readonly Color errorColor = Color.FromArgb(
    0xFF,
    0xA5,
    0x00,
    0x00
  );

  private static readonly Color okayColor = Color.FromArgb(
    0xFF,
    0x00,
    0x80,
    0x00
  );

  private readonly DarkModeCS dm;

  private bool inputIsValid;

  private TaskbarManager? taskbarManager =>
    TaskbarManager.IsPlatformSupported ? TaskbarManager.Instance : null;


  public WbfsFileSplitterWindow(string[] args) {
    InitializeComponent();

    dm = new DarkModeCS(this) {
      //[Optional] Choose your preferred color mode here:
      // ColorMode = DarkModeCS.DisplayMode.ClearMode
      ColorMode = DarkModeCS.DisplayMode.SystemDefault
    };

    progressBar.Minimum = 0;
    progressBar.Maximum = 100;

    Shown += (sender, e) => {
      if (args.Length > 0) {
        // If the application was launched with a file path argument, set the text box to that path.
        textBox1.Text = args[0];

        // Validate the file path.
        Application.DoEvents();

        // If the file path is valid, split the file.
        if (inputIsValid) {
          btnSplitFiles.PerformClick();
        }
      }
    };
  }


  /// <summary>
  ///   Blocks the usage of the UI controls while the file is being split.
  /// </summary>
  /// <param name="block"> Whether to block the UI controls. </param>
  private void BlockUI(bool block) {
    textBox1.Enabled      = !block;
    btnSelectFile.Enabled = !block;

    if (block) {
      btnSplitFiles.Text    = "Splitting...";
      btnSplitFiles.Enabled = false;
    }
    else {
      btnSplitFiles.Text = "Split File";
    }
  }


  private async void btnSelectFile_Click(object sender, EventArgs e) {
    var ofd = new OpenFileDialog();
    ofd.Filter = "WBFS files (*.wbfs)|*.wbfs|All files (*.*)|*.*";
    if (ofd.ShowDialog() == DialogResult.OK) {
      var inputFilePath = ofd.FileName;
      textBox1.Text = inputFilePath;
    }
  }


  private async void btnSplitFiles_Click(object sender, EventArgs e) {
    BlockUI(true);
    lblStatus.Text = "Processing...";
    UpdateProgress(0);
    Application.DoEvents(); // Update UI

    try {
      taskbarManager?.SetProgressState(TaskbarProgressBarState.Normal);
      await SplitFileAsync(textBox1.Text);
      lblStatus.Text               = "Done!";
      lblStatus.ForeColor          = okayColor;
      progressBar.ProgressBarColor = Color.SpringGreen;
      UpdateProgress(100);
      taskbarManager?.SetProgressState(TaskbarProgressBarState.NoProgress);

      // Play system chime on completion
      SystemSounds.Asterisk.Play();
    }
    catch (Exception ex) {
      Messenger.MessageBox("Error: " + ex.Message);
      lblStatus.Text               = "Error occurred.";
      lblStatus.ForeColor          = errorColor;
      progressBar.ProgressBarColor = errorColor;
      UpdateProgress(100);
      taskbarManager?.SetProgressState(TaskbarProgressBarState.Error);

      // Play error sound
      SystemSounds.Hand.Play();

      // Check if the files were created and delete them if they were.
      try {
        var file1 = textBox1.Text + ".tmp";
        var file2 = textBox1.Text.Replace(".wbfs", ".wbf1");
        if (File.Exists(file1)) {
          File.Delete(file1);
        }

        if (File.Exists(file2)) {
          File.Delete(file2);
        }
      }
      catch (Exception ex2) {
        Messenger.MessageBox("Error: " + ex2.Message);
      }
    }
    finally {
      BlockUI(false);
    }
  }


  private async Task CopyStreamAsync(
    FileStream input,
    FileStream output,
    long bytesToCopy,
    IProgress<int> progress,
    long bytesAlreadyCopied,
    long totalBytes
  ) {
    var  buffer = new byte[64 * 1024]; // 64KB buffer
    int  bytesRead;
    long totalBytesCopied = 0;

    while (totalBytesCopied < bytesToCopy &&
           (bytesRead = await input.ReadAsync(
                          buffer,
                          0,
                          (int)Math.Min(buffer.Length, bytesToCopy - totalBytesCopied)
                        )) >
           0) {
      await output.WriteAsync(buffer, 0, bytesRead);
      totalBytesCopied += bytesRead;

      // Calculate progress percentage
      var progressValue = (double)(bytesAlreadyCopied + totalBytesCopied) / totalBytes * 100;
      progress.Report((int)progressValue);
    }
  }


  private void lblStatus_Click(object sender, EventArgs e) {}


  private void ResetProgressBar() {
    UpdateProgress(0);
    progressBar.ProgressBarColor = Color.LimeGreen;
    lblStatus.Text               = "";
    taskbarManager?.SetProgressState(TaskbarProgressBarState.NoProgress);
  }


  private async Task SplitFileAsync(string inputFilePath) {
    var directory                = Path.GetDirectoryName(inputFilePath);
    var fileName                 = Path.GetFileName(inputFilePath);
    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);

    // The first output file will have the same name as the input file, but with a ".tmp" extension.
    // Later, we'll rename it to the correct extension after deleting the original file.
    var outputFilePath1 = Path.Combine(directory, fileName + ".tmp");
    var outputFilePath2 = Path.Combine(directory, fileNameWithoutExtension + ".wbf1");

    using (var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read)) {
      var inputFileLength = inputStream.Length;

      var progressHandler = new Progress<int>(UpdateProgress);

      // Create first output file
      await using (var outputStream1 = new FileStream(
                     outputFilePath1,
                     FileMode.Create,
                     FileAccess.Write
                   )) {
        var bytesToCopy = Math.Min(splitPosition, inputFileLength);
        await CopyStreamAsync(
          inputStream,
          outputStream1,
          bytesToCopy,
          progressHandler,
          0,
          inputFileLength
        );
      }

      // Create second output file if necessary
      if (inputFileLength > splitPosition) {
        await using (var outputStream2 = new FileStream(
                       outputFilePath2,
                       FileMode.Create,
                       FileAccess.Write
                     )) {
          var bytesToCopy = inputFileLength - splitPosition;
          await CopyStreamAsync(
            inputStream,
            outputStream2,
            bytesToCopy,
            progressHandler,
            splitPosition,
            inputFileLength
          );
        }
      }
    }

    // Delete the original file
    File.Delete(inputFilePath);
    // Rename the first output file to the correct extension
    File.Move(outputFilePath1, inputFilePath);
  }


  private void textBox1_TextChanged(object sender, EventArgs e) {
    // Reset the status label.
    lblStatus.Text      = "";
    lblStatus.ForeColor = dm.OScolors.TextActive;

    // Reset the split button.
    btnSplitFiles.Enabled = false;

    // Reset the progress bar.
    ResetProgressBar();

    ValidateFile(textBox1.Text);
  }


  private void UpdateFileLabels(
    string filePath,
    bool isWbfsFile,
    bool fileExists,
    bool file2Exists,
    FileInfo? fileInfo
  ) {
    if (fileExists && fileInfo == null) {
      // fileInfo should always be non-null if fileExists is true.
      throw new ArgumentNullException(nameof(fileInfo));
    }

    // If the file doesn't exist, clear the labels.
    if (!fileExists ||
        !isWbfsFile) {
      lblFile1.Text = "";
      lblFile2.Text = "";
      return;
    }

    lblFile2.ForeColor = file2Exists ? errorColor : dm.OScolors.TextActive;

    var fileSize1 = fileInfo!.Length;
    var fileSize2 = fileSize1 > splitPosition ? fileSize1 - splitPosition : 0;

    lblFile1.Text =
      $"{
        Path.GetFileName(filePath)
      } ({
        Converters.ToFileSizeString(fileSize1 <= splitPosition ? fileSize1 : splitPosition)
      })";
    if (fileSize2 > 0 || file2Exists) {
      lblFile2.Text = filePath.TrimEnd().EndsWith(".wbfs")
                        ? $"{
                          Path.GetFileName(filePath).Replace(".wbfs", ".wbf1")
                        } ({
                          (file2Exists ? "Already Exists" : Converters.ToFileSizeString(fileSize2))
                        })"
                        : "";
    }
    else {
      lblFile2.Text = "";
    }
  }


  /// <summary>
  ///   Updates the progress bar and taskbar progress value.
  /// </summary>
  /// <param name="value">
  ///   The value out of 100 to set the progress bar and taskbar progress value to.
  /// </param>
  private void UpdateProgress(int value) {
    taskbarManager?.SetProgressValue(value, 100);
    progressBar.Value = value;
  }


  private void ValidateFile(string filePath) {
    inputIsValid = false;
    var fileExists  = File.Exists(filePath);
    var isWbfsFile  = filePath.TrimEnd().EndsWith(".wbfs");
    var file2Exists = File.Exists(filePath.Replace(".wbfs", ".wbf1"));
    var fileInfo    = fileExists ? new FileInfo(filePath) : null;

    UpdateFileLabels(filePath, isWbfsFile, fileExists, file2Exists, fileInfo);

    if (!fileExists) {
      textBox1.ForeColor  = errorColor;
      lblStatus.Text      = "The file does not exist.";
      lblStatus.ForeColor = errorColor;
    }
    else if (!isWbfsFile) {
      textBox1.ForeColor  = errorColor;
      lblStatus.Text      = "The file is not a .wbfs file.";
      lblStatus.ForeColor = errorColor;
      SystemSounds.Exclamation.Play();
    }
    else {
      textBox1.ForeColor = dm.OScolors.TextActive;

      if (file2Exists) {
        lblStatus.Text      = "The .wbf1 already exists.";
        lblStatus.ForeColor = errorColor;
        SystemSounds.Exclamation.Play();
        return;
      }

      lblStatus.ForeColor = dm.OScolors.TextActive;

      // If the file exists and is larger than the split position, enable the split button.
      // If it's smaller than the split position, the split button should remain disabled as there
      // is no need to split the file.
      if (fileInfo!.Length > splitPosition) {
        btnSplitFiles.Enabled = true;
        lblStatus.Text        = "Ready to split file.";
        inputIsValid          = true;
      }
      else {
        lblStatus.Text      = "Splitting is not necessary.";
        lblStatus.ForeColor = okayColor;
        SystemSounds.Exclamation.Play();
      }
    }
  }


  private void WbfsFileSplitterWindow_DragDrop(object sender, DragEventArgs e) {
    // Ensure the data is not null
    if (e.Data == null) return;

    if (e.Data.GetDataPresent(DataFormats.Text)) {
      // Get the dropped text
      var text = (string)e.Data.GetData(DataFormats.Text)!;
      textBox1.Text = text;
    }
    else if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
      // Get the dropped file paths
      var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;

      if (files.Length > 0) {
        textBox1.Text = files[0];
      }
    }
  }


  private void WbfsFileSplitterWindow_DragEnter(object sender, DragEventArgs e) {
    // Check if the data being dragged is a file
    if (e.Data != null &&
        (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))) {
      e.Effect = DragDropEffects.Copy; // Show the "Copy" cursor
    }
    else {
      e.Effect = DragDropEffects.None; // Show the "Not allowed" cursor
    }
  }
}
