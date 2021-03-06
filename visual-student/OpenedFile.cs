﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace visual_student
{
    //public class OpenedFile : INotifyPropertyChanged
    public class OpenedFile : INotifyPropertyChanged
    {
        //Implementation of INotifyPropertyChanged interface 
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _name;
        private string _body;
        private string _path;
        private bool _modified;

        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged();} }
        public string Body {
            get { return _body; }
            set {
                    _body = value;
                    NotifyPropertyChanged();
                }
        }
        public string Path { get { return _path; } set { _path = value; NotifyPropertyChanged();} }
        public bool Modified { get { return _modified; } set { _modified = value; NotifyPropertyChanged(); } }
        public OpenedFile(string name, string body, string path)
        {
            Name = name;
            Body = body;
            Path = path;
            Modified = false;
        }

        public OpenedFile()
        {
            Name = "New File";
            Body = "";
            Path = "";
            Modified = true;
        }

        public void Save()
        {
            if(Path == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "C# Files (*cs) |*.cs";
                sfd.AddExtension = true;
                sfd.OverwritePrompt = true;
                if (sfd.ShowDialog() == true)
                {
                    this.Path = System.IO.Path.GetFullPath(sfd.FileName);
                    this.Name = System.IO.Path.GetFileName(sfd.FileName);
                }
                else
                    return;
            }

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Body);
                sw.Close();
                Modified = false;
            } catch (UnauthorizedAccessException e)
            {
            }
        }


        public void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(Path != "")
                sfd.InitialDirectory = System.IO.Path.GetDirectoryName(Path);
            sfd.OverwritePrompt = true;
            sfd.Filter = "C# Files (*cs) |*.cs";
            if (sfd.ShowDialog() == true)
            {
                this.Path = System.IO.Path.GetFullPath(sfd.FileName);
                this.Name = System.IO.Path.GetFileName(sfd.FileName);
            }
            else
                return;

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < Body.Length; i++)
                    sw.Write(Body[i]);
                sw.Close();
                Modified = false;
            }
            catch (UnauthorizedAccessException e)
            {
            }
        }

        public static OpenedFile LoadFromFileStream(string path, string name)
        {
            StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open));
            StringBuilder sb = new StringBuilder();

            while (!sr.EndOfStream)
            {
                sb.Append(sr.ReadLine() + "\n");
            }
            sr.Close();
            return new OpenedFile(name, sb.ToString(), path);
        }
    }
}
