using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class TerminalSelection : UserControl, IDropDownChild
    {
        public TerminalSelection()
        {
            InitializeComponent();
        }

        
        public event EventHandler DropDownCanceled;

        public event EventHandler DropDownSelected;

      
      
    }
}
