using ICSharpCode.AvalonEdit;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace SendStuffToPrinter.Behaviors
{
    public class AvalonEditPositionBehavior : Behavior<TextEditor>
    {
        public static DependencyProperty LineProperty = DependencyProperty.Register("Line", typeof(int), typeof(AvalonEditPositionBehavior),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, LinePropertyChangedCallback));

        public static DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(int), typeof(AvalonEditPositionBehavior),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ColumnPropertyChangedCallback));

        public int Line
        {
            get { return (int)base.GetValue(LineProperty); }
            set { base.SetValue(LineProperty, value); }
        }

        public int Column
        {
            get { return (int)base.GetValue(ColumnProperty); }
            set { base.SetValue(ColumnProperty, value); }
        }

        protected override void OnAttached()
        {
 	        base.OnAttached();
            base.AssociatedObject.TextArea.Caret.PositionChanged += Caret_PositionChanged;
        }

        protected override void OnDetaching()
        {
 	        base.OnDetaching();
            base.AssociatedObject.TextArea.Caret.PositionChanged -= Caret_PositionChanged;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.Line = base.AssociatedObject.TextArea.Caret.Line;
            this.Column = base.AssociatedObject.TextArea.Caret.Column;
        }

        private static void LinePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as AvalonEditPositionBehavior;
            if (behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject as TextEditor;
                if (editor.Document != null)
                {
                    editor.ScrollTo((int)e.NewValue, behavior.Column);
                }
            }
        }

        private static void ColumnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as AvalonEditPositionBehavior;
            if (behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject as TextEditor;
                if (editor.Document != null)
                {
                    editor.ScrollTo(behavior.Line, (int)e.NewValue);
                }
            }
        }
    }
}
