// 
// NHibernate.Mapping.Attributes
// This product is under the terms of the GNU Lesser General Public License.
//
//
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 2.0.50727.x
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
	public class CompositeIndexAttribute : BaseAttribute
	{
		
		private string _class = null;
		
		/// <summary> Default constructor (position=0) </summary>
		public CompositeIndexAttribute() : 
				base(0)
		{
		}
		
		/// <summary> Constructor taking the position of the attribute. </summary>
		public CompositeIndexAttribute(int position) : 
				base(position)
		{
		}
		
		/// <summary> </summary>
		public virtual string Class
		{
			get
			{
				return this._class;
			}
			set
			{
				this._class = value;
			}
		}
		
		/// <summary> </summary>
		public virtual System.Type ClassType
		{
			get
			{
				return System.Type.GetType( this.Class );
			}
			set
			{
				if(value.Assembly == typeof(int).Assembly)
					this.Class = value.FullName.Substring(7);
				else
					this.Class = HbmWriterHelper.GetNameWithAssembly(value);
			}
		}
	}
}
