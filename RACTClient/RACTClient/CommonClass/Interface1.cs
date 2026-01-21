using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public interface IDropDownChild
    {
        event EventHandler DropDownCanceled;
        event EventHandler DropDownSelected;
    }
}
