
//Multiple face detection and recognition in real time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Sergio Andrés Guitérrez Rojas
//"Serg3ant" for the delveloper comunity
// Sergiogut1805@hotmail.com
//Regards from Bucaramanga-Colombia ;)

// Original found at:
// http://www.codeproject.com/Articles/239849/Multiple-face-detection-and-recognition-in-real-ti

// Modified by Sean OBryan
// Props to Serg3ant
// 12/8/2012
// Email: laserface@latenightyardsale.org

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using Calculon.Networking.Server.Video;
using System.Linq;
using Calculon.Networking.UdpAlerts;
using Calculon.Physical.Communication; // For ienume.Count?

namespace Calculon.FaceDetector
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;


        //Command command = new Command();

        int timer = 0;
        bool moving = false;
        // For the image server
        private ImageStreamingServer _Server;
        private string imageServerLink = string.Format("http://{0}:8080", Environment.MachineName);
        Size formSize;
        Point formLocation;

        public FrmPrincipal()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // For the Image Server
            StartImageServer();
            lblServerLink.Text = "Connect to: " + imageServerLink;


            formSize = new Size(this.Width, this.Height);
            formLocation = this.Location;
            _Server.SetFormLocation(this.Location);
            _Server.SetFormSize(this.Size);
        }

        // For the image Server
        private void StartImageServer()
        {

            //_Server = new ImageStreamingServer(this.Size);
            _Server = new ImageStreamingServer();
            _Server.Start(8080);

        }

        // For the image Server
        private int getClientCount()
        {
            // yes... there is a try catch here.
            // on startup, nullref is thrown because the timer goes off before the
            // the ImageServer has been initialized in the constructor of this form.
            // This is quickest fix.
            try
            {
                return (_Server.Clients != null) ? _Server.Clients.Count() : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            int faces = 0;
            Point centerPoint = new Point(imageBoxFrameGrabber.Width / 2 - 25, imageBoxFrameGrabber.Height / 2 - 25);
            Rectangle centerRect = new Rectangle(centerPoint.X, centerPoint.Y, 50, 50);
            // This is to draw the rect      
            currentFrame.Draw(centerRect, new Bgr(Color.Violet), 2);
            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                UdpBroadcast broadcast = new UdpBroadcast("Go to them on the mountain");
                broadcast.Send();

                faces++;
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);



                Point centerFace = new Point(f.rect.X + (f.rect.Width / 2), f.rect.Y + (f.rect.Height / 2));
                Rectangle faceRect = new Rectangle(centerFace.X, centerFace.Y, 5, 5);
                currentFrame.Draw(faceRect, new Bgr(Color.Yellow), 2);




                if (faces == 1)
                {
                    if (centerRect.Contains(centerFace))
                    {
                        lblInfo.Text = "Position: Centered";
                        lblCorrection.Text = "Adjust: No Problem";
                    }
                    else
                    {

                        lblInfo.Text = "Position: Outside";
                        string correction = String.Empty;
                        correction += "Adjust: ";
                        //Not in the middle - Find the correction
                        if (centerFace.X > centerRect.X + centerRect.Width)
                        {
                            correction += "too Right;";
                            if (!moving)
                            {
                                timer = 0;
                                moving = true;
                                // Commands
                                //command.Execute("left");
                            }
                        }
                        else if (centerFace.X < centerRect.X)
                        {
                            correction += "too Left;";
                            if (!moving)
                            {
                                timer = 0;
                                moving = true;
                                
                                // comands
                                //command.Execute("right");
                            }
                        }
                        if (centerFace.Y > centerRect.Y + centerRect.Height)
                        {
                            correction += "too Low;";
                            if (!moving)
                            {
                                timer = 0;
                                moving = true;
                                // commmands
                                //command.Execute("up");
                            }
                        }
                        else if (centerFace.Y < centerRect.Y)
                        {
                            correction += "too High;";
                            if (!moving)
                            {
                                timer = 0;
                                moving = true;
                                // commands
                                //command.Execute("down");
                            }
                        }

                        lblCorrection.Text = correction;
                    }


                }

                //lblInfo.Text = 

                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();



                /*
                //Set the region of interest on the faces
                        
                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            label4.Text = names;
            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblClientCount.Text = "Clients: " + getClientCount();
            if (timer < 4)
            {
                moving = true;
                timer++;
            }
            if (timer >= 4)
            {
                timer = 0;
                moving = false;
            }
        }

        private void lblServerLink_Click(object sender, EventArgs e)
        {
            _Server.SetFormLocation(this.Location);
            System.Diagnostics.Process.Start(imageServerLink);
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            _Server.SetFormLocation(this.Location);
        }

        private void FrmPrincipal_LocationChanged(object sender, EventArgs e)
        {
            _Server.SetFormSize(this.Size);
            _Server.SetFormLocation(this.Location);
        }

        private void FrmPrincipal_Leave(object sender, EventArgs e)
        {
            _Server.SetFormSize(Size.Empty);
            _Server.SetFormLocation(Point.Empty);
            _Server.Stop();
        }

        private void FrmPrincipal_Enter(object sender, EventArgs e)
        {
            _Server.SetFormSize(this.Size);
            _Server.SetFormLocation(this.Location);
            _Server.Start();
        }





    }
}
