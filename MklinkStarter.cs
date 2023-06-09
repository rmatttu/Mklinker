﻿using Mklinker.External;
using Mklinker.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mklinker {
    class MklinkStarter {
        readonly Logger logger;
        ExternalProgramStartForm f;

        public MklinkStarter() {
            logger = new Logger();
        }

        public void Start(string sourcePath, string linkFolderPath,Form owner) {
            string command = BuildCommand(sourcePath, linkFolderPath);
            RunElevated.RunCommand(command, owner);
            logger.WriteLine(command);
            logger.flush();
        }

        string BuildCommand(string sourcePath, string linkDestPath) {
            string command = "mklink ";
            bool isFolder = IsExist(sourcePath, linkDestPath);
            if (isFolder) {
                string createFolderName = GetParentFolderName(sourcePath);
                command += string.Format("/d {0} {1}", BundleDoubleQuotation(linkDestPath + "\\" + createFolderName), BundleDoubleQuotation(sourcePath));
            } else {
                FilePathAnalyzer analyzer = new FilePathAnalyzer(sourcePath);
                command += string.Format("{0} {1}", BundleDoubleQuotation(linkDestPath + "\\" + analyzer.Target), BundleDoubleQuotation(sourcePath));
            }
            return command;
        }

        bool IsExist(string sourcePath, string linkPath) {
            bool src = Directory.Exists(sourcePath);
            bool link = Directory.Exists(linkPath);
            if (link) {
                return src;
            } else {
                throw new Exception("リンク元にはフォルダーを指定して下さい");
            }
        }

        string GetParentFolderName(string path) {
            // TODO: slashのときは？
            int index = path.LastIndexOf('\\');
            return path.Substring(index + 1);
        }

        string BundleDoubleQuotation(string str) {
            return @"""" + str + @"""";
        }

        public void Close() {
            logger.Close();
        }

    }
}
