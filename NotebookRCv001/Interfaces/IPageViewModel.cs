using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotebookRCv001.Interfaces
{
    public interface IPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// коллекция заголовков
        /// </summary>
        public ObservableCollection<string> Headers { get; }
        /// <summary>
        /// коллекция подсказок
        /// </summary>
        public ObservableCollection<string> ToolTips { get; }
        /// <summary>
        /// делегат выполняющийся при готовности Behavior
        /// </summary>
        public Action BehaviorReady { get; set; }
        /// <summary>
        /// команда обработки события окончания загрузки страницы
        /// </summary>
        public ICommand PageLoaded { get; }
        /// <summary>
        /// команда обработки закрытия страницы
        /// </summary>
        public ICommand PageClose { get; }
        /// <summary>
        /// комманда обработки очистки страницы
        /// </summary>
        public ICommand PageClear { get; }
    }
}
