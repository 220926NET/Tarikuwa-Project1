using System;
using System.Threading;
using Expense.Models;
using Expense.Roles;

namespace Expense.UI
{
    public static class Screen
    {
        private static readonly Expenses expenses = new Expenses();

        private static User _currentUser;

        private static void DotAnimation(int timer = 20)
        {
            for (var i = 0; i < timer; i++)
            {
                Console.Write('.');
                Thread.Sleep(200);
            }

            Console.Clear();
        }

        private static void LogOutProgress()
        {
            _currentUser = null;
            Console.WriteLine("Thank you. Bye BYe");
            DotAnimation();
            Console.Clear();
        }

        internal static void Welcome_Page()
        {
            // let clear our screen
            Console.Clear();

            // Change the console title to our project name
            Console.Title = "Expense Reimbursement";

            Console.WriteLine("\n\n--------------Welcome--------------\n");
            Console.WriteLine("Please Register or Login to continue");
            Console.WriteLine("1. Login: ");
            Console.WriteLine("2. Register: ");
            ProcessLoginRegisterOptions();
            Validators.PressEnterToContinue();
        }

        private static void DisplayEnum<T>()
        {
            var i = 0;
            Console.WriteLine("----------------------------------------");
            foreach (var w in Enum.GetNames(typeof(T)))
            {
                i += 1;
                Console.WriteLine($"{i}. {w}: ");
            }
        }

        private static void ProcessLoginRegisterOptions()
        {
            var check = true;
            try
            {
                switch (Validators.Convert<int>("an option"))
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        Register();
                        break;
                    default:
                        Validators.DisplayMessage(false, "Invalid selection!");
                        check = false;
                        break;
                }
            }
            catch (ArgumentException e)
            {
                Validators.DisplayMessage(false, $"The app failed due to {e.Message}.");
                check = false;
            }

            if (!check)
            {
                LogOutProgress();
                Welcome_Page();
            }
        }

        private static T ValidateInput<T>(string prompt)
        {
            bool check;
            T value = default;
            do
            {
                check = true;
                try
                {
                    value = Validators.Convert<T>(prompt);
                }
                catch (ArgumentException e)
                {
                    Validators.DisplayMessage(false, $"The app failed due to {e.Message}.");
                    check = false;
                }
            } while (!check);

            return value;
        }

        private static bool CheckRoleSelection()
        {
            var check = true;
            try
            {
                switch (Validators.Convert<int>("an option"))
                {
                    case 1:
                        _currentUser.Role = Role.Employee.ToString();
                        break;
                    case 2:
                        _currentUser.Role = Role.Manager.ToString();
                        break;
                    default:
                        Validators.DisplayMessage(false, "Invalid selection!");
                        check = false;
                        break;
                }
            }
            catch (ArgumentException e)
            {
                Validators.DisplayMessage(false, $"The app failed due to {e.Message}.");
                check = false;
            }

            return check;
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("\n------------Login Page-------------\n");
            CheckUserExist();
        }

        private static void CheckUserExist()
        {
            string email;
            var check = true;
            do
            {
                email = Validators.GetInput("enter your email");
                Console.WriteLine();
            } while (!Validators.ValidEmail(email));

            try
            {
                var password = ValidatePassword();
                _currentUser = expenses.Login(new User(email, password));
                // Console.WriteLine(currentUser.FullName);
            }
            catch (ArgumentException e)
            {
                Validators.DisplayMessage(false, e.Message);
                check = false;
            }

            if (!check)
            {
                LogOutProgress();
                Welcome_Page();
            }
            else
            {
                WelcomeBack();
            }
        }

        private static void WelcomeBack()
        {
            Console.Clear();
            Validators.DisplayMessage(true, $"Welcome {_currentUser.FullName}");
            Console.WriteLine("----------------------------------------");
            ProcessEmployee();
        }

        private static void GetUserTicket()
        {
            Console.Clear();
            Console.WriteLine("\n--------Ticket Submission----------\n");
            var name = ValidateInput<string>("enter the name of ticket");
            var price = ValidateInput<double>("enter the price of the ticket");
            var managerid = DisplayAllManagersForTickets();
            var tickets = new Tickets(name, price, _currentUser.Id, managerid);
            expenses.SubmitRequest(tickets);
        }

        private static void ProcessTickets()
        {
            Console.Clear();
            //manager role
            if (_currentUser.Role != Role.Manager.ToString())
            {
                Validators.DisplayMessage(false, "Sorry you can't processs any tickets due to your role.");
                return;
            }

            var tickets = expenses.ViewTicketsToProcess(_currentUser.Id);
            var i = 0;
            var id = 0;
            bool check;
            tickets.ForEach(p =>
            {
                i += 1;
                Console.WriteLine($"Ticket no: {i}");
                Console.WriteLine(p + "\n");
            });
            if (tickets.Count == 0)
            {
                Console.WriteLine("No available tickets to select.");
                return;
            }

            do
            {
                check = true;

                var selection = ValidateInput<int>("please select the ticket your which to update his status: ");
                if (selection < 1 || selection > i)
                {
                    check = false;
                    Validators.DisplayMessage(false, "Incorrect choice from list of tickets");
                }
                else
                {
                    id = tickets[selection - 1].Id;
                }
            } while (!check);

            do
            {
                check = true;
                Console.WriteLine("Please select the status you whish to update for the ticket");
                DisplayEnum<Status>();
                var answer = ValidateInput<int>("enter your choice");
                if (answer == 1)
                {
                    expenses.ChangeTicketStatus(_currentUser, id, Status.Pending);
                }
                else if (answer == 2)
                {
                    expenses.ChangeTicketStatus(_currentUser, id, Status.Completed);
                }
                else if (answer == 3)
                {
                    expenses.ChangeTicketStatus(_currentUser, id, Status.Deny);
                }
                else
                {
                    check = false;
                    Validators.DisplayMessage(false, "Wrong selection from the choices");
                }
            } while (!check);

            Validators.DisplayMessage(true, "Thanks for the update. See you soon.");
        }

