using BSolution_.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BSolution_
{
    public enum PropertyType
    {
        Binary,
        Filter
    }
    public partial class PropertiesForm : DockContent

    {
        Dictionary<string, TabPage> _allTabs = new Dictionary<string, TabPage>();
        public PropertiesForm()
        {
            InitializeComponent();

            LoadOptionControl(PropertyType.Binary);
            LoadOptionControl(PropertyType.Filter);

        }

        private void LoadOptionControl(PropertyType propType)
        {
            string tabName = propType.ToString();

            foreach (TabPage tabpage in tabPropControl.TabPages)
            {
                if (tabpage.Text == tabName)
                    return;
            }

            if (_allTabs.TryGetValue(tabName, out TabPage page))
            {
                tabPropControl.TabPages.Add(page);
                return;
            }

            UserControl _inspProp = CreateUserControl(propType);
            if (_inspProp == null)
                return;

            TabPage newTab = new TabPage(tabName)
            {
                Dock = DockStyle.Fill
            };
            _inspProp.Dock = DockStyle.Fill;
            newTab.Controls.Add(_inspProp);
            tabPropControl.TabPages.Add(newTab);
            tabPropControl.SelectedTab = newTab;

            _allTabs[tabName] = newTab;
        }






        private UserControl CreateUserControl(PropertyType propType)
        {
            UserControl curProp = null;
            switch (propType)
            {
                case PropertyType.Binary:
                    BinaryProp blobProp = new BinaryProp();
                    curProp = blobProp;
                    break;

                case PropertyType.Filter:
                    ImageFilterProp filterProp = new ImageFilterProp();
                    curProp = filterProp;
                    break;

                default:
                    MessageBox.Show("유효하지 않은 옵션입니다.");
                    return null;
            }
            return curProp;
        }
    }
}

