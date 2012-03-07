﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace FreelancerModStudio.Data.IO
{
    //format: http://nullprogram.com/projects/bini/binitools.html#The-BINI-Format
    public class BINIManager
    {
        public string File { get; set; }
        public List<INIBlock> Data { get; set; }

        public BINIManager(string file)
        {
            File = file;
        }

        public bool Read()
        {
            Data = new List<INIBlock>();
            BinaryReader binaryReader = null;
            try
            {
                FileStream stream = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read);
                binaryReader = new BinaryReader(stream, Encoding.Default);

                //read header
                if (stream.Length < 4 + 4 ||
                    Encoding.Default.GetString(binaryReader.ReadBytes(4)) != "BINI" ||
                    binaryReader.ReadInt32() != 1)
                {
                    // return false if it is not a bini file
                    binaryReader.Close();
                    return false;
                }

                int stringTablePosition = binaryReader.ReadInt32();
                long dataPosition = stream.Position;

                //goto string table
                stream.Position = stringTablePosition;

                //read string table
                StringTable stringTable = new StringTable(Encoding.Default.GetString(binaryReader.ReadBytes((int)(stream.Length - stream.Position))));

                //go back to data
                stream.Position = dataPosition;

                //read data
                while (stream.Position < stringTablePosition && stream.Position < stream.Length)
                {
                    //read section
                    int sectionStringPosition = binaryReader.ReadInt16();
                    int sectionEntriesCount = binaryReader.ReadInt16();

                    string sectionName = stringTable.GetString(sectionStringPosition);
                    INIOptions block = new INIOptions();
                    //read each entry
                    for (int i = 0; i < sectionEntriesCount; i++)
                    {
                        //read entry
                        int entryStringPosition = binaryReader.ReadInt16();
                        int entryValuesCount = binaryReader.ReadByte();
                        string entryName = stringTable.GetString(entryStringPosition);

                        //read each value
                        List<string> options = new List<string>();
                        for (int j = 0; j < entryValuesCount; j++)
                        {
                            //read value
                            int valueType = binaryReader.ReadByte();

                            string entryValue = null;
                            if (valueType == 1)
                                entryValue = binaryReader.ReadInt32().ToString("D", System.Globalization.CultureInfo.InvariantCulture);
                            else if (valueType == 2)
                                entryValue = binaryReader.ReadSingle().ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                            else //string
                            {
                                int valueStringPosition = binaryReader.ReadInt32();
                                entryValue = stringTable.GetString(valueStringPosition);
                            }
                            options.Add(entryValue);
                        }
                        block.Add(entryName, new INIOption(string.Join(", ", options.ToArray()), i));
                    }
                    Data.Add(new INIBlock { Name = sectionName, Options = block } );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (binaryReader != null)
                binaryReader.Close();

            return true;
        }
    }

    public class StringTable
    {
        SortedList<int, string> Strings = new SortedList<int, string>();

        public StringTable(string content)
        {
            int position = 0;
            foreach (string stringValue in content.Trim('\0').Split('\0'))
            {
                Strings.Add(position, stringValue);
                position += stringValue.Length + 1;
            }
        }

        public string GetString(int position)
        {
            return Strings[position];
        }
    }
}
