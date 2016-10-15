using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class CircleMenu : UserControl
    {
        public CircleMenu()
        {
            this.InitializeComponent();
            Init();
            //Loaded += CircleMenu_Loaded;
            //Unloaded += CircleMenu_Unloaded;
        }

        private void CircleMenu_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void CircleMenu_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private const float _posX = 0;
        private const float _posY = 0;
        private void Init()
        {
            _storyboard = new Storyboard();
            _storyboard.Completed += _storyboard_Completed;
            double theta = 0;
            double thetaRadians = 0;
            int count = 10;
            float circleRadius = 80;
            double millSeconds = 500;
            for (int i = 0; i < count; i++)
            {
                var b = new Button()
                {
                    Content = i.ToString(),
                    Width = Menu.Width,
                    Height = Menu.Height,
                    Visibility = Visibility.Collapsed,
                    Style = Application.Current.Resources["EllipseButtonStyle"] as Style
                };
                canvas.Children.Add(b);
                var x = (float)(circleRadius * Math.Cos(thetaRadians)) + _posX;
                var y = (float)(circleRadius * Math.Sin(thetaRadians)) + _posY;
                var animateX = new DoubleAnimation()
                {
                    EnableDependentAnimation = true,
                    EasingFunction = new ExponentialEase { Exponent = 4, EasingMode = EasingMode.EaseOut },
                    Duration = new Duration(TimeSpan.FromMilliseconds(millSeconds)),
                    From = 0,
                    To = x,
                };
                var animateY = new DoubleAnimation
                {
                    EnableDependentAnimation = true,
                    EasingFunction = new ExponentialEase { Exponent = 4, EasingMode = EasingMode.EaseOut },
                    Duration = new Duration(TimeSpan.FromMilliseconds(millSeconds)),
                    From = 0,
                    To = y,
                };
                Storyboard.SetTarget(animateX, b);
                Storyboard.SetTarget(animateY, b);
                Storyboard.SetTargetProperty(animateX, "(Canvas.Left)");
                Storyboard.SetTargetProperty(animateY, "(Canvas.Top)");
                _storyboard.Children.Add(animateX);
                _storyboard.Children.Add(animateY);
                Canvas.SetLeft(b, x);
                Canvas.SetTop(b, y);
                theta += 360 / count;
                thetaRadians = theta * Math.PI / 180F;
            }
        }

        private Storyboard _storyboard { get; set; }
        bool open = false;
        private void Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _storyboard.Stop();
            if (!open)
            {
                foreach (var child in canvas.Children)
                {
                    var element = child as FrameworkElement;
                    if (element != null)
                        element.Visibility = Visibility.Visible;
                }

                foreach (var child in _storyboard.Children)
                {
                    var animate = child as DoubleAnimation;
                    if (animate != null)
                    {
                        animate.EasingFunction.EasingMode = EasingMode.EaseOut;
                        if (animate.To == 0)
                        {
                            animate.To = animate.From;
                            animate.From = 0;
                        }
                    }
                }
            }
            else
            {
                foreach (var child in _storyboard.Children)
                {
                    var animate = child as DoubleAnimation;
                    if (animate != null)
                    {
                        animate.EasingFunction.EasingMode = EasingMode.EaseIn;
                        if (animate.From == 0)
                        {
                            animate.From = animate.To;
                            animate.To = 0;
                        }
                    }
                }
            }
            open = !open;
            _storyboard.Begin();
        }

        private void _storyboard_Completed(object sender, object e)
        {
            if (!open)
            {
                foreach (var child in canvas.Children)
                {
                    var element = child as FrameworkElement;
                    if (element != null)
                        element.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
