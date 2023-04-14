using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using NotebookRCv001.Infrastructure;

namespace NotebookRCv001.Helpers
{
    public class DirectoryItem : ViewModelBase
    {
        /// <summary>
        /// ключ шифрования если нуждаеется в дешифровке
        /// </summary>
        private string encryptionKey;
        /// <summary>
        /// имя файла/каталога/диска
        /// </summary>
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;
        /// <summary>
        /// флаг: отображать обложку
        /// </summary>
        public bool IsCover { get => isCover; set => SetProperty(ref isCover, value); }
        private bool isCover;
        /// <summary>
        /// объект является файлом
        /// </summary>
        public bool IsDrive { get => isDrive; set => SetProperty(ref isDrive, value); }
        private bool isDrive;
        /// <summary>
        /// текущий объект является файлом
        /// </summary>
        public bool IsFile { get => isFile; private set => SetProperty(ref isFile, value); }
        private bool isFile;
        /// <summary>
        /// объект является папкой
        /// </summary>
        public bool IsFolder { get => isFolder; set => SetProperty(ref isFolder, value); }
        private bool isFolder;
        /// <summary>
        /// файл/директория является системным
        /// </summary>
        public bool IsSystem { get => isSystem; set => SetProperty(ref isSystem, value); }
        private bool isSystem;
        /// <summary>
        /// файл/директория является скрытым
        /// </summary>
        public bool IsHidden { get => isHidden; set => SetProperty(ref isHidden, value); }
        private bool isHidden;
        /// <summary>
        /// расширение файла
        /// </summary>
        public string FileExtension { get => fileExtension; private set => SetProperty(ref fileExtension, value); }
        private string fileExtension;
        /// <summary>
        /// размер файла
        /// </summary>
        public string Size { get => size; private set => SetProperty(ref size, value); }
        private string size;
        /// <summary>
        /// дата и время последнего изменения
        /// </summary>
        public string Date { get => date; private set => SetProperty(ref date, value); }
        private string date;
        /// <summary>
        /// обложка
        /// </summary>
        public BitmapImage Icon { get => icon; set => SetProperty(ref icon, value); }
        private BitmapImage icon;
        /// <summary>
        /// информация о диске/каталоге/файле
        /// </summary>
        public object Tag { get => tag; private set => SetProperty(ref tag, value); }
        private object tag;


        public DirectoryItem(object info, string encryptionKey)
        {
            Tag = info;
            this.encryptionKey = encryptionKey;
            string atributes = null;
            IsFile = false;
            IsFolder = false;
            IsDrive = false;
            IsSystem = false;
            IsHidden = false;
            if (info is DirectoryInfo dir)
            {
                atributes = dir.Attributes.ToString();
                IsFolder = true;
                GetDirectoryInfo(dir);
            }
            else if (info is DriveInfo drive)
            {
                IsDrive = true;
                GetDriveInfo(drive);
            }
            else if (info is FileInfo file)
            {
                atributes = file.Attributes.ToString();
                GetFileInfo(file);
                IsFile = true;
            }
            else
                return;
            if (atributes != null)
            {
                IsSystem = atributes.Contains("System");
                IsHidden = atributes.Contains("Hidden");
            }
        }
        /// <summary>
        /// извлечение информации о диске
        /// </summary>
        /// <param name="driveInfo"></param>
        private void GetDriveInfo(DriveInfo driveInfo)
        {
            Name = driveInfo.Name;
            FileExtension = "Drive";
            Size = driveInfo.TotalFreeSpace.ToString();
            Date = "-----";
        }
        /// <summary>
        /// извлечение информации о каталоге
        /// </summary>
        /// <param name="directoryInfo"></param>
        private void GetDirectoryInfo(DirectoryInfo directoryInfo)
        {
            Name = directoryInfo.Name;
            FileExtension = "Folder";
            Size = "------";
            Date = directoryInfo.LastWriteTime.ToString("MM/dd/yy H:mm:ss");
            //Icon = await Task.Factory.StartNew(()=> RetrievingAnImageFromADirectory(directoryInfo, "001.jpg")).Result;
            IsCover = false;
        }
        /// <summary>
        /// извлечение информации о файле
        /// </summary>
        /// <param name="fileInfo"></param>
        private void GetFileInfo(FileInfo fileInfo)
        {
            Name = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            FileExtension = fileInfo.Extension;
            Size = fileInfo.Length.ToString();
            Date = fileInfo.LastWriteTime.ToString("MM/dd/yy H:mm:ss");
        }
    }
}
