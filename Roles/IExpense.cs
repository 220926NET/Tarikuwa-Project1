using System.Collections.Generic;
using Expense.Models;

namespace Expense.Roles
{
    public interface
        IExpense //interface class //any class wanted to use it needs to implement  it//all its methods is public
    {
        // return all users info when login
        User Login(User user);

        // check if user already exist
        bool CheckEmail(string email);

        // can update his info
        void UpdateInfo(User user);

        // register new User
        void Register(User user);

        // get all users
        List<User> GetAllUsers();

        // get all employees
        List<Tickets> ViewTicketsToProcess(int managerId);

        // get all managers
        List<User> GetManagerUsers();

        // submit requests//for employee and manager cus manager can also ask for refund
        void SubmitRequest(Tickets tickets);

        // view all prior tickets
        List<Tickets> ViewPriorTickets(int employeeId);

        // Manager process tickets
        void ChangeTicketStatus(User manager, int ticketId, Status status);
    }
}