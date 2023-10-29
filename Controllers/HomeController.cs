using Homework03.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.IO;
using CrystalDecisions.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Homework03.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        public ActionResult Index(int studentPage = 1, int bookPage = 1, int pageSize = 10)
        {

            var studentList = db.students.ToList();
            var bookList = db.books.Include(b => b.author).Include(b => b.type).Include(b => b.borrows);
            var books = bookList.ToList();
            var pagenatedstudentList = studentList.ToPagedList(studentPage, pageSize);
            var pagentedBookList = books.ToPagedList(bookPage, pageSize);
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            ViewBag.studentPage = studentPage; 
            ViewBag.bookPage = bookPage; 
            ViewBag.pageSize = pageSize; 

            var viewModel = new HomeViewModel
            {
                StudentList = pagenatedstudentList,
                BookList = pagentedBookList
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<ActionResult> Create(student Student)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(Student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("failed");
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateBook(book Book)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(Book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("failed");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor(author Author)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(Author);
                await db.SaveChangesAsync();
                return RedirectToAction("Maintain");
            }
            else
            {
                Console.WriteLine("failed");
                return RedirectToAction("Maintain");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateType(type Type)
        {
            if (ModelState.IsValid)
            {
                db.types.Add(Type);
                await db.SaveChangesAsync();
                return RedirectToAction("Maintain");
            }
            else
            {
                Console.WriteLine("failed");
                return RedirectToAction("Maintain");
            }
        }

        public ActionResult Report()
        {
            
            ViewBag.bookId = new SelectList(db.books, "bookId", "name");
            return View();
        }

        public PartialViewResult Filter(int bookId)
        {
            
            var filteredData = db.borrows.Where(b => b.bookId == bookId).Include(b => b.book).Include(b => b.student);
            var filteredlist = filteredData.ToList();
            
            return PartialView("_Reports", filteredlist);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateBorrows(borrow Borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Borrow).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Maintain");
            }
            ViewBag.bookId = new SelectList(db.books, "bookId", "name", Borrow.bookId);
            ViewBag.studentId = new SelectList(db.students, "studentId", "name", Borrow.studentId);
            return Content("Data updated successfully");
        }
        [HttpPost]
        public async Task<ActionResult> UpdateAuthors(author Author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Author).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Maintain");
            }
            return Content("Data updated successfully");
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTypes(type Types)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Types).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Maintain");
            }
            return Content("Data updated successfully");
        }
        [HttpPost]
        public ActionResult SaveReport(string pdfData)
        {
            string folderPath = Server.MapPath("~/Reports/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            string filePath = Path.Combine(folderPath, fileName);
            System.IO.File.WriteAllText(filePath, pdfData);

            return Json(new { success = true, filePath = filePath });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteType(int id)
        {
            type type = db.types.Find(id);
            db.types.Remove(type);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            author author = db.authors.Find(id);
            db.authors.Remove(author);
            await db.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteBorrow(int id)
        {
            borrow borrow = db.borrows.Find(id);
            db.borrows.Remove(borrow);
            await db.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }
        public ActionResult Maintain()
        {
            var borrowlist = db.borrows.Include(b => b.book).Include(b => b.student);
            var authorlist = db.authors.ToList();
            var typeslist = db.types.ToList();
            var viewModel = new Maintainance
            {
                BorrowsList = borrowlist.ToList(),
                AuthorsList = authorlist,
                TypesList = typeslist
            };
            ViewBag.bookId = new SelectList(db.books, "bookId", "name");
            ViewBag.studentId = new SelectList(db.students, "studentId", "name");

            return View(viewModel);
        }
    }
}