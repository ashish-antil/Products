#region

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class PropertyExtensions
    {
        public static string GetPropertyName<T>(this T handler, Expression<Func<T, object>> propertySelector)
        {
            //var propertyInfo = ExpressionHelper.ExtractPropertyInfo(propertySelector);
            //return propertyInfo.Name;
            MemberInfo memberInfo = null;
            var body = propertySelector.Body as MemberExpression;
            if (body != null)
            {
                memberInfo = body.Member;
            }
            else
            {
                var unary = propertySelector.Body as UnaryExpression;
                if (unary != null)
                {
                    memberInfo = ((MemberExpression) unary.Operand).Member;
                }
            }

            if (memberInfo == null)
            {
                throw new NullReferenceException();
            }

            return memberInfo.Name;
        }
    }
}