        private static void GetPriorTickets()
        {
            //viewing the previously submitted tickets
            Console.Clear();
            Console.WriteLine("\n----------Prior Tickets------------\n");

            var tickets = expenses.ViewPriorTickets(_currentUser.Id);
            for (var i = 0; i < tickets.Count; i++)
            {
                Console.WriteLine($"Ticket no: {i + 1}");
                Console.WriteLine(tickets[i]);
                Console.WriteLine();
            }
        }

        private static void ModifyAccountUser()
        {
            //User updating/changing info
            Console.Clear();
            Console.WriteLine("\n----------Modify Account-----------\n");

            Console.WriteLine("Your current account info");
            Console.WriteLine(_currentUser + "\n");

            var check = true;
            do
            {
                Console.WriteLine("Do you wish to change your account info:");
                Console.WriteLine("1: Yes");
                Console.WriteLine("2: No");
                var answer = ValidateInput<int>("enter your choice");
                if (answer == 1)
                {
                    ChangeUserInfo();
                }
                else if (answer == 2)
                {
                    Console.WriteLine("Alright see you soon");
                    break;
                }
                else
                {
                    check = false;
                    Validators.DisplayMessage(false, "Wrong selection from the choices");
                }
            } while (!check);
        }

        private static void ProcessEmployee()
        {
            //manager role

            Console.WriteLine("Choose from the following below");
            Console.WriteLine("1: Submit expense tickets");
            Console.WriteLine("2: View prior ticket submissions");
            Console.WriteLine("3: Modify your account");
            Console.WriteLine("4: Process Employee's tickets");
            Console.WriteLine("5: Exit the app");
            var check = true;
            try
            {
                switch (Validators.Convert<int>("an option"))
                {
                    case 1:
                        GetUserTicket();
                        break;
                    case 2:
                        GetPriorTickets();
                        break;
                    case 3:
                        ModifyAccountUser();
                        break;
                    case 4:
                        ProcessTickets();
                        break;
                    case 5:
                        check = false;
                        break;
                    default:
                        Validators.DisplayMessage(false, "Invalid selection!");
                        check = false;
                        break;
                }
            }
            catch (ArgumentException e)
            {
                Validators.DisplayMessage(false, $"The app failed due to {e.Message}.");
                check = false;
            }

            if (!check)
            {
                LogOutProgress();
                Welcome_Page();
            }
            else
            {
                Validators.PressEnterToContinue();
                DotAnimation();
                WelcomeBack();
            }
        }

        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("\n------------Register Page------------\n");
            GetUserInfo();
            expenses.Register(_currentUser);
            WelcomeBack();
        }

        private static int? DisplayAllManagersForTickets()
        {
            bool check;
            int? id = 1;
            var managers = expenses.GetManagerUsers();

            if (managers.Count == 0)
            {
                Console.WriteLine("No available managers to select.");
                return null;
            }

            do
            {
                var i = 0;
                check = true;
                Console.WriteLine("please select your manager");

                foreach (var manager in managers)
                {
                    Console.WriteLine($"{i + 1}: {manager.FullName}");
                    i += 1;
                }

                var selection = ValidateInput<int>("who is your manager");
                if (selection < 1 || selection > i)
                {
                    check = false;
                    Validators.DisplayMessage(false, "Incorrect choice from list of managers");
                }
                else
                {
                    id = managers[selection - 1].Id;
                }
            } while (!check);

            return id;
        }

        private static void GetRole()
        {
            do
            {
                Console.WriteLine("Please select a role to continue");
                DisplayEnum<Role>();
            } while (!CheckRoleSelection());
        }

        private static string ValidatePassword()
        {
            string password;
            string repeatpassword;

            do
            {
                password = Validators.HidePassword("enter your password");
                repeatpassword = Validators.HidePassword("re-enter your password"); //ValidatePassword
            } while (!Validators.Verifypassword(password, repeatpassword));

            return password;
        }

        private static string ValidateEmail()
        {
            string email;
            do
            {
                email = Validators.GetInput("enter your email");
                // Console.WriteLine(expenses.CheckEmail(email));
            } while (!Validators.ValidEmail(email) || expenses.CheckEmail(email));

            return email;
        }

        private static string ChangeEmail()
        {
            string email;
            do
            {
                email = Validators.GetInput("enter your email");
                // Console.WriteLine(expenses.CheckEmail(email));
            } while (!Validators.ValidEmail(email));

            return email;
        }

        private static string ValidatePhone()
        {
            string phone;
            do
            {
                phone = Validators.GetInput("enter your phone number");
            } while (!Validators.IsPhone(phone));

            return phone;
        }

        private static void GetUserInfo()
        {
            var email = ValidateEmail();
            var password = ValidatePassword();
            var phone = ValidatePhone();
            var fullname = Validators.GetInput("enter your full name");
            _currentUser = new User(email, password, phone, fullname);
            GetRole();
        }

        private static void ChangeUserInfo()
        {
            var email = ChangeEmail();
            var password = ValidatePassword();
            var phone = ValidatePhone();
            var fullname = Validators.GetInput("enter your full name");
            var address = Validators.GetInput("enter your full address");

            // currentUser = new User(email, password, phone, fullname);
            GetRole();
            _currentUser.Email = email;
            _currentUser.Password = password;
            _currentUser.Phone = phone;
            _currentUser.FullName = fullname;
            _currentUser.Address = address;
            expenses.UpdateInfo(_currentUser);
            WelcomeBack();
        }
    }
}