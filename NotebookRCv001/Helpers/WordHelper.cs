using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Helpers
{
    public class WordHelper
    {
        internal FileInfo FileInfo { get => fileInfo; set => fileInfo = value; }
        private FileInfo fileInfo;




        public WordHelper( string path )
        {
            if (File.Exists(path))
            {
                FileInfo = new FileInfo(path);

            }
            else
            { throw new ArgumentException("File  Not found"); }
        }

        internal void Process()
        {
            try
            {
                //var app = new word.Application();//запускаем процесс 
                //object file = FileInfo.FullName;
                //object missing = Type.Missing;
                //app.Documents.Open(file);//передаем в процесс обрабатываемый файл

                //object newFileName = Path.Combine(FileInfo.DirectoryName, FileInfo.Name + DateTime.Now.ToString("_mmddhhmmss"));//путь для сохранения измененного файла
                //app.ActiveDocument.SaveAs2(newFileName);//сохраняем измененный файл
                //app.ActiveDocument.Close();//закрываем файл
                //app.Quit();//закрываем процесс
            }
            catch (Exception e) { throw new ArgumentException(e.Message); }
        }

        //    internal List<string> ReadText()
        //    {
        //        //var app = new word.Application();//запускаем процесс 
        //        //try
        //        //{

        //        //    object file = FileInfo.FullName;
        //        //    object missing = Type.Missing;
        //        //    word.Document doc = app.Documents.Open(file);//передаем в процесс обрабатываемый файл

        //        //    List<string> list = new List<string>();

        //        //    foreach (word.Paragraph paragraf in doc.Paragraphs)
        //        //    {
        //        //        list.Add(paragraf.Range.Text);
        //        //    }

        //        //    app.ActiveDocument.Close();//закрываем файл
        //        //    app.Quit();//закрываем процесс

        //        //    return list;
        //        //}
        //        //catch (Exception e) { throw new ArgumentException(e.Message); }
        //        //finally
        //        //{
        //        //    if (app != null)
        //        //    { app.Quit(); }
        //        //}
        //    }

    }
}
