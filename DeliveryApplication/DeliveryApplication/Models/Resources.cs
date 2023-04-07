using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    class Resources

    {
        public static User currentUser;
        public static Order currentOrder;

        public static List<User> users = new List<User>
        {
             new User("admin", "admin@mail.com", "admin", "12345", UserRole.Admin),
             new User("user1", "user1@mail.com","user1", "12345", UserRole.Customer),
             new User("user1", "user2@mail.com","user2", "12345", UserRole.Customer),
        };
        public static List<Order> orders = new List<Order>
        {
             new Order(1, "user1","user1@mail.com","555-555-555", DateTime.Today, OrderRoutePoint.Karakol, OrderStatus.Accepted, OrderPayment.WhenRecieved, users[1]),
             new Order(2, "user1", "user1@mail.com","555-222-222", DateTime.Today, OrderRoutePoint.Naryn, OrderStatus.OnTheWay, OrderPayment.WhileSending,users[1]),
             new Order(3, "user2", "user2@mail.com","555-333-333", DateTime.Today, OrderRoutePoint.Osh, OrderStatus.Returned, OrderPayment.WhenRecieved,users[2])
        };
    }
}
