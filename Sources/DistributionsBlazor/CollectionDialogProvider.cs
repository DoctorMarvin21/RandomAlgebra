namespace DistributionsBlazor
{
    public enum DialogMode
    {
        Add,
        Edit
    }

    public class CollectionDialogProvider<T> where T : new()
    {
        public CollectionDialogProvider(IList<T> arguments)
        {
            Items = arguments;
        }

        public DialogMode DialogMode { get; set; }

        public bool IsDialogOpen { get; set; }

        public bool IsDeleteDialogOpen { get; set; }
        
        public T Item { get; set; }

        public IList<T> Items { get; }

        public void AddItem()
        {
            DialogMode = DialogMode.Add;
            Item = new T();
            IsDialogOpen = true;
        }

        public void EditItem(T item)
        {
            DialogMode = DialogMode.Edit;
            Item = item;
            IsDialogOpen = true;
        }

        public void DeleteItem(T item)
        {
            Item = item;
            IsDeleteDialogOpen = true;
        }

        public void DialogOK()
        {
            if (DialogMode == DialogMode.Add)
            {
                Items.Add(Item);
            }

            IsDialogOpen = false;
        }

        public void DialogCancel()
        {
            IsDialogOpen = false;
        }

        public void CommitDelete()
        {
            Items.Remove(Item);
            IsDeleteDialogOpen = false;
        }

        public void CancelDelete()
        {
            IsDeleteDialogOpen = false;
        }
    }
}
