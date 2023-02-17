using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
//using ParserDownloader_WPF_.PD_Macro;

namespace NotebookRCv001.Converters
{
    public class ToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string result = "";
                switch (parameter.ToString())
                {
                    case "Byte_Size":
                        {
                            long size = (long)value;
                            if (size < 1024)
                                result = $"{size:f2} B";
                            else if (size >= 1024 && size < 1048576)
                                result = $"{(double)size / 1024:f2} KB";
                            else
                                result = $"{(double)size / 1048576:f2} MB";
                            break;
                        }
                    case "Byte_Speed":
                        {
                            long speed = (long)value;
                            if (speed < 1024)
                                result = $"{speed:f2} B/sec.";
                            else if (speed >= 1024 && speed < 1048576)
                                result = $"{(double)speed / 1024:f2} KB/sec.";
                            else
                                result = $"{(double)speed / 1048576:f2} MB/sec.";
                            break;
                        }
                    case "Sec_Time":
                        {
                            long time = (long)value;
                            if (time < 60)
                                result = $"{time} s";
                            else if (time >= 60 && time < 3600)
                                result = $"{time / 60} m";
                            else
                                result = $"{time / 3600} h";
                            break;
                        }
                    case "Status_Downloads":
                        {
                            // Соответсвие статусов текстовым сообщениям в колонке 
                            // 0 - загрузка файлов(не используется)
                            // 1 - Подготовка
                            // 2 - Загрузка
                            // 3 - Загружено
                            // 4 - Пауза
                            // 5 - Ожидание
                            // 6 - Ошибка

                            //if (value is int index)
                            //{
                            //    result = PageRepository.languages.DownloadFilesL[index];
                            //}
                            break;
                        }
                    case "SelectedCommands_Count":
                        {
                            //if (value is PD_Macro_ViewModel viewModel)
                            //{
                            //    result = $" Selected {viewModel.SelectedCommands_Count} commands.";
                            //}
                            break;
                        }
                }
                return result;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                long result = 0;
                if (value is string res)
                    result = long.Parse(res.Split(' ')[0]);
                else
                    return DependencyProperty.UnsetValue;
                return result;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
                return null;
            }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"ToStringConverter.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
