﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AiOnTheEdgeRestAccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // Changes taken from RS.Fritz.Manager
    // x:ClassModifier="internal" must be included in MainWindow.xaml
    //public partial class MainWindow : Window
    internal sealed partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
        }
    }


}