using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReinadelCisne.Behaviors
{
    public class Arrastre : Behavior<View>
    {
        PanGestureRecognizer Recognizer = new PanGestureRecognizer();
        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            bool isValid = double.TryParse(args.NewTextValue, out result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
