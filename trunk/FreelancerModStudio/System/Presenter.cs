﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using FreelancerModStudio.Data;
using HelixEngine;
using HelixEngine.Wires;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;

namespace FreelancerModStudio.SystemPresenter
{
    public class Presenter
    {
        public Table<int, ContentBase> Objects { get; set; }
        public HelixView3D Viewport { get; set; }
        public bool IsUniverse { get; set; }

        ModelVisual3D lightning;
        public ModelVisual3D Lightning
        {
            get
            {
                return lightning;
            }
            set
            {
                int index = Viewport.Children.IndexOf(lightning);
                if (index != -1)
                {
                    if (value != null)
                        Viewport.Children[index] = value;
                    else
                        Viewport.Children.RemoveAt(index);
                }
                else if (value != null)
                {
                    Viewport.Children.Insert(0, value);
                    secondLayerID++;
                }

                lightning = value;
            }
        }

        ModelVisual3D selection;
        public ModelVisual3D Selection
        {
            get
            {
                return selection;
            }
            set
            {
                int index = Viewport.Children.IndexOf(selection);
                if (index != -1)
                {
                    if (value != null)
                        Viewport.Children[index] = value;
                    else
                        Viewport.Children.RemoveAt(index);
                }
                else if (value != null)
                    Viewport.Children.Insert(0, value);

                selection = value;
            }
        }

        ContentBase selectedContent;
        public ContentBase SelectedContent
        {
            get
            {
                return selectedContent;
            }
            set
            {
                if (selectedContent != value)
                    SetSelectedContent(value, true);
            }
        }

        public delegate void SelectionChangedType(ContentBase content);
        public SelectionChangedType SelectionChanged;

        void OnSelectionChanged(ContentBase content)
        {
            if (SelectionChanged != null)
                SelectionChanged(content);
        }

        public delegate void FileOpenType(string file);
        public FileOpenType FileOpen;

        void OnFileOpen(string file)
        {
            if (FileOpen != null)
                FileOpen(file);
        }

        int secondLayerID;

        public Presenter(HelixView3D viewport)
        {
            Objects = new Table<int, ContentBase>();
            Viewport = viewport;
            Viewport.SelectionChanged += camera_SelectionChanged;
            Lightning = new SystemLightsVisual3D();
        }

        void SetSelectedContent(ContentBase content, bool lookAt)
        {
            selectedContent = content;

            if (content != null)
            {
                //goto content
                if (lookAt)
                    LookAt(content);

                //select content visually
                Selection = GetSelectionBox(content);

                Viewport.Title = content.Title;
            }
            else
            {
                Selection = null;
                Viewport.Title = string.Empty;
            }
        }

        public void LookAt(ContentBase content)
        {
            Viewport.LookAt(content.GetPositionPoint(), Animator.AnimationDuration.TimeSpan.TotalMilliseconds);
        }

        void AddContent(ContentBase content)
        {
            //load model it was is not loaded yet
            if (content.Model == null)
                content.LoadModel();

            //only add it to viewpoint if its actually visible
            if (content.Visibility)
            {
                AddModel(content);

                if (content == SelectedContent)
                    Selection = GetSelectionBox(content);
            }

            Objects.Add(content);
        }

        public void Add(List<TableBlock> blocks)
        {
            Animator.AnimationDuration = new Duration(TimeSpan.Zero);

            foreach (TableBlock block in blocks)
            {
                ContentBase content = GetContent(block);

                if (content != null)
                    AddContent(content);
            }

            Animator.AnimationDuration = new Duration(TimeSpan.FromMilliseconds(500));
        }

        void AddModel(ContentBase content)
        {
            if (content.IsEmissive())
                Viewport.Children.Add(content.Model);
            else
            {
                Viewport.Children.Insert(secondLayerID, content.Model);
                secondLayerID++;
            }
        }

