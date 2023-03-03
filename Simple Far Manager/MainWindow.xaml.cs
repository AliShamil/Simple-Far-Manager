﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simple_Far_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext= this;
            Loaded +=PageLoaded;
        }

        public ObservableCollection<string> Files { get; set; } = new ObservableCollection<string>();
      

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            var files = Directory.GetFiles($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}").ToList();
            var dirs = Directory.GetDirectories($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}").ToList();
            foreach (string fileName in files)
                Files.Add(fileName);

        }
    }


}
