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

namespace BouncyBall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FootBall[] footBalls = new FootBall[2];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < footBalls.Length; i++)
            {
                footBalls[i] = new FootBall(this, CanvasFootball);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                for (var i = 0; i < footBalls.Length; i++)
                {
                    footBalls[i].Bounce();
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                for (var i = 0; i < footBalls.Length; i++)
                {
                    footBalls[i].StopBounce();
                }
            }
        }
    }
}
