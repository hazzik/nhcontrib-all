// 
// NHibernate.Mapping.Attributes
// This product is under the terms of the GNU Lesser General Public License.
//
//
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
//
//
// This source code was auto-generated by Refly, Version=2.21.1.0 (modified).
//
namespace NHibernate.Mapping.Attributes
{
	
	
	/// <summary> </summary>
	[System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple=true)]
	[System.Serializable()]
	public class ColumnAttribute : BaseAttribute
	{
		
		private string _index = null;
		
		private int _length = -1;
		
		private string _uniquekey = null;
		
		private string _check = null;
		
		private bool _notnullspecified;
		
		private bool _notnull = false;
		
		private string _name = null;
		
		private string _sqltype = null;
		
		private bool _uniquespecified;
		
		private bool _unique = false;
		
		/// <summary> Default constructor (position=0) </summary>
		public ColumnAttribute() : 
				base(0)
		{
		}
		
		/// <summary> Constructor taking the position of the attribute. </summary>
		public ColumnAttribute(int position) : 
				base(position)
		{
		}
		
		/// <summary> </summary>
		public virtual string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}
		
		/// <summary> </summary>
		public virtual int Length
		{
			get
			{
				return this._length;
			}
			set
			{
				this._length = value;
			}
		}
		
		/// <summary> </summary>
		public virtual bool NotNull
		{
			get
			{
				return this._notnull;
			}
			set
			{
				this._notnull = value;
				_notnullspecified = true;
			}
		}
		
		/// <summary> Tells if NotNull has been specified. </summary>
		public virtual bool NotNullSpecified
		{
			get
			{
				return this._notnullspecified;
			}
		}
		
		/// <summary> </summary>
		public virtual bool Unique
		{
			get
			{
				return this._unique;
			}
			set
			{
				this._unique = value;
				_uniquespecified = true;
			}
		}
		
		/// <summary> Tells if Unique has been specified. </summary>
		public virtual bool UniqueSpecified
		{
			get
			{
				return this._uniquespecified;
			}
		}
		
		/// <summary> </summary>
		public virtual string UniqueKey
		{
			get
			{
				return this._uniquekey;
			}
			set
			{
				this._uniquekey = value;
			}
		}
		
		/// <summary> </summary>
		public virtual string SqlType
		{
			get
			{
				return this._sqltype;
			}
			set
			{
				this._sqltype = value;
			}
		}
		
		/// <summary> </summary>
		public virtual string Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}
		
		/// <summary> </summary>
		public virtual string Check
		{
			get
			{
				return this._check;
			}
			set
			{
				this._check = value;
			}
		}
	}
}
