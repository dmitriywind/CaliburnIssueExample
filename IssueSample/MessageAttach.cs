using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Caliburn.Micro;
using IssueSample;
using IssueSample.ViewModels;
using Action = Caliburn.Micro.Action;

namespace IssueSample
{
    /// <summary>
    /// This behavior is wrapper for Caliburn.Micro.Message.Attach, which need to fix for Win10 the  BUG:WLWPEIGHT-432,WLWPEIGHT-740 
    /// </summary>
    public class MessageAttach : Behavior<DependencyObject>
    {
        protected override void OnLoaded()
        {
            try
            {
                base.OnLoaded();
                if (TargetWithoutContext != null)
                {
                    var bindingTargetWithoutContext = new Binding
                    {
                        Source = this,
                        Path = new PropertyPath(nameof(TargetWithoutContext))
                    };
                    BindingOperations.SetBinding(AssociatedObject, Action.TargetWithoutContextProperty,
                        bindingTargetWithoutContext);
                }

                var bindingAssociatedObject = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(nameof(AttachText))
                };
                BindingOperations.SetBinding(AssociatedObject, Message.AttachProperty, bindingAssociatedObject);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{nameof(MessageAttach)}.{nameof(OnLoaded)} error: {ex}");
            }
        }

        public static readonly DependencyProperty AttachTextProperty = DependencyProperty.Register(
            nameof(AttachText), typeof(string), typeof(MessageAttach), new PropertyMetadata(default(string)));

        public string AttachText
        {
            get { return (string)GetValue(AttachTextProperty); }
            set { SetValue(AttachTextProperty, value); }
        }


        public static readonly DependencyProperty TargetWithoutContextProperty = DependencyProperty.Register(
            nameof(TargetWithoutContext), typeof(object), typeof(MessageAttach), new PropertyMetadata(default(object)));

        public object TargetWithoutContext
        {
            get { return GetValue(TargetWithoutContextProperty); }
            set { SetValue(TargetWithoutContextProperty, value); }
        }
    }


}

namespace Caliburn.Micro
{
    /// <summary>
    /// Must be used this extension instead of Caliburn.Micro.Message.Attach everywhere!!!
    /// This attached property is wrapper for Caliburn.Micro.Message.Attach by Sync.Behaviors.MessageAttach, which need to fix for Win10 the  BUG:WLWPEIGHT-1449, WLWPEIGHT-1448, WLWPEIGHT-1470 
    /// </summary>
    public static class MessageEx
    {
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach", typeof(string), typeof(MessageEx), new PropertyMetadata(default(string), OnAttachChanged));

        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var behaviours = Interaction.GetBehaviors(d);
                var ma = new MessageAttach { AttachText = e.NewValue as string };
                behaviours.Add(ma);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{nameof(MessageEx)}.Attach error: {ex}");
            }
        }

        public static void SetAttach(DependencyObject element, string value)
        {
            element.SetValue(AttachProperty, value);
        }

        public static string GetAttach(DependencyObject element)
        {
            return (string)element.GetValue(AttachProperty);
        }
    }
}