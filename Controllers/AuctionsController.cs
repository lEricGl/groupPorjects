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

        // GET: Auctions/Details
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageUrl,StartingPrice,EndDate,Category,Condition")] Auction auction)
        {
            // Set the user id
            auction.UserId = _userManager.GetUserId(User);

            // Set the start date
            auction.StartDate = DateTime.Now;

            // Validate the model
            if (ModelState.IsValid)
            {
                _context.Add(auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
        }

        // GET: Auctions/Edit
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        // POST: Auctions/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,StartingPrice,EndDate,Category,Condition")] Auction auction)
        {
            if (id != auction.Id)
            {
                return NotFound();
            }

            // Assigns the the startdate and userid to the auction object
            auction.StartDate = _context.Auction.Where(a => a.Id == id).Select(a => a.StartDate).FirstOrDefault();
            auction.UserId = _context.Auction.Where(a => a.Id == id).Select(a => a.UserId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
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

        // POST: Auctions/Delete
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await _context.Auction.FindAsync(id);
            _context.Auction.Remove(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Auctions/Delete
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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

        // GET: Auctions/MyAuctions
        [Authorize]
        public async Task<IActionResult> MyAuctions()
        {
            var userId = _userManager.GetUserId(User);
            var auctions = await _context.Auction.Where(a => a.UserId == userId).ToListAsync();
            return View(auctions);
        }

        // AuctionExists
        private bool AuctionExists(int id)
        {
            return _context.Auction.Any(e => e.Id == id);
        }

        // GET: Auctions/Search View
        public IActionResult SearchView()
        {
            return View();
        }

        // GET: Auctions/Search
        public async Task<IActionResult> Search(string SearchAuction)
        {
            var auctions = from a in _context.Auction
                           select a;

            if (!String.IsNullOrEmpty(SearchAuction))
            {
                auctions = auctions.Where(a => a.Name.Contains(SearchAuction));
            }

            return View(await auctions.ToListAsync());
        }

    }
}
