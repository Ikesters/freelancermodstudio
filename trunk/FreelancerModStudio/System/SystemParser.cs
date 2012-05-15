﻿using System;
using System.Windows.Media.Media3D;
using FreelancerModStudio.Data;
using FreelancerModStudio.Data.IO;
using FreelancerModStudio.SystemPresenter.Content;

namespace FreelancerModStudio.SystemPresenter
{
    public class SystemParser
    {
        public bool ModelChanged { get; set; }

        public const double SIZE_FACTOR = 0.005;

        public static void SetObjectType(TableBlock block, ArchetypeManager archetypeManager)
        {
            switch (block.Block.Name.ToLower())
            {
                case "system":
                    block.ObjectType = ContentType.System;
                    return;
                case "lightsource":
                    block.ObjectType = ContentType.LightSource;
                    return;
                case "object":
                    {
                        if (archetypeManager != null)
                        {
                            //get type of object based on archetype
                            foreach (EditorINIOption option in block.Block.Options)
                            {
                                if (option.Name.Equals("archetype", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (option.Values.Count > 0)
                                    {
                                        block.Archetype = archetypeManager.TypeOf(option.Values[0].Value.ToString());
                                        if (block.Archetype != null)
                                        {
                                            block.ObjectType = block.Archetype.Type;
                                            return;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "zone":
                    {
                        string shape = "box";
                        int flags = 0;

                        const int exlusionFlag = 0x20000;

                        foreach (EditorINIOption option in block.Block.Options)
                        {
                            if (option.Values.Count > 0)
                            {
                                string value = option.Values[0].Value.ToString();
                                switch (option.Name.ToLower())
                                {
                                    case "lane_id":
                                        // overrides exclusion zones as those are set after the loop
                                        block.ObjectType = ContentType.ZonePathTradeLane;
                                        return;
                                    case "usage":
                                        string[] values = value.Split(new[] { ',' });
                                        foreach (string valueEntry in values)
                                        {
                                            if (valueEntry.Equals("trade", StringComparison.OrdinalIgnoreCase))
                                            {
                                                block.ObjectType = ContentType.ZonePathTrade;
                                                return;
                                            }
                                        }
                                        block.ObjectType = ContentType.ZonePath;
                                        return;
                                    case "vignette_type":
                                        switch (value.ToLower())
                                        {
                                            case "open":
                                            case "field":
                                            case "exclusion":
                                                block.ObjectType = ContentType.ZoneVignette;
                                                return;
                                        }
                                        break;
                                    case "shape":
                                        shape = value;
                                        break;
                                    case "property_flags":
                                        flags = Parser.ParseInt(value, 0);
                                        break;
                                }
                            }
                        }

                        bool isExclusion = (flags & exlusionFlag) == exlusionFlag;

                        // set type based on shape and flags
                        switch (shape.ToLower())
                        {
                            case "sphere":
                                block.ObjectType = isExclusion ? ContentType.ZoneSphereExclusion : ContentType.ZoneSphere;
                                return;
                            default: // ellipsoid
                                block.ObjectType = isExclusion ? ContentType.ZoneEllipsoidExclusion : ContentType.ZoneEllipsoid;
                                return;
                            case "cylinder":
                            case "ring":
                                block.ObjectType = isExclusion ? ContentType.ZoneCylinderOrRingExclusion : ContentType.ZoneCylinderOrRing;
                                return;
                            case "box":
                                block.ObjectType = isExclusion ? ContentType.ZoneBoxExclusion : ContentType.ZoneBox;
                                return;
                        }
                    }
            }
            block.ObjectType = ContentType.None;
        }

        public void SetValues(ContentBase content, TableBlock block)
        {
            string positionString = "0,0,0";
            string rotationString = "0,0,0";
            string scaleString = "1,1,1";
            string fileString = string.Empty;

            //get properties of content
            foreach (EditorINIOption option in block.Block.Options)
            {
                if (option.Values.Count > 0)
                {
                    string value = option.Values[0].Value.ToString();
                    switch (option.Name.ToLower())
                    {
                        case "pos":
                            positionString = value;
                            break;
                        case "rotate":
                            rotationString = value;
                            break;
                        case "size":
                            scaleString = value;
                            break;
                        case "file":
                            fileString = value;
                            break;
                    }
                }
            }

            Vector3D position = ParsePosition(positionString);
            Vector3D rotation;
            Vector3D scale;

            //set content values
            switch (block.ObjectType)
            {
                case ContentType.System:
                    position = ParseUniverseVector(positionString);
                    scale = new Vector3D(8, 8, 8);
                    rotation = ParseRotation(rotationString, false);

                    Content.System system = (Content.System)content;
                    system.Path = fileString;
                    break;
                case ContentType.LightSource:
                    scale = new Vector3D(1, 1, 1);
                    rotation = ParseRotation(rotationString, false);
                    break;
                case ContentType.Construct:
                case ContentType.Depot:
                case ContentType.DockingRing:
                case ContentType.JumpGate:
                case ContentType.JumpHole:
                case ContentType.Planet:
                case ContentType.Satellite:
                case ContentType.Ship:
                case ContentType.Station:
                case ContentType.Sun:
                case ContentType.TradeLane:
                case ContentType.WeaponsPlatform:
                    scale = new Vector3D(1, 1, 1);
                    rotation = ParseRotation(rotationString, false);

                    if (block.Archetype != null)
                    {
                        if (block.Archetype.Radius != 0d)
                        {
                            scale = new Vector3D(block.Archetype.Radius, block.Archetype.Radius, block.Archetype.Radius)*SIZE_FACTOR;
                        }
                    }
                    break;
                default: // all zones
                    rotation = ParseRotation(rotationString, block.ObjectType == ContentType.ZonePath || block.ObjectType == ContentType.ZonePathTrade);
                    scale = ParseScale(scaleString, block.ObjectType);
                    break;
            }

            // update the model if the object type was changed
            if (content.Block == null || content.Block.ObjectType != block.ObjectType)
            {
                ModelChanged = true;
            }

            // set reference to block (this one is different than the one passed in the argument because a new copy was create in the undomanager)
            content.Block = block;

            content.SetTransform(position, rotation, scale);
        }

        public static Vector3D ParseScale(string scale, ContentType type)
        {
            string[] values = scale.Split(new[] { ',' });

            switch (type)
            {
                case ContentType.ZoneSphere:
                case ContentType.ZoneSphereExclusion:
                case ContentType.ZoneVignette:
                    if (values.Length > 0)
                    {
                        double tempScale = Parser.ParseDouble(values[0], 1);
                        return new Vector3D(tempScale, tempScale, tempScale)*SIZE_FACTOR;
                    }
                    break;
                case ContentType.ZoneCylinderOrRing:
                case ContentType.ZoneCylinderOrRingExclusion:
                case ContentType.ZonePath:
                case ContentType.ZonePathTrade:
                    if (values.Length > 1)
                    {
                        double tempScale1 = Parser.ParseDouble(values[0], 1);
                        double tempScale2 = Parser.ParseDouble(values[1], 1);
                        return new Vector3D(tempScale1, tempScale2, tempScale1)*SIZE_FACTOR;
                    }
                    break;
            }

            if (values.Length > 2)
            {
                return new Vector3D(Parser.ParseDouble(values[0], 1), Parser.ParseDouble(values[2], 1), Parser.ParseDouble(values[1], 1))*SIZE_FACTOR;
            }

            return new Vector3D(1, 1, 1);
        }

        public static Vector3D ParsePosition(string vector)
        {
            Vector3D tempVector = Parser.ParseVector(vector);
            return new Vector3D(tempVector.X, -tempVector.Z, tempVector.Y)*SIZE_FACTOR;
        }

        public static Vector3D ParseRotation(string vector, bool pathRotation)
        {
            Vector3D tempRotation = Parser.ParseVector(vector);

            if (pathRotation)
            {
                tempRotation.X += 90;
                tempRotation.Z *= 2;
            }

            return new Vector3D(tempRotation.X, -tempRotation.Z, tempRotation.Y);
        }

        public Vector3D ParseUniverseVector(string vector)
        {
            const double axisCenter = 7.5;
            const double positionScale = 1/SIZE_FACTOR/4;

            //Use Point.Parse after implementation of type handling
            string[] values = vector.Split(new[] { ',' });
            if (values.Length > 1)
            {
                double tempScale1 = Parser.ParseDouble(values[0], 0);
                double tempScale2 = Parser.ParseDouble(values[1], 0);
                return new Vector3D(tempScale1 - axisCenter, -tempScale2 + axisCenter, 0)*positionScale;
            }
            return new Vector3D(0, 0, 0);
        }

        public static ContentType ParseContentType(string type)
        {
            switch (type.ToLower())
            {
                case "jump_hole":
                    return ContentType.JumpHole;
                case "jump_gate":
                case "airlock_gate":
                    return ContentType.JumpGate;
                case "sun":
                    return ContentType.Sun;
                case "planet":
                    return ContentType.Planet;
                case "station":
                    return ContentType.Station;
                case "destroyable_depot":
                    return ContentType.Depot;
                case "satellite":
                    return ContentType.Satellite;
                case "mission_satellite":
                    return ContentType.Ship;
                case "weapons_platform":
                    return ContentType.WeaponsPlatform;
                case "docking_ring":
                    return ContentType.DockingRing;
                case "tradelane_ring":
                    return ContentType.TradeLane;
                case "non_targetable":
                    return ContentType.Construct;
            }

            return ContentType.None;
        }
    }
}