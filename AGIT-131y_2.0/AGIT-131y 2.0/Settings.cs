using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace AGIT_131y_2._0
{
    class Settings
    {
        public Settings(Label l1, Label l2, Label l3)
        {

            l1.Font = Properties.Settings.Default.Font1;
            l2.Font = Properties.Settings.Default.Font2;
            l3.Font = Properties.Settings.Default.Font3;
            l1.ForeColor = Properties.Settings.Default.Color1;
            l2.ForeColor = Properties.Settings.Default.Color2;
            l3.ForeColor = Properties.Settings.Default.Color3;
        }
    }
}