        public void Delete(List<TableBlock> blocks)
        {
            foreach (TableBlock block in blocks)
            {
                ContentBase content;
                if (Objects.TryGetValue(block.UniqueID, out content))
                    Delete(content);
            }
        }

        public void Delete(ContentBase content)
        {
            //if we delete a system also delete all universe connections to and from it
            if (IsUniverse && content is System)
                DeleteConnections(content as System);

            Objects.Remove(content);
            RemoveModel(content);
        }

        void RemoveModel(ContentBase content)
        {
            Viewport.Children.Remove(content.Model);

            if (!content.IsEmissive())
                secondLayerID--;
        }

        void camera_SelectionChanged(DependencyObject visual)
        {
            ModelVisual3D model = (ModelVisual3D)visual;
            foreach (ContentBase content in Objects)
            {
                if (content.Model == model)
                {
                    if (SelectedContent == content)
                    {
                        if (IsUniverse && content is System)
                            DisplayContextMenu(((System)content).Path);
                    }
                    else
                        SetSelectedContent(content, false);

                    OnSelectionChanged(content);
                    return;
                }
            }
        }

        void DisplayContextMenu(string path)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem
                                {
                                    Header = string.Format(Properties.Strings.SystemPresenterOpen, global::System.IO.Path.GetFileName(path)),
                                    Tag = path
                                };
            item.Click += item_Click;

            menu.Items.Add(item);
            menu.IsOpen = true;
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            OnFileOpen((string)((MenuItem)sender).Tag);
        }

        ModelVisual3D GetSelectionBox(ContentBase content)
        {
            WireLines lines = GetWireBox(new Vector3D(1.0d, 1.0d, 1.0d));
            lines.Transform = content.Model.Transform;

            return lines;
        }

