using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly Db _db;
        private readonly ILog Log;

        public NoteController(Db context, ILog logger)
        {
            _db = context;
            Log = logger;
        }

        [HttpGet("fnol/{fnolId}")]
        public async Task<ActionResult> GetNotesByFnolId(int fnolId)
        {
            Log.Info($"START: Get Notes by FnolId {fnolId}.");

            var query = from n in _db.Note
                        join u in _db.User on n.UserID equals u.UserID into userJoin
                        from u in userJoin.DefaultIfEmpty()
                        where n.FnolId == fnolId
                        where !String.IsNullOrEmpty(n.NoteText) 
                        select new
                        {
                            NoteId = n.NoteID,
                            NoteText = n.NoteText,
                            TimeStamp = n.TimeStamp,
                            Username = u.Name
                        };

            var results = await query.ToListAsync();

            Log.Info($"Found notes for FnolId {fnolId}.");

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            _db.Note.Add(note);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"We encountered an exception while writing the note. Exception: {ex.Message}");
            }
            return Ok(note);
        }

        [HttpPut("{photosExpressId}")]
        public async Task<ActionResult<Note>> UpdatePhotosExpressNote(int photosExpressId, [FromBody] decimal fnolId)
        {
            Log.Info($"Start: Updating photosExpressNote");
            try
            {
                var noteList = await _db.Note.Where(note => note.FnolId == fnolId && !String.IsNullOrEmpty(note.NoteText)).ToListAsync();

                noteList.ForEach(note => note.PhotosExpressId = photosExpressId);

                _db.UpdateRange(noteList);
                await _db.SaveChangesAsync();
                Log.Info($"Return: Finishinged updating photosExpressNote");
            }
            catch (Exception ex)
            {
                Log.Error($"We encountered an exception while writing the note. Exception: {ex.Message}");
            }
            return Ok();
        }
    }
}
