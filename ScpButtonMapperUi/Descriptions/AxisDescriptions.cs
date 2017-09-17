using System;
using HidReport.Contract.Enums;

namespace ScpProfiler.Descriptions
{
    internal static class AxisDescriptions
    {
        public static AxisDescription Get(AxesEnum axis)
        {
            switch (axis)
            {
                case AxesEnum.Lx: return new AxisDescription("Lx");
                case AxesEnum.Ly: return new AxisDescription("Ly");
                case AxesEnum.Rx: return new AxisDescription("Rx");
                case AxesEnum.Ry: return new AxisDescription("Ry");
                case AxesEnum.Up:
                    return new AxisDescription("Up")
                    {
                        DisplayName = "D-Pad up",
                    };
                case AxesEnum.Right:
                    return new AxisDescription("Right")
                    {
                        DisplayName = "D-Pad right",
                    };
                case AxesEnum.Down:
                    return new AxisDescription("Down")
                    {
                        DisplayName = "D-Pad down",
                    };
                case AxesEnum.Left:
                    return new AxisDescription("Left")
                    {
                        DisplayName = "D-Pad left",
                    };
                case AxesEnum.L2:
                    return new AxisDescription("L2");
                case AxesEnum.R2:
                    return new AxisDescription("R2");
                case AxesEnum.L1:
                    return new AxisDescription("L1");
                case AxesEnum.R1:
                    return new AxisDescription("R1");
                case AxesEnum.Triangle:
                    return new AxisDescription("Triangle");
                case AxesEnum.Circle:
                    return new AxisDescription("Circle");
                case AxesEnum.Cross:
                    return new AxisDescription("Cross");
                case AxesEnum.Square:
                    return new AxisDescription("Square");
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }
    }
}