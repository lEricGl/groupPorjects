using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment_one.Data;
using assignment_one.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace assignment_one.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AuctionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
              return View(await _context.Auction.ToListAsync());
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Auction == null)
            {
                return NotFound();
            }

            var auction = await _context.Auction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,StartingPrice,StartDate,Category,Condition,UserId")] Auction auction)
        {
            auction.UserId = _userManager.GetUserId(User);
            auction.StartDate = DateTime.Now;
            _context.Auction.Add(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Auctions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Auction == null)
            {
                return NotFound();
            }

            var auction = await _context.Auction.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,StartingPrice,StartDate,Category,Condition,UserId")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                _context.Update(auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
        }
    }
}
