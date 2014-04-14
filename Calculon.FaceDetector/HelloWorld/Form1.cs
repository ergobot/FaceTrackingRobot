using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.IO;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        FileInfo myFile;// = new FileInfo(@"C:\users\Sean\Desktop\face.png");
        //Image<Bgr, Byte> My_Image; //= new Image<Bgr, byte>(myFile.FullName);
        Image<Hsv, Byte> My_Image;
        int counter = 0;
        public Form1()
        {
            InitializeComponent();
            //HelloWorldMethod();
            //HelloWorldMethod2();
            //myFile = new FileInfo(@"C:\users\Sean\Desktop\face.png");
            myFile = new FileInfo(Path.Combine(@"C:\test","megaman2.jpg"));
            My_Image = new Image<Hsv, byte>(myFile.FullName);
        }

        private void HelloWorldMethod2()
        {
            //Create an image of 400x200 of Blue color
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)))
            {
                //Create the font
                MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);

                //Draw "Hello, world." on the image using the specific font
                img.Draw("Hello, world", ref f, new Point(10, 80), new Bgr(0, 255, 0));

                //Show the image using ImageViewer from Emgu.CV.UI
                ImageViewer.Show(img, "Test Window");
            }
        }
        public void HelloWorldMethod()
        {
            //The name of the window
            String win1 = "Test Window";

            //Create the window using the specific name
            CvInvoke.cvNamedWindow(win1);

            //Create an image of 400x200 of Blue color
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)))
            {
                //Create the font
                MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);
                //Draw "Hello, world." on the image using the specific font
                img.Draw("Hello, world", ref f, new Point(10, 80), new Bgr(0, 255, 0));

                //Show the image
                CvInvoke.cvShowImage(win1, img.Ptr);
                //Wait for the key pressing event
                CvInvoke.cvWaitKey(0);
                //Destory the window
                CvInvoke.cvDestroyWindow(win1);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //OpenFileDialog Openfile = new OpenFileDialog();
            //if (Openfile.ShowDialog() == DialogResult.OK)
            //{
                //Image<Bgr, Byte> My_Image = new Image<Bgr, byte>(Openfile.FileName);
            
            
                
                pictureBox1.Image = My_Image.ToBitmap();
            //}


        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (counter == 0)
            {
                Image<Gray, Byte> My_Image2 = My_Image.Convert<Gray, Byte>();
                pictureBox1.Image = My_Image2.ToBitmap();
                counter++;
            }
            else if (counter == 1)
            {
                //Image<Gray, Byte> My_Image2 = My_Image.Convert<Hls, Byte>();
                //pictureBox1.Image = My_Image2.ToBitmap();
                //counter++;
            }
        }
    }
}
