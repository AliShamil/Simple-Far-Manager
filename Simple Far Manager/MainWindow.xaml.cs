using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Path = System.IO.Path;

namespace Simple_Far_Manager;
public partial class MainWindow : Window
{
    private DirectoryInfo currentDirectory;
    private DirectoryInfo currentDirectoryRight;

    public MainWindow()
    {
        InitializeComponent();
        currentDirectory = new DirectoryInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}"); // Replace with your desired directory
        currentDirectoryRight = new DirectoryInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}"); // Replace with your desired directory
        LoadFiles();
        LoadFilesRight();
    }

    private void LoadFiles()
    {
        lvFiles.Items.Clear();

        DirectoryInfo parentDirectory = currentDirectory.Parent;

        if (parentDirectory != null)
        {
            lvFiles.Items.Add(new FileItem { Name = "..", IsFolder = true });
        }

        FileInfo[] files = currentDirectory.GetFiles();
        DirectoryInfo[] dirs = currentDirectory.GetDirectories();

        foreach (DirectoryInfo d in dirs)
        {
            lvFiles.Items.Add(new FileItem { Name = d.Name, Size = "", DateModified = d.LastWriteTime.ToString(), IsFolder = true });
        }

        foreach (FileInfo f in files)
        {
            lvFiles.Items.Add(new FileItem { Name = f.Name, Size = f.Length.ToString(), DateModified = f.LastWriteTime.ToString(), IsFolder = false });
        }
    }

    private void LoadFilesRight()
    {

        lvFilesRight.Items.Clear();
        DirectoryInfo parentDirectory = currentDirectoryRight.Parent;

        if (parentDirectory != null)
        {
            lvFilesRight.Items.Add(new FileItem { Name = "..", IsFolder = true });
        }

        FileInfo[] files = currentDirectoryRight.GetFiles();
        DirectoryInfo[] dirs = currentDirectoryRight.GetDirectories();

        foreach (DirectoryInfo d in dirs)
        {
            lvFilesRight.Items.Add(new FileItem { Name = d.Name, Size = "", DateModified = d.LastWriteTime.ToString(), IsFolder = true });
        }

        foreach (FileInfo f in files)
        {
            lvFilesRight.Items.Add(new FileItem { Name = f.Name, Size = f.Length.ToString(), DateModified = f.LastWriteTime.ToString(), IsFolder = false });
        }
    }

    private class FileItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string DateModified { get; set; }
        public bool IsFolder { get; set; }
    }

    private void lvFiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (lvFiles.SelectedItem != null)
        {
            FileItem selectedItem = (FileItem)lvFiles.SelectedItem;

            if (selectedItem.IsFolder)
            {
                if (selectedItem.Name == "..")
                {
                    currentDirectory = currentDirectory.Parent;
                }
                else
                {
                    currentDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, selectedItem.Name));
                }

                LoadFiles();
            }
            else
            {
                var path = Path.Combine(currentDirectory.FullName, selectedItem.Name);
                //Process.Start(path);
                using Process fileopener = new Process();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }

    private void lvFilesRight_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (lvFilesRight.SelectedItem != null)
        {
            FileItem selectedItem = (FileItem)lvFilesRight.SelectedItem;

            if (selectedItem.IsFolder)
            {
                if (selectedItem.Name == "..")
                {
                    currentDirectoryRight = currentDirectoryRight.Parent;
                }
                else
                {
                    currentDirectoryRight = new DirectoryInfo(Path.Combine(currentDirectoryRight.FullName, selectedItem.Name));
                }

                LoadFilesRight();
            }
            else
            {
                var path = Path.Combine(currentDirectoryRight.FullName, selectedItem.Name);
                //Process.Start(path);
                using Process fileopener = new Process();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }

}
