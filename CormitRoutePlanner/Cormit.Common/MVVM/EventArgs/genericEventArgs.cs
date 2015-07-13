namespace Imarda.Lib.MVVM.EventArgs
{
    public class GenericEventArgs<T> : System.EventArgs
    {
        public GenericEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; private set; }
    }
}