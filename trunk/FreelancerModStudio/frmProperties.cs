using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreelancerModStudio
{
    public partial class frmProperties : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public delegate void OptionsChangedType(OptionChangedValue[] options);
        public OptionsChangedType OptionsChanged;

        private void OnOptionsChanged(OptionChangedValue[] options)
        {
            if (this.OptionsChanged != null)
                this.OptionsChanged(options);
        }

        public frmProperties()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Properties;
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
            List<CustomPropertyCollection> propertyObjects = new List<CustomPropertyCollection>();

            //loop each selected block
            foreach (Settings.EditorINIBlock block in blocks)
            {
                CustomPropertyCollection properties = new CustomPropertyCollection();

                //loop each options
                for (int i = 0; i < block.Options.Count; i++)
                {
                    string category = "";
                    string comment = "";
                    int categoryIndex = Helper.Template.Data.Files[templateIndex].Blocks[block.TemplateIndex].Options[block.Options[i].TemplateIndex].Category;
                    if (Helper.Template.Data.Language != null && Helper.Template.Data.Language.Categories.Count > categoryIndex)
                        category = Helper.Template.Data.Language.Categories[categoryIndex].Value;

                    bool multiple = Helper.Template.Data.Files[templateIndex].Blocks[block.TemplateIndex].Options[block.Options[i].TemplateIndex].Multiple;
                    string name = Helper.Template.Data.Files[templateIndex].Blocks[block.TemplateIndex].Options[block.Options[i].TemplateIndex].Name;

                    CustomPropertyCollection subProperties = new CustomPropertyCollection();

                    //loop each option
                    for (int j = 0; j < block.Options[i].Values.Count; j++)
                    {
                        CustomPropertyItem subProperty = new CustomPropertyItem(block.Options[i].Name, block.Options[i].Values[j].Value, block.Options[i].Values[j].Value, new PropertyTag(i, j), category, comment, false);
                        if (block.Options[i].Values[j].SubOptions != null)
                        {
                            //add child properties
                            string childComment = "";
                            CustomPropertyCollection childSubProperties = new CustomPropertyCollection();

                            //loop each child option
                            for (int k = 0; k < block.Options[i].Values[j].SubOptions.Values.Count; k++)
                                childSubProperties.Add(new CustomPropertyItem(block.Options[i].Values[j].SubOptions.Name, block.Options[i].Values[j].SubOptions.Values[k], block.Options[i].Values[j].SubOptions.Values[k], new PropertyTag(j, k), category, childComment, false));

                            //add empty line
                            childSubProperties.Add(new CustomPropertyItem(block.Options[i].Values[j].SubOptions.Name, "", "", new PropertyTag(j, block.Options[i].Values[j].SubOptions.Values.Count), category, childComment, false));

                            CustomPropertyCollection childProperties = new CustomPropertyCollection();
                            childProperties.Add(subProperty);

                            if (Helper.Template.Data.Files[templateIndex].Blocks[block.TemplateIndex].Options[block.Options[i].Values[j].SubOptions.TemplateIndex].Multiple)
                            {
                                //add multiple children
                                childProperties.Add(new CustomPropertyItem(childSubProperties[0].Name, childSubProperties, childSubProperties, null, category, childComment, false,
                                    new TypeConverterAttribute(typeof(CustomExpandableObjectConverter)),
                                    new EditorAttribute(typeof(System.Drawing.Design.UITypeEditor), typeof(System.Drawing.Design.UITypeEditor))));
                            }
                            else
                                //add single child
                                childProperties.Add(childSubProperties[0]);

                            //add new sub property
                            subProperties.Add(new CustomPropertyItem(childProperties[j].Name, childProperties, childProperties, null, category, comment, false,
                                new TypeConverterAttribute(typeof(CustomExpandableObjectConverter)),
                                new EditorAttribute(typeof(System.Drawing.Design.UITypeEditor), typeof(System.Drawing.Design.UITypeEditor))));
                        }
                        else
                            //add new sub property
                            subProperties.Add(subProperty);
                    }
					
                    CustomPropertyItem property;
                    if (multiple)
                    {
                        //add empty line
                        subProperties.Add(new CustomPropertyItem(block.Options[i].Name, "", "", new PropertyTag(i, subProperties.Count), category, comment, false));

                        property = new CustomPropertyItem(block.Options[i].Name, subProperties, subProperties, new PropertyTag(i, 0), category, comment, false,
                            new TypeConverterAttribute(typeof(CustomExpandableObjectConverter)),
                            new EditorAttribute(typeof(System.Drawing.Design.UITypeEditor), typeof(System.Drawing.Design.UITypeEditor)));
                    }
                    else
                    {
                        if (subProperties.Count > 0)
                            property = new CustomPropertyItem(block.Options[i].Name, subProperties[0].Value, subProperties[0].Value, new PropertyTag(i, 0), category, comment, false);
                        else
                            property = new CustomPropertyItem(block.Options[i].Name, "", "", new PropertyTag(i, 0), category, comment, false);
                    }
                    properties.Add(property);
                }
                propertyObjects.Add(properties);
            }

            propertyGrid.SelectedObjects = propertyObjects.ToArray();
            propertyGrid.ExpandAllGridItems();
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertyGrid.HelpVisible = descriptionToolStripMenuItem.Checked;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            List<OptionChangedValue> optionsChanged = new List<OptionChangedValue>();
            for (int i = 0; i < propertyGrid.SelectedObjects.Length; i++)
            {
                PropertyTag propertyTag = (PropertyTag)((CustomPropertyDescriptor)e.ChangedItem.PropertyDescriptor).PropertyItem.Tag;

                CustomPropertyItem property = ((CustomPropertyCollection)propertyGrid.SelectedObjects[i])[propertyTag.OptionIndex];

                //add new empty line if needed
                if (property.Value is CustomPropertyCollection)
                {
                    CustomPropertyCollection subProperties = (CustomPropertyCollection)property.Value;

                    if (e.ChangedItem.Value.ToString().Trim() != "" && propertyTag.OptionEntryIndex == subProperties.Count - 1)
                        subProperties.Add(new CustomPropertyItem(e.ChangedItem.Label, "", "", new PropertyTag(i, subProperties.Count), subProperties[propertyTag.OptionEntryIndex].Category, subProperties[propertyTag.OptionEntryIndex].Description, false));
                    else if (e.ChangedItem.Value.ToString().Trim() == "" && propertyTag.OptionEntryIndex == subProperties.Count - 2)
                        subProperties.RemoveAt(subProperties.Count - 1);

                    propertyGrid.Refresh();
                }

                optionsChanged.Add(new OptionChangedValue(i, propertyTag.OptionIndex, propertyTag.OptionEntryIndex, e.ChangedItem.Value));
            }

            OnOptionsChanged(optionsChanged.ToArray());
        }
    }

    public class PropertyTag
    {
        public int OptionIndex;
        public int OptionEntryIndex;

        public PropertyTag(int optionIndex, int optionEntryIndex)
        {
            OptionIndex = optionIndex;
            OptionEntryIndex = optionEntryIndex;
        }
    }

    public class OptionChangedValue
    {
        public int PropertyIndex;
        public int OptionIndex;
        public int OptionEntryIndex;
        public object NewValue;

        public OptionChangedValue(int propertyIndex, int optionIndex, int optionEntryIndex, object newValue)
        {
            PropertyIndex = propertyIndex;
            OptionIndex = optionIndex;
            OptionEntryIndex = optionEntryIndex;
            NewValue = newValue;
        }
    }
}