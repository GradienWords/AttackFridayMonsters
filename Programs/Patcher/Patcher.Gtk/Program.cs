﻿using System;
using Eto.Forms;
using Patcher.Views;

namespace Patcher.Gtk
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Eto.Platforms.Gtk).Run(new MainForm());
        }
    }
}
