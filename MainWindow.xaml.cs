using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;
using AForge.Video;
using AForge.Video.DirectShow;

public partial class MainWindow : Window
{
    private VideoCaptureDevice videoSource;
    private DispatcherTimer timer;
    private BarcodeReader barcodeReader;

    public MainWindow()
    {
        InitializeComponent();

        barcodeReader = new BarcodeReader();
        timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        timer.Tick += Timer_Tick;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        if (videoDevices.Count == 0)
        {
            MessageBox.Show("Nema dostupnih video uređaja.");
            return;
        }

        videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
        videoSource.NewFrame += VideoSource_NewFrame;
        videoSource.Start();

        StartButton.IsEnabled = false;
        StopButton.IsEnabled = true;
        timer.Start();
    }

    private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        try
        {
            var bitmap = (System.Drawing.Bitmap)eventArgs.Frame.Clone();
            Dispatcher.Invoke(() => {
                CameraImage.Source = ConvertBitmap(bitmap);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
    {
        var bi = new BitmapImage();
        bi.BeginInit();
        bi.StreamSource = new System.IO.MemoryStream();
        bitmap.Save(bi.StreamSource, System.Drawing.Imaging.ImageFormat.Bmp);
        bi.EndInit();
        return bi;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (CameraImage.Source != null)
        {
            var bitmapSource = (BitmapImage)CameraImage.Source;
            var bitmap = new System.Drawing.Bitmap(bitmapSource.StreamSource);

            var result = barcodeReader.Decode(bitmap);
            if (result != null)
            {
                ResultTextBox.Text = result.Text;
                ProcessQRResult(result.Text);
                timer.Stop();
                videoSource.SignalToStop();
            }
        }
    }

    private void ProcessQRResult(string qrText)
    {
        // Ista logika obrade kao u Windows Forms primjeru
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        timer.Stop();
        if (videoSource != null && videoSource.IsRunning)
        {
            videoSource.SignalToStop();
        }

        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        CameraImage.Source = null;
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        if (videoSource != null && videoSource.IsRunning)
        {
            videoSource.SignalToStop();
        }
        base.OnClosing(e);
    }
}