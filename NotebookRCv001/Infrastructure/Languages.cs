using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NotebookRCv001.Infrastructure
{
    public class Languages : ViewModelBase
    {
        public string Key
        {
            get => key;
            set
            {
                if (LanguagesKey.Any((x) => x == value))
                    SetProperty(ref key, value);
            }
        }
        string key;

        public ObservableCollection<string> LanguagesKey
        {
            get => languagesKey;
            set => SetProperty(ref languagesKey, value);
        }
        private ObservableCollection<string> languagesKey;


        public Languages()
        {
            LanguagesKey = new ObservableCollection<string>() { "ru-RU", "en-US" };
            if (LanguagesKey.Any((x) => x == Properties.Settings.Default.LanguageKey))
                Key = Properties.Settings.Default.LanguageKey;
            else
                Key = "en-US";
        }

        #region _________________________MainWindow________________________________
        /// <summary>
        /// заголовок окна
        /// </summary>
        public ObservableCollection<string> ToolTipsMainWindow => DictionaryToolTipsMainWindow[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsMainWindow
        {
            get => dictionaryToolTipsMainWindow ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                    "Найдите файл в папке по имени или части его имени.",//00 
                    "Страниц может быть много, а здесь можно выбрать любую из них.",//01
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                    "Find a file in a folder by name or part of its name.",//00
                    "There can be many pages, but here you can choose any of them.",//01
                   }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsMainWindow;

        /// <summary>
        /// ToolBarStatus внизу окна
        /// </summary>
        public ObservableCollection<string> MainWindowToolBar => DictionaryMainWindowToolBar[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMainWindowToolBar
        {
            get => dictionaryMainWindowToolBar ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Кодировка:",//00
                            "Шифрование:",//01
                            "Рабочая директория:",//02
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "Encoding:",//00
                             "Encryption:",//01
                             "Working directory:",//02
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMainWindowToolBar;
        public ObservableCollection<string> ToolTipsMainWindowToolBar => DictionaryToolTipsMainWindowToolBar[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsMainWindowToolBar
        {
            get => dictionaryToolTipsMainWindowToolBar ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Текущая кодировка символов",//00
                            "Статус шифрования",//01
                            "Каталог, в котором будет производиться поиск файлов",//02
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "Current character encoding",//00
                             "Encryption Status",//01
                             "Directory in which files will be searched",//02
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsMainWindowToolBar;

        #endregion

        #region ______________________________InputWindow___________________________________

        public ObservableCollection<string> InputWindow => DictionaryInputWindow[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryInputWindow
        {
            get => dictionaryInputWindow ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                    {
                    "Ключ шифрования",//00 заголовок окна ввода ключа шифрования
                    "Введите ключ шифрования(минимум 6 символов)",//01 заголовок первой строки ввода ключа шифрования
                    "Введите ключ шифрования еще раз",//02 заголовок второй строки ввода ключа шифрования
                    }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                    "Encryption key",//00 
                    "Enter encryption key(minimum 6 characters)",//01
                    "Enter the encryption key again",//02
                    }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryInputWindow;
        public ObservableCollection<string> ToolTipsInputWindow => DictionaryToolTipsInputWindow[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsInputWindow
        {
            get => dictionaryToolTipsInputWindow ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                    "Для включения режима шифрования необходимо установить ключ шифрования.",//00 
                    "Придумайте и введите ключ шифрования. Не забудьте его.",//01
                    "Во избежание ошибки, введите ключ шифрования повторно.",//02
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                    "To enable encryption mode, you must set the encryption key.",//00
                     "Create and enter an encryption key. Don't forget it.",//01
                     "To avoid an error, enter the encryption key again.",//02
                   }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsInputWindow;



        #endregion

        #region *************RichTextBox******************************

        public ObservableCollection<string> RichTextBox => DictionaryRichTextBox[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryRichTextBox
        {
            get => dictionaryRichTextBox ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                    {
                        "Установить цвет",//00
                    }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                        "Set Color",//00
                    }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryRichTextBox;
        public ObservableCollection<string> ToolTipsRichTextBox => DictionaryToolTipsRichTextBox[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsRichTextBox
        {
            get => dictionaryToolTipsRichTextBox ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                    "Выберите цвета для быстрой смены чернил",//00
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                    "Select colors to change ink quickly",//00
                   }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsRichTextBox;



        #endregion

        #region ____________________________SearchForFiles___________________________________________________


        public ObservableCollection<string> SearchForFiles => DictionarySearchForFiles[Key];
        private Dictionary<string, ObservableCollection<string>> DictionarySearchForFiles
        {
            get => dictionarySearchForFiles ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                    {
                    "Имя файла",//00
                    "Путь к файлу",//01
                    "Дата создания",//02
                    }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                     "File name",//00
                     "Path to file",//01
                     "Date of creation",//02                    
                }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionarySearchForFiles;
        public ObservableCollection<string> ToolTipsSearchForFiles => DictionaryToolTipsSearchForFiles[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsSearchForFiles
        {
            get => dictionaryToolTipsSearchForFiles ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                   }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsSearchForFiles;




        #endregion



        #region _____________________________FolderBrowserDialog_____________________

        public ObservableCollection<string> FolderBrowserDialog => DictionaryFolderBrowserDialog[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryFolderBrowserDialog
        {
            get => dictionaryFolderBrowserDialog ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                    {
                    "Выбор или создание каталога.",//00
                    "Новый каталог",//01
                    "Принять",//02
                    "Отмена",//03
                    "Свернуть все диски",//04
                    }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                     "Select or create a directory.",//00
                     "New directory",//01
                     "Accept",//02
                     "Cancel",//03
                     "Minimize all drives",//04
                     "Select directory",//05
                }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryFolderBrowserDialog;
        public ObservableCollection<string> ToolTipsFolderBrowserDialog => DictionaryToolTipsFolderBrowserDialog[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsFolderBrowserDialog
        {
            get => dictionaryToolTipsFolderBrowserDialog ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                    "Создание или выбор рабочего каталога",//00
                    "Создание каталога в выбранной директории",//01
                    "Принять внесённые изменения",//02
                    "Отменить внесённые изменения",//03
                    "Свернуть все развернутые диски",//04
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                    "Create or select a working directory",//00
                    "Creating a directory in the selected directory",//01
                    "Accept the changes",//02
                    "Revert changes",//03
                    "Minimize all expanded drives",//04          
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsFolderBrowserDialog;



        #endregion


        #region ____________________ContextMenu________________________________



        /// <summary>
        /// заголовки и подсказки для контекстного меню в RichtextBox
        /// </summary>
        public ObservableCollection<string> MyContextMenu => DictionaryMyContextMenu[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMyContextMenu
        {
            get => dictionaryMyContextMenu ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Копировать",//00
                            "Вырезать",//01
                            "Вставить",//02
                            "Удалить",//03
                            "Чернила",//04
                            "Фон",//05
                            "Бумага",//06
                            "Копировать изображение",//07
                            "Вырезать изображение",//08
                            "Вставить изображение перед...",//09
                            "Вставить изображение после...",//10
                            "Удалить изображение",//11
                            "Изменить изображение",//12
                            "Открыть для чтения",//13
                            "Открыть для редактирования",//14
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "Copy",//00
                             "Cut",//01
                             "Insert",//02
                             "Delete",//03
                             "Ink",//04
                             "Background",//05
                             "Paper",//06
                             "Copy Image",//07
                             "Cut Image",//08
                             "Insert Image Before...",//09
                             "Insert Image After...",//10
                             "Delete Image",//11
                             "Change Image",//12
                             "Open for reading",//13
                             "Open for editing",//14
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMyContextMenu;
        public ObservableCollection<string> ToolTipsMyContextMenu => DictionaryToolTipsMyContextMenu[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsMyContextMenu
        {
            get => dictionaryToolTipsMyContextMenu ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Копировать в буфер обмена",//00
                            "Вырезать с сохранением в буфере обмена",//01
                            "Вставить из буфера обмена",//02
                            "Удалить без возможности восстановления",//03
                            "Изменить цвет вводимого текста",//04
                            "Изменить цвет фона вводимого текста",//05
                            "Изменить цвет общего фона",//06
                            "Копировать изображение в буфер обмена",//07
                            "Удалить изображение с копированием в буфер обмена",//08
                            "Вставить изображение из буфера обмена перед текущим изображением",//09
                            "Вставить изображение из буфера обмена после текущего изображения",//10
                            "Удалить выбранное изображение",//11
                            "Изменить выбранное изображение",//12
                            "Открыть файл для чтения",//13
                            "Открыть файл для редактирования",//14
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Copy to clipboard",//00
                            "Cut and save to clipboard",//01
                            "Paste from clipboard",//02
                            "Delete without the possibility of recovery",//03
                            "Change the color of the input text",//04
                            "Change the background color of the input text",//05
                            "Change the general background color",//06
                            "Copy image to clipboard",//07
                            "Delete image with copy to clipboard",//08
                            "Insert image from clipboard before current image",//09
                            "Insert image from clipboard after current image",//10
                            "Delete selected image",//11
                            "Change selected image",//12
                            "Open file for reading",//13
                            "Open file for editing",//14
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsMyContextMenu;

        #endregion


        #region _________________File uploader______________________________


        /// <summary>
        /// заголовки и подсказки для FileUploader
        /// </summary>
        public ObservableCollection<string> HeadersFileUploader => DictionaryHeadersFileUploader[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHeadersFileUploader
        {
            get => dictionaryHeadersFileUploader ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Действия",//00
                    "ID",//01
                    "Имя",//02
                    "Каталог",//03
                    "Статус",//04
                    "Прогресс",//05
                    "Скорость",//06
                    "Размер",//07
                    "Тип",//08
                    "Меню",//09
                    "Каталог для загружаемых файлов",//10
                    "Вставить",//11
                    "Выбрать все",//12
                    "Копировать",//13
                    "Очистить",//14
                    "Переименование",//15
                    "Удаление загрузки",//16
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "Actions",//00
                     "ID",//01
                     "Name",//02
                     "Catalogue",//03
                     "Status",//04
                     "Progress",//05
                     "Speed",//06
                     "Size",//07
                     "Type",//08
                     "Menu",//09
                     "Directory for downloaded files",//10
                     "Insert" ,//11
                     "Select All",//12
                     "Copy",//13
                     "Clear",//14
                     "Renaming",//15
                     "Deleting download",//16
                }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHeadersFileUploader;
        public ObservableCollection<string> ToolTipsFileUploader => DictionaryToolTipsFileUploader[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsFileUploader
        {
            get => dictionaryToolTipsFileUploader ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {

                }
                },
                {"en-US", new ObservableCollection<string>()
                {

                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsFileUploader;

        #endregion


        #region *********************** PopUpTextBox *************************



        /// <summary>
        /// заголовки и подсказки для меню Fie
        /// </summary>
        public ObservableCollection<string> HeadersPopUpTextBox => DictionaryHeadersPopUpTextBox[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHeadersPopUpTextBox
        {
            get => dictionaryHeadersPopUpTextBox ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Окно ввода строки",//00
                    "Редактировать имя загрузки",//01

                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "Line Entry Window",//00
                    "Edit Download Name",//01
                }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHeadersPopUpTextBox;
        public ObservableCollection<string> ToolTipsPopUpTextBox => DictionaryToolTipsPopUpTextBox[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsPopUpTextBox
        {
            get => dictionaryToolTipsPopUpTextBox ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Для принятия изменений - \" Enter \"",//00
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "To accept the change - \" Enter\"",//00
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsPopUpTextBox;


        #endregion


        #region ____________________DocumentTree_________________________________

        /// <summary>
        /// заголовки и подсказки для окна DocumentTree
        /// </summary>
        public ObservableCollection<string> HeadersDocumentTree => DictionaryHeadersDocumentTree[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHeadersDocumentTree
        {
            get => dictionaryHeadersDocumentTree ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                    }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHeadersDocumentTree;
        public ObservableCollection<string> ToolTipsDocumentTree => DictionaryToolTipsDocumentTree[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsDocumentTree
        {
            get => dictionaryToolTipsDocumentTree ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsDocumentTree;

        #endregion


        #region ************ EncryptIndividualFile ******************************


        /// <summary>
        /// заголовки и подсказки для EncryptIndividualFile
        /// </summary>
        public ObservableCollection<string> EncryptIndividualFile => DictionaryEncryptIndividualFile[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryEncryptIndividualFile
        {
            get => dictionaryEncryptIndividualFile ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Шифровальщик",//00
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "Encoder",//00
                }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryEncryptIndividualFile;
        public ObservableCollection<string> ToolTipsEncryptIndividualFile => DictionaryToolTipsEncryptIndividualFile[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsEncryptIndividualFile
        {
            get => dictionaryToolTipsEncryptIndividualFile ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Выберите файл и создайте его зашифрованную копию",//00
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "Select a file and create an encrypted copy of it",//00
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsEncryptIndividualFile;

        #endregion


        #region _______________________________HOME__________________________________________

        /// <summary>
        /// заголовки и подсказки для меню Fie
        /// </summary>
        public ObservableCollection<string> HomeMenuFile => DictionaryHomeMenuFile[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHomeMenuFile
        {
            get => dictionaryHomeMenuFile ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Файл", //00
                    "Новый",//01
                    "Открыть",//02
                    "Сохранить",//03
                    "Сохранить как...",//04
                    "Рабочая директория",//05
                    "Редактировать файл",//06
                    "Синхронизация",//07
                    "Загрузка файлов",//08
                }
                },
                {"en-US", new ObservableCollection<string>()
                    {
                        "File",//00
                        "New",//01
                        "Open",//02
                        "Save",//03
                        "Save As...",//04
                        "Working directory",//05
                        "Edit File",//06
                        "Synchronization",//07
                        "Uploading files",//08
                    }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHomeMenuFile;
        public ObservableCollection<string> ToolTipsHomeMenuFile => DictionaryToolTipsHomeMenuFile[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsHomeMenuFile
        {
            get => dictionaryToolTipsHomeMenuFile ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                {
                    "Работа с файлами",//00
                    "Новый файл",//01
                    "Открыть файл.",//02
                    "Сохранить файл.",//03
                    "Сохранить файл в выбранную директорию.",//04
                    "Укажите каталог, в котором будет осуществляться поиск.",//05
                    "Файл будет открыт в редакторе для внесения изменений",//06"
                    "Синхронизация рабочего каталога со сторонним",//07
                    "Свободно загружай файлы из интернета по ссылкам",//08
                }
                },
                {"en-US", new ObservableCollection<string>()
                {
                    "Working with files",//00
                    "New file",//01
                    "Open file.",//02
                    "Save file.",//03
                    "Save the file to the selected directory.",//04
                    "Specify the directory in which the search will be performed.",//05
                    "The file will be opened in the editor to make changes",//06
                    "Synchronization of the working directory with a third-party",//07
                    "Freely download files from the Internet using links",//08
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsHomeMenuFile;

        /// <summary>
        /// Заголовки и подсказки для меню Settings
        /// </summary>
        public ObservableCollection<string> HomeMenuSettings => DictionaryHomeMenuSettings[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHomeMenuSettings
        {
            get => dictionaryHomeMenuSettings ??= new Dictionary<string, ObservableCollection<string>>()
                {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Настройки",//00
                            "Кодировка",//01
                            "Локализация",//02
                            "Чернила",//03
                            "Фон",//04
                            "Бумага",//05

                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Settings",//00
                            "Encoding",//01
                            "Localization",//02
                            "Ink",//03
                            "Background",//04
                            "Paper",//05
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHomeMenuSettings;
        public ObservableCollection<string> ToolTipsHomeMenuSettings => DictionaryToolTipsHomeMenuSettings[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsHomeMenuSettings
        {
            get => dictionaryToolTipsHomeMenuSettings ??= new Dictionary<string, ObservableCollection<string>>()
                {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Настройки программы",//00
                            "Кодировка символов",//01
                            "Язык программы",//02
                            "Цвет шрифта",//03
                            "Цвет фона",//04
                            "Цвет листа",//05
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Program settings",//00
                            "Character Encoding",//01
                            "Program language",//02
                            "Font color",//03
                            "Background color",//04
                            "Color sheet",//05
                        }
                    }
                };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsHomeMenuSettings;

        /// <summary>
        /// Заголовки и подсказки для меню Encryption
        /// </summary>
        public ObservableCollection<string> HomeMenuEncryption => DictionaryHomeMenuEncryption[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHomeMenuEncryption
        {
            get => dictionaryHomeMenuEncryption ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Шифрование",//00
                            "Установить ключ шифрования",//01
                            "Удалить ключ шифрования",//02
                            "Рабочая диретория",//03
                            "Шифрование файла",//04
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Encryption",//00
                            "Set Encryption Key",//01
                            "Delete Encryption Key",//02
                            "Working Directory",//03
                            "File encryption",//04
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHomeMenuEncryption;
        public ObservableCollection<string> ToolTipsHomeMenuEncryption => DictionaryToolTipsHomeMenuEncryption[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsHomeMenuEncryption
        {
            get => dictionaryToolTipsHomeMenuEncryption ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Шифрование файлов",//00
                            "Задать ключ шифрования",//01
                            "Сохраняемые файлы не будут шифроваться, а зашифрованные файлы не будут расшифровываться",//02
                            "Зашифровать отдельный файл",//03
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Encrypting files",//00
                            "Set Encryption Key",//01
                            "Stored files will not be encrypted and encrypted files will not be decrypted",//02
                            "Encrypt Individual File",//03
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsHomeMenuEncryption;


        /// <summary>
        /// Заголовки и подсказки для меню Content
        /// </summary>
        public ObservableCollection<string> HomeMenuContent => DictionaryHomeMenuContent[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryHomeMenuContent
        {
            get => dictionaryHomeMenuContent ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Контент",//00
                            "Добавить изображение",//01
                            "Открыть дерево документа",//02
                            "Читать документ",//03
                            "Редактировать документ",//04
                            "Добавить текст",//05
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Content",//00
                            "Add image",//01
                            "Open document tree",//02
                            "Read document",//03
                            "Edit document",//04
                            "Add text",//05
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryHomeMenuContent;
        public ObservableCollection<string> ToolTipsHomeMenuContent => DictionaryToolTipsHomeMenuContent[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsHomeMenuContent
        {
            get => dictionaryToolTipsHomeMenuContent ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "",//00
                            "Добавить изображение с диска",//01
                            "Редактировать документ используя дерево документа",//02
                            "Включить режим чтения(редактирование не возможно)",//03
                            "Включить режим редактирования",//04
                            "Вставить текст в выбранный параграф",//05
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "",//00
                             "Add image from disk",//01
                             "Edit document using document tree",//02
                             "Enable reading mode (editing is not possible)",//03
                             "Enable edit mode",//04
                             "Insert text in the selected paragraph",//05
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsHomeMenuContent;



        /// <summary>
        /// заголовки и подсказки для ToolBar в RichtextBox
        /// </summary>
        public ObservableCollection<string> RichTextBoxToolBar => DictionaryRichTextBoxToolBar[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryRichTextBoxToolBar
        {
            get => dictionaryRichTextBoxToolBar ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {

                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryRichTextBoxToolBar;
        public ObservableCollection<string> ToolTipsRichTextBoxToolBar => DictionaryToolTipsRichTextBoxToolBar[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsRichTextBoxToolBar
        {
            get => dictionaryToolTipsRichTextBoxToolBar ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Семейство шрифтов",//00
                            "Высота шрифта",//01
                            "Вырезать с сохранением в буфере обмена",//02
                            "Копировать в буфер обмена",//03
                            "Вставить из буфера обмена",//04
                            "Отменить",//05
                            "Повторить",//06
                            "Вес шрифта Bold",//07
                            "Стиль шрифта Italic",//08
                            "Оформление шрифта Underline",//09
                            "Нормализация текста",//10
                            "Уменьшение межстрочного расстояния",//11
                            "Увеличение межстрочного расстояния",//12
                            "Выравнивание текста по левому краю",//13
                            "Выравнивание текста по центру",//14
                            "Выравнивание текста по правому краю",//15
                            "Равномерное выравнивание текста",//16
                            "Список",//17
                            "Нумерованный список",//18
                            "Увеличить отступ",//19
                            "Уменьшить отступ",//20
                            "Так будет выглядеть написание шрифта.Если нажать то фон шрифта станет прозрачным",//21
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Font family",//00
                            "Font height",//01
                            "Cut and save to clipboard",//02
                            "Copy to clipboard",//03
                            "Paste from clipboard",//04
                            "Cancel",//05
                            "Repeat",//06
                            "Bold font weight",//07
                            "Italic font style",//08
                            "Underline font design",//09
                            "Text Normalization",//10
                            "Reducing line spacing",//11
                            "Increasing line spacing",//12
                            "Align text to the left",//13
                            "Align text to the center",//14
                            "Align text to the right",//15
                            "Uniform text alignment",//16
                            "List",//17
                            "Numbered list",//18
                            "Increase indent",//19
                            "Decrease indent",//20
                            "This is how the font will look like. If you press it, the background of the font will become transparent",//21
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsRichTextBoxToolBar;


        /// <summary>
        /// заголовки, сообщения и подсказки MyMessages
        /// </summary>
        public ObservableCollection<string> MyMessagesHeaders => DictionaryMyMessagesHeaders[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMyMessagesHeaders
        {
            get => dictionaryMyMessagesHeaders ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Ошибка",//00
                            "Ошибка загрузки файла",//01
                            "Ошибка сохранения файла",//02
                            "Синхронизация",//03
                            "Сделано!",//04
                            "Хорошо",//05
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Error",//00
                            "File upload error",//01
                            "Error saving file",//02
                            "Synchronization",//03
                            "Done!",//04
                            "Ok",//05
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMyMessagesHeaders;
        public ObservableCollection<string> MessagesMyMessages => DictionaryMessagesMyMessages[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMessagesMyMessages
        {
            get => dictionaryMessagesMyMessages ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Неизвестная ошибка. Перезапустите программу.",//00
                            "Попытка загрузки файла привела к ошибке",//01
                            "Попытка сохранения файла привела к ошибке",//02
                            "Не удалось загрузить один из компонентов программы...",//03
                            "Данные успешно синхронизированны",//04
                            "При попытке синхронизации произошла ошибка." +
                            "При необходимости, восстановите данные из резервных копий",//05
                            "Перед синхронизацией, во избежание возможной потери данных, " +
                            "создайте резервные копии синхронизируемых каталогов",//06
                            "Задайте рабочую директорию для загрузки файлов",//07
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Unknown error. Restart the program.",//00
                            "An attempt to upload a file resulted in an error",//01
                            "An attempt to save the file resulted in an error",//02
                            "Failed to load one of the program components...",//03
                            "Data successfully synchronized",//04
                            "An error occurred while trying to sync." +
                             "If necessary, restore data from backups",//05
                            "Before synchronization, to avoid possible data loss, " +
                             "back up your synchronized directories",//06
                            "Set the working directory to download files",//07
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMessagesMyMessages;
        public ObservableCollection<string> ToolTipsMyMessages => DictionaryToolTipsMyMessages[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsMyMessages
        {
            get => dictionaryToolTipsMyMessages ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsMyMessages;


        /// <summary>
        /// заголовки, сообщения и подсказки SelectWindow
        /// </summary>
        public ObservableCollection<string> SelectWindowHeaders => DictionarySelectWindowHeaders[Key];
        private Dictionary<string, ObservableCollection<string>> DictionarySelectWindowHeaders
        {
            get => dictionarySelectWindowHeaders ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Подсказка",//00
                            "Понятно",//01
                            "Окно выбора",//02
                            "Дописать",//03
                            "Создать копию",//04
                            "Перезаписать",//05
                            "Удаление загрузки",//06
                            "Удалить вместе с файлом",//07
                            "Удалить только загрузку",//08
                            "Отмена",//09
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "Hint",//00
                            "Understood",//01
                            "Selection window",//02
                             "Append",//03
                             "Create a copy",//04
                             "Overwrite",//05
                             "Download Removal",//06
                             "Delete with file",//07
                             "Delete Download Only",//08
                             "Cancel",//09
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionarySelectWindowHeaders;
        public ObservableCollection<string> MessagesSelectWindow => DictionaryMessagesSelectWindow[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMessagesSelectWindow
        {
            get => dictionaryMessagesSelectWindow ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Если при попытке открыть файл вы получили ошибку,\r\n " +
                            "то попробуйте включить или отключить шифрование.\r\n" +
                            "Возможно, файл зашифрован другим ключом.",//00
                            "Файл с таким именем уже существует в данной директории.\r\n" +
                            "Что я должен сделать?\r\n" +
                            "Если действий не требуется, просто закрой окно.",//01
                            "Текущая загрузка будет отменена и удалена.\nВыберите вариант отмены:",//02
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                            "If you get an error when trying to open a file,\r\n " +
                            "then try enabling or disabling encryption.\r\n" +
                            "Maybe the file is encrypted with a different key.",//00
                            "A file with the same name already exists in this directory.\r\n" +
                             "What should I do?\r\n" +
                             "If no action is required, just close the window.",//01
                            "The current download will be cancelled and deleted .\nSelect a cancellation option:",//02
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMessagesSelectWindow;
        public ObservableCollection<string> ToolTipsSelectWindow => DictionaryToolTipsSelectWindow[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsSelectWindow
        {
            get => dictionaryToolTipsSelectWindow ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {

                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {

                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsSelectWindow;



        /// <summary>
        /// заголовки, сообщения и подсказки SelectAndPasteWindow
        /// </summary>
        public ObservableCollection<string> SelectAndPasteWindowHeaders => DictionarySelectAndPasteWindowHeaders[Key];
        private Dictionary<string, ObservableCollection<string>> DictionarySelectAndPasteWindowHeaders
        {
            get => dictionarySelectAndPasteWindowHeaders ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Вставить контент",//00
                            "Выбор файла",//01
                            "Пропорционально",//02
                            "Ширина",//03
                            "Высота",//04
                            "Слева",//05
                            "Центр",//06
                            "Справа",//07
                            "Заполнить",//08
                            "Конец документа",//09
                            "Последний блок",//10
                            "Перед последним блоком",//11
                            "Заполнить",//12
                            "Обтекание",//13
                            "Поворот",//14
                            "Выравнивание по горизонтали",//15
                            "Выравнивание по вертикали",//16
                            "Применить",//17
                            "Отменить",//18
                            "Сохранить",//19
                            "Выбор каталога",//20
                            "Склеить",//21
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "Insert content",//00
                             "File selection",//01
                             "Proportional",//02
                             "Width",//03
                             "Height",//04
                             "Left",//05
                             "Center",//06
                             "Right",//07
                             "Fill",//08
                             "End of Document",//09
                             "Last block",//10
                             "Before the last block",//11
                             "Fill",//12
                             "Wrap around",//13
                             "Turn",//14
                             "Horizontal Alignment",//15
                             "Vertical Alignment",//16
                             "Apply",//17
                             "Cancel",//18
                             "Save",//19
                             "Select directory",//20
                             "Merge",//21
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionarySelectAndPasteWindowHeaders;
        public ObservableCollection<string> SelectAndPasteWindowMessages => DictionarySelectAndPasteWindowMessages[Key];
        private Dictionary<string, ObservableCollection<string>> DictionarySelectAndPasteWindowMessages
        {
            get => dictionarySelectAndPasteWindowMessages ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionarySelectAndPasteWindowMessages;
        public ObservableCollection<string> SelectAndPasteWindowToolTips => DictionarySelectAndPasteWindowToolTips[Key];
        private Dictionary<string, ObservableCollection<string>> DictionarySelectAndPasteWindowToolTips
        {
            get => dictionarySelectAndPasteWindowToolTips ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                            "Выберите файл, измените его размеры и вставьте в документ",//00
                            "Выбор файла на диске",//01
                            "Размер по горизонтали",//02
                            "Размер по вертикали",//03
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                             "Select a file, resize it and paste it into the document",//00
                             "Select file on disk",//01
                             "Horizontal size",//02
                             "Vertical size",//03
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionarySelectAndPasteWindowToolTips;


        /// <summary>
        /// заголовки, сообщения и подсказки FixedDocument
        /// </summary>
        public ObservableCollection<string> FixedDocumentHeaders => DictionaryFixedDocumentHeaders[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryFixedDocumentHeaders
        {
            get => dictionaryFixedDocumentHeaders ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryFixedDocumentHeaders;
        public ObservableCollection<string> MessagesFixedDocument => DictionaryMessagesFixedDocument[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryMessagesFixedDocument
        {
            get => dictionaryMessagesFixedDocument ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {
                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {
                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryMessagesFixedDocument;
        public ObservableCollection<string> ToolTipsFixedDocument => DictionaryToolTipsFixedDocument[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryToolTipsFixedDocument
        {
            get => dictionaryToolTipsFixedDocument ??= new Dictionary<string, ObservableCollection<string>>()
            {
                    {
                        "ru-RU", new ObservableCollection<string>()
                        {

                        }
                    },
                    {
                        "en-US", new ObservableCollection<string>()
                        {

                        }
                    }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryToolTipsFixedDocument;



        #endregion


        #region _______________________DisplayProgress____________________________________

        public ObservableCollection<string> DisplayProgress => DictionaryDisplayProgress[Key];
        private Dictionary<string, ObservableCollection<string>> DictionaryDisplayProgress
        {
            get => dictionaryDisplayProgress ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                    {

                    }
                },
                {"en-US", new ObservableCollection<string>()
                    {

                    }
                }
            };
        }
        private Dictionary<string, ObservableCollection<string>> dictionaryDisplayProgress;
        public ObservableCollection<string> ToolTipsDisplayProgress => DictionaryToolTipsDisplayProgress[Key];
        Dictionary<string, ObservableCollection<string>> DictionaryToolTipsDisplayProgress
        {
            get => dictionaryToolTipsDisplayProgress ??= new Dictionary<string, ObservableCollection<string>>()
            {
                {"ru-RU", new ObservableCollection<string>()
                   {
                   }
                },
                {"en-US", new ObservableCollection<string>()
                   {
                }
                }
            };
        }
        Dictionary<string, ObservableCollection<string>> dictionaryToolTipsDisplayProgress;



        #endregion



    }
}
