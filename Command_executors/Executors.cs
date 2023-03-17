using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using Forms = System.Windows.Forms;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Command_executors
{
    public abstract class Executors
    {
        private static ObservableCollection<string> OneParameter => oneParameter = oneParameter == null ? new ObservableCollection<string>()
        {
            "Concat", "Contains", "GetElementByID", "GetElementsByClassName", "GetElementsByTagName", "GetInnerHTML", "GetText", "EndsWith", "GoTo",
                "ExecuteScriptJS", "MacroForLinks", "NotContains", "RemoveStart", "Rename", "Split", "StartsWith", "Trim", "TrimEnd", "TrimStart", "NewMacro"
        } : oneParameter;
        static ObservableCollection<string> oneParameter;
        private static ObservableCollection<string> WithoutParameters => withoutParameters = withoutParameters == null ? new ObservableCollection<string>()
        {
            "Copies", "CreateNamePage", "Empty_Command", "FindExtensions", "Identifier", "LoadHTML", "Name", "NewPages", "Primary", "Secondary",
                "Selection"
        } : withoutParameters;
        static ObservableCollection<string> withoutParameters;
        public static ObservableCollection<string> ForMacros => forMacros = forMacros == null ? new ObservableCollection<string>()
        {
            "Concat", "Contains", "Copies", "CreateNamePage", "GoTo", "GetAttribute", "GetElementByID", "GetElementsByClassName", "GetElementsByTagName",
            "GetInnerHTML", "GetText", "EndsWith", "ExecuteScriptJS", "FindExtensions", "Insert", "MacroForLinks", "NewPages", "NotContains", "Primary", "RemoveStart", "RemoveLength", "RenameNameToName", "Replace", "Secondary", "Split", "StartsWith", "Trim", "TrimEnd", "TrimStart"
        } : forMacros;
        static ObservableCollection<string> forMacros;
        private static ObservableCollection<string> AllCommands => allCommands = allCommands == null ? new ObservableCollection<string>()
        {
            "Concat", "Contains", "Copies", "CreateNamePage", "GetAttribute", "GetElementByID", "GetElementsByClassName", "GetElementsByTagName",
            "GetInnerHTML", "GetText", "GoTo", "Empty_Command", "EndsWith", "ExecuteScriptJS", "FindExtensions", "Identifier", "Insert", "LoadHTML",
            "MacroForLinks",  "Name", "NewPages", "NotContains", "Primary", "RemoveStart", "RemoveLength", "Rename", "RenameNameToName",
            "Replace", "Secondary", "Selection", "Split", "StartsWith", "Trim", "TrimEnd", "TrimStart", "NewMacro"
        } : allCommands;
        static ObservableCollection<string> allCommands;
        public static ObservableCollection<string> ForMenuCommand => forMenuCommand = forMenuCommand == null ? new ObservableCollection<string>()
        {
            "Concat", "Contains", "Copies", "EndsWith", "FindExtensions", "GoTo", "Insert", "NewPages", "NotContains",
            "RemoveStart", "RemoveLength", "Replace", "Split", "StartsWith", "Trim", "TrimEnd", "TrimStart"
        } : forMenuCommand;
        static ObservableCollection<string> forMenuCommand;


        /// <summary>
        /// команда имеет один параметр
        /// </summary>
        /// <param name="name">имя команды</param>
        /// <returns>true - команда с одним параметром</returns>
        public static bool IsOneParameter( string name )
        {
            try
            {
                return OneParameter.Any( ( x ) => x == name );
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// команда не имеет параметров
        /// </summary>
        /// <param name="name">имя команды</param>
        /// <returns>true - команда не имеет параметров</returns>
        public static bool IsWithoutParameters( string name )
        {
            try
            {
                return WithoutParameters.Any( ( x ) => x == name );
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// команда может быть использована в макросах
        /// </summary>
        /// <param name="name">имя команды</param>
        /// <returns>true - команда может быть использована в макросах</returns>
        public static bool IsForMacros( string name )
        {
            try
            {
                return ForMacros.Any( ( x ) => x == name );
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// команда с таким именем существует
        /// </summary>
        /// <param name="name">имя команды</param>
        /// <returns>true - команда с таким именем существует</returns>
        public static bool IsAllcommands( string name )
        {
            try
            {
                return AllCommands.Any( ( x ) => x == name );
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// команда отображается в меню Command главного окна
        /// </summary>
        /// <param name="name">имя команды</param>
        /// <returns>true - команда отображается в меню Command главного окна</returns>
        public static bool IsForMenuCommand( string name )
        {
            try
            {
                return ForMenuCommand.Any( ( x ) => x == name );
            }
            catch
            {
                return false;
            }
        }

        public static Action DownloadCanceled { get; set; }

        /// <summary>
        /// добавление строки substring в конец каждой строки коллекции L1
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="substring">добавляемая подстрока</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>преобразованная коллекция</returns>
        public static ObservableCollection<string> Concat( Action<object> execute, ObservableCollection<string> L1, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && substring != null)
                {
                    double count = 0;
                    double LCount = L1.Count;
                    foreach (string str in L1)
                    {
                        list.Add( string.Concat( str, substring ) );
                        execute.Invoke( (count += 1.0) / LCount * 100.0 );
                    }
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// оставляет строки содержащие подстроку substring
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="substring">массив параметров</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>преобразованная коллекция</returns>
        public static ObservableCollection<string> Contains( Action<object> execute, ObservableCollection<string> L1, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (L1 != null && execute != null && substring != null)
                {
                    double count = 0;
                    double LCount = L1.Count;
                    foreach (string str in L1)
                    {
                        if (str.ToLower().Contains( substring.ToLower() ))
                        {
                            list.Add( str );
                        }
                        count++;
                        execute.Invoke( (count / LCount) * 100.0 );
                    }
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// удаление дубликатов (копий) строк
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>преобразованная коллекция</returns>
        public static ObservableCollection<string> Copies( Action<object> execute, ObservableCollection<string> L1 )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (L1 != null && execute != null)
                {
                    double count = 0;
                    double LCount = L1.Count;
                    IEnumerable<string> l1 = L1.Distinct();
                    foreach (string str in l1)
                    {
                        list.Add( str );
                        count++;
                        execute.Invoke( (count / LCount) * 100.0 );
                    }
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// пустая команда 
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="L1"></param>
        /// <param name="options"></param>
        /// <returns>пустая коллекция строк</returns>
        public static ObservableCollection<string> Empty_Command()
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// оставляет строки заканчивающиеся подстрокой заданной в substring
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="substring"></param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>преобразованная коллекция</returns>
        public static ObservableCollection<string> EndsWith( Action<object> execute, ObservableCollection<string> L1, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (L1 != null && execute != null && substring != null)
                {
                    double count = 0;
                    double LCount = L1.Count;
                    foreach (string str in L1)
                    {
                        if (str.EndsWith( substring ))
                            list.Add( str );
                        count++;
                        execute.Invoke( (count / LCount) * 100.0 );
                    }
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// оставляет строки содержащие в своем составе указанные расширения
        /// </summary>
        /// <param name="execute">делегат для возврата прогресса выполнения команды</param>
        /// <param name="L1">коллекция строк HTML-кода</param>
        /// <param name="extension">массив искомых расширений(void delegate(double procent))</param>
        /// <returns>коллекция строк с найденными расширениями</returns>
        public static ObservableCollection<string> FindExtensions( Action<object> execute, ObservableCollection<string> L1, string[] extensions )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                foreach (string str in L1)
                {
                    bool c = false;
                    foreach (var ex in extensions)
                    {
                        string e = ex[0] == '.' ? ex.ToLower() : $".{ex.ToLower()}";
                        if (c = str.ToLower().Contains( e ))
                            break;
                    }
                    if (c)
                        list.Add( str );
                    count++;
                    execute.Invoke( (count / LCount) * 100.0 );
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Возвращает значение именованного атрибута для указанного элемента
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="atribute">имя атрибута</param>
        /// <param name="tag">имя тега в котором находится атрибут</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>коллекция значений атрибутов</returns>
        public static ObservableCollection<string> GetAttribute( Action<object> execute, ObservableCollection<string> L1, string tag, string atribute )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && tag != null && atribute != null)
                {
                    double count = 0;
                    double LCount = 0;
                    CQ dom = CQ.Create( ListText( L1 ) );
                    var elements = dom[tag];
                    if ((LCount = elements.Length) > 0 && elements != null)
                    {
                        foreach (var a in elements)
                        {
                            var attributes = a.Attributes;
                            var attribut = attributes.GetAttribute( atribute );
                            if (!string.IsNullOrEmpty( attribut ))
                                list.Add( attribut );
                            count++;
                            execute.Invoke( (count / LCount) * 100.0 );
                        }
                        elements = null;
                    }
                    else
                        elements = null;
                    dom = null;
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// Возвращает элемент с заданным ID
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="ID">ID элемента</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>искомый элемент</returns>
        public static ObservableCollection<string> GetElementByID( Action<object> execute, ObservableCollection<string> L1, string ID )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && ID != null)
                {
                    CQ Dom = CQ.Create( ListText( L1 ) );
                    var element = Dom.Document.GetElementById( ID );
                    if (element != null)
                        list = TextList( element.Render() );
                }
                else
                    return L1;
                execute.Invoke( 100.0 );
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Возвращает коллекцию элементов с заданным именем класса
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="ClassName">имя класса</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>коллекция искомых элементов</returns>
        public static ObservableCollection<string> GetElementsByClassName( Action<object> execute, ObservableCollection<string> L1, string ClassName )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && ClassName != null)
                {
                    CQ Dom = CQ.Create( ListText( L1 ) );
                    string nameClass = ClassName.Trim( ' ' ).Split( ' ' )[0];
                    Dom = Dom[$".{nameClass}"];
                    if (Dom != null)
                    {
                        for (int i = 0; i < Dom.Length; i++)
                        {
                            list.Add( Dom[i].Render() );
                        }
                    }
                    execute.Invoke( 100.0 );
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Возвращает коллекцию элементов с заданным именем тега
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="TagName">имя тега</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>коллекция искомых элементов</returns>
        public static ObservableCollection<string> GetElementsByTagName( Action<object> execute, ObservableCollection<string> L1, string TagName )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && TagName != null)
                {
                    CQ Dom = CQ.Create( ListText( L1 ) );
                    var elements = Dom.Document.GetElementsByTagName( TagName ).ToList();
                    if (elements != null && elements.Count > 0)
                    {
                        for (int i = 0; i < elements.Count; i++)
                        {
                            list.Add( elements[i].ToString() );
                            //execute.Invoke(i / elements.Count * 100);
                        }
                    }
                    execute.Invoke( 100.0 );
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Возвращает внутренний HTML код, элемента с заданным именем тега
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="options">массив параметров(options[0]- имя тега</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>HTML код преобразованный в коллекцию строк</returns>
        public static ObservableCollection<string> GetInnerHTML( Action<object> execute, ObservableCollection<string> L1, string TagName )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && TagName != null)
                {
                    CQ Dom = CQ.Create( ListText( L1 ) );
                    var elements = Dom[TagName];
                    if (elements != null)
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            list.Add( elements[i].InnerHTML );
                        }
                    }
                    execute.Invoke( 100.0 );
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// Возвращает внутренний текст, элемента с заданным именем тега
        /// </summary>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="TagName">имя тега</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>текст преобразованный в коллекцию строк</returns>
        public static ObservableCollection<string> GetText( Action<object> execute, ObservableCollection<string> L1, string TagName )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && TagName != null)
                {
                    CQ Dom = CQ.Create( ListText( L1 ) );
                    var elements = Dom[TagName];//получаем коллекцию тегов с заданным именем
                    if (elements != null)
                    {
                        double count = 0;
                        double LCount = 0;
                        for (int i = 0; i < (LCount = elements.Length); i++)//перебирае теги
                        {
                            string t = elements[i].InnerText;//получаем внутренний текст тега
                            if (t != null)
                            {
                                string t1 = "";
                                foreach (char ch in t)//оставляем в тексте только буквенные, цифровые и символы пунктуации
                                {
                                    if (char.IsLetterOrDigit( ch ) || char.IsPunctuation( ch ))
                                    {
                                        t1 = $"{t1}{ch}";
                                    }
                                }
                                list.Add( t1 );
                            }
                            count++;
                            execute.Invoke( (count / LCount) * 100.0 );
                        }
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// получение коллекции заголовков содержащихся в html коде
        /// </summary>
        /// <param name="L1">коллекция строк HTML-кода</param>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <returns>коллекция найденных Title</returns>
        public static ObservableCollection<string> GetTitles( Action<object> execute, string html )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (!string.IsNullOrEmpty( html ))
                {
                    CsQuery.CQ dom = CsQuery.CQ.CreateDocument( html );
                    if (execute != null)
                        execute.Invoke( 30 );
                    var title = dom.Find( "title" );
                    if (execute != null)
                        execute.Invoke( 100 );
                    if (title != null)
                    {
                        foreach (var a in title)
                        {
                            list.Add( a.Cq().Text() );
                        }
                    }
                    if (execute != null)
                        execute.Invoke( 100.0 );
                }
                else
                    return null;
                return list;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// вставка подстроки в заданную позицию всех строк коллекции
        /// </summary>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция строк</param>
        /// <param name="start">позиция вставки</param>
        /// <param name="substring">вставляемая подстрока</param>
        /// <returns>преобразованная коллекция</returns>
        public static ObservableCollection<string> Insert( Action<object> execute, ObservableCollection<string> L1, int start, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && start >= 0 && substring != null)
                {
                    foreach (string str in L1)
                    {
                        if (str != null && start <= str.Length)
                        {
                            list.Add( str.Insert( start, substring ) );
                        }
                        count++;
                        execute.Invoke( (count / LCount) * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// удаляет строки содержащие подстроку substring
        /// </summary>
        /// <param name="execute">обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="substring">подстрока</param>
        /// <returns></returns>
        public static ObservableCollection<string> NotContains( Action<object> execute, ObservableCollection<string> L1, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && substring != null)
                {
                    var s1 = L1.Where( ( s ) => !s.ToLower().Contains( substring.ToLower() ) );
                    foreach (var s2 in s1) list.Add( s2 );
                    execute.Invoke( 100.0 );
                    return list;
                }
                else
                    return L1;
            }
            catch
            {
                execute.Invoke( 0 );
                return null;
            }
        }
        /// <summary>
        /// удалить из каждой строки коллекции участок с заданой длиной и с заданной позиции
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="start">позиция старта</param>
        /// <param name="length">длина вырезаемого участка</param>
        /// <returns></returns>
        public static ObservableCollection<string> RemoveLength( Action<object> execute, ObservableCollection<string> L1, int start,
            int length )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && start >= 0 && length >= 0)
                {
                    double LCount = L1.Count;
                    double count = 0;
                    foreach (string str in L1)
                    {
                        if (str.Length > (start + length))
                            list.Add( str.Remove( start, length ) );
                        else if (str.Length > start)
                            list.Add( str.Remove( start, (str.Length - start) ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// обрезает каждую строку в коллекции начиная с заданной позиции
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="start">позиция обрезки строки</param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> RemoveStart( Action<object> execute, ObservableCollection<string> L1, int start )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                if (execute != null && L1 != null && start >= 0)
                {
                    double LCount = L1.Count;
                    double count = 0;
                    foreach (string str in L1)
                    {
                        if (str.Length > start)
                            list.Add( str.Remove( start ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// заменяет в каждой строке коллекции все вхождения заданной строки на другую заданную строку
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="target">заменяемый участок строки</param>
        /// <param name="value">новое значение заменённого участка</param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> Replace( Action<object> execute, ObservableCollection<string> L1, string target,
            string value )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && target != null && value != null)
                {
                    foreach (string str in L1)
                    {
                        list.Add( str.Replace( target, value ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// разбивает все строки коллекции на подстроки в зависимости от символов в массиве
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция строк</param>
        /// <param name="separator">массив с символами, в соответствии с которыми будут разбиваться строки</param>
        /// <returns>результирующая коллекция строк</returns>
        public static ObservableCollection<string> Split( Action<object> execute, ObservableCollection<string> L1, char[] separator )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && separator != null)
                {
                    foreach (string str in L1)
                    {
                        string[] l1 = str.Split( separator, StringSplitOptions.RemoveEmptyEntries );
                        foreach (string str1 in l1)
                            list.Add( str1 );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// оставляет в коллекции только те строки, которые начинаются с заданной подстроки
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="substring">контрольная подстрока</param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> StartsWith( Action<object> execute, ObservableCollection<string> L1, string substring )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && substring != null)
                {
                    foreach (string str in L1)
                    {
                        if (str.StartsWith( substring ))
                            list.Add( str );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// обрезает каждую строку коллекции в начале и конце в соответсвии с заданными символами
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="separator">мвссив с заданными символами по которым будет проводиться обрезка строк </param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> Trim( Action<object> execute, ObservableCollection<string> L1, char[] separator )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && separator != null)
                {
                    foreach (string str in L1)
                    {
                        list.Add( str.Trim( separator ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// обрезает каждую строку коллекции по конечным символам 
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="separstor">контрольные символы</param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> TrimEnd( Action<object> execute, ObservableCollection<string> L1, char[] separstor )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && separstor != null)
                {
                    foreach (string str in L1)
                    {
                        list.Add( str.TrimEnd( separstor ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// обрезает каждую строку коллекции по начальным символам в соответсвии с заданными символами
        /// </summary>
        /// <param name="execute">>обновление прогресса выполнения операции(void delegate(double procent))</param>
        /// <param name="L1">обрабатываемая коллекция</param>
        /// <param name="separator">массив контрольных символов</param>
        /// <returns>результирующая коллекция</returns>
        public static ObservableCollection<string> TrimStart( Action<object> execute, ObservableCollection<string> L1, char[] separator )
        {
            try
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                double LCount = L1.Count;
                double count = 0;
                if (execute != null && L1 != null && separator != null)
                {
                    foreach (string str in L1)
                    {
                        list.Add( str.TrimStart( separator ) );
                        execute.Invoke( (count += 1) / LCount * 100.0 );
                    }
                }
                else
                    return L1;
                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// асинхронная загрузка файла
        /// </summary>
        /// <param name="download">делегат информирующий о размере загружаемого файла</param>
        /// <param name="uploaded">делегат информирующий о размере уже загруженных байт</param>
        /// <param name="url">ссылка на загружаемый файл</param>
        /// <param name="path">полный путь для сохранения файла на диск</param>
        /// <param name="extensionDefault">индекс расширения по умолчанию в коллекции расширений</param>
        /// <param name="Extensions">коллекция расширений</param>
        /// <param name="timeOut">время ожидания ответа сервера (мс)</param>
        public static async void DownloadAsinc( Action<object> download, Action<object> uploaded, string url, string path, int extensionDefault, List<string> Extensions, int timeOut )
        {
            try
            {
                byte[] file = null;//массив байт загружаемого файла
                byte[] bufer = new byte[10240];//массив байт пишущийся на диск за один проход
                string Extension = null;
                long Size = 0;//размер файла в байтах
                long SizeUploaded = 0;//загружено байт всего
                int ByteSize = 0;//загружено байт за проход
                using (HttpWebResponse response = Request( url, timeOut, 0 ).Result)
                {
                    Extension = string.IsNullOrEmpty( response.ContentType ) ? Extensions[extensionDefault] : response.ContentType;
                    Size = response.ContentLength;//размер файла в байтах
                    if (download != null)
                        download.Invoke( (double)Size );
                    SizeUploaded = 0;//загружено байт всего
                    ByteSize = 0;//загружено байт за проход
                    Stream streamResponse = response.GetResponseStream();
                    using (FileStream SaveFileStream = new FileStream( path, FileMode.Create, FileAccess.Write ))
                    {
                        await Task.Run( () =>
                        {
                            do
                            {
                                ByteSize = streamResponse.Read( bufer, 0, bufer.Length );
                                SizeUploaded += ByteSize;
                                if (path != null)
                                {
                                    SaveFileStream.Write( bufer, 0, ByteSize );//пишем на диск
                                    if (uploaded != null)
                                        uploaded.Invoke( (double)SizeUploaded );
                                }
                            }
                            while (ByteSize > 0);
                        } );
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// асинхронная загрузка файла
        /// </summary>
        /// <param name="uploaded">делегат информирующий о размере уже загруженных байт</param>
        /// <param name="response">экземляр HttpWebResponse ответа сервера </param>
        /// <param name="token"'>токен отмены загрузки</param>
        /// <param name="path">полный путь для сохранения файла на диск</param>
        /// <param name="offset">позиция в загружаемом файле, с которой необходимо начать загрузку</param>
        public static async Task<bool> DownloadAsinc( Action<long> uploaded, HttpWebResponse response, CancellationToken token, string path )
        {
            try
            {
                bool c = false;
                byte[] bufer = new byte[1048576];//массив байт пишущийся на диск за один проход( 1 MByte )
                long SizeUploaded = 0;//загружено байт всего
                long FileSize = response.ContentLength;
                int ByteSize = 0;//загружено байт за проход
                int count = 3;//допустимое колличество попыток загрузки
                using (Stream streamResponse = response?.GetResponseStream())
                {
                    if (!File.Exists( path ))
                    {//файл на диске еще не создан
                        using (FileStream SaveFileStream = new FileStream( path, FileMode.Append, FileAccess.Write ))
                        {
                            await Task.Factory.StartNew( () =>
                            {
                                do
                                {
                                    try
                                    {
                                        ByteSize = streamResponse.Read( bufer, 0, bufer.Length );
                                        SaveFileStream.Write( bufer, 0, ByteSize );//пишем на диск
                                        SizeUploaded += ByteSize;
                                        uploaded?.Invoke( SizeUploaded );
                                        count--;
                                    }
                                    catch { }
                                }
                                while (!token.IsCancellationRequested &&
                                ByteSize > 0 ||
                                (ByteSize == 0 && count >= 0 && (SizeUploaded < FileSize) ||
                                FileSize < 0));
                            } );
                        }
                    }
                    else
                    {//файл на диске создан, но его необходимо дописать
                        SizeUploaded = new FileInfo( path ).Length;
                        response = await Request( response.ResponseUri.AbsoluteUri, 50000, SizeUploaded );
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (FileStream fileStream = File.OpenWrite( path ))
                            {
                                fileStream.Seek( SizeUploaded, SeekOrigin.Begin );
                                await Task.Factory.StartNew( () =>
                                {
                                    do
                                    {
                                        try
                                        {
                                            ByteSize = stream.Read( bufer, 0, bufer.Length );
                                            fileStream.Write( bufer, 0, ByteSize );//пишем на диск
                                            SizeUploaded += ByteSize;
                                            uploaded?.Invoke( SizeUploaded );
                                            count--;
                                        }
                                        catch { }
                                    }
                                    while
                                    (!token.IsCancellationRequested &&
                                    ByteSize > 0 ||
                                    (ByteSize == 0 && count >= 0 && (SizeUploaded < response?.ContentLength) ||
                                    response?.ContentLength < 0));
                                } );
                            }
                        }
                    }
                }
                DownloadCanceled?.Invoke();
                c = true;
                return c;
            }
            catch (Exception e)
            { MessageBox.Show( e.Message ); response?.Dispose(); return false; }
        }



        public static byte[] Open( string path )
        {
            try
            {
                byte[] array = null;
                using (FileStream reader = new FileStream( path, FileMode.Open ))
                {
                    array = new byte[reader.Length];
                    if (reader.CanRead)
                    {
                        reader.Read( array, 0, (int)reader.Length );
                    }
                }
                return array;
            }
            catch
            {
                return null;
            }
        }
        public static async void Save( string path, byte[] array )
        {
            try
            {
                using (FileStream writer = new FileStream( path, FileMode.Create ))
                {
                    if (writer.CanWrite)
                    {
                        await writer.WriteAsync( array, 0, array.Length );
                    }
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// открытие и десериализация файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Open<T>( string path )
        {
            try
            {
                T file = default( T );
                using (FileStream reader = new FileStream( path, FileMode.Open ))
                {
                    if (reader.CanRead)
                    {
                        try
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            file = (T)formatter.Deserialize( reader );
                        }
                        catch
                        {
                        }
                    }
                }
                return file;
            }
            catch
            {
                return default( T );
            }
        }

        /// <summary>
        /// сериализация и запись файла на диск
        /// </summary>
        /// <param name="path">адресс записи файла</param>
        /// <param name="file">записывваемый файл</param>
        public static async void Save<T>( string path, T file )
        {
            try
            {
                using (FileStream writer = new FileStream( path, FileMode.Create ))
                {
                    if (writer.CanWrite)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        await Task.Run( () => formatter.Serialize( writer, file ) );
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// удаление файла расположенного по заданному пути
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>"true" - файл найден и удален, "false" - файл не существует, либо его удаление не возможно</returns>
        public static bool Delete( string path )
        {
            try
            {
                FileInfo fileInfo = new FileInfo( path );
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Вывод окна выбора пути для сохранения файла
        /// </summary>
        /// <param name="title">заголовок окна</param>
        /// <param name="initialDirectory">начальный каталог отображаемый в окне</param>
        /// <param name="filter">текущая строка фильтра расширений файлов</param>
        /// <param name="defaultExt">расширение файла по умолчанию</param>
        /// <returns>имя файла выбранное в диалоговом окне</returns>
        public static string SaveFileDialog( string title, string initialDirectory, string filter, string defaultExt )
        {
            try
            {
                Forms.SaveFileDialog saveFileDialog = new Forms.SaveFileDialog()
                {
                    AddExtension = true,
                    Filter = filter,
                    DefaultExt = defaultExt,
                    Title = title,
                    InitialDirectory = initialDirectory,
                    RestoreDirectory = true,
                    ValidateNames = true
                };
                saveFileDialog.ShowDialog();
                return saveFileDialog.FileName;
            }
            catch { return null; }
        }

        /// <summary>
        /// Вывод окна выбора открываемого файла
        /// </summary>
        /// <param name="title">заголовок окна</param>
        /// <param name="initialDirectory">начальный каталог отображаемый в окне</param>
        /// <param name="filter">текущая строка фильтра расширений файлов</param>
        /// <param name="defaultExt">расширение файла по умолчанию</param>
        /// <returns>имя файла выбранное в диалоговом окне</returns>
        public static string OpenFileDialog( string title, string initialDirectory, string filter, string defaultExt )
        {
            try
            {
                Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog()
                {
                    AddExtension = true,
                    Filter = filter,
                    DefaultExt = defaultExt,
                    Title = title,
                    InitialDirectory = initialDirectory,
                    RestoreDirectory = true,
                    ValidateNames = true
                };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            }
            catch { return null; }
        }

        /// <summary>
        /// диалоговое окно для выбора каталога
        /// </summary>
        /// <param name="title">заголовок окна</param>
        /// <returns></returns>
        public static string FolderBrowserDialog( string title, string selectedCatalog )
        {
            try
            {
                Forms.FolderBrowserDialog folderBrowserDialog = new Forms.FolderBrowserDialog()
                {
                    ShowNewFolderButton = true,
                    Description = title,
                    SelectedPath = selectedCatalog
                };
                folderBrowserDialog.ShowDialog();
                return folderBrowserDialog.SelectedPath;
            }
            catch { return null; }
        }

        /// <summary>
        /// создание каталога и получение пути к нему для сохранения файлов
        /// </summary>
        /// <param name="directory">целевая директория, где будет располагаться каталог</param>
        /// <param name="name">имя создаваемого каталога</param>
        /// <returns>полный путь к созданному каталогу </returns>
        public static string DirectoryPath_Create( string directory, string name )
        {
            try
            {
                char[] array = name.ToArray();
                string path = "";
                StringBuilder sb = new StringBuilder( array.Length );
                for (int i = 0; i < array.Length; i++)
                {
                    if (char.IsLetterOrDigit( array[i] ))//только буквы и десятичные цифры
                    {
                        if (!(path.Length == 0 && char.IsDigit( array[i] )))//первый символ только буква
                            sb.Append( array[i] );
                    }
                    else if (char.IsWhiteSpace( array[i] ))//замена пробелов
                        sb.Append( '_' );
                }
                path = sb.ToString();
                if (!string.IsNullOrEmpty( path ))
                {
                    string path1 = Path.Combine( directory, path );
                    path = Directory.CreateDirectory( path1 ).FullName;
                }
                return path;
            }
            catch { return null; }
        }


        /// <summary>
        /// приведение url к валидному виду
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CastToValidURL( string url )
        {
            try
            {
                if (url != null && url != "")
                {
                    string result = "";
                    result = url.Trim();
                    if (!url.Contains( "http" ))
                        result = $"http://{url}";
                    string[] temp = url.Split( '.' );
                    if (temp.Length < 2)
                        result = "about:blank";
                    return result;
                }
                return "about:blank";
            }
            catch
            {
                return "about:blank";
            }
        }
        /// <summary>
        /// загрузка первичного HTML кода страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<string> DownLoad_PrimaryHTML( string url )
        {
            try
            {
                string str = null;
                HttpClient client = new HttpClient();
                str = await client.GetStringAsync( url );
                return str;
            }
            catch (HttpRequestException e)
            {
                return "";
            }
        }
        /// <summary>
        /// загрузка вторичного HTML кода страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownLoad_SecondaryHTML( string url )
        {
            try
            {
                bool c = false;
                string html = null;
                var dom = CQ.CreateFromUrl( url );
                html = ((CQ)dom).Render();
                //     ChromiumWebBrowser browser = new ChromiumWebBrowser(url);
                //     browser.FrameLoadEnd += async (s, e) =>
                //{
                //    if (e.Frame.IsMain)
                //    {
                //        html = await browser.GetSourceAsync();
                //        c = true;
                //    }
                //};
                //     while (!c) { Thread.Sleep(250); };
                return html;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// осуществление запроса к серверу
        /// </summary>
        /// <param name="url"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> Request( string url, int timeOut, long range )
        {
            try
            {
                HttpWebResponse response = null;
                int count = 3;
                do
                {
                    try
                    {
                        HttpWebRequest request = WebRequest.Create( new Uri( url, UriKind.Absolute ) ) as HttpWebRequest;
                        request.Headers.Add( HttpRequestHeader.Range, string.Format( "bytes={0}-", range ) );
                        request.Timeout = timeOut;
                        request.Method = "GET";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36 OPR/63.0.3368.71";
                        request.CookieContainer = request.CookieContainer ?? new CookieContainer();
                        request.CookieContainer.Add( new Uri( url ), new System.Net.Cookie( "beget", "begetok" ) );
                        request.AddRange( range );
                        response = (HttpWebResponse)await request.GetResponseAsync();
                    }
                    catch { response = null; }
                    count += response == null ? -1 : 0;
                } while (response == null && count > 0);
                return response;
            }
            catch { return null; }
        }

        /// <summary>
        /// шифрование массива по ключу
        /// </summary>
        /// <param name="array">массив для шифрования</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>зашифрованный массив</returns>
        public static byte[] Encrypt( byte[] array, string key )
        {
            byte[] result = new byte[0];
            try
            {
                if (array is byte[] data && data.Length > 0)
                {
                    SymmetricAlgorithm Sa = Rijndael.Create();
                    using (var encryptor = Sa.CreateEncryptor( (new PasswordDeriveBytes( key, null )).GetBytes( 16 ), new byte[16] ))
                    {
                        result = PerformCryptography( encryptor, data, key );
                    }
                    Sa.Dispose();
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
        /// <summary>
        /// дешифрование зашифрованного массива по ключу
        /// </summary>
        /// <param name="array">зашифрованный массив</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>дешифрованный массив</returns>
        public static byte[] Decrypt( byte[] array, string key )
        {
            byte[] result = new byte[0];
            try
            {
                if (array is byte[] data && data.Length > 0)
                {
                    SymmetricAlgorithm Sa = Rijndael.Create();
                    using (var decryptor = Sa.CreateDecryptor( (new PasswordDeriveBytes( key, null )).GetBytes( 16 ), new byte[16] ))
                    {
                        result = PerformCryptography( decryptor, data, key );
                    }
                    Sa.Dispose();
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
        private static byte[] PerformCryptography( ICryptoTransform cryptoTransform, byte[] data, string key )
        {
            byte[] result = new byte[0];
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream( memoryStream, cryptoTransform, CryptoStreamMode.Write ))
                    {
                        cryptoStream.Write( data, 0, data.Length );
                        cryptoStream.FlushFinalBlock();
                    }
                    result = memoryStream.ToArray();
                }
                return result;
            }
            catch
            {
                return result;
            }
        }




        /// <summary>
        /// преобразование строки в коллекцию строк в соответствии с управляющими символами в её составе
        /// </summary>
        /// <param name="line">строка</param>
        /// <returns>коллекция строк</returns>
        public static ObservableCollection<string> TextList( string line )
        {
            try
            {
                ObservableCollection<string> ls = new ObservableCollection<string>();
                if (line != null)
                {
                    char[] separator = new char[] { '\r', '\n' };
                    string[] l = line.Split( separator, StringSplitOptions.RemoveEmptyEntries );
                    foreach (string line1 in l)
                    {
                        if (!string.IsNullOrEmpty( line1 ))
                        {
                            line1.Trim( new char[] { ' ', '\r', '\n' } );
                            if (line1.Length > 0)
                                ls.Add( line1.Trim() );
                        }
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// преобразование коллекции строк в строку
        /// </summary>
        /// <param name="list">коллекция строк</param>
        /// <returns>результирующая строка</returns>
        public static string ListText( ObservableCollection<string> list )
        {
            try
            {
                string str = null;
                if (list != null && list.Count > 0)
                    str = list.Aggregate( ( a, b ) => $"{a}\r\n{b}" );
                return str;
            }
            catch
            {
                return null;
            }
        }

    }

}
