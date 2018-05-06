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

    public class NumericInputFloat : TextBox
    {
        private double? oldValue = null;
        private double? value = null;

        private double? Bound(double? value)
        {
            if (value > MaxValue())
                return MaxValue();
            else if (value < MinValue())
                return MinValue();
            else
                return value;
        }

        private string Formatting()
        {
            if (String.IsNullOrEmpty(StringFormat))
                return "{0}";
            else
                return "{0:" + StringFormat + "}";
        }

        private double MaxValue()
        {
            return Maximum ?? Double.MaxValue;
        }

        private double MinValue()
        {
            return Minimum ?? Double.MinValue;
        }

        public double? InitialValue
        {
            get { return (base.GetValue(InitialValueProperty) as double?); }
            set { base.SetValue(InitialValueProperty, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public double? Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Bound(value);
                base.Text = String.Format(Formatting(), this.value);
            }
        }

        private void SetValue()
        {
            string _text = base.Text ?? String.Empty;
            double _value;
            if (Double.TryParse(_text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out _value))
            {
                value = Bound(_value);
            }
            else
                value = null;
            if (oldValue == null && value != null)
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
            base.Text = String.Format(Formatting(), value);
        }

        private void RaiseValueChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericInputFloat.ValueChangedEvent);
            RaiseEvent(newEventArgs);
        }

        public double? Minimum
        {
            get { return (double)(base.GetValue(MinimumProperty)); }
            set { base.SetValue(MinimumProperty, value); }
        }

        public double? Maximum
        {
            get { return (double)(base.GetValue(MaximumProperty)); }
            set { base.SetValue(MaximumProperty, value); }
        }

        public string StringFormat
        {
            get { return (string)(base.GetValue(StringFormatProperty) ?? "0"); }
            set { base.SetValue(StringFormatProperty, value); }
        }

        static NumericInputFloat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericInputFloat), new FrameworkPropertyMetadata(typeof(NumericInputFloat)));
        }

        public static readonly DependencyProperty MinimumProperty =
             DependencyProperty.Register("Minimum", typeof(double), typeof(NumericInputFloat), new PropertyMetadata(Double.MinValue));

        public static readonly DependencyProperty MaximumProperty =
             DependencyProperty.Register("Maximum", typeof(double), typeof(NumericInputFloat), new PropertyMetadata(Double.MaxValue));

        public static readonly DependencyProperty InitialValueProperty =
             DependencyProperty.Register("InitialValue", typeof(double), typeof(NumericInputFloat));

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(NumericInputFloat), new PropertyMetadata(String.Empty));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericInputFloat));
    }
}
