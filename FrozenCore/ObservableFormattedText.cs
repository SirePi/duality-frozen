using Duality;

namespace FrozenCore
{
    public class ObservableFormattedText : FormattedText
    {
        private bool _textChanged;

        public new string SourceText
        {
            get { return base.SourceText; }
            set
            {
                base.SourceText = value;
                _textChanged = true;
            }
        }
        public bool TextChanged
        {
            get { return _textChanged; }
            internal set { _textChanged = value; }
        }
    }
}