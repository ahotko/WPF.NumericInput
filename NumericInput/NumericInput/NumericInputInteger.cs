using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls.NumericInput
{

    public class NumericInputInteger : TextBox
    {
        private int? oldValue = null;
        private int? value = null;

        private int? Bound(int? value)
        {
            if (value > MaxValue())
                return MaxValue();
            else if (value < MinValue())
                return MinValue();
            else 
                return value;
        }

        private int MaxValue()
        {
            return Maximum ?? Int32.MaxValue;
        }

        private int MinValue()
        {
            return Minimum ?? Int32.MinValue;
        }

        public int? InitialValue
        {
            get { return (base.GetValue(InitialValueProperty) as int?); }
            set { base.SetValue(InitialValueProperty, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public int? Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                base.Text = Convert.ToString(value);
            }
        }

        private void SetValue()
        {
            string _text = base.Text ?? String.Empty;
            int _value;
            if (Int32.TryParse(_text, out _value))
            {
                value = Bound(_value);
            }
            else
                value = null;
            if(oldValue == null && value != null)
                RaiseValueChangedEvent();
            else if (oldValue != null && value == null)
                RaiseValueChangedEvent();
            else if (oldValue != value)
                RaiseValueChangedEvent();
            oldValue = value;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            SetValue();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            base.Text = Convert.ToString(value);
        }

        private void RaiseValueChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericInputInteger.ValueChangedEvent);
            RaiseEvent(newEventArgs);
        }

        public int? Minimum
        {
            get { return (int)(base.GetValue(MinimumProperty)); }
            set { base.SetValue(MinimumProperty, value); }
        }

        public int? Maximum
        {
            get { return (int)(base.GetValue(MaximumProperty)); }
            set { base.SetValue(MaximumProperty, value); }
        }

        static NumericInputInteger()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericInputInteger), new FrameworkPropertyMetadata(typeof(NumericInputInteger)));
        }

        public static readonly DependencyProperty MinimumProperty =
             DependencyProperty.Register("Minimum", typeof(int), typeof(NumericInputInteger), new PropertyMetadata(Int32.MinValue));

        public static readonly DependencyProperty MaximumProperty =
             DependencyProperty.Register("Maximum", typeof(int), typeof(NumericInputInteger), new PropertyMetadata(Int32.MaxValue));

        public static readonly DependencyProperty InitialValueProperty =
             DependencyProperty.Register("InitialValue", typeof(int), typeof(NumericInputInteger));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericInputInteger));
    }
}
