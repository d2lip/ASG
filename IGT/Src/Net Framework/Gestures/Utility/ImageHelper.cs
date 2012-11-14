using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Windows;
using TouchToolkit.GestureProcessor.Objects;


namespace TouchToolkit.GestureProcessor.Utility
{


    public class ImageHelper
    {

        private static double accuracy = 0.9;

        public static void getHandType(out string _type, out string _side, TouchPoint2 point)
        {
            accuracy = 0.9;
            int fingers = ComputeFingersNum(point.Snapshot, out _side);

            if (fingers >= 4)
            {
                _type = TouchHand.OPEN;
                //  _side = TouchHand.RIGHT;           
            }
            else if (fingers == 0 && _side == "")
            {
                _type = "";
            }
            else
            {

                _type = TouchHand.VERTICAL;

            }


            //   _side = TouchHand.RIGHT;

        }

        public static Image<Bgr, Byte> getBetterImage(List<Bitmap> imgs, TouchPoint2 point)
        {

            int maxNonZero = 0;

            if (imgs.Count == 0)
                return null;

            int x = Convert.ToInt32(point.Bounds.Location.X) - 350;
            if (x < 0)
                x = 0;
            int y = Convert.ToInt32(point.Bounds.Location.Y) - 300;
            if (y < 0)
                y = 0;
            int width = Convert.ToInt32(point.Bounds.Width);
            int height = Convert.ToInt32(point.Bounds.Height);
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(x, y, 400, 400);

            Image<Bgr, Byte> bestImage = null;

            foreach (Bitmap img in imgs)
            {
                Image<Bgr, Byte> bitImg = new Image<Bgr, byte>(img);
                bitImg.ROI = r;
                int count = bitImg.CountNonzero()[0];
                if (count > maxNonZero)
                {
                    maxNonZero = count;
                    bestImage = bitImg;
                }
            }


            return bestImage;
        }

        private static bool MatchImages(Image<Gray, Byte> observedImage, Bitmap reference)
        {
            MCvSURFParams surfParam = new MCvSURFParams(500, false);
            Image<Gray, Byte> modelImage = new Image<Gray, byte>(reference);

            //extract features from the object image
            SURFFeature[] modelFeatures = modelImage.ExtractSURF(ref surfParam);

            //Create a SURF Tracker
            SURFTracker tracker = new SURFTracker(modelFeatures);

            // extract features from the observed image
            SURFFeature[] imageFeatures = observedImage.ExtractSURF(ref surfParam);

            SURFTracker.MatchedSURFFeature[] matchedFeatures = tracker.MatchFeature(imageFeatures, 2, 20);
            matchedFeatures = SURFTracker.VoteForUniqueness(matchedFeatures, accuracy);
            matchedFeatures = SURFTracker.VoteForSizeAndOrientation(matchedFeatures, 1.5, 20);
            HomographyMatrix homography = SURFTracker.GetHomographyMatrixFromMatchedFeatures(matchedFeatures);

            if (homography != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static Bitmap Convert8bppBMPToGrayscale(Bitmap bmp)
        {
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                pal.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            }

            bmp.Palette = pal;
            return bmp;

        }


        public static Bitmap Binarize(Bitmap bmp)
        {
            double threshold = 0.05;
            ColorPalette pal = bmp.Palette;

            for (int i = 0; i < pal.Entries.Count(); i++)
            {
                if (pal.Entries[i].GetBrightness() > threshold)
                    pal.Entries[i] = Color.White;
                else
                    pal.Entries[i] = Color.Black;
            }
            bmp.Palette = pal;

            return bmp;


        }
        /*

          void FrameGrabber(object sender, EventArgs e)
        {
            currentFrame = grabber.QueryFrame();
            if (currentFrame != null)
            {
                currentFrameCopy = currentFrame.Copy();
                
                // Uncomment if using opencv adaptive skin detector
                //Image<Gray,Byte> skin = new Image<Gray,byte>(currentFrameCopy.Width,currentFrameCopy.Height);                
                //detector.Process(currentFrameCopy, skin);                

                skinDetector = new YCrCbSkinDetector();
                
                Image<Gray, Byte> skin = skinDetector.DetectSkin(currentFrameCopy,YCrCb_min,YCrCb_max);

                ExtractContourAndHull(skin);
                                
                DrawAndComputeFingersNum();

                imageBoxSkin.Image = skin;
                imageBoxFrameGrabber.Image = currentFrame;
            }
        }
               */

        private static bool isVertical(Rectangle rect)
        {
            return (rect.Width < rect.Height * 0.45);
        }

        public static Bitmap BinarizeImage(Bitmap bmp)
        {

            bmp = Convert8bppBMPToGrayscale(bmp);
            return Binarize(bmp);


        }




        public static TouchImage ExtractContourAndHull(Image<Bgr, Byte> newImg)
        {
            TouchImage touchImage = new TouchImage();
            touchImage.Image = newImg;
            //Image<Gray, byte> skin = new Image<Gray, byte>(newImg);
            using (MemStorage storage = new MemStorage())
            {
                Image<Gray, Byte> grayImage = touchImage.Image.Convert<Gray, Byte>();
                Contour<System.Drawing.Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                Contour<System.Drawing.Point> biggestContour = null;

                Double Result1 = 0;
                Double Result2 = 0;
                while (contours != null)
                {
                    Result1 = contours.Area;
                    if (Result1 > Result2)
                    {
                        Result2 = Result1;
                        biggestContour = contours;
                    }
                    contours = contours.HNext;
                }

                if (biggestContour != null)
                {
                    //   currentFrame.Draw(biggestContour, new Bgr(Color.DarkViolet), 2);
                    Contour<System.Drawing.Point> currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, storage);
                    //  currentFrame.Draw(currentContour, new Bgr(Color.LimeGreen), 2);
                    //   currentFrame.Draw(currentContour, new Bgr(Color.Red), 2);
                    biggestContour = currentContour;


                    //      Seq<System.Drawing.Point> hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                    touchImage.Box = biggestContour.GetMinAreaRect();

                    /*
                    
                                        PointF[] points = touchImage.Box.GetVertices();
                                        Rectangle handRect = touchImage.Box.MinAreaRect();
                                        touchImage.Image.Draw(handRect, new Bgr(200, 0, 0), 1);
                                        touchImage.Image.Save(".\\aft\\BEFORE" + DateTime.Now.ToString("hh-mm-ss-fff") + ".jpg");*/
                    /*   System.Drawing.Point[] ps = new System.Drawing.Point[points.Length];
                       for (int i = 0; i < points.Length; i++)
                           ps[i] = new System.Drawing.Point((int)points[i].X, (int)points[i].Y);
                       */


                    // currentFrame.DrawPolyline(hull.ToArray(), true, new Bgr(200, 125, 75), 2);
                    //currentFrame.DrawPolyline(hull.ToArray(), true, new Bgr(Color.White), 2);
                    //  currentFrame.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2);

                    /*  Seq<System.Drawing.Point> filteredHull = new Seq<System.Drawing.Point>(storage);
                        for (int i = 0; i < hull.Total; i++)
                        {
                            if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > touchImage.Box.size.Width / 10)
                            {
                                filteredHull.Push(hull[i]);
                            }
                        }
                        */

                    touchImage.Defects = biggestContour.GetConvexityDefacts(storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE).ToArray();
                    //   touchImage.Image.Save(".\\aft\\BEFORE" + DateTime.Now.ToString("hh-mm-ss-fff") + ".jpg");                   
                    //     string orientatio = "";
                    //   ComputeFingersNum(touchImage, out  orientatio);
                }
                storage.Dispose();
            }
            return touchImage;

        }

