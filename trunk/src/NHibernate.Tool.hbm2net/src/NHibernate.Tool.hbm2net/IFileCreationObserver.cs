﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.hbm2net
{
    public interface IFileCreationObserver
    {
        void FileCreated(string path);
    }
}
