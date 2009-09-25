using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreelancerModStudio
{
    public partial class frmProperties : WeifenLuo.WinFormsUI.Docking.DockContent, ContentInterface
    {
        public delegate void OptionsChangedType(PropertyBlock[] blocks);
        public OptionsChangedType OptionsChanged;

        public delegate void DisplayChangedType(ContentInterface content);
        public DisplayChangedType DisplayChanged;

        private void OnOptionsChanged(PropertyBlock[] blocks)
        {
            if (this.OptionsChanged != null)
                this.OptionsChanged(blocks);
        }

        private void OnDisplayChanged(ContentInterface content)
        {
            if (this.DisplayChanged != null)
                this.DisplayChanged(content);
        }

        public frmProperties()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Properties;

            RefreshSettings();
        }

        public void RefreshSettings()
        {
            this.TabText = Properties.Strings.PropertiesText;
        }

        public void ClearData()
        {
            if (propertyGrid.SelectedObject != null)
                propertyGrid.SelectedObject = null;
        }

        public void ShowData(Settings.EditorINIBlock[] blocks, int templateIndex)
        {
            List<PropertyBlock> propertyBlocks = new List<PropertyBlock>();

            //loop each selected block
            foreach (Settings.EditorINIBlock block in blocks)
                propertyBlocks.Add(new PropertyBlock(block, Helper.Template.Data.Files[templateIndex].Blocks[block.TemplateIndex]));

            propertyGrid.SelectedObjects = propertyBlocks.ToArray();
            propertyGrid.ExpandAllGridItems();

            //ensure visibile of selected grid item
            propertyGrid.SelectedGridItem.Select();
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertyGrid.HelpVisible = descriptionToolStripMenuItem.Checked;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Value != e.OldValue)
                OnOptionsChanged((PropertyBlock[])propertyGrid.SelectedObjects);
        }

        private void propertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (this.DockHandler.IsActivated)
                this.OnDisplayChanged((ContentInterface)this);
        }

        public bool CanAdd()
        {
            if (propertyGrid.SelectedGridItem != null)
                return propertyGrid.SelectedGridItem.Value is PropertySubOptions || propertyGrid.SelectedGridItem.Parent.Value is PropertySubOptions;
            else
                return false;
        }

        public bool CanCopy()
        {
            return false;
        }

        public bool CanCut()
        {
            return false;
        }

        public bool CanPaste()
        {
            return false;
        }

        public bool CanDelete()
        {
            return false;
        }

        public bool CanSelectAll()
        {
            return false;
        }

        public void Add(int index)
        {
            if (propertyGrid.SelectedGridItem.Value is PropertySubOptions)
                propertyGrid.SelectedGridItem.GridItems[propertyGrid.SelectedGridItem.GridItems.Count - 1].Select();
            else if (propertyGrid.SelectedGridItem.Parent.Value is PropertySubOptions)
                propertyGrid.SelectedGridItem.Parent.GridItems[propertyGrid.SelectedGridItem.Parent.GridItems.Count - 1].Select();
        }

        public void Delete()
        {
            PropertyOption option = (PropertyOption)((PropertyOptionDescriptor)propertyGrid.SelectedGridItem.PropertyDescriptor).PropertyOption;
            object oldValue = option.Value;
            option.Value = "";

            propertyGrid_PropertyValueChanged(propertyGrid, new PropertyValueChangedEventArgs(propertyGrid.SelectedGridItem, oldValue));
        }

        public bool CanAddMultiple()
        {
            return false;
        }

        public bool CanSave()
        {
            return false;
        }

        public void SelectAll()
        {
            throw new NotImplementedException();
        }

        public ToolStripDropDown MultipleAddDropDown()
        {
            throw new NotImplementedException();
        }

        public string GetTitle()
        {
            throw new NotImplementedException();
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public void Cut()
        {
            throw new NotImplementedException();
        }
    }
}