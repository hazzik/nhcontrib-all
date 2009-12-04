﻿<?xml version="1.0" encoding="utf-8"?>
<Dsl dslVersion="1.0.0.0" Id="ceaa11bc-3019-4591-8631-cd3c8cdf743d" Description="Description for NHibernate.NHDesigner.NHDesigner" Name="NHDesigner" DisplayName="NHDesigner" Namespace="NHibernate.NHDesigner" MinorVersion="1" Revision="1" ProductName="NHDesigner" CompanyName="NHibernate" PackageGuid="7eccb11c-fa71-4885-b37d-7b522c76c608" PackageNamespace="NHibernate.NHDesigner" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="278cf18b-4ba5-41c2-a355-0ed78459b7a2" Description="The root in which all other elements are embedded. Appears as a diagram." Name="NHibernateModel" DisplayName="NHibernate Model" Namespace="NHibernate.NHDesigner">
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="Entity" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>NHibernateModelHasEntities.Entities</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="db43c997-4966-46ac-9802-01c040526317" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="Entity" DisplayName="Entity" Namespace="NHibernate.NHDesigner">
      <Properties>
        <DomainProperty Id="16ed2d83-ca54-4fe2-93ba-72a7f2e94384" Description="Description for NHibernate.NHDesigner.Entity.Name" Name="Name" DisplayName="Name" DefaultValue="" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Property" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasProperties.Properties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="84d9eca0-bfd1-49ca-8797-57df3abd8810" Description="Description for NHibernate.NHDesigner.Property" Name="Property" DisplayName="Property" Namespace="NHibernate.NHDesigner">
      <Properties>
        <DomainProperty Id="e5232ec3-67be-43a6-a6c4-4cda50561905" Description="Description for NHibernate.NHDesigner.Property.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="142254a1-0da8-455c-a506-55d3948a4ff4" Description="Embedding relationship between the Model and Elements" Name="NHibernateModelHasEntities" DisplayName="NHibernate Model Has Entities" Namespace="NHibernate.NHDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="43e99d14-42b2-44b1-acfc-93a36356d745" Description="" Name="NHibernateModel" DisplayName="NHibernate Model" PropertyName="Entities" PropertyDisplayName="Entities">
          <RolePlayer>
            <DomainClassMoniker Name="NHibernateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4c33d372-409c-4cf1-9b48-46241eea4b9a" Description="" Name="Element" DisplayName="Element" PropertyName="NHibernateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="NHibernate Model">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1c21cd18-96bd-48eb-9864-b03e0f73bc5d" Description="Reference relationship between Elements." Name="EntityReferencesBase" DisplayName="Entity References Base" Namespace="NHibernate.NHDesigner">
      <Source>
        <DomainRole Id="17253bdc-ea79-4183-b076-8d4130296844" Description="Description for NHibernate.NHDesigner.ExampleRelationship.Target" Name="Source" DisplayName="Source" PropertyName="Subclass" PropertyDisplayName="Subclass">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="0f562032-7c8c-478f-ba03-cd6ae7a11a40" Description="Description for NHibernate.NHDesigner.ExampleRelationship.Source" Name="Target" DisplayName="Target" PropertyName="Baseclass" PropertyDisplayName="Baseclass">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1ccdc53f-3265-4e09-a95e-53b7db612686" Description="Description for NHibernate.NHDesigner.EntityHasProperties" Name="EntityHasProperties" DisplayName="Entity Has Properties" Namespace="NHibernate.NHDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="562ba08d-8213-4d30-b0a0-318b9bb6ca17" Description="Description for NHibernate.NHDesigner.EntityHasProperties.Entity" Name="Entity" DisplayName="Entity" PropertyName="Properties" PropertyDisplayName="Properties">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="7313b038-718b-40b5-bb02-16f70402aff6" Description="Description for NHibernate.NHDesigner.EntityHasProperties.Property" Name="Property" DisplayName="Property" PropertyName="Entity" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2dfeebfb-e935-45bf-a165-d676771d4c87" Description="Description for NHibernate.NHDesigner.EntityReferencesBaseWithJoin" Name="EntityReferencesBaseWithJoin" DisplayName="Entity References Base With Join" Namespace="NHibernate.NHDesigner">
      <Source>
        <DomainRole Id="01fa9e6d-e9e8-41c9-9c4a-5410be0a9fb0" Description="Description for NHibernate.NHDesigner.EntityReferencesBaseWithJoin.SourceEntity" Name="SourceEntity" DisplayName="Source Entity" PropertyName="JoinedSubclass" PropertyDisplayName="Joined Subclass">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="eb2df7d6-7835-4cd8-8c27-b4f2e7865a1a" Description="Description for NHibernate.NHDesigner.EntityReferencesBaseWithJoin.TargetEntity" Name="TargetEntity" DisplayName="Target Entity" PropertyName="BaseclassWJoin" PropertyDisplayName="Baseclass WJoin">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
  </Types>
  <Shapes>
    <CompartmentShape Id="d397d37e-21c1-4e7b-989d-42a89cba394c" Description="Description for NHibernate.NHDesigner.EntityShape" Name="EntityShape" DisplayName="Entity Shape" Namespace="NHibernate.NHDesigner" FixedTooltipText="Entity Shape" FillColor="247, 240, 102" InitialHeight="0.3" OutlineThickness="0.02125" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="-0.07">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="EntityIcon" DisplayName="Entity Icon" DefaultIcon="Resources\nhicon.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0.07">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="&lt;&lt;entity&gt;&gt;" />
      </ShapeHasDecorators>
      <Compartment Name="Properties" Title="Properties" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="816f028b-9043-4af0-a84f-0835c81b0245" Description="Subclass derivation" Name="SubclassConnector" DisplayName="Subclass Connector" Namespace="NHibernate.NHDesigner" FixedTooltipText="Subclass Connector" Color="113, 111, 110" TargetEndStyle="EmptyArrow" Thickness="0.01" />
    <Connector Id="6059470f-1da5-474d-a4bc-33833bdc9932" Description="Joined Subclass" Name="JoinedSubclassConnector" DisplayName="Joined Subclass Connector" Namespace="NHibernate.NHDesigner" FixedTooltipText="Joined Subclass Connector" Color="Brown" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.01" />
  </Connectors>
  <XmlSerializationBehavior Name="NHDesignerSerializationBehavior" Namespace="NHibernate.NHDesigner">
    <ClassData>
      <XmlClassData TypeName="NHibernateModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHibernateModelMoniker" ElementName="nHibernateModel" MonikerTypeName="NHibernateModelMoniker">
        <DomainClassMoniker Name="NHibernateModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="entities">
            <DomainRelationshipMoniker Name="NHibernateModelHasEntities" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Entity" MonikerAttributeName="name" MonikerElementName="entityMoniker" ElementName="entity" MonikerTypeName="EntityMoniker">
        <DomainClassMoniker Name="Entity" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="Entity/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="subclass">
            <DomainRelationshipMoniker Name="EntityReferencesBase" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="properties">
            <DomainRelationshipMoniker Name="EntityHasProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="joinedSubclass">
            <DomainRelationshipMoniker Name="EntityReferencesBaseWithJoin" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NHibernateModelHasEntities" MonikerAttributeName="" MonikerElementName="nHibernateModelHasEntitiesMoniker" ElementName="nHibernateModelHasEntities" MonikerTypeName="NHibernateModelHasEntitiesMoniker">
        <DomainRelationshipMoniker Name="NHibernateModelHasEntities" />
      </XmlClassData>
      <XmlClassData TypeName="EntityReferencesBase" MonikerAttributeName="" MonikerElementName="entityReferencesBaseMoniker" ElementName="entityReferencesBase" MonikerTypeName="EntityReferencesBaseMoniker">
        <DomainRelationshipMoniker Name="EntityReferencesBase" />
      </XmlClassData>
      <XmlClassData TypeName="SubclassConnector" MonikerAttributeName="" MonikerElementName="subclassConnectorMoniker" ElementName="subclassConnector" MonikerTypeName="SubclassConnectorMoniker">
        <ConnectorMoniker Name="SubclassConnector" />
      </XmlClassData>
      <XmlClassData TypeName="NHDesignerDiagram" MonikerAttributeName="" MonikerElementName="minimalLanguageDiagramMoniker" ElementName="minimalLanguageDiagram" MonikerTypeName="NHDesignerDiagramMoniker">
        <DiagramMoniker Name="NHDesignerDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="EntityShape" MonikerAttributeName="" MonikerElementName="entityShapeMoniker" ElementName="entityShape" MonikerTypeName="EntityShapeMoniker">
        <CompartmentShapeMoniker Name="EntityShape" />
      </XmlClassData>
      <XmlClassData TypeName="Property" MonikerAttributeName="" MonikerElementName="propertyMoniker" ElementName="property" MonikerTypeName="PropertyMoniker">
        <DomainClassMoniker Name="Property" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Property/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasProperties" MonikerAttributeName="" MonikerElementName="entityHasPropertiesMoniker" ElementName="entityHasProperties" MonikerTypeName="EntityHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="EntityReferencesBaseWithJoin" MonikerAttributeName="" MonikerElementName="entityReferencesBaseWithJoinMoniker" ElementName="entityReferencesBaseWithJoin" MonikerTypeName="EntityReferencesBaseWithJoinMoniker">
        <DomainRelationshipMoniker Name="EntityReferencesBaseWithJoin" />
      </XmlClassData>
      <XmlClassData TypeName="JoinedSubclassConnector" MonikerAttributeName="" MonikerElementName="joinedSubclassConnectorMoniker" ElementName="joinedSubclassConnector" MonikerTypeName="JoinedSubclassConnectorMoniker">
        <ConnectorMoniker Name="JoinedSubclassConnector" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="NHDesignerExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="EntityReferencesBaseBuilder">
      <Notes>Provides for the creation of an ExampleRelationship by pointing at two ExampleElements.</Notes>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityReferencesBase" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="EntityReferencesBaseWithJoinBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityReferencesBaseWithJoin" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="9d3d293a-b75d-4e4c-90cb-993c099cde5a" Description="Description for NHibernate.NHDesigner.NHDesignerDiagram" Name="NHDesignerDiagram" DisplayName="Minimal Language Diagram" Namespace="NHibernate.NHDesigner">
    <Class>
      <DomainClassMoniker Name="NHibernateModel" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Entity" />
        <ParentElementPath>
          <DomainPath>NHibernateModelHasEntities.NHibernateModel/!NHibernateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Entity/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EntityShape/Properties" />
          <ElementsDisplayed>
            <DomainPath>EntityHasProperties.Properties/!Property</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Property/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="SubclassConnector" />
        <DomainRelationshipMoniker Name="EntityReferencesBase" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="JoinedSubclassConnector" />
        <DomainRelationshipMoniker Name="EntityReferencesBaseWithJoin" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer FileExtension="nhx" EditorGuid="7eb0a134-abf8-459b-8c0f-f697a08ef867">
    <RootClass>
      <DomainClassMoniker Name="NHibernateModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="NHDesignerSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="NHDesigner">
      <ElementTool Name="Entity" ToolboxIcon="resources\exampleshapetoolbitmap.bmp" Caption="Entity" Tooltip="Create an Entity" HelpKeyword="CreateExampleClassF1Keyword">
        <DomainClassMoniker Name="Entity" />
      </ElementTool>
      <ConnectionTool Name="SubclassRelationship" ToolboxIcon="resources\exampleconnectortoolbitmap.bmp" Caption="SubclassRelationship" Tooltip="Drag between ExampleElements to create an ExampleRelationship" HelpKeyword="ConnectExampleRelationF1Keyword">
        <ConnectionBuilderMoniker Name="NHDesigner/EntityReferencesBaseBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="JoinedSUbclassRelationship" ToolboxIcon="Resources\ExampleConnectorToolBitmap.bmp" Caption="JoinedSUbclassRelationship" Tooltip="Joined SUbclass Relationship" HelpKeyword="JoinedSUbclassRelationship">
        <ConnectionBuilderMoniker Name="NHDesigner/EntityReferencesBaseWithJoinBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="false" UsesOpen="false" UsesSave="false" UsesLoad="false" />
    <DiagramMoniker Name="NHDesignerDiagram" />
  </Designer>
  <Explorer ExplorerGuid="eba6ef16-e410-4b8c-928a-1be541ae7a62" Title="NHDesigner Explorer">
    <ExplorerBehaviorMoniker Name="NHDesigner/NHDesignerExplorer" />
  </Explorer>
</Dsl>