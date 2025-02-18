using System.Windows;
using System.Windows.Controls;

namespace LitematicaPreviewer.Controls.UserControls
{
    /// <summary>
    /// Card.xaml 的交互逻辑
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }

        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(Card), new PropertyMetadata(default,OnPropChanged));

        public new object Content
        {
            get => GetValue(ContentProperty);
            set
            {
                SetValue(ContentProperty, value);
                CardContent.Content = value;
            }
        }

        public static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Card card) card.Content = e.NewValue;
        }
    }
}
