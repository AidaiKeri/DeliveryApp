using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryApplication
{
    class Program
    {
        static void Main()
        {
            Menu(mainMenu);
        }

        public static void Menu(List<Option> options)
        {
            int index = 0;
            WriteMenu(options, options[index]);

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();
        }

        public static List<Option> mainMenu = new List<Option>
            {
                new Option("Войти", () => Login()),
                new Option("Зарегистрироваться", () => Registration()),
                new Option("Выйти из приложения", () => Environment.Exit(0)),
            };

        public static List<Option> adminMenu = new List<Option>
            {
                new Option("Список заявок\t", () => OrdersList()),
                new Option("Получить отчет по номеру заявки\t", () => CheckOrder()),
                new Option("Изменить статус заявки\t", () => ChangeOrderStatus()),
                new Option("Добавить нового Админа", () => Registration(UserRole.Admin)),
                new Option("Выйти из Личного Кабинета", () => Exit()),
            };

        public static List<Option> clientMenu = new List<Option>
            {
                new Option("Оформить заявку\t", () => NewOrder() ),
                new Option("Получить отчет по номеру заявки\t", () => CheckOrder(UserRole.Customer)),
                new Option("Список моих заявок\t", () => PrintOrderList()),
                new Option("Выйти из Личного Кабинета", () => Exit()),
            };
        
        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            if (options == adminMenu)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Личный Кабинет Админа\n\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Добро пожаловать, Уважаемый {Resources.currentUser.FullName}\n\n");
                Console.ResetColor();
                Console.WriteLine("Выберите действие\n\n");
            }
            else if (options == clientMenu)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Личный Кабинет Клиента\n\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Добро пожаловать, Уважаемый {Resources.currentUser.FullName}\n\n");
                Console.ResetColor();
                Console.WriteLine("Выберите действие\n\n");
            }
          
            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }
                Console.WriteLine(option.Name);
            }
        }

        public static void Login()
        {
            Console.Clear();
            Console.Write("Введите Логин:");
            string login = Console.ReadLine();
            Console.Write("Введите пароль:");
            string password = Console.ReadLine();

            Resources.currentUser = Resources.users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if(Resources.currentUser == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вы ввели неверный Логин или Пароль! Попробуйте еще раз\n");
                Console.ResetColor();
                ConsoleKeyInfo key = Console.ReadKey();
                Main();
            }

            if(Resources.currentUser.Role == UserRole.Admin)
            {
                Menu(adminMenu);
            }
            else
            {
                Menu(clientMenu);
            }
        }

        public static void Registration(UserRole role = UserRole.Customer)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Регистрация Нового Клиента\n");
            Console.ResetColor();
            username:
            Console.Write("Введите Имя и Фамилию:");
            string newFullName= Console.ReadLine();
            email:
            Console.Write("Укажите свой Email:");
            string newEmail = Console.ReadLine();
            if (Resources.users.Any(x => x.Email == newEmail))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пользователь с таким Email уже существует! Попробуйте еще раз");
                Console.ResetColor();
                goto email;
            }
            else if (newFullName =="" || newEmail == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Заполните все данные! Попробуйте еще раз");
                Console.ResetColor();
                goto username;   
            }
            else
            {
                login:
                Console.Write("Введите новое имя пользователя(логин):");
                string newLogin = Console.ReadLine();
                if ( Resources.users.Any(x => x.Login == newLogin))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Такое имя пользователя уже существует! Попробуйте еще раз");
                    Console.ResetColor();
                    goto login;
                }
                else if ( newLogin == "" )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Заполните все данные! Попробуйте еще раз");
                    Console.ResetColor();
                    goto login;
                }
                else
                {
                    parol:
                    Console.Write("Придумайте пароль больше 4х символов:");
                    string newPassword = Console.ReadLine();
                    if (newPassword.Length > 3)
                    {
                        Resources.users.Add(new User(newFullName, newEmail, newLogin, newPassword, role));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Клиент добавлен!\n");
                        Console.ResetColor();
                        Console.WriteLine("Нажмите любую клавишу чтобы продолжить");
                        Console.ReadKey();
                        Menu(mainMenu);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Пароль меньше 3 символов! Попробуйте еще раз");
                        Console.ResetColor();
                        goto parol;
                    }
                }
            }
        }

        public static void CheckOrder(UserRole role = UserRole.Admin)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nОтчет по номеру заявки...\n");
            Console.ResetColor();
        again:
            Console.Write("Введите номер заявки: ");
            if (int.TryParse(Console.ReadLine(),out var orderNumber))
            {
                if (role == UserRole.Customer)
                {
                    Resources.currentOrder = Resources.orders.FirstOrDefault(x => x.OrderID == orderNumber && x.Owner.Login == Resources.currentUser.Login);
                }
                else
                {
                     Resources.currentOrder  = Resources.orders.FirstOrDefault(x => x.OrderID == orderNumber);
                }
                if (Resources.currentOrder == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Такой заявки нет!");
                    Console.ResetColor();
                    goto again;
                }
                else
                {
                    Console.Clear();
                    Resources.currentOrder.PrintFullInfo();
                }
                Console.WriteLine("\nНажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
                if (role == UserRole.Customer) Menu(clientMenu);
                else Menu(adminMenu);
            }
           else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный ввод!");
                Console.ResetColor();
                goto again;
            }
        }
        
        public static void ChangeOrderStatus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nИзменить статус заявки...\n");
            Console.ResetColor();
            p:
            Console.Write("Введите номер заявки: ");
           if (int.TryParse(Console.ReadLine(), out var orderNumber))
            {
                var searchedOrder = Resources.orders.FirstOrDefault(x => x.OrderID == orderNumber);
                if (searchedOrder == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Такой заявки нет!");
                    Console.ResetColor();
                    goto p;
                }
                else if (searchedOrder.Status == OrderStatus.Returned)
                {
                    Console.WriteLine("\nДанный товар был возвращен! Дальнейшие операции недопустимы");
                    Console.WriteLine("\nНажмите на любую клавишу чтобы перейти в личный кабинет");
                    Console.ReadKey();
                    Menu(adminMenu);
                }
                else
                {
                    Console.WriteLine($"\nТекущий статус заявки: {searchedOrder.Status}");
                    Console.WriteLine("\nДоступные статусы:");
                    var b = searchedOrder.Status;
                    OrderStatus[] orderStatuses = { OrderStatus.Accepted, OrderStatus.PrepForShipping, OrderStatus.OnTheWay, OrderStatus.Delivered, OrderStatus.Returned };
                    List<OrderStatus> newOrderStatusList = new List<OrderStatus>();
                    for (int i = 0; i <= 4; i++)
                    {
                        if (orderStatuses[i] > b)
                        {
                            newOrderStatusList.Add(orderStatuses[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    for (int i = 0; i < newOrderStatusList.Count; i++)
                    {
                        Console.WriteLine($"  {i + 1}. {newOrderStatusList[i]}");
                    }
                    c:
                    Console.Write("\nВведите номер нового статуса: ");
                    if (int.TryParse(Console.ReadLine(), out var newStatus) && newStatus <= newOrderStatusList.Count && newStatus >=1)
                    {
                        searchedOrder.Status = newOrderStatusList[newStatus - 1];
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Статус заявки изменен на: {searchedOrder.Status}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некорректный ввод!");
                        Console.ResetColor();
                        goto c;
                    }
                }
                Console.WriteLine("\nНажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
                Menu(adminMenu);
            }
           else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный ввод!");
                Console.ResetColor();
                goto p;
            }
        }

        public static void NewOrder()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Новая заявка...\n");
            Console.ResetColor();
            Console.WriteLine("Город отправки: Bishkek");
            OrderRoutePoint newOrderCity;
            OrderPayment payment;
            Console.WriteLine("\nГорода доставки:");
            OrderRoutePoint[] orderCities = { OrderRoutePoint.Karakol, OrderRoutePoint.Naryn, OrderRoutePoint.Osh, OrderRoutePoint.Talas};
            OrderPayment[] orderPayments = { OrderPayment.WhenRecieved, OrderPayment.WhileSending };
            for (int i = 0; i <= 3; i++)
            {
                Console.WriteLine($"  {i + 1}. {orderCities[i]}");

            }
        c:
            Console.Write("\nВведите номер города: ");
            if (int.TryParse(Console.ReadLine(), out var a) && a <= 4 && a >= 1)
            {
                newOrderCity= orderCities[a - 1];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный ввод!");
                Console.ResetColor();
                goto c;
            }
            Console.WriteLine($"\nCпособы оплаты:");
            Console.WriteLine($"  1.{OrderPayment.WhenRecieved}");
            Console.WriteLine($"  2.{OrderPayment.WhileSending}");
            d:
            Console.Write("\nВыберите способ оплаты: ");
            if (int.TryParse(Console.ReadLine(), out var b) && (b== 1 || b == 2))
            {
                payment = orderPayments[b - 1];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некорректный ввод!");
                Console.ResetColor();
                goto d;
            }
            Resources.currentOrder = new Order(newOrderCity, payment);
            Resources.orders.Add(Resources.currentOrder);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nЗаявка добавлена!\n");
            Console.ResetColor();
            Resources.currentOrder.PrintFullInfo();
            Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
            Console.ReadKey();
            Menu(clientMenu);
        }

        public static void OrdersList()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nСписок Заявок ...\n");
            Console.ResetColor();
            
            foreach (Order o in Resources.orders)
            {
                    o.PrintInfo();
            }
            Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
            Console.ReadKey();
            Menu(adminMenu);
        }
        
        public static void Exit()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                Program.Main();
            }
        }
        public static void PrintOrderList()
        {
            Console.Clear();
            foreach (var x in Resources.orders)
            {
                if (x.Owner.Login == Resources.currentUser.Login)
                {
                    x.PrintInfo();
                }
            }
            Console.WriteLine("\nНажмите на любую клавишу чтобы вернуться назад");
            Console.ReadKey();
            Menu(clientMenu);
        }



    }

}