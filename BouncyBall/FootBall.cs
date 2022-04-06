using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BouncyBall
{
    internal class FootBall
    {
        private readonly Canvas _canvas;
        private readonly Window _window;
        private readonly Image _image;
        private Thread _thread;
        private CancellationTokenSource _cts;

        public FootBall(Window window, Canvas canvas)
        {
            _window = window;
            _canvas = canvas;
            _image = CreatePictureBoxWithFootBallImage();
        }

        public void Bounce()
        {
            if (_thread == null)
            {
                _thread = new Thread(WorkerThreadBounceBall);

                _cts = new CancellationTokenSource();
                _thread.Start(_cts.Token);
            }
        }

        internal void StopBounce()
        {
            if (_thread != null)
            {
                _cts.CancelAfter(10);
                _thread = null;
            }
        }

        private void WorkerThreadBounceBall(object obj)
        {
            var cancellationToken = (CancellationToken)obj;

            var random = new Random();
            var x = random.Next(0, (int)(_window.ActualWidth - _image.ActualWidth));
            var y = random.Next(0, (int)(_window.ActualHeight - _image.ActualHeight));

            var position = new Point(x, y);

            var xFactor = random.Next(5, 20);
            var yFactor = random.Next(5, 20);

            do
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                position.X += xFactor;
                position.Y += yFactor;

                // The imaginary line is used so the ball does not fully go
                // out of the window before bouncing back.
                // If we bounce the left and top of the ball on the imaginary line
                // the ball always stays visible inside the form

                double rightImaginaryLine = 0, bottomImaginaryLine = 0;

                _image.Dispatcher.Invoke(new Action(() =>
                {
                    rightImaginaryLine = _window.ActualWidth - _image.ActualWidth;
                    bottomImaginaryLine = _window.ActualHeight - _image.ActualHeight;
                }));

                if (position.X < 0 || position.X > rightImaginaryLine)
                    xFactor = -xFactor;

                if (position.Y < 0 || position.Y > bottomImaginaryLine)
                    yFactor = -yFactor;

                _image.Dispatcher.Invoke(new Action(() =>
                {
                    Canvas.SetLeft(_image, position.X);
                    Canvas.SetTop(_image, position.Y);
                }));

                Thread.Sleep(10);

            } while (true);
        }

        private Image CreatePictureBoxWithFootBallImage()
        {
            var imageFootBall = new Image
            {
                Source = new BitmapImage(new Uri(@"pack://application:,,,/images/football.jpg")),
                Stretch = System.Windows.Media.Stretch.Fill,
            };

            Canvas.SetLeft(imageFootBall, 100);
            Canvas.SetTop(imageFootBall, 100);

            _canvas.Children.Add(imageFootBall);

            return imageFootBall;
        }
    }
}