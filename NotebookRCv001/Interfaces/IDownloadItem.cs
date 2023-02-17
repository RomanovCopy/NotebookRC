using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotebookRCv001.Interfaces
{
    public interface IDownloadItem
    {
        /// <summary>
        /// Возвращает предложенное имя файла.
        /// </summary>
        public string SuggestedFileName { get; set; }
        /// <summary>
        /// Возвращает URL-адрес, который был до перенаправления.
        /// </summary>
        public string OriginalUrl { get; set; }
        /// <summary>
        /// Возвращает URL.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Возвращает уникальный идентификатор для этой загрузки.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Возвращает полный путь к загруженному или загружаемому файлу.
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// Возвращает время окончания загрузки.
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// Возвращает время начала загрузки.
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Возвращает количество полученных байтов.
        /// </summary>
        public long ReceivedBytes { get; set; }
        /// <summary>
        /// Возвращает общее количество байтов.
        /// </summary>
        public long TotalBytes { get; set; }
        /// <summary>
        /// Возвращает приблизительный процент выполнения или -1, если общий размер приема неизвестен.
        /// </summary>
        public int PercentComplete { get; set; }
        /// <summary>
        /// Возвращает простую оценку скорости в байтах / с.
        /// </summary>
        public long CurrentSpeed { get; set; }
        /// <summary>
        /// Возвращает истину, если загрузка была отменена или прервана.
        /// </summary>
        public bool IsCancelled { get; set; }
        /// <summary>
        /// Возвращает true, если загрузка завершена.
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// Возвращает true, если загрузка выполняется.
        /// </summary>
        public bool IsInProgress { get; set; }
        /// <summary>
        /// Возвращает истину, если этот объект действителен.
        /// Не вызывайте никакие другие методы, если эта функция возвращает false.
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Возвращает расположение содержимого.
        /// </summary>
        public string ContentDisposition { get; set; }
        /// <summary>
        /// Возвращает тип пантомимы.
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// Статус загрузки данного элемента
        /// </summary>
        public string Status { get; }
        /// <summary>
        /// команда подготовки к загрузке файла
        /// </summary>
        public ICommand Preparation { get; }
        /// <summary>
        /// команда запуска загрузки
        /// </summary>
        public ICommand Start { get; }
        /// <summary>
        /// команда принудительной паузы в загрузке
        /// </summary>
        public ICommand Pause { get; }
        /// <summary>
        /// команда принудительного окончания загрузки
        /// </summary>
        public ICommand Stop { get; }
    }
}
