using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sharparam.SharpBlade.Razer;
using Sharparam.SharpBlade.Native;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebBrowserExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RazerManager manager = new RazerManager(false);
            manager.Touchpad.SetWindow(this, Touchpad.RenderMethod.Polling);
            manager.EnableDynamicKey(Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK1, @"C:\Users\Brandon\Pictures\chris.png");
            wb.Navigate(new Uri("http://www.razerzone.com")); 
            
        }

        void CreateBitmapFromVisual(Visual target, string filename)
        {
            if (target == null)
            {
                return;
            }
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            Type t = target.GetType();

            if (t.Name == "WebBrowser")
            {
                System.Windows.Controls.WebBrowser web = (System.Windows.Controls.WebBrowser)target;
                System.Windows.Point p0 = web.PointToScreen(bounds.TopLeft);
                System.Drawing.Point p1 = new System.Drawing.Point((int)p0.X, (int)p0.Y);

                MessageBox.Show(p0.ToString() + " " + p1.ToString());
                Bitmap image = new Bitmap((int)bounds.Width, (int)bounds.Height);
                Graphics imgGraphics = Graphics.FromImage(image);
                imgGraphics.CopyFromScreen(p1.X, p1.Y,0, 0, new System.Drawing.Size((int)bounds.Width +500, (int)bounds.Height + 500));
                image.Save(filename, ImageFormat.Bmp);
            }
            else
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Default);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(target);
                    dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));

                }
                rtb.Render(dv);
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                using (Stream stm = File.Create(filename))
                {
                    png.Save(stm);
                }
            }

        } 


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            int i = 0;

          //  wb.Navigate(tb.Text);

            CreateBitmapFromVisual(wb, @"c:\Users\Brandon\Desktop\1.png"); 
                  
        }
    }
}
