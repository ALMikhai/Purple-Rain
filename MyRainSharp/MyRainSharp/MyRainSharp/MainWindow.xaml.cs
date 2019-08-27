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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyRainSharp
{
    public partial class MainWindow : Window
    {
        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            for (var i = 0; i < 400; i++)
            {
                Draw();
            }
        }

        public void Draw()
        {
            Rectangle rectangle = new Rectangle
            {
                Fill = Brushes.Purple,
                Height = random.Next(8, 25),
                Width = random.Next(1, 4),
            };

            Storyboard storyboard = new Storyboard();

            SetStoryboard(storyboard, rectangle);

            StartAnimation(storyboard);
        }

        public void SetStoryboard(Storyboard storyboard, Rectangle rectangle)
        {
            double weightRatio = 4;

            MyCanvas.Children.Add(rectangle);

            var pathGeom = new PathGeometry();
            var vertPF = new PathFigure();
            var vertLS = new LineSegment();

            var startPoint = new Point(random.Next((int)MyCanvas.Width), random.Next(-300, -70));
            var finishPoint = new Point(startPoint.X, (int)MyCanvas.Height + 20);

            TimeSpan durationAnimation = TimeSpan.FromSeconds(Math.Sqrt((2 * (finishPoint.Y - startPoint.Y)) / (rectangle.Width * rectangle.Height * weightRatio)));

            vertPF.StartPoint = startPoint;
            vertLS.Point = finishPoint;

            vertPF.Segments.Add(vertLS);
            pathGeom.Figures.Add(vertPF);

            var moveCircleAnimation = new DoubleAnimationUsingPath
            {
                PathGeometry = pathGeom,
                Source = PathAnimationSource.X,
                Duration = durationAnimation,
            };

            Storyboard.SetTarget(moveCircleAnimation, rectangle);
            Storyboard.SetTargetProperty(moveCircleAnimation, new PropertyPath(Canvas.LeftProperty));

            var moveCircleAnimation2 = new DoubleAnimationUsingPath
            {
                PathGeometry = pathGeom,
                Source = PathAnimationSource.Y,
                Duration = durationAnimation,
            };

            Storyboard.SetTarget(moveCircleAnimation2, rectangle);
            Storyboard.SetTargetProperty(moveCircleAnimation2, new PropertyPath(Canvas.TopProperty));

            storyboard.Children.Add(moveCircleAnimation);
            storyboard.Children.Add(moveCircleAnimation2);

            EventHandler Storyboard_Completed = null;

            Storyboard_Completed = (o, args) =>
            {
                MyCanvas.Children.Remove(rectangle);
                Draw();
            };

            storyboard.Completed += Storyboard_Completed;
        }

        public void StartAnimation(Storyboard newStoryboard)
        {
            newStoryboard.Begin();
        }
    }
}
