using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;

namespace ResultsAnalysis
{
    class ResultsCollection : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        public Results this[int index]
        {
            get
            {
                return (Results)List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(Results r)
        {
            return List.Add(r);
        }

        public void AddFromResultsFiles(FileInfo[] files)
        {
            foreach (FileInfo fi in files)
            {
                List.Add(new Results(fi));
            }
        }

        public void Remove (Results value)
        {
            List.Remove(value);
        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }

        #region CollectionBase Overrides
        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }
        #endregion

        #region IBindingList Interface
        public void AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        public object AddNew()
        {
            throw new NotSupportedException();
        }

        public bool AllowEdit
        {
            get { return true; }
        }

        public bool AllowNew
        {
            get { return true; }
        }

        public bool AllowRemove
        {
            get { return true; }
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        public bool IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        public void RemoveSort()
        {
            throw new NotSupportedException();
        }

        public ListSortDirection SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        public PropertyDescriptor SortProperty
        {
            get { throw new NotSupportedException(); }
        }

        public bool SupportsChangeNotification
        {
            get { return true; }
        }

        public bool SupportsSearching
        {
            get { return false; }
        }

        public bool SupportsSorting
        {
            get { return false; }
        } 
        #endregion
    }
}
