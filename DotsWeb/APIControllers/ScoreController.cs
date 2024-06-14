using System.Collections;
using System.Collections.Generic;
using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotsWeb.APIControllers
{
    //https://localhost:44324/api/Score
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private IScoreService _scoreService = new ScoreServiceEF();

        //GET: /api/Score
        [HttpGet]
        public IEnumerable<Score> GetScores()
        {
            return _scoreService.GetTopScores();
        }

        //POST: /api/Score
        [HttpPost]
        public void PostScore(Score score)
        {
            _scoreService.AddScore(score);
        }
    }
}