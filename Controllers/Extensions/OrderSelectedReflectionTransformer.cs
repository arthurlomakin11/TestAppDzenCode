using System.Reflection;

namespace TestAppDzenCode.Controllers.Extensions;

using OrderSelected = CommentsController.OrderSelected;

public enum OrderType {
    Asc, 
    Desc
}

public static class OrderSelectedReflectionTransformer
{
    public class OrderSelectedReflection
    {
        public string propertyName { get; set; }
        public OrderType orderType { get; set; }
    }

    public static OrderSelectedReflection getOrderSelected(OrderSelected order)
    {
        string selectedOrderPropName = "";
        OrderType selectedOrderType = OrderType.Asc;
        
        var properties = order.GetType().GetProperties();
        
        foreach(PropertyInfo property in properties)
        {
            var propertyValue = property.GetValue(order);
            switch (propertyValue)
            {
                case -1:
                    continue;
                case 0:
                    selectedOrderPropName = property.Name;
                    selectedOrderType = OrderType.Asc;
                    break;
                case 1:
                    selectedOrderPropName = property.Name;
                    selectedOrderType = OrderType.Desc;
                    break;
            }
        }

        return new OrderSelectedReflection
        {
            propertyName = selectedOrderPropName,
            orderType = selectedOrderType
        };
    }
}