        public static int ComputeFingersNum(TouchImage touchImage, out string orientation)
        {

            if (touchImage == null)
            {
                orientation = "";
                return 0;
            }

            if (touchImage.Box.MinAreaRect().Width * touchImage.Box.MinAreaRect().Height < 12000)
            {
                orientation = "";
                return 0;
            }


            if (isVertical(touchImage.Box.MinAreaRect()))
            {
                orientation = "";
                return 1;
            }

            int fingerNum = 1;
            orientation = "";
            PointF LeftEdge = touchImage.Box.center;
            double lowest = 0;
            if (touchImage.Defects == null)
            {
                return 0;

            }
            // MCvConvexityDefect[] defects = touchImage.Defects.ToArray();
            for (int i = 0; i < touchImage.Defects.Count(); i++)
            {

                PointF startPoint = new PointF((float)touchImage.Defects[i].StartPoint.X,
                                                        (float)touchImage.Defects[i].StartPoint.Y);

                PointF depthPoint = new PointF((float)touchImage.Defects[i].DepthPoint.X,
                                                (float)touchImage.Defects[i].DepthPoint.Y);

                PointF endPoint = new PointF((float)touchImage.Defects[i].EndPoint.X,
                                                (float)touchImage.Defects[i].EndPoint.Y);

                LineSegment2D startDepthLine = new LineSegment2D(touchImage.Defects[i].StartPoint, touchImage.Defects[i].DepthPoint);

                LineSegment2D depthEndLine = new LineSegment2D(touchImage.Defects[i].DepthPoint, touchImage.Defects[i].EndPoint);

                CircleF startCircle = new CircleF(startPoint, 5f);

                CircleF depthCircle = new CircleF(depthPoint, 5f);

                CircleF endCircle = new CircleF(endPoint, 5f);



                //Custom heuristic based on some experiment, double check it before use
                if ((startCircle.Center.Y < touchImage.Box.center.Y
                    || depthCircle.Center.Y < touchImage.Box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y)
                    && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2)
                        + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > touchImage.Box.size.Height / 6.5))
                {
                    fingerNum++;


                    if (startCircle.Center.Y > lowest)
                    {
                        lowest = startCircle.Center.Y;
                        if (startCircle.Center.X < depthCircle.Center.X)
                            orientation = TouchHand.LEFT;
                        else
                            orientation = TouchHand.RIGHT;

                    }




                    //         touchImage.Image.Draw(startDepthLine, new Bgr(Color.Green), 2);
                    //        touchImage.Image.Draw(depthEndLine, new Bgr(Color.Pink), 2);
                }

                //  MCvFont fontv = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_DUPLEX, 0.5, 0.5);
                //   currentFrame.Draw("X:" + startCircle.Center.X + "  Y:" + startCircle.Center.Y, ref fontv, new System.Drawing.Point(System.Convert.ToInt16(startCircle.Center.X), System.Convert.ToInt16(startCircle.Center.Y)), new Bgr(Color.Red));
                //        touchImage.Image.Draw(startCircle, new Bgr(Color.Red), 2);
                //       touchImage.Image.Draw(depthCircle, new Bgr(Color.Yellow), 5);
                //       touchImage.Image.Draw(endCircle, new Bgr(Color.DarkBlue), 4);




            }






            //        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_DUPLEX, 5d, 5d);
            //       touchImage.Image.Draw(fingerNum.ToString(), ref font, new System.Drawing.Point(50, 150), new Bgr(Color.White));
            //      touchImage.Image.Save(".\\aft\\IMGTULIO" + DateTime.Now.ToString("hh-mm-ss-fff") + ".jpg");


            return fingerNum;



        }







    }
}
