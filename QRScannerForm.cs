using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

public class QRScannerForm : Form
{
    private PictureBox pictureBox;
    private Button startButton;
    private Button stopButton;
    private TextBox resultTextBox;
    private Timer timer;
    private VideoCaptureDevice videoSource;

    public QRScannerForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "QR Code Scanner";
        this.Width = 800;
        this.Height = 600;

        // PictureBox za prikaz video snimka
        pictureBox = new PictureBox
        {
            Size = new Size(640, 480),
            Location = new Point(10, 10),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Dugme za pokretanje
        startButton = new Button
        {
            Text = "Start Scanning",
            Location = new Point(10, 500),
            Size = new Size(120, 40)
        };
        startButton.Click += StartButton_Click;

        // Dugme za zaustavljanje
        stopButton = new Button
        {
            Text = "Stop Scanning",
            Location = new Point(140, 500),
            Size = new Size(120, 40),
            Enabled = false
        };
        stopButton.Click += StopButton_Click;

        // TextBox za rezultat
        resultTextBox = new TextBox
        {
            Location = new Point(270, 500),
            Size = new Size(300, 40),
            ReadOnly = true
        };

        // Timer za kontinuirano skeniranje
        timer = new Timer { Interval = 1000 };
        timer.Tick += Timer_Tick;

        // Dodavanje kontrola na formu
        this.Controls.Add(pictureBox);
        this.Controls.Add(startButton);
        this.Controls.Add(stopButton);
        this.Controls.Add(resultTextBox);
    }

    private void StartButton_Click(object sender, EventArgs e)
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

        startButton.Enabled = false;
        stopButton.Enabled = true;
        timer.Start();
    }

    private void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
    {
        var bitmap = (Bitmap)eventArgs.Frame.Clone();
        pictureBox.Image = bitmap;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (pictureBox.Image != null)
        {
            var reader = new BarcodeReader();
            var result = reader.Decode((Bitmap)pictureBox.Image);

            if (result != null)
            {
                resultTextBox.Text = result.Text;
                ProcessQRResult(result.Text);
                timer.Stop();
                videoSource.SignalToStop();
            }
        }
    }

    private void ProcessQRResult(string qrText)
    {
        // Ovdje implementirajte logiku za obradu QR koda
        if (qrText.Contains("rezervacijaid:"))
        {
            var parts = qrText.Split('|');
            var reservationPart = parts.FirstOrDefault(p => p.StartsWith("rezervacijaid:"));

            if (reservationPart != null)
            {
                var reservationId = reservationPart.Split(':')[1];
                MessageBox.Show($"Pronađena validna karta! Rezervacija ID: {reservationId}");

                // Pozovite metodu za validaciju rezervacije
                ValidateReservation(reservationId);
            }
        }
        else
        {
            MessageBox.Show("Nevažeći QR kod.");
        }
    }

    private void ValidateReservation(string reservationId)
    {
        // Ovdje implementirajte logiku za validaciju sa serverom
        // Možete koristiti HttpClient za API poziv
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        timer.Stop();
        if (videoSource != null && videoSource.IsRunning)
        {
            videoSource.SignalToStop();
        }

        startButton.Enabled = true;
        stopButton.Enabled = false;
        pictureBox.Image = null;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (videoSource != null && videoSource.IsRunning)
        {
            videoSource.SignalToStop();
        }
        base.OnFormClosing(e);
    }
}