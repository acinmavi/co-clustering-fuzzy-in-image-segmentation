using System;
using System.ComponentModel;

namespace FCCIAlgorithm
{
	/// <summary>
	/// Structure to define CIE L*a*b*.
	/// </summary>
	public struct CIELab
	{
		/// <summary>
		/// Gets an empty CIELab structure.
		/// </summary>
		public static readonly CIELab Empty = new CIELab();

		#region Fields
		private double l;
		private double a;
		private double b;
		
		public int iWidth { get; set; }
		
		public int iHeight { get; set; }
		#endregion

		#region Operators
		public static bool operator ==(CIELab item1, CIELab item2)
		{
			return (
				item1.L == item2.L 
				&& item1.A == item2.A 
				&& item1.B == item2.B
				);
		}

		public static bool operator !=(CIELab item1, CIELab item2)
		{
			return (
				item1.L != item2.L 
				|| item1.A != item2.A 
				|| item1.B != item2.B
				);
		}

		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets L component.
		/// </summary>
		public double L
		{
			get
			{
				return this.l;
			}
			set
			{
				this.l = value;
			}
		}

		/// <summary>
		/// Gets or sets a component.
		/// </summary>
		public double A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		/// <summary>
		/// Gets or sets a component.
		/// </summary>
		public double B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		#endregion

		
		
		#region Methods
		public override bool Equals(Object obj) 
		{
			if(obj==null || GetType()!=obj.GetType()) return false;

			return (this == (CIELab)obj);
		}

		public override int GetHashCode() 
		{
			return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
		}
	
		public CIELab clone()
		{
			CIELab clone = new CIELab();
			clone.A = A;
			clone.B = B;
			clone.L=L;
			clone.iHeight = iHeight;
			clone.iWidth = iWidth;
			return clone;
		}
		
		
		public override string ToString()
		{
			return string.Format("[CIELab L={0}, A={1}, B={2}, IWidth={3}, IHeight={4}]", l, a, b, iWidth, iHeight);
		}

		#endregion
	} 
}
