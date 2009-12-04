﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using DslDiagrams = global::Microsoft.VisualStudio.Modeling.Diagrams;

[module: global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "type", Target = "NHibernate.NHDesigner.NHDesignerDiagram")]

namespace NHibernate.NHDesigner
{
	/// <summary>
	/// DomainClass NHDesignerDiagram
	/// Description for NHibernate.NHDesigner.NHDesignerDiagram
	/// </summary>
	[DslDesign::DisplayNameResource("NHibernate.NHDesigner.NHDesignerDiagram.DisplayName", typeof(global::NHibernate.NHDesigner.NHDesignerDomainModel), "NHibernate.NHDesigner.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("NHibernate.NHDesigner.NHDesignerDiagram.Description", typeof(global::NHibernate.NHDesigner.NHDesignerDomainModel), "NHibernate.NHDesigner.GeneratedCode.DomainModelResx")]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainObjectId("9d3d293a-b75d-4e4c-90cb-993c099cde5a")]
	public partial class NHDesignerDiagram : DslDiagrams::Diagram
	{
		#region Diagram boilerplate
		private static DslDiagrams::StyleSet classStyleSet;
		private static global::System.Collections.Generic.IList<DslDiagrams::ShapeField> shapeFields;
		/// <summary>
		/// Per-class style set for this shape.
		/// </summary>
		protected override DslDiagrams::StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		
		/// <summary>
		/// Per-class ShapeFields for this shape
		/// </summary>
		public override global::System.Collections.Generic.IList<DslDiagrams::ShapeField> ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#region Toolbox filters
		private static global::System.ComponentModel.ToolboxItemFilterAttribute[] toolboxFilters = new global::System.ComponentModel.ToolboxItemFilterAttribute[] {
					new global::System.ComponentModel.ToolboxItemFilterAttribute(global::NHibernate.NHDesigner.NHDesignerToolboxHelperBase.ToolboxFilterString, global::System.ComponentModel.ToolboxItemFilterType.Require) };
		
		/// <summary>
		/// Toolbox item filter attributes for this diagram.
		/// </summary>
		public override global::System.Collections.ICollection TargetToolboxItemFilterAttributes
		{
			get
			{
				return toolboxFilters;
			}
		}
		#endregion
		#region Auto-placement
		/// <summary>
		/// Indicate that child shapes should added through view fixup should be placed automatically.
		/// </summary>
		public override bool ShouldAutoPlaceChildShapes
		{
			get
			{
				return true;
			}
		}
		#endregion
		#region Compartment support
		/// <summary>
		/// Whether compartment items change events are subscribed to.
		/// </summary>
		private bool subscribedCompartmentItemsEvents;
		
		/// <summary>
		/// Subscribe to events fired when compartment items changes.
		/// </summary>
		public void SubscribeCompartmentItemsEvents()
		{
			if (!subscribedCompartmentItemsEvents && this.Store != null)
			{
				subscribedCompartmentItemsEvents = true;
				this.Store.EventManagerDirectory.ElementAdded.Add(new global::System.EventHandler<DslModeling::ElementAddedEventArgs>(this.CompartmentItemAdded));
				this.Store.EventManagerDirectory.ElementDeleted.Add(new global::System.EventHandler<DslModeling::ElementDeletedEventArgs>(this.CompartmentItemDeleted));
				this.Store.EventManagerDirectory.ElementPropertyChanged.Add(new global::System.EventHandler<DslModeling::ElementPropertyChangedEventArgs>(this.CompartmentItemPropertyChanged));
				this.Store.EventManagerDirectory.RolePlayerChanged.Add(new global::System.EventHandler<DslModeling::RolePlayerChangedEventArgs>(this.CompartmentItemRolePlayerChanged));
				this.Store.EventManagerDirectory.RolePlayerOrderChanged.Add(new global::System.EventHandler<DslModeling::RolePlayerOrderChangedEventArgs>(this.CompartmentItemRolePlayerOrderChanged));
			}
		}
		
		/// <summary>
		/// Unsubscribe to events fired when compartment items changes.
		/// </summary>
		public void UnsubscribeCompartmentItemsEvents()
		{
			if (subscribedCompartmentItemsEvents)
			{
				this.Store.EventManagerDirectory.ElementAdded.Remove(new global::System.EventHandler<DslModeling::ElementAddedEventArgs>(this.CompartmentItemAdded));
				this.Store.EventManagerDirectory.ElementDeleted.Remove(new global::System.EventHandler<DslModeling::ElementDeletedEventArgs>(this.CompartmentItemDeleted));
				this.Store.EventManagerDirectory.ElementPropertyChanged.Remove(new global::System.EventHandler<DslModeling::ElementPropertyChangedEventArgs>(this.CompartmentItemPropertyChanged));
				this.Store.EventManagerDirectory.RolePlayerChanged.Remove(new global::System.EventHandler<DslModeling::RolePlayerChangedEventArgs>(this.CompartmentItemRolePlayerChanged));
				this.Store.EventManagerDirectory.RolePlayerOrderChanged.Remove(new global::System.EventHandler<DslModeling::RolePlayerOrderChangedEventArgs>(this.CompartmentItemRolePlayerOrderChanged));
				subscribedCompartmentItemsEvents = false;
			}
		}
		
		#region Event handlers
		/// <summary>
		/// Event for element added.
		/// </summary>
		private void CompartmentItemAdded(object sender, DslModeling::ElementAddedEventArgs e)
		{
			CompartmentItemAddRule.ElementAdded(e, true /* repaint only */);
		}
		/// <summary>
		/// Event for element deleted.
		/// </summary>
		private void CompartmentItemDeleted(object sender, DslModeling::ElementDeletedEventArgs e)
		{
			CompartmentItemDeleteRule.ElementDeleted(e, true /* repaint only */);
		}
		/// <summary>
		/// Event for element property changed.
		/// </summary>
		private void CompartmentItemPropertyChanged(object sender, DslModeling::ElementPropertyChangedEventArgs e)
		{
			CompartmentItemChangeRule.ElementPropertyChanged(e, true /* repaint only */);
		}
		/// <summary>
		/// Event for role-player changed.
		/// </summary>
		private void CompartmentItemRolePlayerChanged(object sender, DslModeling::RolePlayerChangedEventArgs e)
		{
			CompartmentItemRolePlayerChangeRule.RolePlayerChanged(e, true /* repaint only */);
		}
		/// <summary>
		/// Event for role-player order changed.
		/// </summary>
		private void CompartmentItemRolePlayerOrderChanged(object sender, DslModeling::RolePlayerOrderChangedEventArgs e)
		{
			CompartmentItemRolePlayerPositionChangeRule.RolePlayerPositionChanged(e, true /* repaint only */);
		}
		#endregion
		#endregion
		#region Shape mapping
		/// <summary>
		/// Called during view fixup to ask the parent whether a shape should be created for the given child element.
		/// </summary>
		/// <remarks>
		/// Always return true, since we assume there is only one diagram per model file for DSL scenarios.
		/// </remarks>
		protected override bool ShouldAddShapeForElement(DslModeling::ModelElement element)
		{
			return true;
		}
		
		/// <summary>
		/// Called during view fixup to configure the given child element, after it has been created.
		/// </summary>
		/// <remarks>
		/// Custom code for choosing the shapes attached to either end of a connector is called from here.
		/// </remarks>
		protected override void OnChildConfiguring(DslDiagrams::ShapeElement child, bool createdDuringViewFixup)
		{
			DslDiagrams::NodeShape sourceShape;
			DslDiagrams::NodeShape targetShape;
			DslDiagrams::BinaryLinkShape connector = child as DslDiagrams::BinaryLinkShape;
			if(connector == null)
			{
				base.OnChildConfiguring(child, createdDuringViewFixup);
				return;
			}
			this.GetSourceAndTargetForConnector(connector, out sourceShape, out targetShape);
			
			global::System.Diagnostics.Debug.Assert(sourceShape != null && targetShape != null, "Unable to find source and target shapes for connector.");
			connector.Connect(sourceShape, targetShape);
		}
		
		/// <summary>
		/// helper method to find the shapes for either end of a connector, including calling the user's custom code
		/// </summary>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		internal void GetSourceAndTargetForConnector(DslDiagrams::BinaryLinkShape connector, out DslDiagrams::NodeShape sourceShape, out DslDiagrams::NodeShape targetShape)
		{
			sourceShape = null;
			targetShape = null;
			
			if (sourceShape == null || targetShape == null)
			{
				DslDiagrams::NodeShape[] endShapes = GetEndShapesForConnector(connector);
				if(sourceShape == null)
				{
					sourceShape = endShapes[0];
				}
				if(targetShape == null)
				{
					targetShape = endShapes[1];
				}
			}
		}
		
		/// <summary>
		/// Helper method to find shapes for either end of a connector by looking for shapes associated with either end of the relationship mapped to the connector.
		/// </summary>
		private DslDiagrams::NodeShape[] GetEndShapesForConnector(DslDiagrams::BinaryLinkShape connector)
		{
			DslModeling::ElementLink link = connector.ModelElement as DslModeling::ElementLink;
			DslDiagrams::NodeShape sourceShape = null, targetShape = null;
			if (link != null)
			{
				global::System.Collections.ObjectModel.ReadOnlyCollection<DslModeling::ModelElement> linkedElements = link.LinkedElements;
				if (linkedElements.Count == 2)
				{
					DslDiagrams::Diagram currentDiagram = this.Diagram;
					DslModeling::LinkedElementCollection<DslDiagrams::PresentationElement> presentationElements = DslDiagrams::PresentationViewsSubject.GetPresentation(linkedElements[0]);
					foreach (DslDiagrams::PresentationElement presentationElement in presentationElements)
					{
						DslDiagrams::NodeShape shape = presentationElement as DslDiagrams::NodeShape;
						if (shape != null && shape.Diagram == currentDiagram)
						{
							sourceShape = shape;
							break;
						}
					}
					
					presentationElements = DslDiagrams::PresentationViewsSubject.GetPresentation(linkedElements[1]);
					foreach (DslDiagrams::PresentationElement presentationElement in presentationElements)
					{
						DslDiagrams::NodeShape shape = presentationElement as DslDiagrams::NodeShape;
						if (shape != null && shape.Diagram == currentDiagram)
						{
							targetShape = shape;
							break;
						}
					}
		
				}
			}
			
			return new DslDiagrams::NodeShape[] { sourceShape, targetShape };
		}
		
		/// <summary>
		/// Creates a new shape for the given model element as part of view fixup
		/// </summary>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Generated code.")]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Generated code.")]
		protected override DslDiagrams::ShapeElement CreateChildShape(DslModeling::ModelElement element)
		{
			if(element is global::NHibernate.NHDesigner.Entity)
			{
				global::NHibernate.NHDesigner.EntityShape newShape = new global::NHibernate.NHDesigner.EntityShape(this.Partition);
				if(newShape != null) newShape.Size = newShape.DefaultSize; // set default shape size
				return newShape;
			}
			if(element is global::NHibernate.NHDesigner.EntityReferencesBase)
			{
				global::NHibernate.NHDesigner.SubclassConnector newShape = new global::NHibernate.NHDesigner.SubclassConnector(this.Partition);
				return newShape;
			}
			if(element is global::NHibernate.NHDesigner.EntityReferencesBaseWithJoin)
			{
				global::NHibernate.NHDesigner.JoinedSubclassConnector newShape = new global::NHibernate.NHDesigner.JoinedSubclassConnector(this.Partition);
				return newShape;
			}
			return base.CreateChildShape(element);
		}
		#endregion
		#region Decorator mapping
		/// <summary>
		/// Initialize shape decorator mappings.  This is done here rather than in individual shapes because decorator maps
		/// are defined per diagram type rather than per shape type.
		/// </summary>
		protected override void InitializeShapeFields(global::System.Collections.Generic.IList<DslDiagrams::ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);
			global::NHibernate.NHDesigner.EntityShape.DecoratorsInitialized += EntityShapeDecoratorMap.OnDecoratorsInitialized;
		}
		
		/// <summary>
		/// Class containing decorator path traversal methods for EntityShape.
		/// </summary>
		internal static partial class EntityShapeDecoratorMap
		{
			/// <summary>
			/// Event handler called when decorator initialization is complete for EntityShape.  Adds decorator mappings for this shape or connector.
			/// </summary>
			public static void OnDecoratorsInitialized(object sender, global::System.EventArgs e)
			{
				DslDiagrams::ShapeElement shape = (DslDiagrams::ShapeElement)sender;
				DslDiagrams::AssociatedPropertyInfo propertyInfo;
				
				propertyInfo = new DslDiagrams::AssociatedPropertyInfo(global::NHibernate.NHDesigner.Entity.NameDomainPropertyId);
				DslDiagrams::ShapeElement.FindDecorator(shape.Decorators, "NameDecorator").AssociateValueWith(shape.Store, propertyInfo);
			}
		}
		
		#endregion
		#region Connect actions
		private global::NHibernate.NHDesigner.SubclassRelationshipConnectAction subclassRelationshipConnectAction;
		private global::NHibernate.NHDesigner.JoinedSUbclassRelationshipConnectAction joinedSUbclassRelationshipConnectAction;
		/// <summary>
		/// Override to provide the right mouse action when trying
		/// to create links on the diagram
		/// </summary>
		/// <param name="pointArgs"></param>
		public override void OnViewMouseEnter(DslDiagrams::DiagramPointEventArgs pointArgs)
		{
			if (pointArgs  == null) throw new global::System.ArgumentNullException("pointArgs");
		
			DslDiagrams::DiagramView activeView = this.ActiveDiagramView;
			if(activeView != null)
			{
				DslDiagrams::MouseAction action = null;
				if (activeView.SelectedToolboxItemSupportsFilterString(global::NHibernate.NHDesigner.NHDesignerToolboxHelper.SubclassRelationshipFilterString))
				{
					if (this.subclassRelationshipConnectAction == null)
					{
						this.subclassRelationshipConnectAction = new global::NHibernate.NHDesigner.SubclassRelationshipConnectAction(this);
						this.subclassRelationshipConnectAction.MouseActionDeactivated += new DslDiagrams::MouseAction.MouseActionDeactivatedEventHandler(OnConnectActionDeactivated);
					}
					action = this.subclassRelationshipConnectAction;
				} 
				else if (activeView.SelectedToolboxItemSupportsFilterString(global::NHibernate.NHDesigner.NHDesignerToolboxHelper.JoinedSUbclassRelationshipFilterString))
				{
					if (this.joinedSUbclassRelationshipConnectAction == null)
					{
						this.joinedSUbclassRelationshipConnectAction = new global::NHibernate.NHDesigner.JoinedSUbclassRelationshipConnectAction(this);
						this.joinedSUbclassRelationshipConnectAction.MouseActionDeactivated += new DslDiagrams::MouseAction.MouseActionDeactivatedEventHandler(OnConnectActionDeactivated);
					}
					action = this.joinedSUbclassRelationshipConnectAction;
				} 
				else
				{
					action = null;
				}
				
				if (pointArgs.DiagramClientView.ActiveMouseAction != action)
				{
					pointArgs.DiagramClientView.ActiveMouseAction = action;
				}
			}
		}
		
		/// <summary>
		/// Snap toolbox selection back to regular pointer after using a custom connect action.
		/// </summary>
		private void OnConnectActionDeactivated(object sender, DslDiagrams::DiagramEventArgs e)
		{
			DslDiagrams::DiagramView activeView = this.ActiveDiagramView;
		
			if (activeView != null && activeView.Toolbox != null)
			{
				activeView.Toolbox.SelectedToolboxItemUsed();
			}
		}
		
		/// <summary>
		/// Dispose of connect actions.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if(disposing)
				{
					if(this.subclassRelationshipConnectAction != null)
					{
						this.subclassRelationshipConnectAction.Dispose();
						this.subclassRelationshipConnectAction = null;
					}
					if(this.joinedSUbclassRelationshipConnectAction != null)
					{
						this.joinedSUbclassRelationshipConnectAction.Dispose();
						this.joinedSUbclassRelationshipConnectAction = null;
					}
					this.UnsubscribeCompartmentItemsEvents();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		#endregion
		#region Constructors, domain class Id
	
		/// <summary>
		/// NHDesignerDiagram domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x9d3d293a, 0xb75d, 0x4e4c, 0x90, 0xcb, 0x99, 0x3c, 0x09, 0x9c, 0xde, 0x5a);
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public NHDesignerDiagram(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public NHDesignerDiagram(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion
	}
}
namespace NHibernate.NHDesigner
{
		/// <summary>
		/// Rule that initiates view fixup when an element that has an associated shape is added to the model. 
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityReferencesBaseWithJoin), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AddConnectionRulePriority, InitiallyDisabled=true)]
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityReferencesBase), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AddConnectionRulePriority, InitiallyDisabled=true)]
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.Entity), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AddShapeParentExistRulePriority, InitiallyDisabled=true)]
		internal sealed partial class FixUpDiagram : DslModeling::AddRule
		{
			[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
			public override void ElementAdded(DslModeling::ElementAddedEventArgs e)
			{
				if(e == null) throw new global::System.ArgumentNullException("e");
			
				DslModeling::ModelElement childElement = e.ModelElement;
				if (childElement.IsDeleted)
					return;
				DslModeling::ModelElement parentElement;
				if(childElement is DslModeling::ElementLink)
				{
					parentElement = GetParentForRelationship((DslModeling::ElementLink)childElement);
				} else
				if(childElement is global::NHibernate.NHDesigner.Entity)
				{
					parentElement = GetParentForEntity((global::NHibernate.NHDesigner.Entity)childElement);
				} else
				{
					parentElement = null;
				}
				
				if(parentElement != null)
				{
					DslDiagrams::Diagram.FixUpDiagram(parentElement, childElement);
				}
			}
			public static global::NHibernate.NHDesigner.NHibernateModel GetParentForEntity( global::NHibernate.NHDesigner.Entity root )
			{
				// Segments 0 and 1
				global::NHibernate.NHDesigner.NHibernateModel result = root.NHibernateModel;
				if ( result == null ) return null;
				return result;
			}
			private static DslModeling::ModelElement GetParentForRelationship(DslModeling::ElementLink elementLink)
			{
				global::System.Collections.ObjectModel.ReadOnlyCollection<DslModeling::ModelElement> linkedElements = elementLink.LinkedElements;
	
				if (linkedElements.Count == 2)
				{
					DslDiagrams::ShapeElement sourceShape = linkedElements[0] as DslDiagrams::ShapeElement;
					DslDiagrams::ShapeElement targetShape = linkedElements[1] as DslDiagrams::ShapeElement;
	
					if(sourceShape == null)
					{
						DslModeling::LinkedElementCollection<DslDiagrams::PresentationElement> presentationElements = DslDiagrams::PresentationViewsSubject.GetPresentation(linkedElements[0]);
						foreach (DslDiagrams::PresentationElement presentationElement in presentationElements)
						{
							DslDiagrams::ShapeElement shape = presentationElement as DslDiagrams::ShapeElement;
							if (shape != null)
							{
								sourceShape = shape;
								break;
							}
						}
					}
					
					if(targetShape == null)
					{
						DslModeling::LinkedElementCollection<DslDiagrams::PresentationElement> presentationElements = DslDiagrams::PresentationViewsSubject.GetPresentation(linkedElements[1]);
						foreach (DslDiagrams::PresentationElement presentationElement in presentationElements)
						{
							DslDiagrams::ShapeElement shape = presentationElement as DslDiagrams::ShapeElement;
							if (shape != null)
							{
								targetShape = shape;
								break;
							}
						}
					}
					
					if(sourceShape == null || targetShape == null)
					{
						global::System.Diagnostics.Debug.Fail("Unable to find source and/or target shape for view fixup.");
						return null;
					}
	
					DslDiagrams::ShapeElement sourceParent = sourceShape.ParentShape;
					DslDiagrams::ShapeElement targetParent = targetShape.ParentShape;
	
					while (sourceParent != targetParent && sourceParent != null)
					{
						DslDiagrams::ShapeElement curParent = targetParent;
						while (sourceParent != curParent && curParent != null)
						{
							curParent = curParent.ParentShape;
						}
	
						if(sourceParent == curParent)
						{
							break;
						}
						else
						{
							sourceParent = sourceParent.ParentShape;
						}
					}
	
					while (sourceParent != null)
					{
						// ensure that the parent can parent connectors (i.e., a diagram or a swimlane).
						if(sourceParent is DslDiagrams::Diagram || sourceParent is DslDiagrams::SwimlaneShape)
						{
							break;
						}
						else
						{
							sourceParent = sourceParent.ParentShape;
						}
					}
	
					global::System.Diagnostics.Debug.Assert(sourceParent != null && sourceParent.ModelElement != null, "Unable to find common parent for view fixup.");
					return sourceParent.ModelElement;
				}
	
				return null;
			}
		}
		
		/// <summary>
		/// Rule to update compartments when an item is added to the list
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityHasProperties), FireTime=DslModeling::TimeToFire.TopLevelCommit, InitiallyDisabled=true)]
		internal sealed class CompartmentItemAddRule : DslModeling::AddRule
		{
			/// <summary>
			/// Called when an element is added. 
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(DslModeling::ElementAddedEventArgs e)
			{
				ElementAdded(e, false);
			}
	
			internal static void ElementAdded(DslModeling::ElementAddedEventArgs e, bool repaintOnly)
			{
				if(e==null) throw new global::System.ArgumentNullException("e");
				if (e.ModelElement.IsDeleted)
					return;
				if(e.ModelElement is global::NHibernate.NHDesigner.EntityHasProperties)
				{
					global::System.Collections.IEnumerable elements = GetEntityForEntityShapePropertiesFromLastLink((global::NHibernate.NHDesigner.EntityHasProperties)e.ModelElement);
					UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
				}
			}
			
			#region static DomainPath traversal methods to get the list of compartments to update
			internal static global::System.Collections.ICollection GetEntityForEntityShapePropertiesFromLastLink(global::NHibernate.NHDesigner.EntityHasProperties root)
			{
				// Segment 0
				global::NHibernate.NHDesigner.Entity result = root.Entity;
				if ( result == null ) return new DslModeling::ModelElement[0];
				return new DslModeling::ModelElement[] {result};
			}
			internal static global::System.Collections.ICollection GetEntityForEntityShapeProperties(global::NHibernate.NHDesigner.Property root)
			{
				// Segments 1 and 0
				global::NHibernate.NHDesigner.Entity result = root.Entity;
				if ( result == null ) return new DslModeling::ModelElement[0];
				return new DslModeling::ModelElement[] {result};
			}
			#endregion
	
			#region helper method to update compartments 
			/// <summary>
			/// Updates the compartments for the shapes associated to the given list of model elements
			/// </summary>
			/// <param name="elements">List of model elements</param>
			/// <param name="shapeType">The type of shape that needs updating</param>
			/// <param name="compartmentName">The name of the compartment to update</param>
			/// <param name="repaintOnly">If true, the method will only invalidate the shape for a repaint, without re-initializing the shape.</param>
			internal static void UpdateCompartments(global::System.Collections.IEnumerable elements, global::System.Type shapeType, string compartmentName, bool repaintOnly)
			{
				foreach (DslModeling::ModelElement element in elements)
				{
					DslModeling::LinkedElementCollection<DslDiagrams::PresentationElement> pels = DslDiagrams::PresentationViewsSubject.GetPresentation(element);
					foreach (DslDiagrams::PresentationElement pel in pels)
					{
						DslDiagrams::CompartmentShape compartmentShape = pel as DslDiagrams::CompartmentShape;
						if (compartmentShape != null && shapeType.IsAssignableFrom(compartmentShape.GetType()))
						{
							if (repaintOnly)
							{
								compartmentShape.Invalidate();
							}
							else
							{
								foreach(DslDiagrams::CompartmentMapping mapping in compartmentShape.GetCompartmentMappings())
								{
									if(mapping.CompartmentId==compartmentName)
									{
										mapping.InitializeCompartmentShape(compartmentShape);
										break;
									}
								}
							}
						}
					}
				}
			}
			#endregion
		}
		
		/// <summary>
		/// Rule to update compartments when an items is removed from the list
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityHasProperties), FireTime=DslModeling::TimeToFire.TopLevelCommit, InitiallyDisabled=true)]
		internal sealed class CompartmentItemDeleteRule : DslModeling::DeleteRule
		{
			/// <summary>
			/// Called when an element is deleted
			/// </summary>
			/// <param name="e"></param>
			public override void ElementDeleted(DslModeling::ElementDeletedEventArgs e)
			{
				ElementDeleted(e, false);
			}
			
			internal static void ElementDeleted(DslModeling::ElementDeletedEventArgs e, bool repaintOnly)
			{
				if(e==null) throw new global::System.ArgumentNullException("e");
				if(e.ModelElement is global::NHibernate.NHDesigner.EntityHasProperties)
				{
					global::System.Collections.ICollection elements = CompartmentItemAddRule.GetEntityForEntityShapePropertiesFromLastLink((global::NHibernate.NHDesigner.EntityHasProperties)e.ModelElement);
					CompartmentItemAddRule.UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
				}
			}
		}
		
		/// <summary>
		/// Rule to update compartments when the property on an item being displayed changes.
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.Property), FireTime=DslModeling::TimeToFire.TopLevelCommit, InitiallyDisabled=true)]
		internal sealed class CompartmentItemChangeRule : DslModeling::ChangeRule 
		{
			/// <summary>
			/// Called when an element is changed
			/// </summary>
			/// <param name="e"></param>
			public override void ElementPropertyChanged(DslModeling::ElementPropertyChangedEventArgs e)
			{
				ElementPropertyChanged(e, false);
			}
			
			internal static void ElementPropertyChanged(DslModeling::ElementPropertyChangedEventArgs e, bool repaintOnly)
			{
				if(e==null) throw new global::System.ArgumentNullException("e");
				if(e.ModelElement is global::NHibernate.NHDesigner.Property && e.DomainProperty.Id == global::NHibernate.NHDesigner.Property.NameDomainPropertyId)
				{
					global::System.Collections.IEnumerable elements = CompartmentItemAddRule.GetEntityForEntityShapeProperties((global::NHibernate.NHDesigner.Property)e.ModelElement);
					CompartmentItemAddRule.UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
				}
			}
		}
		
		/// <summary>
		/// Rule to update compartments when a roleplayer change happens
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityHasProperties), FireTime=DslModeling::TimeToFire.TopLevelCommit, InitiallyDisabled=true)]
		internal sealed class CompartmentItemRolePlayerChangeRule : DslModeling::RolePlayerChangeRule 
		{
			/// <summary>
			/// Called when the roleplayer on a link changes.
			/// </summary>
			/// <param name="e"></param>
			public override void RolePlayerChanged(DslModeling::RolePlayerChangedEventArgs e)
			{
				RolePlayerChanged(e, false);
			}
			
			internal static void RolePlayerChanged(DslModeling::RolePlayerChangedEventArgs e, bool repaintOnly)
			{
				if(e==null) throw new global::System.ArgumentNullException("e");
				if(typeof(global::NHibernate.NHDesigner.EntityHasProperties).IsAssignableFrom(e.DomainRelationship.ImplementationClass))
				{
					if(e.DomainRole.IsSource)
					{
						//global::System.Collections.IEnumerable oldElements = CompartmentItemAddRule.GetEntityForEntityShapePropertiesFromLastLink((global::NHibernate.NHDesigner.Property)e.OldRolePlayer);
						//foreach(DslModeling::ModelElement element in oldElements)
						//{
						//	DslModeling::LinkedElementCollection<DslDiagrams::PresentationElement> pels = DslDiagrams::PresentationViewsSubject.GetPresentation(element);
						//	foreach(DslDiagrams::PresentationElement pel in pels)
						//	{
						//		global::NHibernate.NHDesigner.EntityShape compartmentShape = pel as global::NHibernate.NHDesigner.EntityShape;
						//		if(compartmentShape != null)
						//		{
						//			compartmentShape.GetCompartmentMappings()[0].InitializeCompartmentShape(compartmentShape);
						//		}
						//	}
						//}
						
						global::System.Collections.IEnumerable elements = CompartmentItemAddRule.GetEntityForEntityShapePropertiesFromLastLink((global::NHibernate.NHDesigner.EntityHasProperties)e.ElementLink);
						CompartmentItemAddRule.UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
					}
					else 
					{
						global::System.Collections.IEnumerable elements = CompartmentItemAddRule.GetEntityForEntityShapeProperties((global::NHibernate.NHDesigner.Property)e.NewRolePlayer);
						CompartmentItemAddRule.UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
					}
				}
			}
		}
	
		/// <summary>
		/// Rule to update compartments when the order of items in the list changes.
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityHasProperties), FireTime=DslModeling::TimeToFire.TopLevelCommit, InitiallyDisabled=true)]
		internal sealed class CompartmentItemRolePlayerPositionChangeRule : DslModeling::RolePlayerPositionChangeRule 
		{
			/// <summary>
			/// Called when the order of a roleplayer in a relationship changes
			/// </summary>
			/// <param name="e"></param>
			public override void RolePlayerPositionChanged(DslModeling::RolePlayerOrderChangedEventArgs e)
			{
				RolePlayerPositionChanged(e, false);
			}
			
			internal static void RolePlayerPositionChanged(DslModeling::RolePlayerOrderChangedEventArgs e, bool repaintOnly)
			{
				if(e==null) throw new global::System.ArgumentNullException("e");
				if(typeof(global::NHibernate.NHDesigner.EntityHasProperties).IsAssignableFrom(e.DomainRelationship.ImplementationClass))
				{
					if(!e.CounterpartDomainRole.IsSource)
					{
						global::System.Collections.IEnumerable elements = CompartmentItemAddRule.GetEntityForEntityShapeProperties((global::NHibernate.NHDesigner.Property)e.CounterpartRolePlayer);
						CompartmentItemAddRule.UpdateCompartments(elements, typeof(global::NHibernate.NHDesigner.EntityShape), "Properties", repaintOnly);
					}
				}
			}
		}
	
		/// <summary>
		/// Reroute a connector when the role players of its underlying relationship change
		/// </summary>
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityReferencesBase), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AddConnectionRulePriority, InitiallyDisabled=true)]
		[DslModeling::RuleOn(typeof(global::NHibernate.NHDesigner.EntityReferencesBaseWithJoin), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AddConnectionRulePriority, InitiallyDisabled=true)]
		internal sealed class ConnectorRolePlayerChanged : DslModeling::RolePlayerChangeRule
		{
			/// <summary>
			/// Reroute a connector when the role players of its underlying relationship change
			/// </summary>
			public override void RolePlayerChanged(DslModeling::RolePlayerChangedEventArgs e)
			{
				if (e == null) throw new global::System.ArgumentNullException("e");
	
				global::System.Collections.ObjectModel.ReadOnlyCollection<DslDiagrams::PresentationViewsSubject> connectorLinks = DslDiagrams::PresentationViewsSubject.GetLinksToPresentation(e.ElementLink);
				foreach (DslDiagrams::PresentationViewsSubject connectorLink in connectorLinks)
				{
					// Fix up any binary link shapes attached to the element link.
					DslDiagrams::BinaryLinkShape linkShape = connectorLink.Presentation as DslDiagrams::BinaryLinkShape;
					if (linkShape != null)
					{
						global::NHibernate.NHDesigner.NHDesignerDiagram diagram = linkShape.Diagram as global::NHibernate.NHDesigner.NHDesignerDiagram;
						if (diagram != null)
						{
							if (e.NewRolePlayer != null)
							{
								DslDiagrams::NodeShape fromShape;
								DslDiagrams::NodeShape toShape;
								diagram.GetSourceAndTargetForConnector(linkShape, out fromShape, out toShape);
								if (fromShape != null && toShape != null)
								{
									if (!object.Equals(fromShape, linkShape.FromShape))
									{
										linkShape.FromShape = fromShape;
									}
									if (!object.Equals(linkShape.ToShape, toShape))
									{
										linkShape.ToShape = toShape;
									}
								}
								else
								{
									// delete the connector if we cannot find an appropriate target shape.
									linkShape.Delete();
								}
							}
							else
							{
								// delete the connector if the new role player is null.
								linkShape.Delete();
							}
						}
					}
				}
			}
		}
	}
