﻿using System.Collections.Generic;
using System.IO;
using FreelancerModStudio.Data;
using FreelancerModStudio.Data.IO;

namespace FreelancerModStudio.SystemPresenter
{
    public class Analyzer
    {
        public Dictionary<int, UniverseConnection> Connections { get; set; }

        public List<TableBlock> Universe { get; set; }
        public ArchetypeManager Archetype { get; set; }
        public string UniversePath { get; set; }
        public int SystemTemplate { get; set; }

        public void Analyze()
        {
            LoadUniverseConnections();
        }

        public void LoadUniverseConnections()
        {
            Connections = new Dictionary<int, UniverseConnection>();

            foreach (TableBlock block in Universe)
            {
                foreach (EditorINIOption option in block.Block.Options)
                {
                    if (option.Name.ToLower() == "file" && option.Values.Count > 0)
                    {
                        // GetConnections throws an exception if the file cant be read
                        try
                        {
                            Table<int, ConnectionPart> systemConnections = GetConnections(block.UniqueID, Path.Combine(UniversePath, option.Values[0].Value.ToString()));
                            if (systemConnections != null)
                            {
                                AddConnections(block.UniqueID, systemConnections);
                            }
                        }
                        catch
                        {
                        }

                        break;
                    }
                }
            }
        }

        void AddConnections(int id, Table<int, ConnectionPart> connections)
        {
            foreach (ConnectionPart connectionPart in connections)
            {
                UniverseConnection connection = new UniverseConnection
                    {
                        From = new ConnectionPart
                            {
                                Id = id,
                                Jumpgate = connectionPart.Jumpgate,
                                Jumphole = connectionPart.Jumphole
                            },
                        To = new ConnectionPart
                            {
                                Id = connectionPart.Id
                            }
                    };

                UniverseConnection existingConnection;
                if (Connections.TryGetValue(connection.ID, out existingConnection))
                {
                    existingConnection.To.Jumpgate = connection.From.Jumpgate;
                    existingConnection.To.Jumphole = connection.From.Jumphole;
                }
                else
                {
                    Connections[connection.ID] = connection;
                }
            }
        }

        Table<int, ConnectionPart> GetConnections(int id, string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            Table<int, ConnectionPart> connections = new Table<int, ConnectionPart>();

            FileManager fileManager = new FileManager(file);
            EditorINIData iniContent = fileManager.Read(FileEncoding.Automatic, SystemTemplate);

            foreach (EditorINIBlock block in iniContent.Blocks)
            {
                if (block.Name.ToLower() == "object")
                {
                    string archetypeString = null;
                    string gotoString = null;

                    foreach (EditorINIOption option in block.Options)
                    {
                        if (option.Values.Count > 0)
                        {
                            string value = option.Values[0].Value.ToString();
                            switch (option.Name.ToLower())
                            {
                                case "archetype":
                                    archetypeString = value;
                                    break;
                                case "goto":
                                    gotoString = value;
                                    break;
                            }
                        }
                    }

                    if (archetypeString != null && gotoString != null)
                    {
                        ArchetypeInfo archetypeInfo = Archetype.TypeOf(archetypeString);
                        if (archetypeInfo != null)
                        {
                            ConnectionPart connection = new ConnectionPart();
                            connection.Id = GetConnectionID(BeforeSeperator(gotoString, ","));

                            if (archetypeInfo.Type == ContentType.JumpGate)
                            {
                                connection.Jumpgate = true;
                            }
                            else if (archetypeInfo.Type == ContentType.JumpHole)
                            {
                                connection.Jumphole = true;
                            }

                            ConnectionPart existingConnection;
                            if (connections.TryGetValue(connection.Id, out existingConnection))
                            {
                                if (connection.Jumpgate)
                                {
                                    existingConnection.Jumpgate = true;
                                }

                                if (connection.Jumphole)
                                {
                                    existingConnection.Jumphole = true;
                                }
                            }
                            else if (id != connection.Id && connection.Id != -1)
                            {
                                connections.Add(connection);
                            }
                        }
                    }
                }
            }

            return connections;
        }

        static string BeforeSeperator(string value, string seperator)
        {
            int index = value.IndexOf(seperator);
            if (index != -1)
            {
                return value.Substring(0, index);
            }

            return value;
        }

        int GetConnectionID(string blockName)
        {
            blockName = blockName.ToLower();
            foreach (TableBlock block in Universe)
            {
                if (block.Name.ToLower() == blockName)
                {
                    return block.UniqueID;
                }
            }
            return -1;
        }
    }

    public class UniverseConnection
    {
        const int ID_OFFSET = 0x1000;

        public int ID
        {
            get
            {
                if (From != null && To != null)
                    return (From.Id + ID_OFFSET) * (To.Id + ID_OFFSET);

                return -1;
            }
        }

        public ConnectionPart From { get; set; }
        public ConnectionPart To { get; set; }
    }

    public class ConnectionPart : ITableRow<int>
    {
        public int Id { get; set; }
        public bool Jumpgate { get; set; }
        public bool Jumphole { get; set; }
    }
}