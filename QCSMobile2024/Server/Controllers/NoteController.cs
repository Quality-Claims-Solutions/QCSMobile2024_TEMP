using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using System.Runtime.CompilerServices;

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

        static string MethodName([CallerMemberName] string name = null) => name;


        [HttpGet("fnol/{fnolId}")]
        public async Task<ActionResult> GetNotesByFnolId(int fnolId)
        {
            Log.Info($"NoteController_{MethodName()} START: Get Notes associated FnolId: {fnolId}");

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

            Log.Info($"NoteController_{MethodName()} RETURN: Found {results.Count} Notes associated FnolId: {fnolId}");

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            Log.Info($"NoteController_{MethodName()} START: Creating Note");

            _db.Note.Add(note);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"NoteController_{MethodName()} ERROR: We encountered an exception while writing the note. Exception: {ex.Message}");
            }

            Log.Info($"NoteController_{MethodName()} RETURN: Created Note by {note.Author} which says: {note.NoteText}");
            return Ok(note);
        }

        [HttpPut("{photosExpressId}")]
        public async Task<ActionResult<Note>> UpdatePhotosExpressNote(int photosExpressId, [FromBody] decimal fnolId)
        {
            Log.Info($"NoteController_{MethodName()} START: Updating photosExpressNote with photosExpressId: {photosExpressId} and fnolId: {fnolId}");
            try
            {
                var noteList = await _db.Note.Where(note => note.FnolId == fnolId && !String.IsNullOrEmpty(note.NoteText)).ToListAsync();

                noteList.ForEach(note => note.PhotosExpressId = photosExpressId);

                _db.UpdateRange(noteList);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"NoteController_{MethodName()} ERROR: We encountered an exception while Updating photosExpressNote with photosExpressId: {photosExpressId} and fnolId: {fnolId}. Exception: {ex.Message}");
            }

            Log.Info($"NoteController_{MethodName()} RETURN: Finishinged updating photosExpressNote");
            return Ok();
        }
    }
}
