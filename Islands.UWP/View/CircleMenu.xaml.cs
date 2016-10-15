using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class CircleMenu : UserControl
    {
        private List<AppBarButton> MenuItems { get; set; }
        public CircleMenu()
        {
            this.InitializeComponent();
            double width = 28, height = 28;
            Style style = Application.Current.Resources["EllipseAppBarButtonStyle"] as Style;
            SolidColorBrush background = Application.Current.Resources["SystemControlHighlightAltListAccentLowBrush"] as SolidColorBrush;
            MenuItems = new List<AppBarButton>() {
                new AppBarButton() {
                    Content = MenuType.home,
                    Icon = new FontIcon() {  Glyph = "\uE80F"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background,                                        
                },new AppBarButton() {
                    Content = MenuType.mark,
                    Icon = new FontIcon() {  Glyph = "\uE728"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },new AppBarButton() {
                    Content = MenuType.myreply,
                    Icon = new FontIcon() {  Glyph = "\uE120"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },new AppBarButton() {
                    Content = MenuType.image,
                    Icon = new FontIcon() {  Glyph = "\uEB9F"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },new AppBarButton() {
                    Content = MenuType.forums,
                    Icon = new FontIcon() {  Glyph = "\uE169"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },new AppBarButton() {
                    Content = MenuType.gotothread,
                    Icon = new FontIcon() {  Glyph = "\uE8AD"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },new AppBarButton() {
                    Content = MenuType.setting,
                    Icon = new FontIcon() {  Glyph = "\uE713"},
                    Width = width,
                    Height = height,
                    Visibility = Visibility.Collapsed,
                    Style = style,
                    Background = background
                },
            };
            Init();
            //Loaded += CircleMenu_Loaded;
            //Unloaded += CircleMenu_Unloaded;
        }

        public delegate void MenuItemTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event MenuItemTappedEventHandler MenuItemTapped;

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
            int count = MenuItems.Count;
            float circleRadius = 80;
            double millSeconds = 500;
            for (int i = 0; i < count; i++)
            {
                var b = MenuItems[i];
                b.Tapped += MenuItem_Tapped;
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

        private void MenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StartStoryBoard();
            MenuItemTapped?.Invoke(sender, e);
        }

        private Storyboard _storyboard { get; set; }
        bool IsOpen = false;
        private void Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StartStoryBoard();
        }

        private void StartStoryBoard()
        {
            _storyboard.Stop();
            if (!IsOpen)
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
            IsOpen = !IsOpen;
            _storyboard.Begin();
        }

        private void _storyboard_Completed(object sender, object e)
        {
            if (!IsOpen)
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
