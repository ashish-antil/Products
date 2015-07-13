#region

using System;
using System.Linq.Expressions;
using Imarda.Lib.MVVM.Common.Interfaces;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class PropertyChangedExtensions
    {
        public static void FirePropertyChanged<T>(this T handler, Expression<Func<T, object>> propertyExpression)
            where T : INotifyPropertyChangedHandler
        {
            var propertyName = handler.GetPropertyName(propertyExpression);
            handler.FirePropertyChanged(propertyName);
        }
    }
}