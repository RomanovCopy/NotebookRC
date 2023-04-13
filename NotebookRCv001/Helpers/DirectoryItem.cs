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
            IsFile = false;
            IsFolder = false;
            IsDrive = false;
            if (info is DirectoryInfo dir)
            {
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
                GetFileInfo(file);
                IsFile = true;
            }
            else
                return;
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
        /// <summary>
        /// извлечение изображения с заданным именем из заданного каталога
        /// </summary>
        /// <param name="dir">каталог в котором находится изображение</param>
        /// <param name="imageName">имя изображения вместе с расширением(.jpg)</param>
        /// <returns></returns>
        private async Task<BitmapImage> RetrievingAnImageFromADirectory(DirectoryInfo dir, string imageName)
        {
            BitmapImage bitmap = null;
            try
            {
                string path = Path.Combine(dir.FullName, imageName);
                if (File.Exists(path) && Path.GetExtension(path) == ".jpg")
                {
                    if (dir.GetFiles().Any((x) => x.Name == imageName))
                    {
                        if (!string.IsNullOrWhiteSpace(encryptionKey))
                            bitmap = await Command_executors.Executors.ImageDecrypt(path, encryptionKey, 24);
                        else
                        {
                            using (FileStream fs = new(path, FileMode.Open))
                            {
                                await Task.Factory.StartNew(() =>
                                {
                                    bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = fs;
                                    bitmap.DecodePixelHeight = 32;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();
                                    bitmap.Freeze();
                                });
                            }
                        }
                    }
                }
                return bitmap;
            }
            catch (Exception e) { ErrorWindow(e); return bitmap; }
        }
    }
}
