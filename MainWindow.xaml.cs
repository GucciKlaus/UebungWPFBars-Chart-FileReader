using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace UebungWPFBars_Chart_FileReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Patient> PatientListe { get; set; }
        public double Radius { get; set; }
        public Line? needleLine { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PatientListe = new List<Patient>();

        }
        private void closeMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofD = new OpenFileDialog();
            if (ofD.ShowDialog() == true)
            {
                String path = ofD.FileName;

                StreamReader sr = new StreamReader(path);
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    if (Patient.TryParse(line, out Patient temp))
                    {
                        PatientListe.Add(temp);
                        Debug.WriteLine(temp.Pointers.Count);
                    }

                }
                FillTreeView();
            }
        }

        private void FillTreeView()
        {
            treeViewDoc.Items.Clear();
            if (PatientListe != null)
            {
                foreach (Patient temp in PatientListe)
                {
                    TreeViewItem treeViewItem = new TreeViewItem();
                    treeViewItem.Header = temp.ID;
                    treeViewItem.Items.Add(temp.ToString());
                    treeViewDoc.Items.Add(treeViewItem);
                }
            }
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void canvasApp_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasApp.Children.Clear();
            Radius = canvasApp.ActualWidth / 2 > canvasApp.ActualHeight * 0.9 ? canvasApp.ActualHeight * 0.9 : canvasApp.ActualWidth / 2;
            RedrawNeedle();
            RedrawScale();
            RotateNeedle(slider.Value);
        }

        private void RedrawNeedle()
        {
            needleLine = new Line
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.AliceBlue),
                X1 = canvasApp.ActualWidth * 0.9,
                Y1 = canvasApp.ActualHeight * 0.9,
                X2 = canvasApp.ActualWidth * 0.9 - (Radius * 0.66),
                Y2 = canvasApp.ActualHeight * 0.9
            };
            canvasApp.Children.Add(needleLine);
        }

        public void RotateNeedle(double value)
        {
            if (needleLine != null)
            {
                needleLine.RenderTransform = new RotateTransform(value * 10, canvasApp.ActualWidth * 0.9, canvasApp.ActualHeight * 0.9);

            }
        }

        private void RedrawScale()
        {
            for (int angle = 0; angle <= 90; angle += 10)
            {
                Line scaleLine = new Line
                {
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.AliceBlue),
                    X1 = canvasApp.ActualWidth * 0.9 - (Radius * 0.75),
                    Y1 = canvasApp.ActualHeight * 0.9,
                    X2 = canvasApp.ActualWidth * 0.9 - (Radius * 0.7),
                    Y2 = canvasApp.ActualHeight * 0.9
                };
                scaleLine.RenderTransform = new RotateTransform(angle, canvasApp.ActualWidth * 0.9, canvasApp.ActualHeight * 0.9);
                canvasApp.Children.Add(scaleLine);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotateNeedle(slider.Value);
        }
    }
}
