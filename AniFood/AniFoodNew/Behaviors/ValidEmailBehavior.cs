using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Behaviors
{
    public class ValidEmailBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += EntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= EntryTextChanged;
        }
        private void EntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            if(sender is Entry entry && !string.IsNullOrEmpty(e.NewTextValue))
            {
                if(!IsValidEmail(entry.Text))
                {
                    entry.TextColor = Colors.Red;
                }
                else
                {
                    entry.TextColor = Colors.White;
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            if (email.Contains('@') && email[email.IndexOf('@')..].Contains('.') && !email.EndsWith('.'))
            {
                return true;
            }
            return false;
        }

    }
}
