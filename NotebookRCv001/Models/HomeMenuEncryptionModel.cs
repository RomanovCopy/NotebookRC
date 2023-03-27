using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Cryptography;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;

namespace NotebookRCv001.Models
{
    public class HomeMenuEncryptionModel : ViewModelBase
    {
        public readonly MainWindowViewModel MainWindowViewModel;

        internal ObservableCollection<string> Headers => MainWindowViewModel.Language.HomeMenuEncryption;
        internal ObservableCollection<string> ToolTips => MainWindowViewModel.Language.ToolTipsHomeMenuEncryption;


        /// <summary>
        /// ключ шифрования
        /// </summary>
        internal string EncryptionKey
        {
            get => encryptionKey;
            private set => SetProperty(ref encryptionKey, value);
        }
        private string encryptionKey;


        internal string EncryptionStatus { get => encryptionStatus; private set => SetProperty(ref encryptionStatus, value); }
        private string encryptionStatus;

        internal HomeMenuEncryptionModel()
        {
            MainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            MainWindowViewModel.Language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            EncryptionStatus = string.IsNullOrWhiteSpace(EncryptionKey) ? "Off" : "On";
        }

        /// <summary>
        /// шифрование
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_DeleteKey( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(EncryptionKey);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_DeleteKey( object obj )
        {
            try
            {
                EncryptionKey = null;
                EncryptionStatus = "Off";
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// установка ключа шифрования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_InstalKey( object obj )
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace(EncryptionKey);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InstalKey( object obj )
        {
            try
            {
                Views.InputWindow inputWindow = new Views.InputWindow();
                inputWindow.Owner = Application.Current.MainWindow;
                inputWindow.Closing += ( s, e ) => InputWindow_Closing(s, e);
                inputWindow.ShowDialog();
                EncryptionStatus = string.IsNullOrWhiteSpace(EncryptionKey) ? "Off" : "On";
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// шифрование отдельного файла
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_EncryptIndividualFile( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( EncryptionKey ) && !MainWindowViewModel.FrameList.Any( ( x ) => x is MyControls.EncryptIndividualFile );
                return c;
            }catch(Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_EncryptIndividualFile( object obj )
        {
            try
            {
                var page = new MyControls.EncryptIndividualFile();
                if (MainWindowViewModel.FrameListAddPage.CanExecute( page ))
                    MainWindowViewModel.FrameListAddPage.Execute( page );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        private void InputWindow_Closing( object s, CancelEventArgs e )
        {
            try
            {
                if (s is Views.InputWindow window && window.DataContext is InputWindowViewModel viewmodel)
                {
                    EncryptionKey = viewmodel.EncryptionKey;
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        internal string Encryption( string str, string keyCrypt, Encoding cod )//шифрование строки str по ключу keyCrypt с кодировкой строки cod
        {
            return Convert.ToBase64String(Encrypt(cod.GetBytes(str), keyCrypt));
        }
        internal byte[] Encrypt( byte[] key, string value )
        {

            SymmetricAlgorithm Sa = Rijndael.Create();
            ICryptoTransform Ct = Sa.CreateEncryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]);

            MemoryStream Ms = new MemoryStream();
            CryptoStream Cs = new CryptoStream(Ms, Ct, CryptoStreamMode.Write);

            Cs.Write(key, 0, key.Length);
            Cs.FlushFinalBlock();

            byte[] Result = Ms.ToArray();

            Ms.Close();
            Ms.Dispose();

            Cs.Close();
            Cs.Dispose();

            Ct.Dispose();

            return Result;
        }

        internal string Decryption( string str, string keyCrypt )//дешифровка строки str по ключу keyCrypt
        {
            string Result;
            try
            {
                CryptoStream Cs = Decrypt(Convert.FromBase64String(str), keyCrypt);
                StreamReader Sr = new StreamReader(Cs);

                Result = Sr.ReadToEnd();

                Cs.Close();
                Cs.Dispose();

                Sr.Close();
                Sr.Dispose();
            }
            catch (Exception)
            {
                return null;
            }

            return Result;
        }
        internal CryptoStream Decrypt( byte[] key, string value )
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]);
            MemoryStream ms = new MemoryStream(key);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }



    }
}
