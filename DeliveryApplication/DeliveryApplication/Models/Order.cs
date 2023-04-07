using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    public class Order

    {
        public Order(OrderRoutePoint deliveryPoint, OrderPayment payment )
        {
            OrderID = RandomId();
            UserFullName = Resources.currentUser.FullName;
            Email = Resources.currentUser.Email;
            DateOfCreate= DateTime.Now;
            DeliveryPoint = deliveryPoint;
            Status = OrderStatus.Accepted;
            Payment = payment;
            Owner = Resources.currentUser;
        }
        public Order(int orderId, string userFullName, OrderRoutePoint deliveryPoint, OrderStatus status)
        {
            OrderID = orderId;
            UserFullName = userFullName;
            DeliveryPoint = deliveryPoint;
            Status = status;
            Owner = Resources.currentUser;
        }
        public Order (int orderID, string userFullName, string email, string telNumber, DateTime date, OrderRoutePoint deliveryPoint, OrderStatus status, OrderPayment payment, User user)
        {
            OrderID = orderID;
            UserFullName = userFullName;
            Email = email;
            TelNumber = telNumber;
            Status = status;
            DateOfCreate = date;
            Status = status;
            DeliveryPoint = deliveryPoint;
            Payment = payment;
            Owner = user;
        }
        public int OrderID { get; set; }
        public string Email { get; set; }
        public string TelNumber { get; set; }
        public string UserFullName { get; set; }
        public DateTime DateOfCreate { get; set; }
        public OrderRoutePoint DeliveryPoint { get; set; }
        public OrderStatus Status { get; set; }
        public OrderPayment Payment { get; set; }
        public User Owner { get; set; }

        public void PrintInfo()
        {
            Console.WriteLine("\nНомер заявки: "+OrderID);
            Console.WriteLine("ФИО клиента: "+UserFullName);
            Console.WriteLine($"Маршрут: Bishkek --> {DeliveryPoint}" );
            Console.WriteLine($"Статус заявки: {Status}\n");
            Console.WriteLine("----------------");
        }
        public void PrintFullInfo()
        {
            Console.WriteLine("\nНомер заявки: " + OrderID);
            Console.WriteLine("\nКонтактные данные Клиента\n");
            Console.WriteLine("ФИО клиента: " + UserFullName);
            Console.WriteLine($"Email: {Email}, Телефон: {TelNumber}");
            Console.WriteLine("\nДанные о заявке\n");
            Console.WriteLine($"Дата создания заявки: {DateOfCreate}");
            Console.WriteLine($"Маршрут: Bishkek --> {DeliveryPoint}");
            Console.WriteLine($"Статус заявки: {Status}");
            Console.WriteLine($"Вид оплаты: {Payment}");
            Console.WriteLine($"К оплате: {Sum(DeliveryPoint)} сомов\n");
            Console.WriteLine("----------------");
        }
        public decimal Sum(OrderRoutePoint deliveryPoint)
        {
            decimal sum;
            if (deliveryPoint == OrderRoutePoint.Osh) sum = 999;
            else if (deliveryPoint == OrderRoutePoint.Karakol) sum = 799;
            else if (deliveryPoint == OrderRoutePoint.Naryn) sum = 699;
            else sum = 599;
            return sum;

        }
        public int RandomId()
        {
            int maxValue = 0;
            maxValue = Resources.orders.Max(x=>x.OrderID);
            maxValue += 1;
            return maxValue;
        }
    }

}
namespace DeliveryApplication.Models
{
    public enum OrderRoutePoint
    {
        Karakol =1,
        Naryn =2,
        Osh =3,
        Talas =4
    }
}

namespace DeliveryApplication.Models
{
    public enum OrderPayment
    {
        WhileSending= 1,
        WhenRecieved = 2
    }
}

namespace DeliveryApplication.Models
{
    public enum OrderStatus
    {
        Accepted= 1,
        PrepForShipping = 2,
        OnTheWay = 3,
        Delivered =4,
        Returned =5
    }
}
