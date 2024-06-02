using System;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DetailPreparationMachine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new PrepareDetailSimulation(OnStartedCreation, OnCreated, OnReady);
            DataContext = viewModel;
        }

        private PrepareDetailSimulation viewModel;
        private Random rnd = new Random();

        private List<int> _machineCoordinates = new List<int>()
        { 50, 150, 250 };

        private List<int> _horizontalCoordinates = new List<int>()
        { 60, 260, 460 };

        private List<string> _labels = new List<string>()
        { "Machine", "Miller", "Loader" };

        private List<Color> _colors = new List<Color>()
        { Colors.DarkGreen, Colors.Blue, Colors.Red };

        private List<Color> _detailColors = new List<Color>()
        { Colors.Aquamarine, Colors.Pink, Colors.Violet, Colors.Yellow };

        public void OnStartedCreation(object sender, EventArgs e)
        {
            var detail = (Detail)sender;
            Application.Current.Dispatcher.Invoke(() =>
            {
                int detailIndex = (detail.Id - 7) % (7 + int.Parse(DetailCount.Text));
                int machineIndex = (detail.Id - 7) / (7 + int.Parse(DetailCount.Text));
                Ellipse el = (Ellipse)canvas.Children[detail.Id];
                Rectangle rect = (Rectangle)canvas.Children[(7 + int.Parse(DetailCount.Text)) * machineIndex + 1];
                Label label = new Label();
                label.Content = "Creating";
                label.Foreground = el.Fill;
                Canvas.SetLeft(label, Canvas.GetLeft(rect));
                Canvas.SetTop(label, Canvas.GetTop(rect) + rect.Height + 5 * detailIndex);
                canvas.Children.Add(label);
                
            });
        }

        public void OnCreated(object sender, EventArgs e)
        {
            var detail = (Detail)sender;
            Application.Current.Dispatcher.Invoke(() =>
            {
                int detailIndex = (detail.Id - 7) % (7 + int.Parse(DetailCount.Text));
                int machineIndex = (detail.Id - 7) / (7 + int.Parse(DetailCount.Text));
                Ellipse el = (Ellipse)canvas.Children[detail.Id];
                Rectangle rect = (Rectangle)canvas.Children[(7 + int.Parse(DetailCount.Text)) * machineIndex + 3];
                Label label = new Label();
                label.Content = "Processing";
                label.Foreground = el.Fill;
                Canvas.SetLeft(label, Canvas.GetLeft(rect));
                Canvas.SetTop(label, Canvas.GetTop(rect) + rect.Height + 5*detailIndex);
                canvas.Children.Add(label);

                var translate = el.RenderTransform;
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = Canvas.GetLeft(el),
                    To = Canvas.GetLeft(rect) - rect.Width/2,
                    Duration = TimeSpan.FromSeconds(rnd.Next(2, 5)),
                };

                translate.BeginAnimation(TranslateTransform.XProperty, animation);
            });
        }

        public void OnReady(object sender, EventArgs e)
        {
            var detail = (Detail)sender;
            Application.Current.Dispatcher.Invoke(() =>
            {
                int detailIndex = (detail.Id - 7) % (7 + int.Parse(DetailCount.Text));
                int machineIndex = (detail.Id - 7) / (7 + int.Parse(DetailCount.Text));
                Ellipse el = (Ellipse)canvas.Children[detail.Id];
                Rectangle rect = (Rectangle)canvas.Children[(7 + int.Parse(DetailCount.Text)) * machineIndex + 5];
                Rectangle prevRect = (Rectangle)canvas.Children[(7 + int.Parse(DetailCount.Text)) * machineIndex + 3];
                Label label = new Label();
                label.Content = "Loading";
                label.Foreground = el.Fill;
                Canvas.SetLeft(label, Canvas.GetLeft(rect));
                Canvas.SetTop(label, Canvas.GetTop(rect) + rect.Height + 5*detailIndex);
                canvas.Children.Add(label);

                var translate = el.RenderTransform;
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = Canvas.GetLeft(prevRect) - prevRect.Width/2,
                    To = Canvas.GetLeft(rect) - rect.Width / 2,
                    Duration = TimeSpan.FromSeconds(rnd.Next(2, 5)),
                };

                translate.BeginAnimation(TranslateTransform.XProperty, animation);
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && int.TryParse(MachineCount.Text, out int countM) &&
                int.TryParse(DetailCount.Text, out int countD))
            {

                canvas.Children.Clear();
                for (int i = 0; i < countM; i++)
                {
                    Rectangle rect1 = new Rectangle();
                    rect1.Width = 800;
                    rect1.Height = 50;
                    rect1.Fill = new SolidColorBrush(Colors.DarkGray);
                    Canvas.SetTop(rect1, _machineCoordinates[i]);
                    canvas.Children.Add(rect1);

                    Rectangle rect2 = new Rectangle();
                    Label label1 = new Label();
                    _setUpMachines(rect2, label1, i, 0);

                    Rectangle rect3 = new Rectangle();
                    Label label2 = new Label();
                    _setUpMachines(rect3, label2, i, 1);

                    Rectangle rect4 = new Rectangle();
                    Label label3 = new Label();
                    _setUpMachines(rect4, label3, i, 2);

                    for (int j = 0; j < countD; j++)
                    {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Width = 50;
                        ellipse.Height = 50;
                        ellipse.Fill = new SolidColorBrush(_detailColors[j % _detailColors.Count]);

                        int diff = (i < 2) ? (i+1)*(i+1) : 7;

                        Canvas.SetLeft(ellipse, Canvas.GetLeft(rect2) - rect2.Width/2);
                        Canvas.SetTop(ellipse, Canvas.GetTop(rect2) - 15*diff);

                        Console.WriteLine(Canvas.GetTop(rect2));
                        Console.WriteLine(Canvas.GetTop(ellipse));

                        TranslateTransform translateTransform = new TranslateTransform();
                        translateTransform.X = Canvas.GetLeft(rect2) - rect2.Width/2;
                        translateTransform.Y = Canvas.GetTop(rect2) - 15*diff;

                        ellipse.RenderTransform = translateTransform;

                        canvas.Children.Add(ellipse);

                    }
                }
                viewModel.StartSimulationCommand.Execute(null);
            }
        }

        private void _setUpMachines(Rectangle rect, Label label, int i, int j)
        {
            rect.Width = 50;
            rect.Height = 50;
            rect.Fill = new SolidColorBrush(_colors[j]);
            Canvas.SetTop(rect, _machineCoordinates[i] - 25);
            Canvas.SetLeft(rect, _horizontalCoordinates[j]);
            label.Content = _labels[j];
            Canvas.SetTop(label, _machineCoordinates[i] - 50);
            Canvas.SetLeft(label, _horizontalCoordinates[j]);
            canvas.Children.Add(rect);
            canvas.Children.Add(label);
        }
    }
}
