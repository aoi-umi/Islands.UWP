using System.ComponentModel;
using Windows.UI.Xaml;

namespace Islands.UWP
{
    public class MyGlobal :DependencyObject, INotifyPropertyChanged
    {
        public double TitleFontSize
        {
            get { double value = (double)GetValue(TitleFontSizeProperty); return value == 0 ? 1 : value; }
            set { SetValue(TitleFontSizeProperty, value); MyPropertyChanged("TitleFontSize"); }
        }
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(double), typeof(MyGlobal), null);

        public double ContentFontSize
        {
            get { double value = (double)GetValue(ContentFontSizeProperty); return value == 0 ? 1 : value; }
            set { SetValue(ContentFontSizeProperty, value); MyPropertyChanged("ContentFontSize"); }
        }
        public static readonly DependencyProperty ContentFontSizeProperty = DependencyProperty.Register("ContentFontSize", typeof(double), typeof(MyGlobal), null);

        public double MaskOpacity
        {
            get { double value = (double)GetValue(MaskOpacityProperty); return value; }
            set { SetValue(MaskOpacityProperty, value); MyPropertyChanged("MaskOpacity"); }
        }
        public static readonly DependencyProperty MaskOpacityProperty = DependencyProperty.Register("MaskOpacity", typeof(double), typeof(MyGlobal), null);

        public bool IsAskEachTime
        {
            get { bool value = (bool)GetValue(IsAskEachTimeProperty); return value; }
            set { SetValue(IsAskEachTimeProperty, value); MyPropertyChanged("IsAskEachTime"); }
        }
        public static readonly DependencyProperty IsAskEachTimeProperty = DependencyProperty.Register("IsAskEachTime", typeof(bool), typeof(MyGlobal), null);

        public string BackgroundImagePath
        {
            get { string value = (string)GetValue(BackgroundImagePathProperty); return value; }
            set { SetValue(BackgroundImagePathProperty, value); MyPropertyChanged("BackgroundImagePath"); }
        }
        public static readonly DependencyProperty BackgroundImagePathProperty = DependencyProperty.Register("BackgroundImagePath", typeof(string), typeof(MyGlobal), null);

        public bool IsHideMenu
        {
            get { bool value = (bool)GetValue(IsHideMenuProperty); return value; }
            set { SetValue(IsHideMenuProperty, value); MyPropertyChanged("IsHideMenu"); }
        }
        public static readonly DependencyProperty IsHideMenuProperty = DependencyProperty.Register("IsHideMenu", typeof(bool), typeof(MyGlobal), null);

        public bool NoImage
        {
            get { bool value = (bool)GetValue(NoImageProperty); return value; }
            set { SetValue(NoImageProperty, value); MyPropertyChanged("NoImage"); }
        }
        public static readonly DependencyProperty NoImageProperty = DependencyProperty.Register("NoImage", typeof(bool), typeof(MyGlobal), null);

        public event PropertyChangedEventHandler PropertyChanged;        

        private void MyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
