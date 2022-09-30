using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProtoRogue
{
    public class AttackPattern
    {
        //Array that holds the relative coords of the gridcells that are part of the pattern.
        public Vector2Int[] PatternArray { get; }

        //Constructor gets pattern data
        public AttackPattern(params Vector2Int[] patternArray)
        {
            this.PatternArray = patternArray;
        }

        public enum Dir
        {
            Up,
            Right,
            Down,
            Left
        }

        //Return the attackpattern (array), adjusted for the relative position of param origin, and rotated in dir. 
        //Simplified: Places and rotates pattern on game Grid, returns new relative pattern.
        public Vector2Int[] GetRelativePatternAt(Vector2Int originPos, Dir dir)
        {
            var resultingPattern = PatternArray
                //Rotate each relative position of the cells in the pattern to a direction. In effect rotates the whole pattern.
                .Select(cellPos => RotatePatternPosition(cellPos, dir))
                //Add the cell position of the pattern origin to each cell position in the pattern. In effect moves the whole pattern to a specific gridcell location.
                .Select(cellPos => originPos + cellPos)
                //Return the rotated and moved array as an array.
                .ToArray();
            return resultingPattern;
        }

        private Vector2Int RotatePatternPosition(Vector2Int relativeCellPos, Dir dir)
        {
            switch (dir)
            {
                case Dir.Up:
                    return relativeCellPos;
                case Dir.Right:
                    //Return the cell pos if the pattern was turned to the right. Quick way of rotating pattern vector 90 degrees clockwise.
                    return new Vector2Int(relativeCellPos.y, -relativeCellPos.x);
                case Dir.Down:
                    //Flip vector (that is in up dir by default), to get downwards facing vector.
                    return new Vector2Int(-relativeCellPos.x, -relativeCellPos.y);
                case Dir.Left:
                    //Return the cell pos if the pattern was turned to the left. Quick way of rotating pattern vector 90 degrees counter-clockwise.
                    return new Vector2Int(-relativeCellPos.y, relativeCellPos.x);
                default:
                    Debug.LogError("Pattern rotation direction not valid.");
                    //Return the input vector if direction was not valid.
                    return relativeCellPos;
            }
            
        }
    }
}