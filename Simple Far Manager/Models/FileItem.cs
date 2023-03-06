namespace Simple_Far_Manager.Models;
#nullable disable

public class FileItem
{
    public string Name { get; set; }
    public string Size { get; set; }
    public string DateModified { get; set; }
    public bool IsFolder { get; set; }
}

