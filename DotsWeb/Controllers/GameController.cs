using System;
using System.Linq;
using Dots.Core;
using DotsCore.Entity;
using DotsCore.Service;
using DotsWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotsWeb.Controllers
{
    public class GameController : Controller
    {
        private const string FieldSessionKey = "field";

        private IScoreService _scoreService = new ScoreServiceEF();
        private IRatingService _ratingService = new RatingServiceEF();
        private ICommentService _commentService = new CommentServiceEF();
        
        public IActionResult Index()
        {
            Field field = new Field(5, 5, 60);
            field.Timer();
            HttpContext.Session.SetObject(FieldSessionKey, field);

            return View("Index", CreateModel());
        }

        public IActionResult Connect(int row, int col)
        {
            Field field = (Field) HttpContext.Session.GetObject(FieldSessionKey);
            if (!field.GameRunning)
            {
                return View("Index", CreateModel());
            }
            field.Timer();
            Coordinate newCoordinate = new Coordinate(row, col);
            if (!field.GetLine().Any() && row >= 0 && col >= 0)
            {
                field.Start(newCoordinate);
            }
            else
            {
                field.ConnectionHandling(newCoordinate);
            }
            HttpContext.Session.SetObject(FieldSessionKey, field);
            if (field.GameRunning)
            {
                return View("Index", CreateModel());
            }
            else
            {
                return View("PostGame", CreateModel());
            }
        }

        public IActionResult SaveScore(string player, int points)
        {
            if (player == null)
            {
                return View("PostGame", CreateModel());
            }
            _scoreService.AddScore(new Score{PlayerName = player, Points = points});

            return View("PostGame", CreateModel());
        }

        public IActionResult SaveRatingComment(string player, int rate, string commentText)
        {
            if (player == null)
            {
                return View("PostGame", CreateModel());
            }

            if (rate > 0 && rate <= 100)
            {
                _ratingService.AddRating(new Rating {PlayerName = player, Rate = rate});
            }

            if (commentText != null)
            {
                _commentService.AddComment(new Comment {PlayerName = player, Text = commentText});
            }

            return View("PostGame", CreateModel());
        }

        public IActionResult GoHome()
        {
            return Redirect("/Home/Index");
        }

        private GameModel CreateModel()
        {
            Field field = (Field) HttpContext.Session.GetObject(FieldSessionKey);
            var scores = _scoreService.GetTopScores();
            
            return new GameModel(field, scores);
        }
    }
}