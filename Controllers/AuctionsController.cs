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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,StartingPrice,StartDate,Category,Condition")] Auction auction)
        {

            if (ModelState.IsValid)
            {
                // Get the current user's ID as a string
                string userIdString = _userManager.GetUserId(User);

                // Convert the user ID to an int
                int userId = int.Parse(userIdString);

                auction.UserId = userId;

                _context.Add(auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
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

            // Ensure that only the user who created the product can edit it
            if (auction.UserId != int.Parse(_userManager.GetUserId(User)))
            {
                return Forbid();
            }
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,StartingPrice,StartDate,Category,Condition,UserId")] Auction auction)
        {
            if (id != auction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that only the user who created the product can edit it
                    if (auction.UserId != int.Parse(_userManager.GetUserId(User)))
                    {
                        return Forbid();
                    }

                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
        }

        private bool AuctionExists(int id)
        {
          return _context.Auction.Any(e => e.Id == id);
        }
    }
}
