using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace Mklinker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        enum MklinkerPathType
        {
            Directory,
            File,
        }


        [Serializable]
        public class MklinkerException : Exception
        {
            public MklinkerException() { }
            public MklinkerException(string message) : base(message) { }
            public MklinkerException(string message, Exception inner) : base(message, inner) { }
            protected MklinkerException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        class MklinkerJob
        {
            public MklinkerPathType PathType { get; }
            public string TargetPath { get; }
            public string LastName { get; }

            public MklinkerJob(string path)
            {
                PathType = MklinkerPathType.File;
                if (Directory.Exists(path))
                {
                    PathType = MklinkerPathType.Directory;
                }
                TargetPath = path;
                LastName = Path.GetFileName(path);
            }

            public override string ToString()
            {
                return PathType.ToString() + " " + TargetPath;
            }
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var current = Directory.GetCurrentDirectory();

            // e.g. C:\Program Files\Git\bin
            var source = new MklinkerJob(TextBoxSource.Text);

            // e.g. C:\Users\user\Documents
            var dest = new MklinkerJob(TextBoxDest.Text);

            // e.g. C:\Users\user\Documents + \ + bin
            var generateLink = Path.Combine(dest.TargetPath, Path.GetFileName(Path.TrimEndingDirectorySeparator(source.TargetPath)));

            switch (source.PathType)
            {
                case MklinkerPathType.Directory:
                    Debug.WriteLine(current);
                    Directory.CreateSymbolicLink(generateLink, source.TargetPath);
                    break;
                case MklinkerPathType.File:
                    File.CreateSymbolicLink(generateLink, source.TargetPath);
                    break;
                default:
                    throw new MklinkerException("No such MklinkerPathType");
                    break;
            }

        }
    }
}
