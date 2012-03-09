﻿using System.Collections.Generic;

namespace FreelancerModStudio.Data
{
    public class UndoManager<T>
    {
        public delegate void DataChangedType(List<T> o, bool undo);
        public DataChangedType DataChanged;

        List<List<T>> changes = new List<List<T>>();
        int current = 0;
        int savedIndex = 0;

        void OnDataChanged(List<T> o, bool undo)
        {
            if (DataChanged != null)
                DataChanged(o, undo);
        }

        public List<T> CurrentData
        {
            get
            {
                if (current > changes.Count)
                    return null;

                return changes[current - 1];
            }
            set
            {
                changes[current] = value;
            }
        }

        public void Undo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (current > 0)
                {
                    current--;
                    List<T> data = changes[current];

                    OnDataChanged(data, true);
                }
            }
        }

        public void Redo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (current < changes.Count)
                {
                    List<T> data = changes[current];
                    current++;

                    OnDataChanged(data, false);
                }
            }
        }

        public void Execute(T o)
        {
            List<T> newData = new List<T> { o };

            //remove every data which comes after the current
            changes.RemoveRange(current, changes.Count - current);

            //add the data
            changes.Add(newData);
            current++;

            //raise event that the data was changed
            OnDataChanged(newData, false);
        }

        public bool CanUndo()
        {
            return current > 0;
        }

        public bool CanRedo()
        {
            return changes.Count - current > 0;
        }

        public void SetAsSaved()
        {
            savedIndex = current;
        }

        public bool IsModified()
        {
            return savedIndex != current;
        }
    }
}