        WireLines GetWireBox(Vector3D bounds)
        {
            var points = new Point3DCollection
                                           {
                                               new Point3D(bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, -bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, bounds.Z),
                                               new Point3D(bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(bounds.X, -bounds.Y, -bounds.Z),
                                               new Point3D(-bounds.X, bounds.Y, -bounds.Z),
                                               new Point3D(-bounds.X, -bounds.Y, -bounds.Z)
                                           };

            return new WireLines { Lines = points, Color = Colors.Yellow };
        }

        public void SetVisibility(ContentBase content, bool visibility)
        {
            if (content.Visibility != visibility)
            {
                content.Visibility = visibility;

                if (visibility)
                    //show model
                    AddModel(content);
                else
                    //hide model
                    RemoveModel(content);
            }
        }

        public void ClearDisplay(bool light)
        {
            Viewport.Children.Clear();

            if (light || Lightning == null)
                secondLayerID = 0;
            else
            {
                Viewport.Children.Add(Lightning);
                secondLayerID = 1;
            }
        }

        public void DisplayUniverse(string path, int systemTemplate, List<TableBlock> blocks, ArchetypeManager archetype)
        {
            //filter the systems to improve speed as we need to loop them often in the analyzer
            List<TableBlock> systems = new List<TableBlock>();
            foreach (TableBlock block in blocks)
            {
                if (block.ObjectType == ContentType.System)
                    systems.Add(block);
            }

            Analyzer analyzer = new Analyzer
                                    {
                                        Universe = systems,
                                        UniversePath = path,
                                        SystemTemplate = systemTemplate,
                                        Archetype = archetype
                                    };
            analyzer.Analyze();

            DisplayUniverseConnections(analyzer.Connections);
        }

        void DisplayUniverseConnections(Table<UniverseConnectionID, UniverseConnection> connections)
        {
            Viewport.Dispatcher.Invoke((MethodInvoker)(delegate
            {
                foreach (UniverseConnection connection in connections)
                    Viewport.Children.Add(GetConnection(connection).Model);
            }));
        }

        void DeleteConnections(System system)
        {
            foreach (Connection connection in system.Connections)
                Delete(connection);
        }

        void UpdateConnections(System system)
        {
            foreach (Connection connection in system.Connections)
                SetConnection(connection);
        }

        Connection GetConnection(UniverseConnection connection)
        {
            Connection line = new Connection();
            SetConnection(line, connection);

            line.LoadModel();
            return line;
        }

        void SetConnection(Connection line, UniverseConnection connection)
        {
            line.From = Objects[connection.From.ID];
            line.To = Objects[connection.To.ID];
            line.FromType = GetConnectionType(connection.From.Jumpgate, connection.From.Jumphole);
            line.ToType = GetConnectionType(connection.To.Jumpgate, connection.To.Jumphole);

            ((System)line.From).Connections.Add(line);
            ((System)line.To).Connections.Add(line);

            SetConnection(line);
        }

        void SetConnection(Connection line)
        {
            Vector3D fromPosition = line.From.GetPosition();
            Vector3D toPosition = line.To.GetPosition();

            Vector3D position = (fromPosition + toPosition) / 2;
            Vector3D scale = new Vector3D(0.4, (fromPosition - toPosition).Length, 1);

            if (line.FromType == ConnectionType.Both || line.ToType == ConnectionType.Both)
                scale.X = 0.7;

            Vector v1 = new Vector(fromPosition.X, fromPosition.Y);
            Vector v2 = new Vector(toPosition.X, toPosition.Y);

            double a = Difference(v2.X, v1.X);
            double b = Difference(v2.Y, v1.Y);
            double factor = 1;
            double angleOffset = 90;

            if (v2.X < v1.X)
                factor = -1;

            if (v2.Y < v1.Y)
            {
                angleOffset = -90;
                factor *= -1;
            }

            double c = Math.Sqrt(a * a + b * b);
            double angle = Math.Acos(a / c) * 180 / Math.PI;

            Vector3D rotation = new Vector3D(0, 0, (angle + angleOffset) * factor);

            line.SetDisplay(position, rotation, scale);
        }

        ConnectionType GetConnectionType(bool jumpgate, bool jumphole)
        {
            if (jumpgate && !jumphole)
                return ConnectionType.JumpGate;
            if (!jumpgate && jumphole)
                return ConnectionType.JumpHole;
            if (jumpgate && jumphole)
                return ConnectionType.Both;

            return ConnectionType.None;
        }

        double Difference(double x, double y)
        {
            if (x > y)
                return x - y;

            return y - x;
        }

        public void ChangeValues(ContentBase content, TableBlock block)
        {
            if (block.ObjectType == ContentType.None)
            {
                //delete content if it was changed back to an invalid type
                Delete(content);
                if (selectedContent == content)
                    SelectedContent = null;
            }
            else
                SetValues(content, block);
        }

        void SetValues(ContentBase content, TableBlock block)
        {
            SystemParser parser = new SystemParser();
            parser.SetValues(content, block);
            content.Title = block.Name;

            if (parser.ModelChanged && content.Model != null)
                ReloadModel(content);

            if (IsUniverse && content is System)
                UpdateConnections(content as System);

            if (selectedContent == content)
            {
                //update selection if changed content is selected
                SetSelectedContent(content, true);
            }
        }

        void ReloadModel(ContentBase content)
        {
            int index = Viewport.Children.IndexOf(content.Model);
            if (index != -1)
            {
                Viewport.Children.RemoveAt(index);
                content.LoadModel();
                Viewport.Children.Insert(index, content.Model);
            }
        }

        ContentBase GetContent(TableBlock block)
        {
            ContentBase content = CreateContent(block.ObjectType);
            if (content == null)
                return null;

            content.Visibility = block.Visibility;
            SetValues(content, block);

            content.ID = block.UniqueID;
            return content;
        }

        ContentBase CreateContent(ContentType type)
        {
            switch (type)
            {
                case ContentType.None:
                    return null;
                case ContentType.LightSource:
                    return new LightSource();
                case ContentType.Zone:
                    return new Zone();
                case ContentType.System:
                    return new System();
                default:
                    return new SystemObject();
            }
        }
    }
}