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
using System.Windows.Shapes;
using HorseBarn.WPF;

namespace HorseBarn.WPF.Views
{
    /// <summary>
    /// Interaction logic for HorseBarnView.xaml
    /// </summary>
    public partial class HorseBarnView : Window
    {
        public HorseBarnView()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var accentBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
            if (accentBrush != null) accentBrush.Color.CreateAccentColors();
        }
    }
}
