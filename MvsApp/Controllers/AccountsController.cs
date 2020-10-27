using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvsApplication.Models;
using Transactions = MvsApplication.ViewModels.Transactions;

namespace MvsApplication.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MvsAppContext _context;
        
        public AccountsController(MvsAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Put()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Put(Transactions model)
        {
            if(ModelState.IsValid)
            {
                Account account = await _context.Accounts.FirstOrDefaultAsync(
                    a => a.User.Email == User.Identity.Name);
                account.Balance = account.Balance + model.Sum;
                account.Transactions = new List<Models.Transaction>() {new Transaction
                    {
                        Sum = model.Sum,
                        ReceiverAccountId = account.BalanceName,
                        SenderAccountId = "Terminal",
                        Accounts = account
                    }};
                _context.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Withdraw()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(Transactions model)
        {
            if(ModelState.IsValid)
            {
                Account account = await _context.Accounts.FirstOrDefaultAsync(
                    a => a.User.Email == User.Identity.Name);
                if (account.Balance >= model.Sum)
                {
                    account.Balance = account.Balance - model.Sum;
                    account.Transactions = new List<Models.Transaction>()
                    {
                        new Transaction
                        {
                            Sum = model.Sum,
                            ReceiverAccountId = account.BalanceName,
                            SenderAccountId = "ATM",
                            Accounts = account
                        }
                    };
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }else
                {
                    ModelState.AddModelError("", "There is not enough money in your account");    
                }
            }
            return View(model);
        }
        
        public async Task<IActionResult> SendToAnotherAccount()
        {
            ViewData["BalanceName"] = new SelectList(
                _context.Accounts.Where(a=> a.User.Email != User.Identity.Name),
                "BalanceNumber",
                "BalanceName"
            );
            return View();
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendToAnotherAccount(Transactions model)
        {
            if (ModelState.IsValid)
            {
                Account accountSender = await _context.Accounts.FirstOrDefaultAsync(
                    a => a.User.Email == User.Identity.Name);
                Account accountReceiver = await _context.Accounts.FirstOrDefaultAsync(
                    a => a.BalanceNumber == model.BalanceName);
                if (accountSender.Balance >= model.Sum)
                {
                    accountSender.Balance = accountSender.Balance - model.Sum;
                    accountReceiver.Balance = accountReceiver.Balance + model.Sum;
                    accountSender.Transactions = new List<Transaction>(){new Transaction()
                        {
                            ReceiverAccountId = accountReceiver.BalanceName,
                            SenderAccountId = accountSender.BalanceName,
                            Sum = model.Sum,
                        }};
                    _context.Update(accountReceiver);
                    _context.Update(accountSender);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                } 
                ModelState.AddModelError("", "There is not enough money in your account");
            }
            ViewData["BalanceName"] = new SelectList(
                _context.Accounts.Where(a=> a.User.Email != User.Identity.Name),
                "BalanceNumber",
                "BalanceName"
            );
            return View(model);
        }
        
        public async Task<IActionResult> ShowTransactions()
        {
            var webContext = _context.Transactions.Where(
                t => t.Accounts.User.Email == User.Identity.Name).Include(
                t => t.Accounts);
            return View(await webContext.ToListAsync());
        }
        
    }
}