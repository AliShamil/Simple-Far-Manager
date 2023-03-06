using FileManager.Commands;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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


       // OpenCommand = new RelayCommand(ExecuteOpenCommand, CanEcexuteCommand);
        
       // MoveCommand = new RelayCommand(ExecuteCopyCommand, CanEcexuteCommand);
       // DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanEcexuteCommand);
       // PropertiesCommand = new RelayCommand(ExecutePasteCommand, CanPasteEcexuteCommand);

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

    private void lvFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenLeft();
    }

    private void OpenLeft()
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

                using Process fileopener = new Process();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }

    private void lvFilesRight_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        OpenRight();
    }

    private void OpenRight()
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

                using Process fileopener = new Process();

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }
    }





    private void OpenLeft_Click(object sender, RoutedEventArgs e)
    {
        OpenLeft();
    }

    private void OpenRight_Click(object sender, RoutedEventArgs e)
    {
        OpenRight();
    }

    #region OpenPropertiesDialog
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHELLEXECUTEINFO
    {
        public int cbSize;
        public uint fMask;
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpVerb;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpParameters;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpDirectory;
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpClass;
        public IntPtr hkeyClass;
        public uint dwHotKey;
        public IntPtr hIcon;
        public IntPtr hProcess;
    }

    private const int SW_SHOW = 5;
    private const uint SEE_MASK_INVOKEIDLIST = 12;
    public static bool ShowFileProperties(string Filename)
    {
        SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
        info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
        info.lpVerb = "properties";
        info.lpFile = Filename;
        info.nShow = SW_SHOW;
        info.fMask = SEE_MASK_INVOKEIDLIST;
        return ShellExecuteEx(ref info);
    }

    #endregion

    private void PropertiesLeft_Click(object sender, RoutedEventArgs e)
    {
        var path = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));
        ShowFileProperties(path);
    }

    private void PropertiesRight_Click(object sender, RoutedEventArgs e)
    {
        var path = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));
        ShowFileProperties(path);
    }

    private void DeleteLeft_Click(object sender, RoutedEventArgs e)
    {
        if (lvFiles.SelectedItem != null)
        {
            FileItem selectedItem = (FileItem)lvFiles.SelectedItem;

            if (selectedItem.IsFolder)
            {
                var path = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));
                FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

            }
            else
            {
                var path = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

            }
            LoadFiles();
            LoadFilesRight();
        }
    }

    private void DeleteRight_Click(object sender, RoutedEventArgs e)
    {

        if (lvFilesRight.SelectedItem != null)
        {
            FileItem selectedItem = (FileItem)lvFilesRight.SelectedItem;

            if (selectedItem.IsFolder)
            {
                var path = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));
                FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                
            }
            else
            {
                var path = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                
            }
            LoadFiles();
            LoadFilesRight();
        }
    }

    private void MoveLeft_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lvFiles.SelectedItem != null)
            {
                FileItem selectedItem = (FileItem)lvFiles.SelectedItem;

                if (selectedItem.IsFolder)
                {
                    if (!Directory.Exists(currentDirectoryRight.FullName))
                    {
                        Directory.CreateDirectory(currentDirectoryRight.FullName);
                    }

                    var path = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));
                    
                    Directory.Move(path, Path.Combine(currentDirectoryRight.FullName, Path.GetFileName(path)));

                }
                else
                {
                    var path = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));
                    if (!Directory.Exists(currentDirectoryRight.FullName))
                    {
                        Directory.CreateDirectory(currentDirectoryRight.FullName);
                    }

                    // Get the file name and destination file path
                    string fileName = Path.GetFileName(path);
                    string destFilePath = Path.Combine(currentDirectoryRight.FullName, fileName);

                    // Move the file
                    File.Move(path, destFilePath);

                }
                LoadFiles();
                LoadFilesRight();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            
        }
        
    }

    private void MoveRight_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lvFilesRight.SelectedItem != null)
            {
                FileItem selectedItem = (FileItem)lvFilesRight.SelectedItem;

                if (selectedItem.IsFolder)
                {
                    if (!Directory.Exists(currentDirectory.FullName))
                    {
                        Directory.CreateDirectory(currentDirectory.FullName);
                    }

                    var path = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));

                    Directory.Move(path, Path.Combine(currentDirectory.FullName, Path.GetFileName(path)));

                }
                else
                {
                    var path = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));
                    if (!Directory.Exists(currentDirectory.FullName))
                    {
                        Directory.CreateDirectory(currentDirectory.FullName);
                    }

                    // Get the file name and destination file path
                    string fileName = Path.GetFileName(path);
                    string destFilePath = Path.Combine(currentDirectory.FullName, fileName);

                    // Move the file
                    File.Move(path, destFilePath);

                }
                LoadFiles();
                LoadFilesRight();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);

        }

    }

    private void CopyLeft_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (lvFiles.SelectedItem != null)
            {
                FileItem selectedItem = (FileItem)lvFiles.SelectedItem;
                var destDirPath = currentDirectoryRight.FullName;
                var sourceDirPath = (Path.Combine(currentDirectory.FullName, (lvFiles.SelectedItem as FileItem).Name));

                if (selectedItem.IsFolder)
                {
                    if (!Directory.Exists(destDirPath))
                    {
                        Directory.CreateDirectory(destDirPath);
                    }

                    // Copy the directory and its contents recursively
                    foreach (string dirPath in Directory.GetDirectories(sourceDirPath, "*", System.IO.SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(Path.Combine(destDirPath, dirPath.Substring(sourceDirPath.Length + 1)));
                    }

                    foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", System.IO.SearchOption.AllDirectories))
                    {
                        File.Copy(filePath, Path.Combine(destDirPath, filePath.Substring(sourceDirPath.Length + 1)));
                    }

                }
                else
                {
                    if (!Directory.Exists(destDirPath))
                    {
                        Directory.CreateDirectory(destDirPath);
                    }

                    // Get the file name and destination file path
                    string fileName = Path.GetFileName(sourceDirPath);
                    string destFilePath = Path.Combine(destDirPath, fileName);

                    // Copy the file
                    File.Copy(sourceDirPath, destFilePath);

                }
                LoadFiles();
                LoadFilesRight();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);

        }


    }

    private void CopyRight_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            if (lvFilesRight.SelectedItem != null)
            {
                FileItem selectedItem = (FileItem)lvFilesRight.SelectedItem;
                var destDirPath = currentDirectory.FullName;
                var sourceDirPath = (Path.Combine(currentDirectoryRight.FullName, (lvFilesRight.SelectedItem as FileItem).Name));

                if (selectedItem.IsFolder)
                {
                    if (!Directory.Exists(destDirPath))
                    {
                        Directory.CreateDirectory(destDirPath);
                    }

                    // Copy the directory and its contents recursively
                    foreach (string dirPath in Directory.GetDirectories(sourceDirPath, "*", System.IO.SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(Path.Combine(destDirPath, dirPath.Substring(sourceDirPath.Length + 1)));
                    }

                    foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", System.IO.SearchOption.AllDirectories))
                    {
                        File.Copy(filePath, Path.Combine(destDirPath, filePath.Substring(sourceDirPath.Length + 1)));
                    }

                }
                else
                {
                    if (!Directory.Exists(destDirPath))
                    {
                        Directory.CreateDirectory(destDirPath);
                    }

                    // Get the file name and destination file path
                    string fileName = Path.GetFileName(sourceDirPath);
                    string destFilePath = Path.Combine(destDirPath, fileName);

                    // Copy the file
                    File.Copy(sourceDirPath, destFilePath);

                }
                LoadFiles();
                LoadFilesRight();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);

        }
    }
}
