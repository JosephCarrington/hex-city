﻿
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Grids
{
	/// <summary>
	/// Extensions for IMap.
	/// </summary>
	[Version(1,10)]
	public static class MapExtensions
	{

		/// <summary>
		/// This method returns a list of grid points that form a straight line between the two given grid points.
		/// </summary>
		public static List<TPoint> GetLine<TPoint>(this IMap<TPoint> map, TPoint p1, TPoint p2)
			where TPoint : IGridPoint<TPoint>, IVectorPoint<TPoint>
		{
			int distance = p1.DistanceFrom(p2);

			var line = new List<TPoint>();

			if (distance == 0)
			{
				line.Add(p1);

				return line;
			}

			for (int i = 0; i <= distance; i++)
			{
				var t = i/(float) distance;
				var point = map[map[p1]*(1 - t) + map[p2]*t];
				line.Add(point);
			}

			return line;
		}
	}
}