﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreelancerModStudio.Data.IO;
using System.Globalization;
using FreelancerModStudio.SystemPresenter;

namespace FreelancerModStudio.Data
{
    public class ArchetypeManager
    {
        SortedList<string, ArchetypeInfo> contentTable;

        public ArchetypeInfo TypeOf(string archetype)
        {
            ArchetypeInfo info;
            if (contentTable != null && contentTable.TryGetValue(archetype, out info))
                return info;

            return null;
        }

        public static string GetRelativeArchetype(string file, int fileTemplate, int archetypeTemplate)
        {
            //get archetype path
            if (fileTemplate > 0 && fileTemplate < Helper.Template.Data.Files.Count)
            {
                string[] directories = Helper.Template.Data.Files[fileTemplate].Pathes[0].Split(new char[] { System.IO.Path.DirectorySeparatorChar });
                StringBuilder archetypePath = new StringBuilder(file);
                for (int i = 0; i < directories.Length; i++)
                {
                    int lastIndex = archetypePath.ToString().LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                    if (lastIndex == -1)
                        break;
                    archetypePath.Remove(lastIndex, archetypePath.Length - lastIndex);
                }

                archetypePath.Append(@"\Solar\SolarArch.ini");

                if (System.IO.File.Exists(archetypePath.ToString()))
                    return archetypePath.ToString();
            }

            return null;
        }

        public void CreateContentTable(List<TableBlock> blocks)
        {
            CultureInfo usCulture = new CultureInfo("en-US", false);
            contentTable = new SortedList<string, ArchetypeInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (TableBlock block in blocks)
            {
                if (block.Block.Name.ToLower() == "solar")
                {
                    ContentType type = ContentType.None;
                    string name = null;
                    double radius = -1;

                    foreach (EditorINIOption option in block.Block.Options)
                    {
                        if (option.Values.Count > 0)
                        {
                            switch (option.Name.ToLower())
                            {
                                case "nickname":
                                    name = option.Values[0].Value.ToString();
                                    break;
                                case "solar_radius":
                                    radius = ParseRadius(option.Values[0].Value.ToString());
                                    break;
                                case "type":
                                    type = ParseType(option.Values[0].Value.ToString());
                                    break;
                            }
                        }
                    }

                    if (name != null && type != ContentType.None)
                        contentTable[name] = new ArchetypeInfo() { Type = type, Radius = radius };
                }
            }
        }

        double ParseRadius(string radius)
        {
            double value;
            if (double.TryParse(radius, NumberStyles.Any, new CultureInfo("en-US", false), out value))
                return value;

            return 1;
        }

        ContentType ParseType(string type)
        {
            type = type.ToLower();

            if (type == "jump_hole")
                return ContentType.JumpHole;
            else if (type == "jump_gate")
                return ContentType.JumpGate;
            else if (type == "sun")
                return ContentType.Sun;
            else if (type == "planet")
                return ContentType.Planet;
            else if (type == "station")
                return ContentType.Station;
            else if (type == "destroyable_depot")
                return ContentType.Depot;
            else if (type == "satellite")
                return ContentType.Satellite;
            else if (type == "mission_satellite")
                return ContentType.Ship;
            else if (type == "weapons_platform")
                return ContentType.WeaponsPlatform;
            else if (type == "docking_ring")
                return ContentType.DockingRing;
            else if (type == "tradelane_ring")
                return ContentType.TradeLane;
            else if (type == "non_targetable")
                return ContentType.Construct;
            else if (type == "airlock_gate")
                return ContentType.JumpGate;

            return ContentType.None;
        }

        public ArchetypeManager(string file, int templateIndex)
        {
            if (file != null)
            {
                FileManager fileManager = new FileManager(file);
                EditorINIData iniContent = fileManager.Read(FileEncoding.Automatic, templateIndex);
                TableData solarArch = new TableData(iniContent);

                CreateContentTable(solarArch.Blocks);
            }
        }
    }

    [Serializable]
    public class ArchetypeInfo
    {
        public ContentType Type { get; set; }
        public double Radius { get; set; }

        public override string ToString()
        {
            return Type.ToString() + ", " + Radius.ToString();
        }
    }
}
