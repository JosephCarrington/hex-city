﻿//----------------------------------------------//
// Gamelogic Grids                              //
// http://www.gamelogic.co.za                   //
// Copyright (c) 2013 Gamelogic (Pty) Ltd       //
//----------------------------------------------//

// Auto-generated File

using System.Linq;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Grids
{
	/// <summary>
	/// Place holder class.
	/// </summary>
	[Version(1,6)]
	public class __CellType{}

	/// <summary>
	/// This class provides static methods that ensure all the code is 
	/// generated by to AOT compiler for iOS. 
	///
	/// The easiest way to use it is to copy the appropriate methods to
	/// one of your classes, modify the __CellType type, and call the methods.
	/// </summary>
	[Version(1,6)]
	public static class __CompilerHints
	{

		public static bool __CompilerHint__Rect()
		{
			return __CompilerHint1__Rect() && __CompilerHint2__Rect();
		}

		/// <summary>
		/// Call this method if you use a RectGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__Rect()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>		
		public static bool __CompilerHint1__Rect()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new RectGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<RectPoint>(new IntRect(), p => true);
			var shapeInfo = new RectShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.Translate(RectPoint.Zero) != null;
		}

		public static bool __CompilerHint2__Rect()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new RectGrid<__CellType>(1, 1, p => p == RectPoint.Zero, x => x, x => x, new List<RectPoint>());

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<RectPoint>(new IntRect(), p => true);
			var shapeInfo = new RectShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()] == null || shapeInfo.Translate(RectPoint.Zero) != null;
		}


		public static bool __CompilerHint__Diamond()
		{
			return __CompilerHint1__Diamond() && __CompilerHint2__Diamond();
		}

		/// <summary>
		/// Call this method if you use a DiamondGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__Diamond()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>		
		public static bool __CompilerHint1__Diamond()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new DiamondGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<DiamondPoint>(new IntRect(), p => true);
			var shapeInfo = new DiamondShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.Translate(DiamondPoint.Zero) != null;
		}

		public static bool __CompilerHint2__Diamond()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new DiamondGrid<__CellType>(1, 1, p => p == DiamondPoint.Zero, x => x, x => x, new List<DiamondPoint>());

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<DiamondPoint>(new IntRect(), p => true);
			var shapeInfo = new DiamondShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()] == null || shapeInfo.Translate(DiamondPoint.Zero) != null;
		}


		public static bool __CompilerHint__PointyHex()
		{
			return __CompilerHint1__PointyHex() && __CompilerHint2__PointyHex();
		}

		/// <summary>
		/// Call this method if you use a PointyHexGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__PointyHex()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>		
		public static bool __CompilerHint1__PointyHex()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new PointyHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<PointyHexPoint>(new IntRect(), p => true);
			var shapeInfo = new PointyHexShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.Translate(PointyHexPoint.Zero) != null;
		}

		public static bool __CompilerHint2__PointyHex()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new PointyHexGrid<__CellType>(1, 1, p => p == PointyHexPoint.Zero, x => x, x => x, new List<PointyHexPoint>());

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<PointyHexPoint>(new IntRect(), p => true);
			var shapeInfo = new PointyHexShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()] == null || shapeInfo.Translate(PointyHexPoint.Zero) != null;
		}


		public static bool __CompilerHint__FlatHex()
		{
			return __CompilerHint1__FlatHex() && __CompilerHint2__FlatHex();
		}

		/// <summary>
		/// Call this method if you use a FlatHexGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__FlatHex()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>		
		public static bool __CompilerHint1__FlatHex()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new FlatHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<FlatHexPoint>(new IntRect(), p => true);
			var shapeInfo = new FlatHexShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.Translate(FlatHexPoint.Zero) != null;
		}

		public static bool __CompilerHint2__FlatHex()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new FlatHexGrid<__CellType>(1, 1, p => p == FlatHexPoint.Zero, x => x, x => x, new List<FlatHexPoint>());

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<FlatHexPoint>(new IntRect(), p => true);
			var shapeInfo = new FlatHexShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()] == null || shapeInfo.Translate(FlatHexPoint.Zero) != null;
		}


		/// <summary>
		/// Call this method if you use a FlatTriGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__FlatTri()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>
		public static bool __CompilerHint__FlatTri()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new PointyHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<FlatTriPoint>(new IntRect(), p => true);
			var shapeInfo = new FlatTriShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.IncIndex(0) != null;
		}

		/// <summary>
		/// Call this method if you use a PointyTriGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__PointyTri()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>
		public static bool __CompilerHint__PointyTri()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new FlatHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<PointyTriPoint>(new IntRect(), p => true);
			var shapeInfo = new PointyTriShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.IncIndex(0) != null;
		}

		/// <summary>
		/// Call this method if you use a FlatRhombGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__FlatRhomb()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>
		public static bool __CompilerHint__FlatRhomb()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new FlatHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<FlatRhombPoint>(new IntRect(), p => true);
			var shapeInfo = new FlatRhombShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.IncIndex(0) != null;
		}

		/// <summary>
		/// Call this method if you use a PointyRhombGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__PointyRhomb()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>
		public static bool __CompilerHint__PointyRhomb()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new PointyHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<PointyRhombPoint>(new IntRect(), p => true);
			var shapeInfo = new PointyRhombShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.IncIndex(0) != null;
		}

		/// <summary>
		/// Call this method if you use a CairoGrid.
		/// Replace	the type __CellType to whatever type you have in your grid.
		///
		/// You can call the method anywhere in your code.
		///
		/// <code>
		/// 	if(!__CompilerHint__Cairo()) return;
		/// </code>
		///
		/// Since 1.6
		/// </summary>
		/// <remarks>
		/// This methods always returns true.
		/// </remarks>
		public static bool __CompilerHint__Cairo()
		{
			//Ensures abstract super classes for base grids gets created
			var grid = new PointyHexGrid<__CellType[]>(1, 1);

			foreach(var point in grid)
			{
				grid[point] = new __CellType[1];
			} 

			//Ensures shape infpo classes get created
			var shapeStorageInfo = new ShapeStorageInfo<CairoPoint>(new IntRect(), p => true);
			var shapeInfo = new CairoShapeInfo<__CellType>(shapeStorageInfo);

			return grid[grid.First()][0] == null || shapeInfo.IncIndex(0) != null;
		}
	}
}
