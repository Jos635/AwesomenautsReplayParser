﻿using AwesomenautsReplayParser;
using AwesomenautsReplayParser.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReplayViewer
{
    class ReplayCanvas : Canvas
    {
        private Typeface font = new Typeface("Arial");

        public AbsoluteTime TimePoint { get; set; }

        public Replay Replay { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            if (Replay == null)
            {
                dc.DrawText(Text("Error"), new Point(10, 10));
            }
            else
            {
                dc.DrawText(Text($"Replay {TimePoint}"), new Point(10, 10));

                var block = Replay.GetBlockData(TimePoint);
                var relativeTime = TimePoint.RelativeTo(block.BaseRange);
                foreach(var character in block.Characters)
                {
                    RenderCharacter(dc, relativeTime, character);
                }

                foreach(var turret in block.Turrets)
                {
                    RenderTurret(dc, relativeTime, turret);
                }
            }
        }

        private void RenderCharacter(DrawingContext dc, TimePoint time, CharacterEntity character)
        {
            if (character.VisibleTime.Contains(time))
            {
                var point = TransformPoint(character.Position[time]);
                var brush = Brushes.Gray;

                if(character.TeamId == 0)
                {
                    brush = Brushes.Red;
                }else if(character.TeamId == 1)
                {
                    brush = Brushes.Blue;
                }

                dc.DrawEllipse(brush, null, point, 10, 10);

                var point2 = new Point(point.X - 10, point.Y - 20);
                dc.DrawText(Text($"{character.Username}:{character.CharacterName}"), point2);
            }
        }

        private void RenderTurret(DrawingContext dc, TimePoint time, TurretEntity turret)
        {
            var point = TransformPoint(turret.Position);
            var brush = Brushes.Gray;
            var aim = turret.Aim[time];

            if (turret.TeamId == 0)
            {
                brush = Brushes.Red;
            }
            else if (turret.TeamId == 1)
            {
                brush = Brushes.Blue;
                aim -= 180;
            }

            dc.DrawRectangle(brush, null, new Rect(point.X - 5, point.Y - 10, 10, 20));
            dc.DrawLine(new Pen(Brushes.Black, 3), point, AddAim(point, aim, 10));
        }

        private Point AddAim(Point basePoint, double direction, double length)
        {
            return new Point(
                basePoint.X + Math.Cos(DegreeToRadian(direction)) * length,
                basePoint.Y + -Math.Sin(DegreeToRadian(direction)) * length
            );
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private Point TransformPoint(Position pos)
        {
            var x = (pos.X + 10) / 30.0 * ActualWidth;
            var y = (1 - (pos.Y + 10) / 40.0) * ActualHeight;
            
            return new Point(x, y);
        }

        private FormattedText Text(string str)
        {
            return new FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, font, 12, Brushes.Black);
        }
    }
}
