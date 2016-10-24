using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Islands.UWP
{
    public class BaseContentView : ContentControl
    {
        private static string ProgressRingName = "ProgressRing";
        private static string MaskName = "Mask";
        public BaseContentView():base()
        {
            this.DefaultStyleKey = typeof(BaseContentView);            
        }

        #region DependencyProperty
        public FrameworkElement TopContent
        {
            get { return (FrameworkElement)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(FrameworkElement), typeof(BaseContentView), new PropertyMetadata(null));

        public FrameworkElement BottomContent
        {
            get { return (FrameworkElement)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(FrameworkElement), typeof(BaseContentView), new PropertyMetadata(null));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(BaseContentView), new PropertyMetadata(false));

        public double MaskOpacity
        {
            get { return (double)GetValue(MaskOpacityProperty); }
            set { SetValue(MaskOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaskOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaskOpacityProperty =
            DependencyProperty.Register(nameof(MaskOpacity), typeof(double), typeof(BaseContentView), new PropertyMetadata(0.0));        
        #endregion
        
        private ProgressRing progressRing { get; set; }
        private FrameworkElement MaskView { get; set; }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            MaskView = GetTemplateChild(MaskName) as FrameworkElement;
        }
    }
}
