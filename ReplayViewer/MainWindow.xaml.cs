using AwesomenautsReplayParser;
using AwesomenautsReplayParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ReplayViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Tick(object state)
        {
            replayCanvas.TimePoint += new AbsoluteTime(1.0 / 20.0);
            replayCanvas.Dispatcher.Invoke(() => replayCanvas.InvalidateVisual());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            replayCanvas.TimePoint = new AbsoluteTime(31);
            replayCanvas.Replay = new Replay(new DirectoryInfo("r1"));
            replayCanvas.InvalidateVisual();

            timer = new Timer(new TimerCallback(Tick), null, 0, 50);
        }
    }
}
